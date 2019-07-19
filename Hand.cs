using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    class Hand
    {
        private List<Card> cards = new List<Card> { };
        public Hand(List<Card> c)
        {
            if (c.Count != 5)
                throw new Exception("Something other than a 5-Card List was passed to the Hand() constructor");
            cards.Capacity = 5;
            foreach (var ci in c)
                cards.Add(ci);
        }
        public void addCard(Card c)
        {
            if (cards.Count == 5)
                throw new Exception("Can't add a card to a hand that already has 5 cards!");
            cards.Add(c);
        }
        public void printHand()
        {
            foreach (var c in this.cards)
            {
                Console.Write($"{c.getRank()}-{c.getSuit()} ");
            }
            Console.Write("\n");
        }
    }
}
