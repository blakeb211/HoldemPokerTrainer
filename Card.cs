namespace PokerConsoleApp
{
    public class Card
    {
        public enum Suit { Heart = 1, Diamond = 2, Spade = 3, Club = 4 };
        public enum Rank
        {
            TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT,
            NINE, TEN, JACK, QUEEN, KING, ACE
        };
        private Suit suit;
        private Rank rank;
        public Card(Card.Suit cs, Card.Rank cr)
        {
            this.suit = cs;
            this.rank = cr;
        }
        public Card()
        {

        }
        public void SetRank(Rank _rank)
        {
            this.rank = _rank;
        }
        public override string ToString()
        {
            string ret_string = this.GetRank() + "-" + this.GetSuit();
            return ret_string;
        }
        public Rank GetRank()
        {
            return this.rank;
        }
        public void SetSuit(Suit _suit)
        {
            this.suit = _suit;
        }
        public Suit GetSuit()
        {
            return this.suit;
        }
    }
}
