using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private static SQLiteTransaction transaction;
        private static int recordsTotal;
        private static int recordsWritten = 0;
        private static int gamesPerTransaction = 2000;
        private static int recordsAdded = 0;

        public static int SimulateGames(int playerCount, int targetGameCount)
        {
            recordsTotal = targetGameCount;
            // Database writing setup code
            SqliteMethods.InitDatabase(playerCount);
            conn = SqliteMethods.CreateConnection(playerCount);

            // CALL PRODUCERS AND CONSUMER HERE
            ThreadStart tProd = new ThreadStart(RecordProducer);
            ThreadStart tCons = new ThreadStart(RecordConsumer);
            Thread producerThread = new Thread(tProd);
            Thread producerThread2 = new Thread(tProd);
            Thread producerThread3 = new Thread(tProd);
            Thread consumerThread = new Thread(tCons);

            Timing timer = new Timing();
            timer.StartTime();
            producerThread.Start();
            producerThread2.Start();

            consumerThread.Start();
            while (true)
            {
                if (consumerThread.ThreadState == ThreadState.Stopped)
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

            do
            {
                Board b = new Board(Program.PlayerCount);

                // reset deck every 2 deals
                if ((_dealCount + 1) % 2 == 0)
                {
                    b.Deck = Board.BuildDeck();
                    _dealCount = 0;
                }

                b.DealGame();

                List<Hand> bestHands = new List<Hand> { };
                List<Hand> _allPossibleHands = new List<Hand>(21);
                List<int> _winningHandIndices;

                for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
                {
                    // Find individual players' best hand out of all possible
                    // combos of hole, flop, turn, and river cards
                    Hand.Build21Hands(b.Players[playerIndex].Hole, b.Cards, ref _allPossibleHands);
                    _winningHandIndices = Hand.FindBestHand(_allPossibleHands);
                    bestHands.Add(_allPossibleHands[_winningHandIndices[0]]);
                }

                List<int> winningPlayerIndices = Hand.FindBestHand(bestHands);

                // mark the winner(s)
                foreach (var wi in winningPlayerIndices)
                    b.Players[wi].IsWinner = true;

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
                        if (collection.TryAdd(record, 2) == true)
                        {
                            recordsAdded++;
                            break;
                        }
                    }
                }
            } while (recordsWritten < recordsTotal);
        }

        public static void RecordConsumer()
        {
            // Record consumer writes records to database
            command = conn.CreateCommand();
            transaction = conn.BeginTransaction();
            do
            {
                // Execute Insert command on database, one row per player
                GameRecord record;
                while (true)
                {
                    if (collection.TryTake(out record, 1))
                        break;
                }
                SqliteMethods.InsertResultItem(record, command);
                recordsWritten++;
                if (recordsWritten % gamesPerTransaction == 0)
                {
                    transaction.Commit();
                    transaction = conn.BeginTransaction();
                }
            } while (recordsWritten < recordsTotal);
            // Inevitably we broke out of loop with a partial transaction. Flush it to disk.
            transaction.Commit();

            // Clean up
            command.Dispose();
            transaction.Dispose();
            conn.Dispose();
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
