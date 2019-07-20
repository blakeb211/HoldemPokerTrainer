using System;
using System.Collections.Generic;
/*
 * Purpose: This program draws a random hand from the deck and displays the handtype. 
 *          Ultimately, it will use monte carlo to calculate the odds of winning for different hole cards. 
 * Input:   None. Player number is fixed at 4.
 * Output:  Hand types, odds of winning for each different set of hole cards.             
 *
 *Todo: Add evaluator method
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
            d.buildDeck();
            d.shuffleDeck();
            d.printDeck();
            Card c1 = d.removeCard();
            Card c2 = d.removeCard();
            Card c3 = d.removeCard();
            Card c4 = d.removeCard();
            Card c5 = d.removeCard();
            Hand h = new Hand(new List<Card> { c1, c2, c3, c4, c5 });
            h.printHand();
        }
    }
}
