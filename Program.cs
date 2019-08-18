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
            //Test_method();

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
                    sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards like '{str_rank}%{str_rank}%' AND Winflag = 1;";
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
            SQLite_Methods.Drop_Index_On_HoleCards(sqlite_conn);
            int GAMES_PER_TRANSACTION = 50;
            for (int games_count = 0; games_count < games_to_simulate; games_count += 3*GAMES_PER_TRANSACTION)
            {
                // BEGIN SQLITE SETUP CODE
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                SQLiteTransaction transaction = sqlite_conn.BeginTransaction();
                for (int games_per_tran_index = 0; games_per_tran_index < GAMES_PER_TRANSACTION; games_per_tran_index++)
                {
                    Board b = new Board(NUMBER_OF_PLAYERS);
                    
                    // END SQLITE SETUP CODE
                    for (int deal_count = 0; deal_count <= 2; deal_count++) // two deals per deck
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
                            b.players[wi].Won_The_Hand = true;

                        /**************************************************************
                        * GAME HAS BEEN SIMULATED, NOW WRITE IT TO DATABASE
                        ***************************************************************/

                        sqlite_cmd.CommandText = "INSERT INTO PlayerHandsTable "
                            + "(holecards, flop, winflag) "
                            + "VALUES (@holecards, @flop, @win_flag)";
                        sqlite_cmd.Parameters.AddWithValue("@holecards", "");
                        sqlite_cmd.Parameters.AddWithValue("@flop", "");
                        sqlite_cmd.Parameters.AddWithValue("@win_flag", "");
                        // INSERT GAME DATA INTO DB - ONE ROW PER PLAYER
                        for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
                        {
                            List<Card> lst_hole_cards = new List<Card> { };
                            List<Card> lst_flop_cards = new List<Card> { };
                            // SORT HOLE CARDS UNIQUELY
                            // SORT FLOP CARDS UNIQUELY
                            // INSERT HOLE + FLOP + DB
                            for (int i = 0; i < 2; i++)
                                lst_hole_cards.Add(b.players[player_index].hole[i]);
                            for (int i = 0; i < 3; i++)
                                lst_flop_cards.Add(b.flop_cards[i]);
                            Card.Reorder_Cards_Uniquely(ref lst_hole_cards);
                            Card.Reorder_Cards_Uniquely(ref lst_flop_cards);
                            /*************************************************************************
                            * SQLite Insert Data
                            * ***********************************************************************/
                            SQLite_Methods.InsertResultItem(lst_hole_cards[0].ToString() + " " + lst_hole_cards[1].ToString(), lst_flop_cards[0].ToString() + " " + lst_flop_cards[1].ToString() + " " + lst_flop_cards[2].ToString(), b.players[player_index].GetWinflag(), sqlite_cmd);
                        } // end of loop to insert row for each player

                    } // end of loop to do 3 games in one transaction
                }
                transaction.Commit();
                sqlite_cmd.Dispose();


            } // end of games loop
              // Create index for fast querying of the database
            SQLite_Methods.Create_Fresh_Index_On_HoleCards(sqlite_conn);
            sqlite_conn.Dispose();
            return 0;
        }
        enum State { hole_cards_dealt, flop_dealt, turn_dealt, river_dealt, game_over };
        static int Play_Game_Showing_Statistics()
        {
            bool exit_flag = false;
            do
            { // begin outer game loop so user can keep playing
                Console.Clear();
                State state = new State();
                state = State.hole_cards_dealt;
                // DEAL A NEW GAME
                Blake_Utility_Methods.Get_User_To_Press_A_Key();
                Board b = new Board(NUMBER_OF_PLAYERS);
                b.Deal_Cards(NUMBER_OF_PLAYERS);
                // FIND WINNERS
                List<Hand> lst_best_hands = new List<Hand> { }; // holds best hand of each player
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
                    b.players[player_index].best_hand = lst_hand[winning_hand_indices[0]];
                }
                List<int> winning_player_indices = Hand.Find_Best_Hand(lst_best_hands);
                foreach (var w_index in winning_player_indices)
                    b.players[w_index].Won_The_Hand = true;


                // CYCLE THROUGH STATES PRINTING BOARD OUT
                do
                {
                    Console.Clear();
                    String str_Board = Build_Game_Table(b, NUMBER_OF_PLAYERS, state);
                    Console.WriteLine(str_Board);
                    if (state < State.river_dealt)
                    {
                        Blake_Utility_Methods.Get_User_To_Press_A_Key();
                        Thread.Sleep(300);
                        state++;
                    }
                    else
                    {
                        Blake_Utility_Methods.Get_User_To_Press_A_Key();
                        Thread.Sleep(300);
                        state = State.flop_dealt;
                        break;
                    }
                } while (1 == 1); // END STATE LOOP FOR INDIVIDUAL GAMES
            } while (exit_flag == false);
            return 0;
        }

        private static string Build_Game_Table(Board b, int num_players, State state)
        {
            // BUILD COMMUNITY CARD BOARD
            var tbl_board = new ConsoleTable("flop", "turn", "river");
            switch (state)
            {
                case State.hole_cards_dealt:
                    tbl_board.AddRow("       ", "    ", "     ");
                    break;
                case State.flop_dealt:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", "    ", "     ");
                    break;
                case State.turn_dealt:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", b.turn_card.ToString(), "     ");
                    break;
                case State.river_dealt:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", b.turn_card.ToString(), b.river_card.ToString());
                    break;
                case State.game_over:
                    break;
                default:
                    break;
            }
            string str_board = Blake_Utility_Methods.Trim_To_End(tbl_board.ToString(), "Count:");

            // BUILD PLAYER TABLE
            ConsoleTable tbl_players = new ConsoleTable("Player", "Hole Cards", "Pre-flop %", "Post-flop %", "Best Hand", "HandType");
            for (int player_index = 0; player_index < num_players; player_index++)
            {
                switch (state)
                {
                    case State.hole_cards_dealt:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), "  ", "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.flop_dealt:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index).ToString(), "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.turn_dealt:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index).ToString(), "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.river_dealt:
                        if (b.players[player_index].Won_The_Hand == true)
                            tbl_players.AddRow(player_index.ToString() + " - win", $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index).ToString(), b.players[player_index].best_hand.DoSort().ToString(), b.players[player_index].best_hand.GetHandType().ToString());
                        else
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index).ToString(), b.players[player_index].best_hand.DoSort().ToString(), b.players[player_index].best_hand.GetHandType().ToString());
                        break;
                    default:
                        break;
                }
            }
            string ret_string = Blake_Utility_Methods.Trim_To_End(tbl_players.ToString(), "Count:");
            ret_string += str_board;
            return ret_string;
        }

        private static int Get_Post_Flop_Percentage(Board b, int player_index)
        {
            double records_with_those_cards = 0;
            double records_that_won = 0;
            using (var conn = SQLite_Methods.CreateConnection(b.players.Length))
            {
                SQLite_Methods.CreateTableIfNotExists(conn);
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();

                // get total records with those hole cards
                List<Card> holes = new List<Card> { };
                holes.Add(b.players[player_index].hole[0]);
                holes.Add(b.players[player_index].hole[1]);
                Card.Reorder_Cards_Uniquely(ref holes);
                List<Card> flop = new List<Card> { };
                flop.Add(b.flop_cards[0]);
                flop.Add(b.flop_cards[1]);
                flop.Add(b.flop_cards[2]);
                Card.Reorder_Cards_Uniquely(ref flop);
                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards like '{Card.Card_Rank_ToString(holes[0].GetRank())}%{Card.Card_Rank_ToString(holes[1].GetRank())}%' AND Flop like '{Card.Card_Rank_ToString(flop[0].GetRank())}%{Card.Card_Rank_ToString(flop[1].GetRank())}%{Card.Card_Rank_ToString(flop[2].GetRank())}%';";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_with_those_cards = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards like '{Card.Card_Rank_ToString(holes[0].GetRank())}%{Card.Card_Rank_ToString(holes[1].GetRank())}%' AND Flop like '{Card.Card_Rank_ToString(flop[0].GetRank())}%{Card.Card_Rank_ToString(flop[1].GetRank())}%{Card.Card_Rank_ToString(flop[2].GetRank())}%' And WinFlag = 1;";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_that_won = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
            }
            return (int)(records_that_won / records_with_those_cards * 100.0);
        }

        private static int Get_Pre_Flop_Percentage(Board b, int player_index)
        {
            double records_with_those_hole_cards = 0;
            double records_that_won = 0;
            using (var conn = SQLite_Methods.CreateConnection(b.players.Length))
            {
                SQLite_Methods.CreateTableIfNotExists(conn);
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();

                // get total records with those hole cards
                List<Card> holes = new List<Card> { };
                holes.Add(b.players[player_index].hole[0]);
                holes.Add(b.players[player_index].hole[1]);
                Card.Reorder_Cards_Uniquely(ref holes);

                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards = '{holes[0].ToString()} {holes[1].ToString()}';";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_with_those_hole_cards = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE HoleCards = '{holes[0].ToString()} {holes[1].ToString()}' AND WinFlag = 1 ;";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_that_won = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
            }
            return (int)(records_that_won / records_with_those_hole_cards * 100.0);

        }

        static void Test_method()
        {
            // TEST FIVE HANDS, with two hands that tie and two that are same HandType

            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is flush with Queen High
            c1 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            c3 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c5 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fourth hand is four of a kind, 9s with a four kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Hand h4 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fifth hand is a three of a kind, threes with a five and a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c2 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h5 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h4.EvaluateHandtype();
            h5.EvaluateHandtype();
            Hand hand1_sorted = h1.DoSort();
            Hand hand2_sorted = h2.DoSort();
            Hand hand3_sorted = h3.DoSort();
            Hand hand4_sorted = h4.DoSort();
            Hand hand5_sorted = h5.DoSort();
            // hand3 is passed in twice so those will tie, hand4 is best hand
            List<int> result = Hand.Find_Best_Hand(new List<Hand> { hand1_sorted, hand4_sorted, hand2_sorted, hand3_sorted, hand3_sorted, hand5_sorted });
            // Hand4 is best so result should be 2
            
        }
    }

}
