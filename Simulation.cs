using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Diagnostics.Tracing;
using System;

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
            conn = SQLite_Methods.CreateConnection(Program.NUMBER_OF_PLAYERS);
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
            //producerThread3.Start();
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
                Board b = new Board(Program.NUMBER_OF_PLAYERS);
                for (int deal_count = 0; deal_count <= 2; deal_count++) // two deals per deck
                {
                    b.Deal_Cards(Program.NUMBER_OF_PLAYERS);
                    if ((deal_count + 1) % 2 == 0)
                        b.Get_New_Deck();
                    List<Hand> lst_best_hands = new List<Hand> { };
                    for (int player_index = 0; player_index < Program.NUMBER_OF_PLAYERS; player_index++)
                    {
                        Card hole1 = b.players[player_index].hole[0];
                        Card hole2 = b.players[player_index].hole[1];
                        Card flop1 = b.flop_cards[0];
                        Card flop2 = b.flop_cards[1];
                        Card flop3 = b.flop_cards[2];
                        Card turn = b.turn_card;
                        Card river = b.river_card;
                        // Find individual players' best hand out of all possible
                        // combos of hole, flop, turn, and river cards
                        List<Hand> lst_hand = Hand.Build_List_21_Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                        List<int> winning_hand_indices = Hand.FindBestHand(lst_hand);
                        lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);
                    }
                    List<int> winning_player_indices = Hand.FindBestHand(lst_best_hands);
                    // Set WON_THE_HAND boolean inside player class
                    foreach (var wi in winning_player_indices)
                        b.players[wi].Won_The_Hand = true;

                    // Sort the cards uniquely and add GameRecord to the BlockingCollection
                    for (int player_index = 0; player_index < Program.NUMBER_OF_PLAYERS; player_index++)
                    {
                        // Sort hole and flop cards uniquely 
                        List<Card> lst_hole_cards = new List<Card> { };
                        List<Card> lst_flop_cards = new List<Card> { };
                        for (int i = 0; i < 2; i++)
                            lst_hole_cards.Add(b.players[player_index].hole[i]);
                        for (int i = 0; i < 3; i++)
                            lst_flop_cards.Add(b.flop_cards[i]);
                        Card.Reorder_Cards_Uniquely(ref lst_hole_cards);
                        Card.Reorder_Cards_Uniquely(ref lst_flop_cards);
                        var record = new GameRecord(lst_hole_cards[0], lst_hole_cards[1], lst_flop_cards[0], lst_flop_cards[1], lst_flop_cards[2], b.players[player_index].GetWinflag());
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
            public Card hole1, hole2, flop1, flop2, flop3;
            public int winFlag;
            public GameRecord(Card hole1, Card hole2, Card flop1, Card flop2, Card flop3, int winFlag)
            {
                this.hole1 = hole1;
                this.hole2 = hole2;
                this.flop1 = flop1;
                this.flop2 = flop2;
                this.flop3 = flop3;
                this.winFlag = winFlag;
            }
        }
    }
}
