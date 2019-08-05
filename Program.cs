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
            // TEST FIVE HANDS, with the winning hands being two hands that tie so have to check for returning either one as the right answer

            // first hand is flush with Jack High
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SEVEN);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Hand h1 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // second hand is flush with Straight Flush Queen High
            c1 = new Card(Card.Suit.Heart, Card.Rank.QUEEN);
            c2 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            c3 = new Card(Card.Suit.Heart, Card.Rank.TEN);
            c4 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.EIGHT);
            Hand h2 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // third hand is four of a kind, 9s with a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h3 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fourth hand is four of a kind, 9s with a four kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.NINE);
            c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.NINE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            Hand h4 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });

            // fifth hand is a three of a kind, threes with a five and a two kicker
            c1 = new Card(Card.Suit.Heart, Card.Rank.THREE);
            c2 = new Card(Card.Suit.Club, Card.Rank.THREE);
            c3 = new Card(Card.Suit.Spade, Card.Rank.THREE);
            c4 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            c5 = new Card(Card.Suit.Heart, Card.Rank.TWO);
            Hand h5 = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h1.EvaluateHandtype();
            h2.EvaluateHandtype();
            h3.EvaluateHandtype();
            h4.EvaluateHandtype();
            h5.EvaluateHandtype();
            // hand2 is straight flush with queen high so it wins
            int result = Hand.Find_Best_Hand(new List<Hand> { h3, h4 });
            // TODO WHEN I COMPARE HAND3 TO HAND4 I get an error

            Console.WriteLine($"Result = {result}");
        }
    }
    
}
