﻿using ConsoleTables;
using System;
using System.Collections.Generic;

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
                CompleteGame(b, out List<Hand> bestHands);
                GameState state = GameState.FLOP_DEALT;

                while (state < GameState.GAME_OVER)
                {
                    Console.Clear();
                    Console.WriteLine(BuildGameTable(b, Program.PlayerCount, state, bestHands));
                    UtilityMethods.GetKeyPress();
                    state++;
                }
                if (UtilityMethods.Ask_User_For_Quit_Signal())
                    break;
            }
        }

        internal static void CompleteGame(Board b, out List<Hand> _bestHands)
        {
            _bestHands = new List<Hand> { };
            List<Hand> _allPossibleHands = new List<Hand>(21);
            List<int> _winningHandIndices;

            for (int playerIndex = 0; playerIndex < Program.PlayerCount; playerIndex++)
            {
                // Find individual players' best hand out of all possible
                // combos of hole, flop, turn, and river cards
                Hand.Build21Hands(b.Players[playerIndex].Hole, b.Cards, ref _allPossibleHands);
                _winningHandIndices = Hand.FindBestHand(_allPossibleHands);
                _bestHands.Add(_allPossibleHands[_winningHandIndices[0]]);
            }

            List<int> winningPlayerIndices = Hand.FindBestHand(_bestHands);

            // mark the winner(s)
            foreach (var wi in winningPlayerIndices)
                b.Players[wi].IsWinner = true;
        }

        internal static string BuildGameTable(Board b, int num_players, GameState state, List<Hand> bestHands)
        {
            var tblPlayers = new ConsoleTable("Player", "Hole Cards", "Best Hand", "IsWinner");
            var   tblBoard = new ConsoleTable("Flop", "Turn", "River");

            switch(state)
            {
                case GameState.HOLE_CARDS_DEALT:
                    for(int i = 0; i < b.Players.Count; i++)
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
                        tblPlayers.AddRow(i, holeStr, $"{bestHands[i]}", b.Players[i].IsWinner.ToString());
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
