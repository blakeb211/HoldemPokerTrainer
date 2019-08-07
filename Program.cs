/*
 * Purpose: This program draws a random hand from the deck and displays the handtype. 
 *          Ultimately, it will use monte carlo to calculate the odds of winning for different hole cards. 
 * Input:   None. Player number is fixed at 4.
 * Output:  Print four hands, their hand types, and who wins           
 *
 *Todo: Add evaluator method
 *             Identify and print out correct hand type, e.g. four 5s and a queen kicker, full house, 3s over 2s. 
 *             
 *      Add testing 
 * 
 */
using System.Collections.Generic;
using System;
namespace PokerConsoleApp
{
    class Program
    {
        static void Main()
        {
            Test5();
        }
        static List<Hand> Build_List_21_Hands(Card hole1, Card hole2, Card c1, Card c2, Card c3, Card c4, Card c5)
        {
            // hole1, hole2 = hole cards
            // c1, c2, c3 = flop cards
            // c4, c5 = turn and river cards
            List<Hand> ret_list = new List<Hand> { };
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
            for(int i = 0; i < 21; i++)
                ret_list.Add(h[i]);

            return ret_list;
        }
        static int Simulate_Game_and_Save_to_DB()
        {
            return 0;
        }
        static int Play_Game_Showing_Statistics()
        {
            return 0;
        }
        static void Test5()
        {
            // Compare two of a kind differing in high pair
            Card c1 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            Card c3 = new Card(Card.Suit.Club, Card.Rank.QUEEN);
            Card c4 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            Card c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            c1 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            c2 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            c3 = new Card(Card.Suit.Club, Card.Rank.JACK);
            c4 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c5 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            int ret_val = Hand.DoesThisHandBeatThatHand(h1, h2);
            Console.WriteLine($"return valule of DoesThisHandBeatThatHand(): {ret_val}");
        }
    }
    
}
