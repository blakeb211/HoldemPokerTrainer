/*
 *
 * 
 */
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
namespace PokerConsoleApp
{
    class Program
    {
        static int NUMBER_OF_PLAYERS = 4;
        const int HEIGHT = 40;
        const int WIDTH = 90;
        static void Main()
        {
            Set_Window_Size(130, 50);
            DisplayMenu();

        }
        public static void Debug_Test_Simulation_Speed()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int games_to_simulate = 5000;
            Simulate_Game_and_Save_to_DB(games_to_simulate);
            watch.Stop();
            Console.WriteLine($"Total Execution Time: {watch.ElapsedMilliseconds / 60000.0 } min");
            
        }
        public static void DisplayMenu()
        {
            bool exit_flag = false;
            do
            {
                Console.Clear();
                int userChoice = 0;
                string sInput = "";
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("                              MAIN MENU                                  ");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("1 - Simulate games to build up database");
                Console.WriteLine("2 - Enter poker training mode");
                Console.WriteLine($"3 - Change number of players (currently set to {NUMBER_OF_PLAYERS})");
                Console.WriteLine("4 - View database statistics");
                Console.WriteLine("5 - Exit");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("Please make a selection:");
                sInput = Console.ReadLine();
                if (Int32.TryParse(sInput, out userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            // get number of games to simulate
                            //Simulate_games_and_add_to_DB();
                            int num_games = Get_Number_Of_Games_To_Simulate_From_User();
                            var watch = new System.Diagnostics.Stopwatch();
                            watch.Start();
                            Simulate_Game_and_Save_to_DB(num_games);
                            watch.Stop();
                            Console.WriteLine($"Total Execution Time: {(watch.ElapsedMilliseconds / 60000.0).ToString("0.##")} minutes");
                            Blake_Utility_Methods.Get_User_To_Press_A_Key();
                            break;
                        case 2:
                            Play_Game_Showing_Statistics();
                            Blake_Utility_Methods.Get_User_To_Press_A_Key();
                            Thread.Sleep(1000);
                            break;
                        case 3:
                            // ask for number of players
                            // and change value if b/w 2 and 8
                            Console.WriteLine("Enter number of players (2 to 8):");
                            NUMBER_OF_PLAYERS = Get_Number_Of_Players_From_User();
                            Thread.Sleep(1000);
                            break;
                        case 4:
                            Show_Database_Statistics();
                            Blake_Utility_Methods.Get_User_To_Press_A_Key();
                            break;
                        case 5:
                            exit_flag = true;
                            break;
                        default:
                            break;
                    }
                }
            } while (exit_flag == false);
            
        }
        private static int Get_Number_Of_Games_To_Simulate_From_User()
        {
            string sInput = "";
            bool exit_flag = false;
            int userChoice;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter a number between 10 and 10,000,000: ");
                sInput = Console.ReadLine();
                if (Int32.TryParse(sInput, out userChoice))
                {
                    if (userChoice >= 10 && userChoice <= 10000000)
                    {
                        exit_flag = true;
                        return userChoice;
                    }
                }
            } while (exit_flag == false);
            return 0; // return default number of games to simulate
        }
        private static int Get_Number_Of_Players_From_User()
        {
            string sInput = "";
            bool exit_flag = false;
            int userChoice;
            do
            {
                Console.Clear();
                Console.WriteLine("Please enter a number between 2 and 8: ");
                sInput = Console.ReadLine();
                if (Int32.TryParse(sInput, out userChoice))
                {
                    switch (userChoice)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            exit_flag = true;
                            return userChoice;
                            
                        default:
                            break;
                    }
                }
            } while (exit_flag == false);
            return 4; // return default number of players
        }

        private static void Show_Database_Statistics()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("                        DATABASE STATISTICS                              ");
            Console.WriteLine("-------------------------------------------------------------------------");
            using (var conn = SQLite_Methods.CreateConnection(NUMBER_OF_PLAYERS))
            {
                SQLite_Methods.CreateTableIfNotExists(conn);
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();

                // get total record count
                int i_tot_records = 0;
                sqlite_cmd.CommandText = "SELECT COUNT(*) FROM PlayerHandsTable;";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        i_tot_records = myDataReader.GetInt32(0);
                        Console.WriteLine($"Filename:\t\t\t{NUMBER_OF_PLAYERS}-player-database.db");
                        Console.WriteLine($"Total number of records:\t{i_tot_records.ToString("N0")}\n");
                    }
                } // Reader will be Disposed/Closed here

                // COUNT UP POCKET PAIR OCCURENCES AND WINS
                int[] counts = new int[15];
                int[] wins = new int[15];
                // mark unused array elements
                counts[0] = -1; counts[1] = -1;
                wins[0] = -1; wins[1] = -1;
                foreach (var cr in Enum.GetValues(typeof(Card.Rank)))   // count up pocket pair occurences
                {
                    string str_rank = Card.Card_Rank_ToString((Card.Rank)cr);
                    sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards like '{str_rank}%{str_rank}%';";
                    using (var myReader = sqlite_cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            int i_count = myReader.GetInt32(0);
                            int i_card_rank = (int)((Card.Rank)cr);
                            counts[i_card_rank] = i_count;
                        }
                    } // Reader will be Disposed/Closed here
                }
                foreach (var cr in Enum.GetValues(typeof(Card.Rank))) // count up pocket pair wins
                {
                    string str_rank = Card.Card_Rank_ToString((Card.Rank)cr);
                    sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards like '{str_rank}%{str_rank}%' AND Winflag like '%True%';";
                    using (var myReader = sqlite_cmd.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            int i_count = myReader.GetInt32(0);
                            int i_card_rank = (int)((Card.Rank)cr);
                            wins[i_card_rank] = i_count;
                        }
                    } // Reader will be Disposed/Closed here
                }
                // PRINT OUT POCKET PAIR RESULTS TABLE
                var table = new ConsoleTable("Hole Cards", "Occurrences", "Wins", "Win %");
                foreach (var cr in Enum.GetValues(typeof(Card.Rank))) // count up pocket pair wins
                {
                    string str_rank = Card.Card_Rank_ToString((Card.Rank)cr);
                    int i_card_rank = (int)((Card.Rank)cr);
                    double chance = (wins[i_card_rank] * 1.0) / counts[i_card_rank] * 100.0;
                    string str_cr = Card.Card_Rank_ToString((Card.Rank)cr);
                    table.AddRow($"{str_cr} {str_cr}", counts[i_card_rank].ToString(), wins[i_card_rank].ToString(), string.Format("{0:F1}", chance));
                }
                Console.WriteLine(Blake_Utility_Methods.Trim_To_End(table.ToString(), "Count:"));
            }// Connection will be Disposed/Closed here
            Blake_Utility_Methods.Get_User_To_Press_A_Key();
        }

        static void Set_Window_Size(int w, int h)
        {
            Console.SetWindowPosition(0, 0);

            if (h < Console.LargestWindowHeight && w < Console.LargestWindowWidth)
            {
                Console.SetBufferSize(w, h);
                Console.SetWindowSize(w, h);
            }
            else
            {
                Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            }

        }
        static void Print_Board_And_Show_Winner()
        {
            Board b = new Board(NUMBER_OF_PLAYERS);
            b.Deal_Cards(NUMBER_OF_PLAYERS);
            Console.WriteLine(b);
            List<Hand> lst_best_hands = new List<Hand> { };

            for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
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
            // Find Winners
            List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
            // Print out winners
            var table = new ConsoleTable("Player #", "Best Hand", "Hand Type");
            for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
            {
                bool is_winner_flag = false;
                foreach (var i in winning_player_indices)
                    if (player_index == i)
                        is_winner_flag = true;
                string winner_mark = "";
                if (is_winner_flag == true && winning_player_indices.Count == 1)
                    winner_mark = " - winner";
                else if (is_winner_flag == true && winning_player_indices.Count > 1)
                    winner_mark = " - tie";
                table.AddRow(player_index.ToString() + winner_mark, lst_best_hands[player_index].DoSort().ToString(), lst_best_hands[player_index].GetHandType().ToString());
            }
            Console.WriteLine(table);

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
        static int Simulate_Game_and_Save_to_DB(int games_to_simulate)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = SQLite_Methods.CreateConnection(NUMBER_OF_PLAYERS);
            SQLite_Methods.CreateTableIfNotExists(sqlite_conn);
            for (int games_count = 0; games_count < games_to_simulate; games_count += 3)
            {
                Board b = new Board(NUMBER_OF_PLAYERS);
                // BEGIN SQLITE SETUP CODE
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                SQLiteTransaction transaction = sqlite_conn.BeginTransaction();
                // END SQLITE SETUP CODE
                for (int deal_count = 0; deal_count < 3; deal_count++) // two deals per deck
                {
                    if (games_to_simulate == 1)
                        deal_count += 2;
                    if (games_to_simulate == 2)
                        deal_count += 1;
                    b.Deal_Cards(NUMBER_OF_PLAYERS);
                    if ((deal_count + 1) % 2 == 0)
                        b.Get_New_Deck();
                    List<Hand> lst_best_hands = new List<Hand> { };
                    for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
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
                    List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
                    // Set WON_THE_HAND boolean inside player class
                    foreach (var wi in winning_player_indices)
                        b.players[wi].won_the_hand = true;

                    /**************************************************************
                    * GAME HAS BEEN SIMULATED, NOW WRITE IT TO DATABASE
                    ***************************************************************/

                    sqlite_cmd.CommandText = "INSERT INTO PlayerHandsTable "
                        + "(holecards, flop, turn, river, winflag) "
                        + "VALUES (@holecards, @flop, @turn, @river, @win_flag)";
                    sqlite_cmd.Parameters.AddWithValue("@holecards", "");
                    sqlite_cmd.Parameters.AddWithValue("@flop", "");
                    sqlite_cmd.Parameters.AddWithValue("@turn", "");
                    sqlite_cmd.Parameters.AddWithValue("@river", "");
                    sqlite_cmd.Parameters.AddWithValue("@win_flag", "");
                    // INSERT GAME DATA INTO DB - ONE ROW PER PLAYER
                    for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
                    {
                        List<Card> lst_hole_cards = new List<Card> { };
                        List<Card> lst_flop_cards = new List<Card> { };
                        // SORT HOLE CARDS UNIQUELY
                        // SORT FLOP CARDS UNIQUELY
                        // INSERT HOLE + FLOP + TURN + RIVER TO DB
                        for (int i = 0; i < 2; i++)
                            lst_hole_cards.Add(b.players[player_index].hole[i]);
                        for (int i = 0; i < 3; i++)
                            lst_flop_cards.Add(b.flop_cards[i]);
                        Card.Reorder_Cards_Uniquely(ref lst_hole_cards);
                        Card.Reorder_Cards_Uniquely(ref lst_flop_cards);
                        Card[] cards_to_insert = new Card[7];
                        cards_to_insert[0] = lst_hole_cards[0];
                        cards_to_insert[1] = lst_hole_cards[1];
                        cards_to_insert[2] = lst_flop_cards[0];
                        cards_to_insert[3] = lst_flop_cards[1];
                        cards_to_insert[4] = lst_flop_cards[2];
                        cards_to_insert[5] = b.turn_card;
                        cards_to_insert[6] = b.river_card;
                        /*************************************************************************
                         * SQLite Insert Data
                         * ***********************************************************************/
                        SQLite_Methods.InsertResultItem(cards_to_insert[0].ToString() + " " + cards_to_insert[1].ToString(), cards_to_insert[2].ToString() + " " + cards_to_insert[3].ToString() + " " + cards_to_insert[4].ToString(), cards_to_insert[5].ToString(), cards_to_insert[6].ToString(), b.players[player_index].won_the_hand.ToString(), sqlite_cmd);
                    } // end of loop to insert row for each player

                } // end of loop to do 3 games in one transaction
                transaction.Commit();
                sqlite_cmd.Dispose();
                

            } // end of games loop
              // Create index for fast querying of the database
            SQLite_Methods.Create_Fresh_Index_On_HoleCards(sqlite_conn);
            sqlite_conn.Dispose();
            return 0;
        }
        static int Play_Game_Showing_Statistics()
        {
            Console.Clear();
            // ADD TABLES TO PRINT THEM
            Board b = new Board(NUMBER_OF_PLAYERS);
            b.Deal_Cards(NUMBER_OF_PLAYERS);
            Console.WriteLine(b);
            List<Hand> lst_best_hands = new List<Hand> { };

            for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
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
            // Find Winners
            List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
            // Print out winners
            var table = new ConsoleTable("Player #", "Hole Cards", "Best Hand", "Hand Type");
            for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
            {
                bool is_winner_flag = false;
                foreach (var i in winning_player_indices)
                    if (player_index == i)
                        is_winner_flag = true;
                string winner_mark = "";
                if (is_winner_flag == true && winning_player_indices.Count == 1)
                    winner_mark = " - winner";
                else if (is_winner_flag == true && winning_player_indices.Count > 1)
                    winner_mark = " - tie";
                table.AddRow(player_index.ToString() + winner_mark, b.players[player_index].hole[0].ToString() + " " + b.players[player_index].hole[1].ToString(), lst_best_hands[player_index].DoSort().ToString(), lst_best_hands[player_index].GetHandType().ToString());
            }
            Console.WriteLine(table);
            return 0;
        }
        static void Test_method()
        {

        }
    }

}
