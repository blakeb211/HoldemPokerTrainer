using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.Threading;

namespace PokerConsoleApp
{
    class Simulation
    {
        // Variables that need to be accessible by producer and consumer methods
        private static BlockingCollection<GameRecord> collection = new BlockingCollection<GameRecord>();
        private static SQLiteConnection conn;
        private static SQLiteCommand command;
        // total games to be simulated (from user input)
        private static int recordsTotal;
        // counter for games that have been simulated by the RecordProducer threads
        private static int recordsAdded = 0;
        // counter for games that have been inserted into database by RecordConsumer threads
        private static int recordsWritten = 0;
        // when recordsWritten % gamesPerTransaction == 0, commit the sqlite transaction and start a new one
        private static int gamesPerTransaction = 10_000;

        // producers and consumers
        private static ThreadStart tProd = new ThreadStart(RecordProducer);
        private static ThreadStart tCons = new ThreadStart(RecordConsumer);
        private static Thread producerThread1 = new Thread(tProd);
        private static Thread consumerThread = new Thread(tCons);

        public static int SimulateGames(int playerCount, int targetGameCount)
        {
            //Check if we need to build sqlite tables for the first time.
            SqliteMethods.InitDatabaseIfNeeded(playerCount);
            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
            // total games to be simulated (from user input)
            recordsTotal = targetGameCount;

            // open connection to the database
            conn = SqliteMethods.CreateConnection(playerCount);

            Timing timer = new Timing();
            timer.StartTime();

            // play with thread priorities to get optimal balance of adding and writing GameRecords
            // on your system
            Console.WriteLine("Starting producer and consumer threads...");

            producerThread1.Start();
            consumerThread.Start();
            producerThread1.Priority = ThreadPriority.Lowest;
            consumerThread.Priority = ThreadPriority.Highest;

            while (true)
            {
                if (consumerThread.ThreadState == ThreadState.Stopped &&
                    recordsWritten >= recordsTotal &&
                    producerThread1.ThreadState == ThreadState.Stopped)
                {
                    timer.StopTime();
                    Thread.Sleep(1000);
                    break;
                }
            }

            Console.WriteLine($"Runtime duration: {timer.Result().TotalMinutes} minutes");
            conn.Dispose();
            return 0;
        }

        public static void RecordProducer()
        {
            // Record producer simulates a game and adds the result to a BlockingCollection
            int _dealCount = 0;
            Board b = new Board(Program.PlayerCount);

            do
            {
                // reset deck every 2 deals
                if ((_dealCount + 1) % 2 == 0)
                {
                    b = new Board(Program.PlayerCount);
                    _dealCount = 0;
                }

                b.DealGame();
                _dealCount++;
                Game.CompleteGame(b);

                // Calculate unique primes and add GameRecord to the BlockingCollection
                for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
                {
                    long holeUniquePrime = Card.CardUniquePrimeDict[b.Players[playerIndex].Hole[0]] *
                                            Card.CardUniquePrimeDict[b.Players[playerIndex].Hole[1]];
                    long flopUniquePrime = Card.CardUniquePrimeDict[b.Cards[0]] *
                                            Card.CardUniquePrimeDict[b.Cards[1]] *
                                            Card.CardUniquePrimeDict[b.Cards[2]];

                    var record = new GameRecord(holeUniquePrime, flopUniquePrime, b.Players[playerIndex].IsWinner);

                    while (true)
                    {
                        if (collection.TryAdd(record, 3) == true)
                        {
                            recordsAdded++;
                            break;
                        }
                    }
                }


            } while (recordsAdded < recordsTotal);
            Console.WriteLine("Producer thread ended");
        }



        public static void RecordConsumer()
        {
            // Record consumer writes records to database
            command = conn.CreateCommand();
            command.Transaction = conn.BeginTransaction();
            do
            {
                // Execute Insert command on database, one row per player
                GameRecord record;
                while (true)
                {
                    if (collection.TryTake(out record, 3))
                        break;
                }
                SqliteMethods.InsertResultItem(record, command);
                recordsWritten++;
                if (recordsWritten % gamesPerTransaction == 0)
                {
                    command.Transaction.Commit();
                    command = conn.CreateCommand();
                    command.Transaction = conn.BeginTransaction();
                    Console.WriteLine($"Records Added: {String.Format("{0:n0}", recordsAdded)}  " +
                        $"Records Written: {String.Format("{0:n0}", recordsWritten)} Difference: {String.Format("{0:n0}", recordsAdded - recordsWritten)}");

                }
            } while (recordsWritten < recordsTotal);
            // Inevitably we broke out of loop with a partial transaction. Flush it to disk.
            if (command.Transaction != null)
            {
                command.Transaction.Commit();
            }
            // Clean up
            if (command != null)
            {
                command.Dispose();
            }
            Console.WriteLine("Record writing thread ended.");
        }

        public struct GameRecord
        {
            public long holeUniquePrime;
            public long flopUniquePrime;
            public int winFlag;
            public GameRecord(long holeCardsUniquePrime, long flopCardsUniquePrime, bool winFlag)
            {
                holeUniquePrime = holeCardsUniquePrime;
                flopUniquePrime = flopCardsUniquePrime;
                if (winFlag == true)
                {
                    this.winFlag = 1;
                }
                else
                {
                    this.winFlag = 0;
                }
            }
        }
    }
}
