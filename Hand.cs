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
        private List<Card> cards = new List<Card> { };
        private HandType hand_type = new HandType();
        public Hand(List<Card> c)
        {
            if (c.Count != 5)
                throw new Exception("Something other than a 5-Card List was passed to the Hand() constructor");
            cards.Capacity = 5;
            this.hand_type = HandType.NotAssignedYet;
            foreach (var ci in c)
                cards.Add(ci);

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
        private bool IsThisAStraight(int[] rankcount)
        {
            if (rankcount.Length != 15)
                throw new Exception("The array passed to IsThisAStraight doesn't have 15 elements!");
            // count single cards that are in order
            int cards_in_a_row_counter = 0;
            int last_card = -2;
            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 1)
                {
                    if (last_card == -2 || (i - last_card) == 1)
                    {
                        cards_in_a_row_counter++;
                        last_card = i;
                    }
                }
            }
            // check for Ace, Two, Three, Four, Five straight
            if (rankcount[(int)Card.Rank.ACE] == 1 && rankcount[(int)Card.Rank.TWO] == 1 && rankcount[(int)Card.Rank.THREE] == 1 && rankcount[(int)Card.Rank.FOUR] == 1 && rankcount[(int)Card.Rank.FIVE] == 1)
                cards_in_a_row_counter = 5;

            if (cards_in_a_row_counter == 5)
                return true;
            else
                return false;
        }
        private bool IsThisAFlush(int[] suitcount)
        {
            if (suitcount.Length != 5)
                throw new Exception("The array passed to IsThisAFlush doesn't have 5 elements!");

            bool flush_flag = false;
            // check for 5 cards of a single suit
            for (int i = 1; i < 5; i++)
            {
                if (suitcount[i] == 5)
                    flush_flag = true;
            }

            return flush_flag;
        }
        private bool IsThisFourOfAKind(int[] rankcount)
        {
            bool ret_flag = false;
            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 4)
                    ret_flag = true;
            }
            return ret_flag;
        }
        private bool IsThisAFullHouse(int[] rankcount)
        {
            bool ret_flag = false;
            bool has_a_set_of_three = false;
            bool has_a_pair = false;
            int pair_rank = -1; // store the rank of the pair found
            int set_of_three_rank = -1; // store the rank of the three of a kind found

            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 3)
                {
                    set_of_three_rank = i;
                    has_a_set_of_three = true;
                }
                if (rankcount[i] == 2)
                {
                    pair_rank = i;
                    has_a_pair = true;
                }
            }
            if (has_a_pair && has_a_set_of_three)
                ret_flag = true;

            return ret_flag;
        }
        private bool IsThisThreeOfAKind(int[] rankcount)
        {
            // note that this method would return false positive on a FourOfAKind hand or FullHouse 
            // if run by itself outside of the evaluate hand method
            bool ret_flag = false;
            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 3)
                    ret_flag = true;
            }
            return ret_flag;
        }
        private bool IsThisTwoPair(int[] rankcount)
        {
            bool ret_flag = false;
            int rank_of_first_pair = -1;
            int rank_of_second_pair = -1;
            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 2 && rank_of_first_pair == -1)
                {
                    rank_of_first_pair = i;
                    continue;
                }
                if (rankcount[i] == 2 && rank_of_first_pair != -1)
                {
                    rank_of_second_pair = i;
                    continue;
                }
            }
            if (rank_of_first_pair != -1 && rank_of_second_pair != -1 && rank_of_first_pair != rank_of_second_pair)
                ret_flag = true;
            return ret_flag;
        }

        private bool IsThisOnePair(int[] rankcount)
        {
            bool ret_flag = false;
            int rank_of_pair = -1;
            for (int i = 2; i < 15; i++)
            {
                if (rankcount[i] == 2 && rank_of_pair == -1)
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
            if (hand_1.hand_type == Hand.HandType.NotAssignedYet || hand_2.hand_type == Hand.HandType.NotAssignedYet)
                throw new Exception("One of the hands passed to DoesThisHandBeatThatHand() has not been assigned a handtype yet!! Need to call EvaluateHandtype method first.");

            // If this hand has a higher rank, we know right away that it beats the hand_to_compare_to
            if ((int)hand_1.GetHandType() > (int)hand_2.GetHandType())
            {
                ret_val = 1;
                return ret_val;
            }
            // if hands are of the same rank, we need to compare them card by card

            return ret_val;
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
            int[] rankcount = new int[15];  // let 0 and 1 indices be a waste to make code more clear. 
            int[] suitcount = new int[5];   // let 0 be waste, 1 = hearts, 2 = diamonds, 3 = spade, 4 = club
            // zero the counts and mark the unused indices in arrays 
            {
                for (int i = 1; i < 5; i++)
                    suitcount[i] = 0;
                for (int i = 2; i < 15; i++)
                    rankcount[i] = 0;
                // mark the unused indices
                suitcount[0] = -1;
                rankcount[0] = rankcount[1] = -1;
            }
            // count the number of cards with each suit and rank
            foreach (var ci in this.cards)
            {
                // tally up the number of each suit
                if (ci.GetSuit() == Card.Suit.Heart)
                    suitcount[HEART]++;
                else if (ci.GetSuit() == Card.Suit.Diamond)
                    suitcount[DIAMOND]++;
                else if (ci.GetSuit() == Card.Suit.Spade)
                    suitcount[SPADE]++;
                else if (ci.GetSuit() == Card.Suit.Club)
                    suitcount[CLUB]++;
                // tally up the number of each rank
                int x = (int)ci.GetRank();
                rankcount[x]++;
            }

            // print out number of each suit
            Console.WriteLine($"\nHearts: {suitcount[HEART]}, Diamonds: {suitcount[DIAMOND]}, Spades: {suitcount[SPADE]}, Clubs: {suitcount[CLUB]}\n");
            // print out number of each rank
            for (int r_i = 2; r_i < 15; r_i++)
            {
                if (rankcount[r_i] != 0) // only print ranks with non-zero counts
                {
                    switch (r_i)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            Console.Write($" # of {r_i} cards: {rankcount[r_i]} ,");
                            break;
                        case 11:
                            Console.Write($" # of Jack cards: {rankcount[r_i]} ,");
                            break;
                        case 12:
                            Console.Write($" # of Queen cards: {rankcount[r_i]} ,");
                            break;
                        case 13:
                            Console.Write($" # of King cards: {rankcount[r_i]} ,");
                            break;
                        case 14:
                            Console.Write($" # of Ace cards: {rankcount[r_i]} ,");
                            break;
                        default:
                            break;
                    }
                }

            }
            Console.WriteLine("");
            Console.WriteLine("");
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
            straight_flag = this.IsThisAStraight(rankcount);
            flush_flag = this.IsThisAFlush(suitcount);

            // Check for straight flush
            if (straight_flag == true && flush_flag == true)
            {
                this.hand_type = Hand.HandType.StraightFlush;
                return;
            }
            // Check for Four of A Kind
            if (this.IsThisFourOfAKind(rankcount))
            {
                this.hand_type = Hand.HandType.FourOfAKind;
                return;
            }
            // Check for Full House
            if (this.IsThisAFullHouse(rankcount))
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
            if (this.IsThisThreeOfAKind(rankcount))
            {
                this.hand_type = Hand.HandType.ThreeOfAKind;
                return;
            }
            // Check for Two Pair
            if (this.IsThisTwoPair(rankcount))
            {
                this.hand_type = Hand.HandType.TwoPair;
                return;
            }
            // Check for One Pair
            if (this.IsThisOnePair(rankcount))
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


