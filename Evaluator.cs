using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    class Evaluator
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
        public Evaluator()
        {
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
    }
}
