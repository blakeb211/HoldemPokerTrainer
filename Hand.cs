using System;
using System.Collections.Generic;

/* TODO
 * add evaluateTypeOfHand(void) method
 * add doesItBeat(Hand h) method
 * 
 * This way both of these methods is separately testable
 * 
 * Also, if one hand has a higher rank than the other, it automatically beats it and we can
 * skip extra analysis steps.
 */
namespace PokerConsoleApp
{
    public class Hand
    {
        public enum HandType
        {
            NotAssignedYet = 9,
            StraightFlush = 8,
            FourOfAKind = 7,
            FullHouse = 6,
            Flush = 5,
            Straight = 4,
            ThreeOfAKind = 3,
            TwoPair = 2,
            OnePair = 1,
            HighCard = 0
        };
        private readonly List<Card> cards = new List<Card> { };
        private HandType hand_type = new HandType();
        readonly int[] rank_tally = new int[15];  // let 0 and 1 indices be a waste to make code more clear. 
        readonly int[] suit_tally = new int[5];   // let 0 be waste, 1 = hearts, 2 = diamonds, 3 = spade, 4 = club
        public Hand(List<Card> c)
        {
            if (c.Count != 5)
                throw new Exception("Something other than a 5-Card List was passed to the Hand() constructor");
            cards.Capacity = 5;
            this.hand_type = HandType.NotAssignedYet;
            foreach (var ci in c)
                cards.Add(ci);

        }
        public Hand()
        {
            hand_type = HandType.NotAssignedYet;
            for (int i = 0; i < 4; i++)
                suit_tally[i] = 0;
            for (int i = 0; i < 14; i++)
                rank_tally[i] = 0;
        }

        public override string ToString()
        {
            string ret_string = "";
            for (int i = 0; i < 5; i++)
            {
                ret_string += "( ";
                ret_string = ret_string + this.cards[i].GetRank() + "-" + this.cards[i].GetSuit();
                ret_string += " )";
            }
            return ret_string;
        }
        public int GetCount()
        {
            return this.cards.Count;
        }
        public HandType GetHandType()
        {
            return this.hand_type;
        }
        public void AddCard(Card c)
        {
            if (cards.Count == 5)
                throw new Exception("Can't add a card to a hand that already has 5 cards!");
            cards.Add(c);
        }
        public void PrintHand()
        {
            foreach (var c in this.cards)
            {
                Console.Write($"{c.GetRank()}-{c.GetSuit()} ");
            }
            Console.Write("\n");
        }
        private bool IsThisAStraight(int[] rank_tally)
        {
            if (rank_tally.Length != 15)
                throw new Exception("The array passed to IsThisAStraight doesn't have 15 elements!");
            // count single cards that are in order
            int cards_in_a_row_counter = 0;
            int last_card = -2;
            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 1)
                {
                    if (last_card == -2 || (i - last_card) == 1)
                    {
                        cards_in_a_row_counter++;
                        last_card = i;
                    }
                }
            }
            // check for Ace, Two, Three, Four, Five straight
            if (rank_tally[(int)Card.Rank.ACE] == 1 && rank_tally[(int)Card.Rank.TWO] == 1 && rank_tally[(int)Card.Rank.THREE] == 1 && rank_tally[(int)Card.Rank.FOUR] == 1 && rank_tally[(int)Card.Rank.FIVE] == 1)
                cards_in_a_row_counter = 5;

