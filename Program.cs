/*
 * Purpose: This program draws a random hand from the deck and displays the handtype. 
 *          Ultimately, it will use monte carlo to calculate the odds of winning for different hole cards. 
 * Input:   None. Player number is fixed at 4.
 * Output:  Print four hands, their hand types, and who wins           
 *
 *Todo: Add evaluator method
 *             Identify and print out correct hand type, e.g. four 5s and a queen kicker, full house, 3s over 2s. 
 *             
 *      Add testing 
 * 
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;
namespace PokerConsoleApp
{
    class Program
    {
        const int NUMBER_OF_PLAYERS = 4;
        static void Main()
        {
            Simulate_Game_and_Save_to_DB();
        }
        static void Print_Board_And_Show_Winner()
        {
            Board b = new Board(NUMBER_OF_PLAYERS);
            b.Deal_Cards();
            Console.WriteLine(b);
            List<Hand> lst_best_hands = new List<Hand> { };
            for (int player_index = 0; player_index < 4; player_index++)
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
                List<Hand> lst_hand = Build_List_21_Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                List<int> winning_hand_indices = Hand.Find_Best_Hand(lst_hand);
                lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);

            }
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Player {i} " + " Best Hand = " + lst_best_hands[i].DoSort() + " " + lst_best_hands[i].GetHandType());
            }
            // Find Winners
            List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
            // Print out winners
            Console.Write($"\n   Winners = ");
            foreach (var ii in winning_player_indices)
            {
                Console.Write($" {ii} ");
            }
            Console.WriteLine(".");
        }
        static List<Hand> Build_List_21_Hands(Card hole1, Card hole2, Card c1, Card c2, Card c3, Card c4, Card c5)
        {
            // Find individual players' best hand out of all possible
            // combos of hole, flop, turn, and river cards
            // hole1, hole2 = hole cards
            // c1, c2, c3 = flop cards
            // c4, c5 = turn and river cards
            List<Hand> ret_list = new List<Hand> { };
            Hand[] h = new Hand[21];
            // UNIQUE HAND COMBINATIONS USING BOTH HOLE CARDS + COMBINATIONS OF 3 FROM THE REST
            h[0] = new Hand(new List<Card> { hole1, hole2, c1, c2, c3 });
            h[1] = new Hand(new List<Card> { hole1, hole2, c1, c3, c4 });
            h[2] = new Hand(new List<Card> { hole1, hole2, c1, c3, c5 });

            h[3] = new Hand(new List<Card> { hole1, hole2, c1, c2, c4 });
            h[4] = new Hand(new List<Card> { hole1, hole2, c1, c2, c5 });
            h[5] = new Hand(new List<Card> { hole1, hole2, c1, c4, c5 });

            h[6] = new Hand(new List<Card> { hole1, hole2, c2, c3, c4 });
            h[7] = new Hand(new List<Card> { hole1, hole2, c2, c3, c5 });
            h[8] = new Hand(new List<Card> { hole1, hole2, c2, c4, c5 });

            h[9] = new Hand(new List<Card> { hole1, hole2, c3, c4, c5 });
            // UNIQUE HAND COMBINATIONS USING ONE HOLE CARD + COMBINATIONS OF 4 FROM REST
            // hole card 1 
            h[10] = new Hand(new List<Card> { hole1, c1, c2, c3, c4 });
            h[11] = new Hand(new List<Card> { hole1, c1, c2, c3, c5 });
            h[12] = new Hand(new List<Card> { hole1, c1, c2, c4, c5 });

            h[13] = new Hand(new List<Card> { hole1, c1, c3, c4, c5 });
            h[14] = new Hand(new List<Card> { hole1, c2, c3, c4, c5 });
            // hole card 2 
            h[15] = new Hand(new List<Card> { hole2, c1, c2, c3, c4 });
            h[16] = new Hand(new List<Card> { hole2, c1, c2, c3, c5 });
            h[17] = new Hand(new List<Card> { hole2, c1, c2, c4, c5 });

            h[18] = new Hand(new List<Card> { hole2, c1, c3, c4, c5 });
            h[19] = new Hand(new List<Card> { hole2, c2, c3, c4, c5 });
            // hand with 0 hole cards
            h[20] = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // build List<hand> to return from method
            for (int i = 0; i < 21; i++)
                ret_list.Add(h[i]);

            return ret_list;
        }
        static int Simulate_Game_and_Save_to_DB()
        {
            Board b = new Board(NUMBER_OF_PLAYERS);
            b.Deal_Cards();
            Console.WriteLine(b);
            List<Hand> lst_best_hands = new List<Hand> { };
            for (int player_index = 0; player_index < 4; player_index++)
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
                List<Hand> lst_hand = Build_List_21_Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                List<int> winning_hand_indices = Hand.Find_Best_Hand(lst_hand);
                lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);

            }
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Player {i} " + " Best Hand = " + lst_best_hands[i].DoSort() + " " + lst_best_hands[i].GetHandType());
            }
            // Find Winners
            List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
            // Set WON_THE_HAND boolean inside player class
            foreach(var wi in winning_player_indices)
            {
                b.players[wi].won_the_hand = true;
            }
            // Print out winners
            Console.Write($"\n   Winners = ");
            foreach (var ii in winning_player_indices)
            {
                Console.Write($" {ii} ");
            }
            Console.WriteLine(".");
            // GAME HAS BEEN SIMULATED, NOW WRITE IT TO DATABASE
            SQLiteConnection sqlite_conn;
            sqlite_conn = SQLite_Methods.CreateConnection(NUMBER_OF_PLAYERS);
            // CREATE TABLE IF IT DOESN'T ALREADY EXIST
            SQLite_Methods.CreateTableIfNotExists(sqlite_conn);
            // INSERT GAME DATA INTO DB - ONE ROW PER HAND
            for (int player_index = 0; player_index < 4; player_index++)
            {
                Card[] cards_to_insert = new Card[7];
                cards_to_insert[0] = b.players[player_index].hole[0];
                cards_to_insert[1] = b.players[player_index].hole[1];
                cards_to_insert[2] = b.flop_cards[0];
                cards_to_insert[3] = b.flop_cards[1];
                cards_to_insert[4] = b.flop_cards[2];
                cards_to_insert[5] = b.turn_card;
                cards_to_insert[6] = b.river_card;
                SQLite_Methods.InsertData(sqlite_conn, cards_to_insert, b.players[player_index].won_the_hand);
            }
                SQLite_Methods.ReadData(sqlite_conn);
            return 0;
        }
        static int Play_Game_Showing_Statistics()
        {
            return 0;
        }
        static void Test_method()
        {

        }
    }

}
