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
        public bool isRoyalFlush;
        public bool isStraightFlush;
        public bool isFourOfAKind;
        public bool isFullHouse;
        public bool isFlush;
        public bool isStraight;
        public bool isThreeOfAKind;
        public bool isTwoPair;
        public bool isOnePair;
        public bool isHighCard;
        private List<Card> cards = new List<Card> { };
        public Hand(List<Card> c)
        {
            if (c.Count != 5)
                throw new Exception("Something other than a 5-Card List was passed to the Hand() constructor");
            cards.Capacity = 5;
            foreach (var ci in c)
                cards.Add(ci);

            // reset flags
            isRoyalFlush = false;
            isStraightFlush = false;
            isFourOfAKind = false;
            isFullHouse = false;
            isFlush = false;
            isStraight = false;
            isThreeOfAKind = false;
            isTwoPair = false;
            isOnePair = false;
            isHighCard = false;
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