            if (cards_in_a_row_counter == 5)
                return true;
            else
                return false;
        }
        private bool IsThisAFlush(int[] suit_tally)
        {
            if (suit_tally.Length != 5)
                throw new Exception("The array passed to IsThisAFlush doesn't have 5 elements!");

            bool flush_flag = false;
            // check for 5 cards of a single suit
            for (int i = 1; i < 5; i++)
            {
                if (suit_tally[i] == 5)
                    flush_flag = true;
            }

            return flush_flag;
        }
        private bool IsThisFourOfAKind(int[] rank_tally)
        {
            bool ret_flag = false;
            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 4)
                    ret_flag = true;
            }
            return ret_flag;
        }
        private bool IsThisAFullHouse(int[] rank_tally)
        {
            bool ret_flag = false;
            bool has_a_set_of_three = false;
            bool has_a_pair = false;

            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 3)
                {
                    has_a_set_of_three = true;
                }
                if (rank_tally[i] == 2)
                {
                    has_a_pair = true;
                }
            }
            if (has_a_pair && has_a_set_of_three)
                ret_flag = true;

            return ret_flag;
        }
        private bool IsThisThreeOfAKind(int[] rank_tally)
        {
            // note that this method would return false positive on a FourOfAKind hand or FullHouse 
            // if run by itself outside of the evaluate hand method
            bool ret_flag = false;
            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 3)
                    ret_flag = true;
            }
            return ret_flag;
        }
        private bool IsThisTwoPair(int[] rank_tally)
        {
            bool ret_flag = false;
            int rank_of_first_pair = -1;
            int rank_of_second_pair = -1;
            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 2 && rank_of_first_pair == -1)
                {
                    rank_of_first_pair = i;
                    continue;
                }
                if (rank_tally[i] == 2 && rank_of_first_pair != -1)
                {
                    rank_of_second_pair = i;
                    continue;
                }
            }
            if (rank_of_first_pair != -1 && rank_of_second_pair != -1 && rank_of_first_pair != rank_of_second_pair)
                ret_flag = true;
            return ret_flag;
        }

        private bool IsThisOnePair(int[] rank_tally)
        {
            bool ret_flag = false;
            int rank_of_pair = -1;
            for (int i = 2; i < 15; i++)
            {
                if (rank_tally[i] == 2 && rank_of_pair == -1)
                {
                    rank_of_pair = i;
                }
            }
            if (rank_of_pair != -1)
                ret_flag = true;
            return ret_flag;
        }
        public static int DoesThisHandBeatThatHand(Hand hand_1, Hand hand_2)
        {
            // This method compares two hands.
            // Return a 1 if hand1 beats hand2
            // Return a 0 if hand1 loses to hand2
            // Return a -1 if hand1 ties hand2
            int ret_val = 0;
            // Check whether hand objects can be compared safely
            Hand.HandType ht_1 = hand_1.GetHandType();
            Hand.HandType ht_2 = hand_2.GetHandType();
            if (ht_1 == Hand.HandType.NotAssignedYet || ht_2 == Hand.HandType.NotAssignedYet)
                throw new Exception("One of the hands passed to DoesThisHandBeatThatHand() has not been assigned a handtype yet!! Need to call EvaluateHandtype method first.");

            // If hand1 has a higher rank, we know right away that it beats hand2
            if ((int)ht_1 > (int)ht_2)
            {
                return 1;
            }
            // If hand2 has a higher rank, we know right away that it beats hand1
            if ((int)ht_2 > (int)ht_1)
            {
                return 0;
            }
            // hands should be of same handtype if we are executing code below here
            if ((int)ht_1 != (int)ht_2)
                throw new Exception("ht_1 != ht_2 in the DoesThisHandBeatThatHand() and that shouldn't happen");

            // if hands are of the same rank, we need to compare them card by card
            Hand sorted_hand_1 = hand_1.DoSort();
            Hand sorted_hand_2 = hand_2.DoSort();
            // COMPARING STRAIGHT FLUSHES
            if (ht_1 == Hand.HandType.StraightFlush)
            {
                // Check for low ace straights
                Card.Rank hand1_last_card = sorted_hand_1.cards[4].GetRank();
                Card.Rank hand2_last_card = sorted_hand_2.cards[4].GetRank();
                Card.Rank hand1_first_card = sorted_hand_1.cards[0].GetRank();
                Card.Rank hand2_first_card = sorted_hand_2.cards[0].GetRank();
                // two low ace straights tie
                if ((int)hand1_last_card == 14 && (int)hand1_first_card == 2 && ((int)hand2_last_card == 14 && (int)hand2_first_card == 2))
                    return -1;
                // first hand is low ace straight and other hand isn't so other hand must win 
                if ((int)hand1_last_card == 14 && (int)hand1_first_card == 2 && (int)hand2_first_card >= 2)
                    return 0;
                // second hand is low ace straight and other hand isn't so other hand must win 
                if ((int)hand2_last_card == 14 && (int)hand2_first_card == 2 && (int)hand1_first_card >= 2)
                    return 1;
                // Done checking for low ace straights

                int tied_cards = 0;
                for (int i = 4; i >= 0; i--) // progress from right to left
                {
                    Card.Rank hand1_card_rank = sorted_hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = sorted_hand_2.cards[i].GetRank();
                    if (hand1_card_rank > hand2_card_rank)
                        return 1;
                    if (hand2_card_rank > hand1_card_rank)
                        return 0;
                    if (hand1_card_rank == hand2_card_rank)
                        tied_cards++;
                }
                if (tied_cards == 5)
                    return -1;

            }
            // COMPARING FOUR OF A KINDS
            if (ht_1 == Hand.HandType.FourOfAKind)
            {

                // first check quads rank
                Card.Rank hand1_quads_rank = sorted_hand_1.cards[4].GetRank();
                Card.Rank hand2_quads_rank = sorted_hand_2.cards[4].GetRank();
                if (hand1_quads_rank > hand2_quads_rank)
                    return 1;
                if (hand2_quads_rank > hand1_quads_rank)
                    return 0;
                // second check kicker rank
                Card.Rank hand1_kicker_rank = sorted_hand_1.cards[0].GetRank();
                Card.Rank hand2_kicker_rank = sorted_hand_2.cards[0].GetRank();
                //Console.WriteLine("Comparing Kicker ranks of four of a kinds");
                if (hand1_kicker_rank > hand2_kicker_rank)
                    return 1;
                if (hand2_kicker_rank > hand1_kicker_rank)
                    return 0;
                // if quads ranks and kicker ranks are the same then they tie
                if ((hand1_quads_rank == hand2_quads_rank) && (hand1_kicker_rank == hand2_kicker_rank))
                {
                    return -1;
                }
            }
            // COMPARING FLUSHES
            if (ht_1 == Hand.HandType.Flush)
            {
                int tied_cards = 0;
                for (int i = 4; i >= 0; i--) // progress from right to left
                {
                    Card.Rank hand1_card_rank = sorted_hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = sorted_hand_2.cards[i].GetRank();
                    if (hand1_card_rank > hand2_card_rank)
                        return 1;
                    if (hand2_card_rank > hand1_card_rank)
                        return 0;
                    if (hand1_card_rank == hand2_card_rank)
                        tied_cards++;

                }
                if (tied_cards == 5)
                    return -1;

            }
            // COMPARING STRAIGHTS
            if (ht_1 == Hand.HandType.Straight)
            {
                // Check for low ace straights
                Card.Rank hand1_last_card = sorted_hand_1.cards[4].GetRank();
                Card.Rank hand2_last_card = sorted_hand_2.cards[4].GetRank();
                Card.Rank hand1_first_card = sorted_hand_1.cards[0].GetRank();
                Card.Rank hand2_first_card = sorted_hand_2.cards[0].GetRank();
                // two low ace straights tie
                if ((int)hand1_last_card == 14 && (int)hand1_first_card == 2 && ((int)hand2_last_card == 14 && (int)hand2_first_card == 2))
                    return -1;
                // first hand is low ace straight and other hand isn't so other hand must win 
                if ((int)hand1_last_card == 14 && (int)hand1_first_card == 2 && (int)hand2_first_card >= 2)
                    return 0;
                // second hand is low ace straight and other hand isn't so other hand must win 
                if ((int)hand2_last_card == 14 && (int)hand2_first_card == 2 && (int)hand1_first_card >= 2)
                    return 1;
                int tied_cards = 0;
                for (int i = 4; i >= 0; i--) // progress from right to left
                {
                    Card.Rank hand1_card_rank = sorted_hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = sorted_hand_2.cards[i].GetRank();

                    if (hand1_card_rank > hand2_card_rank)
                        return 1;
                    if (hand2_card_rank > hand1_card_rank)
                        return 0;
                    if (hand1_card_rank == hand2_card_rank)
                        tied_cards++;

                }
                if (tied_cards == 5)
                    return -1;

            }
            // COMPARING THREE OF A KINDS
            if (ht_1 == Hand.HandType.ThreeOfAKind)
            {
                // first check trips rank
                Card.Rank hand1_trips_rank = sorted_hand_1.cards[4].GetRank();
                Card.Rank hand2_trips_rank = sorted_hand_2.cards[4].GetRank();
                if (hand1_trips_rank > hand2_trips_rank)
                    return 1;
                if (hand2_trips_rank > hand1_trips_rank)
                    return 0;
                // second check - first kicker rank
                Card.Rank hand1_kicker_rank = sorted_hand_1.cards[1].GetRank();
                Card.Rank hand2_kicker_rank = sorted_hand_2.cards[1].GetRank();
                if (hand1_kicker_rank > hand2_kicker_rank)
                    return 1;
                if (hand2_kicker_rank > hand1_kicker_rank)
                    return 0;
                // third check - second kicker rank
                hand1_kicker_rank = sorted_hand_1.cards[0].GetRank();
                hand2_kicker_rank = sorted_hand_2.cards[0].GetRank();
                if (hand1_kicker_rank > hand2_kicker_rank)
                    return 1;
                if (hand2_kicker_rank > hand1_kicker_rank)
                    return 0;
                // check for tie
                if ((hand1_trips_rank == hand2_trips_rank) && (hand1_kicker_rank == hand2_kicker_rank))
                {
                    return -1;
                }
            }
            // COMPARING TWO PAIRS
            if (ht_1 == Hand.HandType.TwoPair)
            {
                // first check big pair rank
                Card.Rank big_pair_rank1 = sorted_hand_1.cards[4].GetRank();
                Card.Rank big_pair_rank2 = sorted_hand_2.cards[4].GetRank();
                if (big_pair_rank1 > big_pair_rank2)
                    return 1;
                if (big_pair_rank2 > big_pair_rank1)
                    return 0;
                // second check - small pair rank
                Card.Rank small_pair_rank1 = sorted_hand_1.cards[2].GetRank();
                Card.Rank small_pair_rank2 = sorted_hand_2.cards[2].GetRank();
                if (small_pair_rank1 > small_pair_rank2)
                    return 1;
                if (small_pair_rank2 > small_pair_rank1)
                    return 0;
                // third check - check kicker
                Card.Rank hand1_kicker_rank = sorted_hand_1.cards[0].GetRank();
                Card.Rank hand2_kicker_rank = sorted_hand_2.cards[0].GetRank();
                if (hand1_kicker_rank > hand2_kicker_rank)
                    return 1;
                if (hand2_kicker_rank > hand1_kicker_rank)
                    return 0;
                // if nothing has returned yet then its a tie
                return -1;

            }
            // COMPARING ONE PAIRS
            if (ht_1 == Hand.HandType.OnePair)
            {
                // first check big pair rank
                Card.Rank pair_rank1 = sorted_hand_1.cards[4].GetRank();
                Card.Rank pair_rank2 = sorted_hand_2.cards[4].GetRank();
                if (pair_rank1 > pair_rank2)
                    return 1;
                if (pair_rank2 > pair_rank1)
                    return 0;
                // second check - first kicker
                Card.Rank kicker_rank1 = sorted_hand_1.cards[2].GetRank();
                Card.Rank kicker_rank2 = sorted_hand_2.cards[2].GetRank();
                if (kicker_rank1 > kicker_rank2)
                    return 1;
                if (kicker_rank2 > kicker_rank1)
                    return 0;
                // third check - second kicker
                kicker_rank1 = sorted_hand_1.cards[1].GetRank();
                kicker_rank2 = sorted_hand_2.cards[1].GetRank();
                if (kicker_rank1 > kicker_rank2)
                    return 1;
                if (kicker_rank2 > kicker_rank1)
                    return 0;
                // fourth check - third kicker
                kicker_rank1 = sorted_hand_1.cards[0].GetRank();
                kicker_rank2 = sorted_hand_2.cards[0].GetRank();
                if (kicker_rank1 > kicker_rank2)
                    return 1;
                if (kicker_rank2 > kicker_rank1)
                    return 0;
                // if nothing has been returned yet then its a tie
                return -1;

            }
            //COMPARING FULL HOUSE
            if (ht_1 == Hand.HandType.FullHouse)
            {
                // first check trips rank
                Card.Rank hand1_trips_rank = sorted_hand_1.cards[4].GetRank();
                Card.Rank hand2_trips_rank = sorted_hand_2.cards[4].GetRank();
                if (hand1_trips_rank > hand2_trips_rank)
                    return 1;
                if (hand2_trips_rank > hand1_trips_rank)
                    return 0;
                // second check doublets rank
                Card.Rank hand1_doublet_rank = sorted_hand_1.cards[0].GetRank();
                Card.Rank hand2_doublet_rank = sorted_hand_2.cards[0].GetRank();
                if (hand1_doublet_rank > hand2_doublet_rank)
                    return 1;
                if (hand2_doublet_rank > hand1_doublet_rank)
                    return 0;
                // if nothing has returned yet, doublet and triple ranks are same
                return -1;
            }
            // COMPARING HIGH CARDS
            if (ht_1 == Hand.HandType.HighCard)
            {
                int tied_cards = 0;
                for (int i = 4; i >= 0; i--) // progress from right to left
                {
                    Card.Rank hand1_card_rank = sorted_hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = sorted_hand_2.cards[i].GetRank();

                    if (hand1_card_rank > hand2_card_rank)
                        return 1;
                    if (hand2_card_rank > hand1_card_rank)
                        return 0;
                    if (hand1_card_rank == hand2_card_rank)
                        tied_cards++;

                }
                if (tied_cards == 5)
                    return -1;

            }
            return ret_val;
        }
        public static List<int> Find_Best_Hand(List<Hand> lst_input_hands)
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
            //Console.WriteLine($"hand_count in FindBestHand ={hand_count}");
            List<Hand> lst_winning_hands = new List<Hand> { };

            for (int i = 0; i < hand_count; i++)
            {
                int loss_counter = 0;

                if (lst_hand_copy[i].GetHandType() == Hand.HandType.NotAssignedYet)
                    lst_hand_copy[i].EvaluateHandtype();

                for (int j = 0; j < hand_count; j++)
                {
                    if (lst_hand_copy[j].GetHandType() == Hand.HandType.NotAssignedYet)
                        lst_hand_copy[j].EvaluateHandtype();

                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        int comparison_result = Hand.DoesThisHandBeatThatHand(lst_hand_copy[i], lst_hand_copy[j]);
                        if (comparison_result == 0) // if i loses
                        {
                            loss_counter++;
                        }
                    }
                }
                if (loss_counter == 0) // if i-th hand always wins or ties then its a winner
                {
                    lst_winning_hands.Add(lst_hand_copy[i]);
                }
            }
            // Only thing left in lst_winning_hands should be tied winners or a single a winner
            //Console.WriteLine($"lst_winning_hands.Count = {lst_winning_hands.Count}");
            for (int i = 0; i < lst_winning_hands.Count; i++)
            {
                lst_winning_hand_indices.Add(lst_input_hands.IndexOf(lst_winning_hands[i]));
            }
            return lst_winning_hand_indices;
        }


        public Hand DoSort()
        {
            //put doubles triples and quads at end
            //each multiplet should be sorted by suit
            //separate quads, triples, doubles to new lists
            //don't forget the case of a low ace
            bool[] has_been_added = new bool[5];
            // init to false
            for (int k = 0; k < 5; k++)
                has_been_added[k] = false;

            bool has_pair_been_found = false;
            List<Card> lst_singles = new List<Card> { };
            List<Card> lst_doubles1 = new List<Card> { };
            List<Card> lst_doubles2 = new List<Card> { };
            List<Card> lst_triples = new List<Card> { };
            List<Card> lst_quads = new List<Card> { };
            List<Card> mylist = new List<Card> { };
            for (int i = 2; i < 15; i++)
            {
                //1) decompose hand into singles, doubles, triples, etc lists
                //2) sort singles by rank 
                //3) concatenate lists together in order
                //4) this way the hands can be compared to each other easily

                switch (rank_tally[i])
                {
                    case 4:
                        for (int k = 0; k < 5; k++)
                        {
                            Card.Rank cr = cards[k].GetRank();
                            if ((int)cr == i && has_been_added[k] == false)
                            {
                                Card.Suit cs = cards[k].GetSuit();
                                Card cc = new Card(cs, cr);
                                lst_quads.Add(cc);
                                has_been_added[k] = true;
                            }
                        }
                        break;

                    case 3:
                        for (int k = 0; k < 5; k++)
                        {
                            Card.Rank cr = cards[k].GetRank();
                            if ((int)cr == i && has_been_added[k] == false)
                            {
                                Card.Suit cs = cards[k].GetSuit();
                                Card cc = new Card(cs, cr);
                                lst_triples.Add(cc);
                                has_been_added[k] = true;
                            }
                        }
                        break;

                    case 2:
                        // TODO: REPLACE WITH METHOD CALL BC ITS BLOCK COPIED BELOW
                        if (has_pair_been_found == false)
                        {
                            has_pair_been_found = true;

                            for (int k = 0; k < 5; k++)
                            {
                                Card.Rank cr = cards[k].GetRank();

                                if ((int)cr == i && has_been_added[k] == false)
                                {
                                    Card.Suit cs = cards[k].GetSuit();
                                    Card cc = new Card(cs, cr);
                                    lst_doubles1.Add(cc);
                                    has_been_added[k] = true;
                                }
                            }

                        }
                        if (has_pair_been_found == true)
                        {
                            for (int k = 0; k < 5; k++)
                            {
                                Card.Rank cr = cards[k].GetRank();

                                if ((int)cr == i && has_been_added[k] == false)
                                {
                                    Card.Suit cs = cards[k].GetSuit();
                                    Card cc = new Card(cs, cr);
                                    lst_doubles2.Add(cc);
                                    has_been_added[k] = true;
                                }
                            }
                        }
                        break;

                    case 1:
                        for (int k = 0; k < 5; k++)
                        {
                            Card.Rank cr = cards[k].GetRank();

                            if ((int)cr == i && has_been_added[k] == false)
                            {
                                Card.Suit cs = cards[k].GetSuit();
                                Card cc = new Card(cs, cr);
                                lst_singles.Add(cc);
                                has_been_added[k] = true;
                            }
                        }

                        break;

                    default:
                        break;

                } // end of switch statement

            } // end of for loop over rank_tally. cards should be split up into separate lists

            for (int i = 0; i < lst_singles.Count; i++)
            {
                mylist.Add(lst_singles[i]);
            }
            // check which double is bigger so they are sorted low to high
            if (lst_doubles1.Count == 2 && lst_doubles2.Count == 2 && (lst_doubles1[0].GetRank() > lst_doubles2[0].GetRank()))
            {
                for (int i = 0; i < lst_doubles2.Count; i++)
                {
                    mylist.Add(lst_doubles2[i]);
                }
                for (int i = 0; i < lst_doubles1.Count; i++)
                {
                    mylist.Add(lst_doubles1[i]);
                }
            }
            else if (lst_doubles1.Count == 2 && lst_doubles2.Count == 2 && (lst_doubles1[0].GetRank() < lst_doubles2[0].GetRank()))
            {
                for (int i = 0; i < lst_doubles1.Count; i++)
                {
                    mylist.Add(lst_doubles1[i]);
                }
                for (int i = 0; i < lst_doubles2.Count; i++)
                {
                    mylist.Add(lst_doubles2[i]);
                }
            }
            else
            {
                for (int i = 0; i < lst_doubles1.Count; i++)
                {
                    mylist.Add(lst_doubles1[i]);
                }
            }
            for (int i = 0; i < lst_triples.Count; i++)
            {
                mylist.Add(lst_triples[i]);
            }
            for (int i = 0; i < lst_quads.Count; i++)
            {
                mylist.Add(lst_quads[i]);
            }
            //Console.WriteLine($"mylist.count = {mylist.Count}");
            Hand ret_hand = new Hand(mylist);
            Copy_Hand_Info(this, ref ret_hand);

            return ret_hand;
        }

        private void Copy_Hand_Info(Hand hand, ref Hand ret_hand)
        {
            ret_hand.SetHandtype(hand.GetHandType());
            for (int i = 0; i < hand.rank_tally.Length; i++)
            {
                ret_hand.rank_tally[i] = hand.rank_tally[i];
            }
            for (int i = 0; i < hand.suit_tally.Length; i++)
            {
                ret_hand.suit_tally[i] = hand.suit_tally[i];
            }
        }
        private void SetHandtype(Hand.HandType handtype)
        {
            this.hand_type = handtype;
        }
        public void EvaluateHandtype()
        {
            // This method sets the general Handtype of the hand.
            // Examples of handtypes are FourOfAKind, Straight, etc
            // At end set this.handtype =  the handtype
            // Evaluate from top type down, like check straight and flush.
            // Start by counting how many of each suit, and how many of each rank.

            const int HEART = 1;
            const int DIAMOND = 2;
            const int SPADE = 3;
            const int CLUB = 4;
            //int[] rank_tally = new int[15];  // let 0 and 1 indices be a waste to make code more clear. 
            //int[] suit_tally = new int[5];   // let 0 be waste, 1 = hearts, 2 = diamonds, 3 = spade, 4 = club
            // zero the counts and mark the unused indices in arrays 
            {
                for (int i = 1; i < 5; i++)
                    suit_tally[i] = 0;
                for (int i = 2; i < 15; i++)
                    rank_tally[i] = 0;
                // mark the unused indices
                suit_tally[0] = -1;
                rank_tally[0] = rank_tally[1] = -1;
            }
            // count the number of cards with each suit and rank
            foreach (var ci in this.cards)
            {
                // tally up the number of each suit
                if (ci.GetSuit() == Card.Suit.Heart)
                    suit_tally[HEART]++;
                else if (ci.GetSuit() == Card.Suit.Diamond)
                    suit_tally[DIAMOND]++;
                else if (ci.GetSuit() == Card.Suit.Spade)
                    suit_tally[SPADE]++;
                else if (ci.GetSuit() == Card.Suit.Club)
                    suit_tally[CLUB]++;
                // tally up the number of each rank
                int x = (int)ci.GetRank();
                rank_tally[x]++;
            }



            /* ALGORITHM TO IDENTIFY WHAT HAND WE HAVE
            * 
            * Start out by writing methods for flush and straight - done
            * 
            *  StraightFlush
            *  FourOfAKind
            *  FullHouse
            *  Flush
            *  Straight
            *  ThreeOfAKind
            *  TwoPair
            *  OnePair
            *  HighCard
            */
            bool straight_flag = false;
            bool flush_flag = false;
            straight_flag = this.IsThisAStraight(rank_tally);
            flush_flag = this.IsThisAFlush(suit_tally);

            // Check for straight flush
            if (straight_flag == true && flush_flag == true)
            {
                this.hand_type = Hand.HandType.StraightFlush;
                return;
            }
            // Check for Four of A Kind
            if (this.IsThisFourOfAKind(rank_tally))
            {
                this.hand_type = Hand.HandType.FourOfAKind;
                return;
            }
            // Check for Full House
            if (this.IsThisAFullHouse(rank_tally))
            {
                this.hand_type = Hand.HandType.FullHouse;
                return;
            }
            // Check for Flush using flag we set when we checked for straight flush
            if (flush_flag == true)
            {
                this.hand_type = Hand.HandType.Flush;
                return;
            }
            // Check for Straight using flag we set when we checked for straight flush
            if (straight_flag == true)
            {
                this.hand_type = Hand.HandType.Straight;
                return;
            }
            // Check for Three of a kind 
            if (this.IsThisThreeOfAKind(rank_tally))
            {
                this.hand_type = Hand.HandType.ThreeOfAKind;
                return;
            }
            // Check for Two Pair
            if (this.IsThisTwoPair(rank_tally))
            {
                this.hand_type = Hand.HandType.TwoPair;
                return;
            }
            // Check for One Pair
            if (this.IsThisOnePair(rank_tally))
            {
                this.hand_type = Hand.HandType.OnePair;
                return;
            }
            // Check for High Card
            // If the functions above didn't return from the method already, it must be high card
            this.hand_type = Hand.HandType.HighCard;
            return;
        }

    }
}


