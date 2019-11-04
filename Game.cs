using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
                GameState state = GameState.FLOP_DEALT;

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
            var tblPlayers = new ConsoleTable("Player", "Hole Cards", "Best Hand", "IsWinner");
            var tblBoard = new ConsoleTable("Flop", "Turn", "River");

            switch (state)
            {
                case GameState.HOLE_CARDS_DEALT:
                    for (int i = 0; i < b.Players.Count; i++)
                    {
                        string holeStr = $"{b.Players[i].Hole[0]} {b.Players[i].Hole[1]}";
                        tblPlayers.AddRow(i, holeStr, "   ", "    ");
                    }
                    break;
                case GameState.FLOP_DEALT:
                    for (int i = 0; i < b.Players.Count; i++)
                    {
                        string holeStr = $"{b.Players[i].Hole[0]} {b.Players[i].Hole[1]}";
                        tblPlayers.AddRow(i, holeStr, "   ", "    ");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", " ", " ");
                    break;
                case GameState.TURN_DEALT:
                    for (int i = 0; i < b.Players.Count; i++)
                    {
                        string holeStr = $"{b.Players[i].Hole[0]} {b.Players[i].Hole[1]}";
                        tblPlayers.AddRow(i, holeStr, "   ", "    ");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", " ");
                    break;
                case GameState.RIVER_DEALT:
                    for (int i = 0; i < b.Players.Count; i++)
                    {
                        string holeStr = $"{b.Players[i].Hole[0]} {b.Players[i].Hole[1]}";
                        tblPlayers.AddRow(i, holeStr, $"{b.Players[i].BestHand}", b.Players[i].IsWinner.ToString());
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", $"{b.Cards[4]}");
                    break;
                case GameState.GAME_OVER:
                    Console.Clear();
                    break;
            }
            return tblBoard.ToString() + tblPlayers.ToString();
        }

        internal static string GetPostFlopPercentage(Board b)
        {
            throw new NotImplementedException();
        }

        internal static int GetPreFlopPercentage(Board b)
        {
            throw new NotImplementedException();
        }

    }
}
