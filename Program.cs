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
