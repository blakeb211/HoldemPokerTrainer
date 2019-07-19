using System;
using System.Collections.Generic;
/*
 * Purpose: this program uses monte carlo simulation to calculate
 *          a) the odds of winning a game texas holdem for a given set of hole cards.
 *                   
 * Input:                number of players
 * Output:               odds of winning and tieing for each set of hole cards
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
