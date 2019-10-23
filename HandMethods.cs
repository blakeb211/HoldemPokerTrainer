using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public partial class Hand
    {

        public static List<Hand> Build21Hands(List<Card> hole, List<Card> board)
        {
            // validate arguments
            if (hole == null || board == null || hole.Count != 2 || board.Count != 5)
            {
                throw new ArgumentException($"{nameof(hole)} or {nameof(board)} were bad arguments to {nameof(Build21Hands)}");
            }

            // Find individual players' best hand out of all possible
            // combos of hole, flop, turn, and river cards
            List<Hand> retList = new List<Hand>(21)
            {
                // hands with both hole cards and 3 of the board cards
                [0] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[2] }),
                [1] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[2], board[3] }),
                [2] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[2], board[4] }),

                [3] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[3] }),
                [4] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[4] }),
                [5] = new Hand(new List<Card> { hole[0], hole[1], board[0], board[3], board[4] }),

                [6] = new Hand(new List<Card> { hole[0], hole[1], board[1], board[2], board[3] }),
                [7] = new Hand(new List<Card> { hole[0], hole[1], board[1], board[2], board[4] }),
                [7] = new Hand(new List<Card> { hole[0], hole[1], board[1], board[2], board[4] }),
                [8] = new Hand(new List<Card> { hole[0], hole[1], board[1], board[3], board[4] }),

                [9] = new Hand(new List<Card> { hole[0], hole[1], board[2], board[3], board[4] }),

                // hands with hole card #1 and 4 board cards
                [10] = new Hand(new List<Card> { hole[0], board[0], board[1], board[2], board[3] }),
                [11] = new Hand(new List<Card> { hole[0], board[0], board[1], board[2], board[4] }),
                [12] = new Hand(new List<Card> { hole[0], board[0], board[1], board[3], board[4] }),

                [13] = new Hand(new List<Card> { hole[0], board[0], board[2], board[3], board[4] }),
                [14] = new Hand(new List<Card> { hole[0], board[1], board[2], board[3], board[4] }),
                // hands with hole card #2 and 4 board cards
                [15] = new Hand(new List<Card> { hole[1], board[0], board[1], board[2], board[3] }),
                [16] = new Hand(new List<Card> { hole[1], board[0], board[1], board[2], board[4] }),
                [17] = new Hand(new List<Card> { hole[1], board[0], board[1], board[3], board[4] }),

                [18] = new Hand(new List<Card> { hole[1], board[0], board[2], board[3], board[4] }),
                [19] = new Hand(new List<Card> { hole[1], board[1], board[2], board[3], board[4] }),
                // hand made up of all the board cards
                [20] = new Hand(new List<Card> { board[0], board[1], board[2], board[3], board[4] })
            };

            return retList;
        }

        public static List<int> FindBestHand(List<Hand> inputHands)
        {
            /*********************************************************
             * INPUT:   List of Hands to compare
             * OUTPUT:  List<int> of the winnings hand indices 
             * NOTES:   If two hands tie, they are both returned.
             *********************************************************/
            if (inputHands == null || inputHands.Count < 1)
                throw new ArgumentException($"{nameof(inputHands)} passed to {nameof(FindBestHand)} was invalid.");

            Hand[] lst_hand_copy = inputHands.ToArray();

            List<int> winnerIndices = new List<int> { };
            List<Hand> winningHands = new List<Hand> { };
            List<int> losingHands = new List<int> { };

            for (int i = 0; i < lst_hand_copy.Length; i++)
            {
                for (int j = i + 1; j < lst_hand_copy.Length; j++)
                {
                    // assign ranks if they need us to
                    if (lst_hand_copy[i].Rank == default)
                        lst_hand_copy[i].AssignRankAndName();

                    if (lst_hand_copy[j].Rank == default)
                        lst_hand_copy[j].AssignRankAndName();

                    // Continue if i OR j are one of losing indices
                    if (lst_hand_copy[i] == null)
                    {
                        break;
                    }
                    if (lst_hand_copy[j] == null)
                    {
                        continue;
                    }

                    int _compareResult = lst_hand_copy[i].CompareTo(lst_hand_copy[j]);
                    if (_compareResult == -1)
                    {
                        // i loses
                        lst_hand_copy[i] = null;
                    }
                    else if (_compareResult == 1)
                    {
                        // j loses
                        lst_hand_copy[j] = null;
                    }
                }
            }
        
            // Only non-null value in lst_hand_copy should be tied winners or a single winner
            for (int i = 0; i < lst_hand_copy.Length; i++)
            {
                if (lst_hand_copy[i] != null)
                {
                    winnerIndices.Add(i);
                }
            }
            return winnerIndices;
        }

    }
}
