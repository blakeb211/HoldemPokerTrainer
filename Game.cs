using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;

namespace PokerConsoleApp
{
    internal class Game
    {
        internal static void PlayGame()
        {
            // Deal a game
            while (true)
            {
                Board b = new Board(Program.PlayerCount);
                b.DealGame();
                CompleteGame(b);
                GameState state = GameState.HOLE_CARDS_DEALT;

                while (state < GameState.GAME_OVER)
                {
                    Console.Clear();
                    Console.WriteLine(BuildGameTable(b, state));
                    UtilityMethods.GetKeyPress();
                    state++;
                }
                if (UtilityMethods.Ask_User_For_Quit_Signal())
                    break;
            }
        }

        internal static void CompleteGame(Board b)
        {
            List<Hand> _bestHands = new List<Hand> { };
            List<Hand> _allPossibleHands = new List<Hand>(21);
            List<int> _winningHandIndices;

            for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
            {
                // Find individual players' best hand out of all possible
                // combos of hole, flop, turn, and river cards
                Hand.Build21Hands(b.Players[playerIndex].Hole, b.Cards, ref _allPossibleHands);
                Debug.Assert(_allPossibleHands.Count == 21);
                _winningHandIndices = Hand.FindBestHand(_allPossibleHands);
                // store best hand to the player object and sort it
                b.Players[playerIndex].BestHand = _allPossibleHands[_winningHandIndices[0]];
                b.Players[playerIndex].BestHand.Sort();
                // store best hand to the _bestHands list so that the winning players can be 
                // found later from it
                _bestHands.Add(_allPossibleHands[_winningHandIndices[0]]);
                _winningHandIndices.Clear();
                _allPossibleHands.Clear();
            }

            Debug.Assert(_bestHands.Count == Program.PlayerCount);
            List<int> winningPlayerIndices = Hand.FindBestHand(_bestHands);
            // mark the winner(s)
            foreach (var wi in winningPlayerIndices)
                b.Players[wi].IsWinner = true;
        }

        internal static string BuildGameTable(Board b, GameState state)
        {
            var tblPlayers = new ConsoleTable("Player", "Hole Cards", "Pre-Flop %", "Post-Flop %", "Best Hand", "IsWinner");
            var tblBoard = new ConsoleTable("Flop", "Turn", "River");
            int playerIndex = 0;

            Trace.WriteLine($"{nameof(BuildGameTable)} method running... State = {state}");

           

            switch (state)
            {
                case GameState.HOLE_CARDS_DEALT:
                    AssignPreFlopPercentages(b);
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), p.PreFlopOdds, "-", "-", "-");
                    }
                    break;
                case GameState.FLOP_DEALT:
                    AssignPostFlopPercentages(b);
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), p.PreFlopOdds, p.PostFlopOdds, "-", "-");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", " ", " ");
                    break;
                case GameState.TURN_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), p.PreFlopOdds, p.PostFlopOdds, "-", "-");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", " ");
                    break;
                case GameState.RIVER_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), p.PreFlopOdds, p.PostFlopOdds, p.BestHand, p.IsWinner);
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", $"{b.Cards[4]}");
                    break;
                case GameState.GAME_OVER:
                    Console.Clear();
                    break;
            }

            // Remove "Count :" from bottom of ConsoleTable for the board
            string boardStr = tblBoard.ToString();
            int countPos = boardStr.LastIndexOf('C');
            boardStr = boardStr.Substring(0, countPos - 1);

            // Remove "Count :" from bottom of ConsoleTables for the players
            string playerStr = tblPlayers.ToString();
            countPos = playerStr.LastIndexOf('C');
            playerStr = playerStr.Substring(0, countPos - 1);

            return boardStr + "\n" + playerStr;
        }

        internal static void AssignPreFlopPercentages(Board b)
        {
            //    wrap all players in a transaction
            //    preflop calc 
            //    commit transaction
            Trace.WriteLine($"{nameof(AssignPreFlopPercentages)} method running...");

            var conn = SqliteMethods.CreateConnection(b.Players.Count);

            using (SQLiteTransaction tran = conn.BeginTransaction())
            {
                foreach (var p in b.Players)
                {
                    long holeId = Card.CardUniquePrimeDict[p.Hole[0]] * Card.CardUniquePrimeDict[p.Hole[1]];

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        p.PreFlopOdds = String.Format("{0:0.0}", SqliteMethods.CalculatePreFlopPercentage(holeId, cmd));
                    }
                }
                tran.Commit();
            }
            conn.Dispose();
        }

        internal static void AssignPostFlopPercentages(Board  b)
        {
            //    wrap all players in a transaction
            //    postflop calc 
            //    commit transaction
            Trace.WriteLine($"{nameof(AssignPostFlopPercentages)} method running...");
            var conn = SqliteMethods.CreateConnection(b.Players.Count);

            using (SQLiteTransaction tran = conn.BeginTransaction())
            {
                foreach (var p in b.Players)
                {
                    long holeId = Card.CardUniquePrimeDict[p.Hole[0]] * Card.CardUniquePrimeDict[p.Hole[1]];
                    long flopId = Card.CardUniquePrimeDict[b.Cards[0]] * Card.CardUniquePrimeDict[b.Cards[1]] * Card.CardUniquePrimeDict[b.Cards[2]];

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                       p.PostFlopOdds = String.Format("{0:0.0}", SqliteMethods.CalculatePostFlopPercentage(holeId, flopId, cmd));
                    }
                }
                tran.Commit();
            }
            conn.Dispose();
        }

    }
}
