using System;
using System.Diagnostics;

namespace PokerConsoleApp
{
    class CardMethods
    {
        public void Create_Deck(ref Card[] deck)
        {
            Debug.Assert(deck.Length == 52);
        }

        internal static Card RemoveCard(Card[] deck)
        {
            throw new NotImplementedException();
        }

        public int GetPrimeId()
        {
            switch ((int)this.GetRank())
            {
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return 5;
                case 5:
                    return 7;
                case 6:
                    return 11;
                case 7:
                    return 13;
                case 8:
                    return 17;
                case 9:
                    return 19;
                case 10:
                    return 23;
                case 11:
                    return 29;
                case 12:
                    return 31;
                case 13:
                    return 37;
                case 14:
                    return 41;
                default:
                    break;
            }
            return 0;
        }
        public static string Card_Rank_ToString(Card.Rank cr)
        {
            string str_ret = "";
            switch ((int)cr)
            {
                case 2:
                    str_ret = "2";
                    break;
                case 3:
                    str_ret = "3";
                    break;
                case 4:
                    str_ret = "4";
                    break;
                case 5:
                    str_ret = "5";
                    break;
                case 6:
                    str_ret = "6";
                    break;
                case 7:
                    str_ret = "7";
                    break;
                case 8:
                    str_ret = "8";
                    break;
                case 9:
                    str_ret = "9";
                    break;
                case 10:
                    str_ret = "10";
                    break;
                case 11:
                    str_ret = "J";
                    break;
                case 12:
                    str_ret = "Q";
                    break;
                case 13:
                    str_ret = "K";
                    break;
                case 14:
                    str_ret = "A";
                    break;
                default:
                    break;
            }
            return str_ret;
        }
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
            string rank = "";
            string suit = "";
            switch ((int)this.GetRank())
            {
                case 2:
                    rank = "2";
                    break;
                case (int)Card.Rank.THREE:
                    rank = "3";
                    break;
                case 4:
                    rank = "4";
                    break;
                case 5:
                    rank = "5";
                    break;
                case (int)Card.Rank.SIX:
                    rank = "6";
                    break;
                case 7:
                    rank = "7";
                    break;
                case 8:
                    rank = "8";
                    break;
                case 9:
                    rank = "9";
                    break;
                case 10:
                    rank = "10";
                    break;
                case 11:
                    rank = "J";
                    break;
                case 12:
                    rank = "Q";
                    break;
                case (int)Card.Rank.KING:
                    rank = "K";
                    break;
                case (int)Card.Rank.ACE:
                    rank = "A";
                    break;
            }
            switch ((int)this.GetSuit())
            {
                case (int)Card.Suit.Heart:
                    suit = "h";
                    break;
                case (int)Card.Suit.Diamond:
                    suit = "d";
                    break;
                case (int)Card.Suit.Spade:
                    suit = "s";
                    break;
                case (int)Card.Suit.Club:
                    suit = "c";
                    break;
            }
            string ret_string = rank + "-" + suit;
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
}
