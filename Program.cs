﻿using System;
using System.Threading;

namespace PokerConsoleApp
{
    public static class Program
    {
        public static int PlayerCount { get; private set; }

        static void Main()
        {
            

            // var conn = SqliteMethods.InitDatabase(2);
            var conn = SqliteMethods.CreateConnection(2);
            var comm = conn.CreateCommand();

            Simulation.GameRecord gr = new Simulation.GameRecord(10, 42, false);
            SqliteMethods.InsertResultItem(gr, comm);
            comm.Dispose();
            conn.Dispose();
        }

        static Program()
        {
            PlayerCount = 4;
        }
        public static void DebugSimulation()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            int games_to_simulate = 5000;
            Simulation.Simulate_Games(PlayerCount, games_to_simulate);
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
                PrintMenuText();
                sInput = Console.ReadLine();

                if (Int32.TryParse(sInput, out userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            int num_games = UtilityMethods.GetIntegerFromUser(3000, 2000000000);
                            var watch = new System.Diagnostics.Stopwatch();
                            watch.Start();
                            Simulation.Simulate_Games(PlayerCount, num_games);
                            watch.Stop();
                            Console.WriteLine($"Total Execution Time: {(watch.ElapsedMilliseconds / 60000.0).ToString("0.##")} minutes");
                            UtilityMethods.GetKeyPress();
                            break;
                        case 2:
                            Game.Play_Game();
                            UtilityMethods.GetKeyPress();
                            Thread.Sleep(1000);
                            break;
                        case 3:
                            Console.WriteLine("Enter number of players (2 to 8):");
                            PlayerCount = UtilityMethods.GetIntegerFromUser(2, 8);
                            Thread.Sleep(1000);
                            break;
                        case 4:
                            SqliteMethods.ShowDatabaseStatistics();
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

        private static void PrintMenuText()
        {
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
        }

    }

}
