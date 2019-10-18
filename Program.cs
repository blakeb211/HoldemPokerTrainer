
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
namespace PokerConsoleApp
{
    public class Program
    {
        public static int NUMBER_OF_PLAYERS = 4;
        public static Dictionary<int, int> pairRankDict = new Dictionary<int, int> { };
        public static Dictionary<string, int> card_to_int_dict = new Dictionary<string, int> { };
        static void Main()
        {

            Build_Card_To_Int_Table();
            DisplayMenu();

        }

        public static void Build_Card_To_Int_Table()
        {
            int card_value_index = 0;
            foreach (var r in Enum.GetValues(typeof(RankType)))
            {
                foreach (var s in Enum.GetValues(typeof(SuitType)))
                {
                    Card tempcard = new Card((RankType)r, (SuitType)s);
                    String str_card = tempcard.ToString();
                    card_to_int_dict.Add(str_card, card_value_index);
                    card_value_index++;
                }
            }
            if (card_value_index != 52)
                throw new Exception("error card_value_index not equal to 51 at end of building table method");
        }
        public static void DebugSimulation()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int games_to_simulate = 5000;
            Simulation.Simulate_Games(games_to_simulate);
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
                            int num_games = UtilityMethods.GetIntegerFromUser(3000, 2000000000);
                            var watch = new System.Diagnostics.Stopwatch();
                            watch.Start();
                            Simulation.Simulate_Games(num_games);
                            watch.Stop();
                            Console.WriteLine($"Total Execution Time: {(watch.ElapsedMilliseconds / 60000.0).ToString("0.##")} minutes");
                            UtilityMethods.GetKeyPress();
                            break;
                        case 2:
                            Play_Game();
                            UtilityMethods.GetKeyPress();
                            Thread.Sleep(1000);
                            break;
                        case 3:
                            // ask for number of players
                            // and change value if b/w 2 and 8
                            Console.WriteLine("Enter number of players (2 to 8):");
                            NUMBER_OF_PLAYERS = UtilityMethods.GetIntegerFromUser(2, 8);
                            Thread.Sleep(1000);
                            break;
                        case 4:
                            Show_Database_Statistics();
                            UtilityMethods.GetKeyPress();
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
        private static void Show_Database_Statistics()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("                        DATABASE STATISTICS                              ");
            Console.WriteLine("-------------------------------------------------------------------------");
            using (var conn = SQLite_Methods.CreateConnection(NUMBER_OF_PLAYERS))
            {
                SQLite_Methods.CreateTableIfNotExists(conn);
                SQLite_Methods.Create_Fresh_Index_On_HoleCards(conn);
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
                    string card_poss_str1 = str_rank + "-c";
                    string card_poss_str2 = str_rank + "-d";
                    string card_poss_str3 = str_rank + "-h";
                    string card_poss_str4 = str_rank + "-s";
                    int card_poss_int1 = card_to_int_dict[card_poss_str1];
                    int card_poss_int2 = card_to_int_dict[card_poss_str2];
                    int card_poss_int3 = card_to_int_dict[card_poss_str3];
                    int card_poss_int4 = card_to_int_dict[card_poss_str4];
                    sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE (Hole1 = {card_poss_int1} OR Hole1 = {card_poss_int2} OR Hole1 = {card_poss_int3} OR Hole1 = {card_poss_int4}) AND (Hole2 = {card_poss_int1} OR Hole2 = {card_poss_int2} OR Hole2 = {card_poss_int3} OR Hole2 = {card_poss_int4});";
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
                    string card_poss_str1 = str_rank + "-c";
                    string card_poss_str2 = str_rank + "-d";
                    string card_poss_str3 = str_rank + "-h";
                    string card_poss_str4 = str_rank + "-s";
                    int card_poss_int1 = card_to_int_dict[card_poss_str1];
                    int card_poss_int2 = card_to_int_dict[card_poss_str2];
                    int card_poss_int3 = card_to_int_dict[card_poss_str3];
                    int card_poss_int4 = card_to_int_dict[card_poss_str4];
                    sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE (Hole1 = {card_poss_int1} OR Hole1 = {card_poss_int2} OR Hole1 = {card_poss_int3} OR Hole1 = {card_poss_int4}) AND (Hole2 = {card_poss_int1} OR Hole2 = {card_poss_int2} OR Hole2 = {card_poss_int3} OR Hole2 = {card_poss_int4}) AND Winflag = 1;";
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
                Console.WriteLine(UtilityMethods.Trim_To_End(table.ToString(), "Count:"));
            }// Connection will be Disposed/Closed here

        }

        enum State { HOLE_CARDS_DEALT, FLOP_DEALT, TURN_DEALT, RIVER_DEALT, GAME_OVER };
        static int Play_Game()
        {
            bool exit_flag = false;
            do
            {
                // begin outer game loop so user can keep playing
                Console.Clear();
                State state = new State();
                state = State.HOLE_CARDS_DEALT;
                // DEAL A NEW GAME
                UtilityMethods.GetKeyPress();
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
                    List<Hand> lst_hand = Hand.Build_List_21_Hands(hole1, hole2, flop1, flop2, flop3, turn, river);
                    List<int> winning_hand_indices = Hand.FindBestHand(lst_hand);
                    lst_best_hands.Add(lst_hand[winning_hand_indices[0]]);
                    b.players[player_index].best_hand = lst_hand[winning_hand_indices[0]];
                }
                List<int> winning_player_indices = Hand.FindBestHand(lst_best_hands);
                foreach (var w_index in winning_player_indices)
                    b.players[w_index].Won_The_Hand = true;


                // CYCLE THROUGH STATES PRINTING BOARD OUT
                do
                {
                    Console.Clear();
                    String str_Board = Build_Game_Table(b, NUMBER_OF_PLAYERS, state);
                    Console.WriteLine(str_Board);
                    if (state < State.RIVER_DEALT)
                    {
                        UtilityMethods.GetKeyPress();
                        Thread.Sleep(300);
                        state++;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        exit_flag = UtilityMethods.Ask_User_For_Quit_Signal();
                        if (exit_flag == true)
                            return 0;
                        else
                            state = State.FLOP_DEALT;
                        break;
                    }
                } while (true); // END STATE LOOP FOR INDIVIDUAL GAMES
            } while (exit_flag == false);
            return 0;
        }

