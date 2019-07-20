using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    class Card
    {
        public enum Suit { Heart = 1, Diamond = 2, Spade = 3, Club = 4 };
        public enum Rank { TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT,
            NINE, TEN, JACK, QUEEN, KING, ACE };
        private Suit suit;
        private Rank rank;
        public void setRank(Rank _rank)
        {
            this.rank = _rank;
        }
        public Rank getRank()
        {
            return this.rank;
        }
        public void setSuit(Suit _suit)
        {
            this.suit = _suit;
        }
        public Suit getSuit()
        {
            return this.suit;
        }
    }
}
