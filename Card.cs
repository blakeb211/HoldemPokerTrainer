using System;
using System.Collections.Generic;
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
        public static void Reorder_Cards_Uniquely(ref List<Card> lst_input)
        {
            int MAX_COUNT = lst_input.Count;
            // reorder list of cards by rank and suit, modifying the input list
            List<Card> lst_output = new List<Card> { };
            foreach (Card.Rank r in Enum.GetValues(typeof(Card.Rank)))
            {
                if (lst_output.Count == MAX_COUNT)
                    break;
                foreach (Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
                {
                    for (int input_cards_index = 0; input_cards_index < lst_input.Count; input_cards_index++)
                    {
                        if (lst_input[input_cards_index].GetRank() == r && lst_input[input_cards_index].GetSuit() == s)
                        {
                            lst_output.Add(lst_input[input_cards_index]);
                        }
                    }
                }
            }
            lst_input = lst_output;
        }
    }
}