        private static string Build_Game_Table(Board b, int num_players, State state)
        {
            // BUILD COMMUNITY CARD BOARD
            var tbl_board = new ConsoleTable("flop", "turn", "river");
            switch (state)
            {
                case State.HOLE_CARDS_DEALT:
                    tbl_board.AddRow("       ", "    ", "     ");
                    break;
                case State.FLOP_DEALT:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", "    ", "     ");
                    break;
                case State.TURN_DEALT:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", b.turn_card.ToString(), "     ");
                    break;
                case State.RIVER_DEALT:
                    tbl_board.AddRow($"{b.flop_cards[0].ToString()} {b.flop_cards[1].ToString()} {b.flop_cards[2].ToString()}", b.turn_card.ToString(), b.river_card.ToString());
                    break;
                case State.GAME_OVER:
                    break;
                default:
                    break;
            }
            string str_board = UtilityMethods.Trim_To_End(tbl_board.ToString(), "Count:");

            // BUILD PLAYER TABLE
            ConsoleTable tbl_players = new ConsoleTable("Player", "Hole Cards", "Pre-flop %", "Post-flop %", "Best Hand", "HandType");
            for (int player_index = 0; player_index < num_players; player_index++)
            {
                switch (state)
                {
                    case State.HOLE_CARDS_DEALT:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), "  ", "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.FLOP_DEALT:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index), "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.TURN_DEALT:
                        if (player_index == 0)
                        {
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index), "   ", "   ");
                        }
                        else
                            tbl_players.AddRow(player_index.ToString(), "hidden", "  ", "  ", "   ", "   ");
                        break;
                    case State.RIVER_DEALT:
                        if (b.players[player_index].best_hand.IsSorted() == false)
                            b.players[player_index].best_hand.Sort();
                        if (b.players[player_index].Won_The_Hand == true)
                            tbl_players.AddRow(player_index.ToString() + " - win", $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index), b.players[player_index].best_hand.ToString(), b.players[player_index].best_hand.GetHandType().ToString());
                        else
                            tbl_players.AddRow(player_index.ToString(), $"{b.players[player_index].hole[0].ToString()} {b.players[player_index].hole[1].ToString()}", Get_Pre_Flop_Percentage(b, player_index).ToString(), Get_Post_Flop_Percentage(b, player_index), b.players[player_index].best_hand.ToString(), b.players[player_index].best_hand.GetHandType().ToString());
                        break;
                    default:
                        break;
                }
            }
            string ret_string = UtilityMethods.Trim_To_End(tbl_players.ToString(), "Count:");
            ret_string += str_board;
            return ret_string;
        }

        private static string Get_Post_Flop_Percentage(Board b, int player_index)
        {
            double records_with_those_cards = 0;
            double records_that_won = 0;
            using (var conn = SQLite_Methods.CreateConnection(b.players.Length))
            {
                SQLite_Methods.Create_Fresh_Index_On_HoleCards(conn);
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
                // Convert cards to integers for fast reading and writing
                int hole1_int = card_to_int_dict[holes[0].ToString()];
                int hole2_int = card_to_int_dict[holes[1].ToString()];

                int flop1_int = card_to_int_dict[flop[0].ToString()];
                int flop2_int = card_to_int_dict[flop[1].ToString()];
                int flop3_int = card_to_int_dict[flop[2].ToString()];

                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE Hole1 = {hole1_int} AND Hole2 = {hole2_int} AND Flop1 ={flop1_int} AND Flop2 = {flop2_int} AND Flop3 = {flop3_int};";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_with_those_cards = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE Hole1 = {hole1_int} AND Hole2 = {hole2_int} AND Flop1 ={flop1_int} AND Flop2 = {flop2_int} AND Flop3 = {flop3_int} AND Winflag = 1;";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_that_won = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
            }
            int percentage = (int)(records_that_won / records_with_those_cards * 100.0);
            if (percentage > 100 || percentage < 0)
                return "n/a - database too small";
            else
                return percentage.ToString();
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
                // Convert cards to integers for fast reading and writing
                int hole1_int = card_to_int_dict[holes[0].ToString()];
                int hole2_int = card_to_int_dict[holes[1].ToString()];


                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE Hole1 = {hole1_int} AND Hole2 = {hole2_int};";
                using (var myDataReader = sqlite_cmd.ExecuteReader())
                {
                    while (myDataReader.Read())
                    {
                        records_with_those_hole_cards = myDataReader.GetInt32(0);
                    }
                } // Reader will be Disposed/Closed here
                sqlite_cmd.CommandText = $"SELECT COUNT(*) FROM PlayerHandsTable WHERE Hole1 = {hole1_int} AND hole2 = {hole2_int} AND Winflag = 1 ;";
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



    }

}
