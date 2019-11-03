using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public partial class Hand
    {

        public static void Build21Hands(List<Card> hole, List<Card> board, ref List<Hand> retList)
        {
            // validate arguments
            if (retList == null || hole == null || board == null || hole.Count != 2 || board.Count != 5)
            {
                throw new ArgumentException($"{nameof(hole)} or {nameof(board)} were bad arguments to {nameof(Build21Hands)}");
            }

            // Find individual players' best hand out of all possible
            // combos of hole, flop, turn, and river cards

            // hands with both hole cards and 3 of the board cards
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[2] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[2], board[3] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[2], board[4] }));

            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[3] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[1], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[0], board[3], board[4] }));

            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[1], board[2], board[3] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[1], board[2], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[1], board[3], board[4] }));

            retList.Add(new Hand(new List<Card> { hole[0], hole[1], board[2], board[3], board[4] }));

            // hands with hole card #1 and 4 board cards
            retList.Add(new Hand(new List<Card> { hole[0], board[0], board[1], board[2], board[3] }));
            retList.Add(new Hand(new List<Card> { hole[0], board[0], board[1], board[2], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[0], board[0], board[1], board[3], board[4] }));

            retList.Add(new Hand(new List<Card> { hole[0], board[0], board[2], board[3], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[0], board[1], board[2], board[3], board[4] }));
            // hands with hole card #2 and 4 board cards
            retList.Add(new Hand(new List<Card> { hole[1], board[0], board[1], board[2], board[3] }));
            retList.Add(new Hand(new List<Card> { hole[1], board[0], board[1], board[2], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[1], board[0], board[1], board[3], board[4] }));

            retList.Add(new Hand(new List<Card> { hole[1], board[0], board[2], board[3], board[4] }));
            retList.Add(new Hand(new List<Card> { hole[1], board[1], board[2], board[3], board[4] }));
            // hand made up of all the board cards
            retList.Add(new Hand(new List<Card> { board[0], board[1], board[2], board[3], board[4] }));
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
                    // Continue if i OR j are one of losing indices
                    if (lst_hand_copy[i] == null)
                    {
                        break;
                    }
                    if (lst_hand_copy[j] == null)
                    {
                        continue;
                    }

                    // assign ranks if they need us to
                    if (lst_hand_copy[i].Rank == default)
                        lst_hand_copy[i].AssignRankAndName();

                    if (lst_hand_copy[j].Rank == default)
                        lst_hand_copy[j].AssignRankAndName();

                    int _compareResult = lst_hand_copy[i].CompareTo(lst_hand_copy[j]);
                    if (_compareResult == 1)
                    {
                        // i loses
                        lst_hand_copy[i] = null;
                    }
                    else if (_compareResult == -1)
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
