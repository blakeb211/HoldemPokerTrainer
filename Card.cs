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
