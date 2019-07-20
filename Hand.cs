using System;
using System.Collections.Generic;
using System.Text;

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
    class Hand
    {
        public enum handrank
        {
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


