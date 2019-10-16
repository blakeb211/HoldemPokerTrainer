using System.Collections.Generic;

namespace PokerConsoleApp
{
    public partial class Hand
    {

        public static Hand[] Build_21_Hands(Card hole1, Card hole2, Card c1, Card c2, Card c3, Card c4, Card c5)
        {
            // Find individual players' best hand out of all possible
            // combos of hole, flop, turn, and river cards
            // hole1, hole2 = hole cards
            // c1, c2, c3 = flop cards
            // c4, c5 = turn and river cards
            List<Hand> ret_list = new List<Hand> { };
            ret_list.Capacity = 21;
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

        public static List<int> FindBestHand(List<Hand> lst_input_hands)
        {
            /*********************************************************
             * INPUT: a List of Hands to compare
             * OUTPUT: a List<int> of the winnings hand indices,
             *          starting at 0 for the first hand.
             * 
             * if two hands tie, they are both returned.
             *********************************************************/
            Hand[] lst_hand_copy = lst_input_hands.ToArray();
            int hand_count = lst_input_hands.Count;
            List<int> lst_winning_hand_indices = new List<int> { };
            List<Hand> lst_winning_hands = new List<Hand> { };
            List<int> lst_losing_hand_indices = new List<int> { };
            for (int i = 0; i < hand_count; i++)
            {
                int loss_counter = 0;

                if (lst_hand_copy[i].GetHandType() == Hand.HandType.NotAssignedYet)
                    lst_hand_copy[i].AssignName();

                for (int j = 0; j < hand_count; j++)
                {
                    if (lst_hand_copy[j].GetHandType() == Hand.HandType.NotAssignedYet)
                        lst_hand_copy[j].AssignName();
                    // Continue if i OR j are one of losing indices
                    if (lst_losing_hand_indices.Contains(i))
                        break;
                    if (lst_losing_hand_indices.Contains(j))
                        continue;
                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        //DEBUG LINE TO SEE NUMBER OF COMPARISONS BEING DONE
                        //Console.WriteLine($"CompareTo: Does i = {i} beat j = {j} being evaluated");
                        int comparison_result = Hand.CompareTo(lst_hand_copy[i], lst_hand_copy[j]);
                        if (comparison_result == 0) // if i loses
                        {
                            loss_counter++;
                            lst_losing_hand_indices.Add(i);
                        }
                    }
                }
                if (loss_counter == 0) // if i-th hand always wins or ties then its a winner
                {
                    lst_winning_hands.Add(lst_hand_copy[i]);
                }
            }
            // Only thing left in lst_winning_hands should be tied winners or a single a winner
            for (int i = 0; i < lst_winning_hands.Count; i++)
            {
                lst_winning_hand_indices.Add(lst_input_hands.IndexOf(lst_winning_hands[i]));
            }
            return lst_winning_hand_indices;
        }

    }
}
