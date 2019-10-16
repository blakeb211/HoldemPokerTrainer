using System;
using static PokerConsoleApp.CustomTypes;

namespace PokerConsoleApp
{
    class HandMethods
    {
        public int GetPrimeRank()
        {
            int ret_val = 1;
            // if prime_rank hasn't been calculated, we need to calculate it
            if (this.prime_rank == -1)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    ret_val *= this.cards[i].GetPrimeId();
                }
                prime_rank = ret_val;
                return ret_val;
            }
            else
            {
                // if prime_rank has already been calculated, just return it
                return prime_rank;
            }
        }

        public void AddCard(Card c)
        {
            if (cards.Count == 5)
                throw new Exception("Can't add a card to a hand that already has 5 cards!");
            cards.Add(c);
        }
        public void AddCard(Card.Suit cs, Card.Rank cr)
        {
            Card c = new Card(cs, cr);
            this.cards.Add(c);

        }

        public void Sort(ref Hand h)
        {
           

        }


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

        // Methods to evaluate the hand that a player has
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
            straight_flag = this.IsStraight(rank_tally);
            flush_flag = this.IsFlush(suit_tally);

            // Check for straight flush
            if (straight_flag == true && flush_flag == true)
            {
                this.hand_type = Hand.HandType.StraightFlush;
                return;
            }
            // Check for Four of A Kind
            if (this.IsFourOfAKind(rank_tally))
            {
                this.hand_type = Hand.HandType.FourOfAKind;
                return;
            }
            // Check for Full House
            if (this.IsFullHouse(rank_tally))
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
            if (this.IsThreeOfAKind(rank_tally))
            {
                this.hand_type = Hand.HandType.ThreeOfAKind;
                return;
            }
            // Check for Two Pair
            if (this.IsTwoPair(rank_tally))
            {
                this.hand_type = Hand.HandType.TwoPair;
                return;
            }
            // Check for One Pair
            if (this.IsOnePair(rank_tally))
            {
                this.hand_type = Hand.HandType.OnePair;
                return;
            }
            // Check for High Card
            // If the functions above didn't return from the method already, it must be high card
            this.hand_type = Hand.HandType.HighCard;
            return;
        }
        public HandType GetHandType()
        {
            return this.hand_type;
        }

        public bool IsFlush(int[] suit_tally)
        {
            if (suit_tally.Length != 5)
                throw new Exception("The array passed to IsFlush doesn't have 5 elements!");

            bool flush_flag = false;
            // check for 5 cards of a single suit
            for (int i = 1; i < 5; i++)
            {
                if (suit_tally[i] == 5)
                    flush_flag = true;
            }

            return flush_flag;
        }


        // Methods to compare hands and determine there strength
        public static int CompareTo(Hand hand_1, Hand hand_2)
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
                throw new Exception("One of the hands passed to CompareTo() has not been assigned a handtype yet!! Need to call EvaluateHandtype method first.");

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
                throw new Exception("ht_1 != ht_2 in the CompareTo() and that shouldn't happen");

            // if hands are of the same rank, we need to compare them card by card
            if (ht_1 != HandType.OnePair && hand_1.IsSorted() == false)
            {
                hand_1.Sort();
            }
            if (ht_2 != HandType.OnePair && hand_2.IsSorted() == false)
            {
                hand_2.Sort();
            }
            // COMPARING STRAIGHT FLUSHES
            if (ht_1 == Hand.HandType.StraightFlush)
            {
                // Check for low ace straights
                Card.Rank hand1_last_card = hand_1.cards[4].GetRank();
                Card.Rank hand2_last_card = hand_2.cards[4].GetRank();
                Card.Rank hand1_first_card = hand_1.cards[0].GetRank();
                Card.Rank hand2_first_card = hand_2.cards[0].GetRank();
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
                    Card.Rank hand1_card_rank = hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = hand_2.cards[i].GetRank();
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
                Card.Rank hand1_quads_rank = hand_1.cards[4].GetRank();
                Card.Rank hand2_quads_rank = hand_2.cards[4].GetRank();
                if (hand1_quads_rank > hand2_quads_rank)
                    return 1;
                if (hand2_quads_rank > hand1_quads_rank)
                    return 0;
                // second check kicker rank
                Card.Rank hand1_kicker_rank = hand_1.cards[0].GetRank();
                Card.Rank hand2_kicker_rank = hand_2.cards[0].GetRank();
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
                    Card.Rank hand1_card_rank = hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = hand_2.cards[i].GetRank();
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
                Card.Rank hand1_last_card = hand_1.cards[4].GetRank();
                Card.Rank hand2_last_card = hand_2.cards[4].GetRank();
                Card.Rank hand1_first_card = hand_1.cards[0].GetRank();
                Card.Rank hand2_first_card = hand_2.cards[0].GetRank();
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
                    Card.Rank hand1_card_rank = hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = hand_2.cards[i].GetRank();

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
                Card.Rank hand1_trips_rank = hand_1.cards[4].GetRank();
                Card.Rank hand2_trips_rank = hand_2.cards[4].GetRank();
                if (hand1_trips_rank > hand2_trips_rank)
                    return 1;
                if (hand2_trips_rank > hand1_trips_rank)
                    return 0;
                // second check - first kicker rank
                Card.Rank hand1_kicker_rank = hand_1.cards[1].GetRank();
                Card.Rank hand2_kicker_rank = hand_2.cards[1].GetRank();
                if (hand1_kicker_rank > hand2_kicker_rank)
                    return 1;
                if (hand2_kicker_rank > hand1_kicker_rank)
                    return 0;
                // third check - second kicker rank
                hand1_kicker_rank = hand_1.cards[0].GetRank();
                hand2_kicker_rank = hand_2.cards[0].GetRank();
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
                Card.Rank big_pair_rank1 = hand_1.cards[4].GetRank();
                Card.Rank big_pair_rank2 = hand_2.cards[4].GetRank();
                if (big_pair_rank1 > big_pair_rank2)
                    return 1;
                if (big_pair_rank2 > big_pair_rank1)
                    return 0;
                // second check - small pair rank
                Card.Rank small_pair_rank1 = hand_1.cards[2].GetRank();
                Card.Rank small_pair_rank2 = hand_2.cards[2].GetRank();
                if (small_pair_rank1 > small_pair_rank2)
                    return 1;
                if (small_pair_rank2 > small_pair_rank1)
                    return 0;
                // third check - check kicker
                Card.Rank hand1_kicker_rank = hand_1.cards[0].GetRank();
                Card.Rank hand2_kicker_rank = hand_2.cards[0].GetRank();
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
                //// first check big pair rank
                //Card.Rank pair_rank1 = hand_1.cards[4].GetRank();
                //Card.Rank pair_rank2 = hand_2.cards[4].GetRank();
                //if (pair_rank1 > pair_rank2)
                //    return 1;
                //if (pair_rank2 > pair_rank1)
                //    return 0;
                //// second check - first kicker
                //Card.Rank kicker_rank1 = hand_1.cards[2].GetRank(); 
                //Card.Rank kicker_rank2 = hand_2.cards[2].GetRank();
                //if (kicker_rank1 > kicker_rank2)
                //    return 1;
                //if (kicker_rank2 > kicker_rank1)
                //    return 0;
                //// third check - second kicker
                //kicker_rank1 = hand_1.cards[1].GetRank();
                //kicker_rank2 = hand_2.cards[1].GetRank();
                //if (kicker_rank1 > kicker_rank2)
                //    return 1;
                //if (kicker_rank2 > kicker_rank1)
                //    return 0;
                //// fourth check - third kicker
                //kicker_rank1 = hand_1.cards[0].GetRank();
                //kicker_rank2 = hand_2.cards[0].GetRank();
                //if (kicker_rank1 > kicker_rank2)
                //    return 1;
                //if (kicker_rank2 > kicker_rank1)
                //    return 0;
                //// if nothing has been returned yet then its a tie
                //return -1;
                int hand1_pair_rank = Program.pairRankDict[hand_1.GetPrimeRank()];
                int hand2_pair_rank = Program.pairRankDict[hand_2.GetPrimeRank()];
                if (hand1_pair_rank == hand2_pair_rank)
                    return -1;
                else if (hand1_pair_rank < hand2_pair_rank)
                    return 1;
                else if (hand1_pair_rank > hand2_pair_rank)
                    return 0;
            }
            //COMPARING FULL HOUSE
            if (ht_1 == Hand.HandType.FullHouse)
            {
                // first check trips rank
                Card.Rank hand1_trips_rank = hand_1.cards[4].GetRank();
                Card.Rank hand2_trips_rank = hand_2.cards[4].GetRank();
                if (hand1_trips_rank > hand2_trips_rank)
                    return 1;
                if (hand2_trips_rank > hand1_trips_rank)
                    return 0;
                // second check doublets rank
                Card.Rank hand1_doublet_rank = hand_1.cards[0].GetRank();
                Card.Rank hand2_doublet_rank = hand_2.cards[0].GetRank();
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
                    Card.Rank hand1_card_rank = hand_1.cards[i].GetRank();
                    Card.Rank hand2_card_rank = hand_2.cards[i].GetRank();

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
                    lst_hand_copy[i].EvaluateHandtype();

                for (int j = 0; j < hand_count; j++)
                {
                    if (lst_hand_copy[j].GetHandType() == Hand.HandType.NotAssignedYet)
                        lst_hand_copy[j].EvaluateHandtype();
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
