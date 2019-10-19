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
        private static int gamesPerTransaction = 1000;
        private static int recordsAdded = 0;
        private static object recordsAddedlock = new object();

        public static int Simulate_Games(int games_to_simulate)
        {
            recordsTotal = games_to_simulate;
            // Database writing setup code
            conn = SQLite_Methods.CreateConnection(Program.PlayerCount);
            SQLite_Methods.CreateTableIfNotExists(conn);
            SQLite_Methods.DropIndexIfExists(conn);

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
            return 0;
        }



        // Record producer simulates a game and adds the result to a BlockingCollection
        public static void RecordProducer()
        {
            do
            {
                Board b = new Board(Program.PlayerCount);
                for (int deal_count = 0; deal_count <= 2; deal_count++) // two deals per deck
                {
                    b.DealCards();
                    if ((deal_count + 1) % 2 == 0)
                        b.BuildDeck();
                    List<Hand> lst_best_hands = new List<Hand> { };
                    for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
                    {
                        Card hole1 = b.Players[playerIndex].Hole[0];
                        Card hole2 = b.Players[playerIndex].Hole[1];
                        Card flop1 = b.Cards[0];
                        Card flop2 = b.Cards[1];
                        Card flop3 = b.Cards[2];
                        Card turn = b.Cards[3];
                        Card river = b.Cards[4];

                        // Find individual players' best hand out of all possible
                        // combos of hole, flop, turn, and river cards
                        List<Hand> lst_hand = Hand.Build21Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                        List<int> winning_hand_indices = Hand.FindBestHand(lst_hand);
                        lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);
                    }
                    List<int> winning_player_indices = Hand.FindBestHand(lst_best_hands);
                    // Set WON_THE_HAND boolean inside player class
                    foreach (var wi in winning_player_indices)
                        b.Players[wi].IsWinner = true;

                    // Sort the cards uniquely and add GameRecord to the BlockingCollection
                    for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
                    {
                        // Sort hole and flop cards uniquely 
                        List<Card> lst_hole_cards = new List<Card> { };
                        List<Card> lst_flop_cards = new List<Card> { };
                        for (int i = 0; i < 2; i++)
                            lst_hole_cards.Add(b.Players[playerIndex].Hole[i]);
                        for (int i = 0; i < 3; i++)
                            lst_flop_cards.Add(b.Cards[i]);

                        ulong holeUniquePrime = 0;
                        ulong flopUniquePrime = 0;
                        var record = new GameRecord(holeUniquePrime, flopUniquePrime, b.Players[playerIndex].IsWinner);
                        while (true)
                        {
                            if (collection.TryAdd(record, 1) == true)
                            {
                                break;
                            }
                        }
                    }
                }
            } while (recordsWritten < recordsTotal);
        }

        // Record consumer writes records to the sqlite database
        public static void RecordConsumer()
        {
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
                SQLite_Methods.InsertResultItem(record, command);
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
            public ulong holeUniquePrime;
            public ulong flopUniquePrime;
            public int winFlag;
            public GameRecord(ulong holeCardsUniquePrime, ulong flopCardsUniquePrime, bool winFlag)
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
