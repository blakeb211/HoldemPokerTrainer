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
            var tblPlayers = new ConsoleTable("Player", "Hole Cards", "Pre-Flop %", "Post-Flop %", "Best Hand", "IsWinner");
            var tblBoard = new ConsoleTable("Flop", "Turn", "River");
            int playerIndex = 0;
            switch (state)
            {
                case GameState.HOLE_CARDS_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), "-", "-", "-", "-");
                    }
                    break;
                case GameState.FLOP_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), "-", "-", "-", "-");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", " ", " ");
                    break;
                case GameState.TURN_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex++, p.GetHoleCardsString(), "-", "-", "-", "-");
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", " ");
                    break;
                case GameState.RIVER_DEALT:
                    foreach (var p in b.Players)
                    {
                        tblPlayers.AddRow(playerIndex, p.GetHoleCardsString(), "-", "-", p.BestHand, p.IsWinner);
                    }
                    tblBoard.AddRow($"{b.Cards[0]} {b.Cards[1]} {b.Cards[2]}", $"{b.Cards[3]}", $"{b.Cards[4]}");
                    break;
                case GameState.GAME_OVER:
                    Console.Clear();
                    break;
            }
            return tblBoard.ToString() + tblPlayers.ToString();
        }

        internal static int CalcPreFlopPercentage(Board b)
        {
            void Main()
            {
                // wrap all players in a transaction
                // preflop calc and postflop calc depending on if needed
                // commit transaction
            }

            // Define other methods, classes and namespaces here
            float CalculatePreFlopPercentage(long holeId, SQLiteCommand cmd)
            {
                /**********************************************
                * Each table corresponds to a specific set of hole
                * cards.Add up all the wins and losses of in a given
                * table and take the ratio of wins to the total games
                * that the holecards have participated in (wins + losses)
                * to get the pre-flop probability of winning.
                ***********************************************/
                long _winTotal = 0;
                long _lossTotal = 0;
                // need to test this command in Sqlite DB Viewer
                cmd.CommandText = $"SELECT (W, L) FROM Tbl{holeId};";
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // note the indices start at 0 because we only
                        // selected the win and loss columns
                        _winTotal += dr.GetInt64(0);
                        _lossTotal += dr.GetInt64(1);
                    }
                }
                return (float)(_winTotal / (_winTotal + _lossTotal));
            }


        }

        internal static string CalcPostFlopPercentage(Board b)
        {
            //CalculatePostFlopPercentage(long holeId, long flopId, SQLiteCommand cmd)
            /**********************************************
            * Each table corresponds to a specific set of hole
            * cards. Each FlopId corresponds to a specific
            * set of flop cards. Take the ratio of wins
            * to the total games (wins + losses) that hole & flop combination
            * has participated in to get the post-flop
            * probability of winning.
            ***********************************************/
            foreach (var p in b.Players)
            {
                long _winTotal = 0;
                long _lossTotal = 0;
                // need to test this command in Sqlite DB Viewer
                cmd.CommandText = $"SELECT (W, L) FROM Tbl{holeId} WHERE Flop = {flopId};";
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // note the indices start at 0 because we only
                        // selected the win and loss columns
                        _winTotal += dr.GetInt64(0);
                        _lossTotal += dr.GetInt64(1);
                    }
                }
                (float)(_winTotal / (_winTotal + _lossTotal));
            }
        }
    }
}
