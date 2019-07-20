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
            Card c1 = new Card(Card.Suit.Diamond, Card.Rank.ACE);
            Card c2 = new Card(Card.Suit.Diamond, Card.Rank.TWO);
            Card c3 = new Card(Card.Suit.Diamond, Card.Rank.QUEEN);
            Card c4 = new Card(Card.Suit.Diamond, Card.Rank.FOUR);
            Card c5 = new Card(Card.Suit.Diamond, Card.Rank.FIVE);
            
            
           
            //Card c2 = d.RemoveCard();

            //Card c3 = d.RemoveCard();
            //Card c4 = d.RemoveCard();
            //Card c5 = d.RemoveCard();
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.PrintHand();
            h.EvaluateHandtype();
        }
    }
}
