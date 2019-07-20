using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    class Deck
    {
        private List<Card> deck;
        private int cardCount;
        public Deck()
        {
            this.cardCount = 0;
            this.deck = new List<Card> { };
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
        public void PrintDeck()
        {
            int count = 0;
            foreach(var c in deck)
            {
                ++count;
                String message = $"{c.GetRank()} of {c.GetSuit()}";
                Console.Write($"{c.GetRank()} of {c.GetSuit()}");
                // add spaces after printing card
                for (int i = 0; i < 20 - message.Length; i++)
                    Console.Write(" ");
                if (count % 4 == 0)
                    Console.Write("\n");
            }
            Console.Write("\n");
        }

        public void ShuffleDeck()
        {
            Card temp = new Card();
            for (int i = 0; i < 5000; i++)
            {
                // shuffle deck by generating random numbers and exchanging the cards
                Random random_number1 = new Random();
                Random random_number2 = new Random();
                int rand1 = random_number1.Next(0, 52);
                int rand2 = random_number2.Next(0, 52);
                temp.SetRank(this.deck[rand1].GetRank());
                temp.SetSuit(this.deck[rand1].GetSuit());
                this.deck[rand1].SetRank(this.deck[rand2].GetRank());
                this.deck[rand1].SetSuit(this.deck[rand2].GetSuit());
                this.deck[rand2].SetRank(temp.GetRank());
                this.deck[rand2].SetSuit(temp.GetSuit());
            }
        }
        public void BuildDeck()
        {
            foreach(Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
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
