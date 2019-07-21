using System;
using System.Collections.Generic;
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
namespace PokerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new Deck();
            d.BuildDeck();
            d.ShuffleDeck();

            // UN-COMMENT TO TEST STRAIGHT FLUSH
            //Card c1 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            //Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            //Card c4 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            //Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            // UN-COMMENT TO TEST STRAIGHT FLUSH WITH A LOW ACE
            //Card c1 = new Card(Card.Suit.Diamond, Card.Rank.ACE);
            //Card c2 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.THREE);
            //Card c4 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            //Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            // UN-COMMENT TO TEST FOUR OF A KIND
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.SIX);
            //Card c2 = new Card(Card.Suit.Heart, Card.Rank.SIX);
            //Card c3 = new Card(Card.Suit.Heart, Card.Rank.SEVEN);
            //Card c4 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            //Card c5 = new Card(Card.Suit.Spade, Card.Rank.SIX);
            // UN-COMMENT TO TEST FULL HOUSE
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            //Card c3 = new Card(Card.Suit.Heart, Card.Rank.JACK);
            //Card c4 = new Card(Card.Suit.Diamond, Card.Rank.JACK);
            //Card c5 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            // UN-COMMENT TO TEST FLUSH
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Club, Card.Rank.NINE);
            //Card c3 = new Card(Card.Suit.Club, Card.Rank.QUEEN);
            //Card c4 = new Card(Card.Suit.Club, Card.Rank.TEN);
            //Card c5 = new Card(Card.Suit.Club, Card.Rank.KING);
            // UN-COMMENT TO TEST STRAIGHT
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Club, Card.Rank.FIVE);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.SIX);
            //Card c4 = new Card(Card.Suit.Club, Card.Rank.SEVEN);
            //Card c5 = new Card(Card.Suit.Club, Card.Rank.EIGHT);
            // UN-COMMENT TO TEST THREE OF A KIND
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            //Card c4 = new Card(Card.Suit.Heart, Card.Rank.FOUR);
            //Card c5 = new Card(Card.Suit.Club, Card.Rank.EIGHT);
            // UN-COMMENT TO TEST FOR TWO PAIR
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            //Card c4 = new Card(Card.Suit.Spade, Card.Rank.FIVE);
            //Card c5 = new Card(Card.Suit.Club, Card.Rank.EIGHT);
            // UN-COMMENT TO TEST FOR ONE PAIR
            Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            Card c2 = new Card(Card.Suit.Spade, Card.Rank.FOUR);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            Card c4 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            Card c5 = new Card(Card.Suit.Club, Card.Rank.QUEEN);
            // UN-COMMENT TO TEST FOR HIGH CARD
            //Card c1 = new Card(Card.Suit.Club, Card.Rank.FOUR);
            //Card c2 = new Card(Card.Suit.Spade, Card.Rank.SEVEN);
            //Card c3 = new Card(Card.Suit.Diamond, Card.Rank.NINE);
            //Card c4 = new Card(Card.Suit.Spade, Card.Rank.JACK);
            //Card c5 = new Card(Card.Suit.Club, Card.Rank.TWO);
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.PrintHand();

            h.EvaluateHandtype();
            Console.WriteLine($"hand type = {h.GetHandType()}");
        }
    }
}
