
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
namespace PokerConsoleApp
{
    public static class Program
    {
        public static int PlayerCount { get; private set; }

        static void Main()
        { 
            // assign rank test
            Hand h1 = new Hand("Tc-9c-8c-7c-6c");
            h1.AssignRankAndName();
            Console.WriteLine($"hand {h1} rank = {h1.Rank} name = {h1.Name}");

            h1 = new Hand("Td-9c-8s-7s-6h");
            h1.AssignRankAndName();
            Console.WriteLine($"hand {h1} rank = {h1.Rank} name = {h1.Name}");

            h1 = new Hand("2c-3c-3d-3h-3s");
            h1.AssignRankAndName();
            Console.WriteLine($"hand {h1} rank = {h1.Rank} name = {h1.Name}");

        }

        static  Program()
        {
            PlayerCount = 4;
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
                Console.WriteLine($"3 - Change number of players (currently set to {PlayerCount})");
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
                            PlayerCount = UtilityMethods.GetIntegerFromUser(2, 8);
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
            throw new NotImplementedException();
        }

        enum State { HOLE_CARDS_DEALT, FLOP_DEALT, TURN_DEALT, RIVER_DEALT, GAME_OVER };
        static int Play_Game()
        {
            throw new NotImplementedException();
        }

        private static string BuildGameTable(Board b, int num_players, State state)
        {
            throw new NotImplementedException();
        }

        private static string getPostFlopPercentage(Board b)
        {
            throw new NotImplementedException();
        }

        private static int getPreFlopPercentage(Board b)
        {
            throw new NotImplementedException();
        }

       

    }

}
