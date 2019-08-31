using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Deck
    {
        private List<Card> deck;
        private int cardCount;
        public Deck()
        {
            this.cardCount = 0;
            this.deck = new List<Card> { };
            this.deck.Capacity = 52;
            this.BuildDeck();
        }
        public void AddCard(Card c)
        {
            deck.Add(c);
            this.cardCount++;
        }
        public Card RemoveCard()
        {
            Card c = new Card();
            c.SetRank(this.deck[0].GetRank());
            c.SetSuit(this.deck[0].GetSuit());
            this.deck.RemoveAt(0);
            this.cardCount--;
            return c;
        }

        public int GetCount()
        {
            return this.cardCount;
        }

        public void Shuffle()
        {
            this.deck = Utility_Methods.ShuffleList(this.deck);
        }
        public void BuildDeck()
        {
            foreach (Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.Rank r in Enum.GetValues(typeof(Card.Rank)))
                {
                    Card c = new Card();
                    c.SetRank(r);
                    c.SetSuit(s);
                    this.AddCard(c);
                }
            }

        }
    }
}
