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
        public void addCard(Card c)
        {
            deck.Add(c);
            this.cardCount++;
        }
        public Card removeCard()
        {
            Card c = new Card();
            c.setRank(this.deck[0].getRank());
            c.setSuit(this.deck[0].getSuit());
            this.deck.RemoveAt(0);
            this.cardCount--;
            return c;
        }
        public int getCount()
        {
            return this.cardCount;
        }
        public void printDeck()
        {
            int count = 0;
            foreach(var c in deck)
            {
                ++count;
                String message = $"{c.getRank()} of {c.getSuit()}";
                Console.Write($"{c.getRank()} of {c.getSuit()}");
                // add spaces after printing card
                for (int i = 0; i < 20 - message.Length; i++)
                    Console.Write(" ");
                if (count % 4 == 0)
                    Console.Write("\n");
            }
            Console.Write("\n");
        }

        public void shuffleDeck()
        {
            Card temp = new Card();
            for (int i = 0; i < 5000; i++)
            {
                // shuffle deck by generating random numbers and exchanging the cards
                Random random_number1 = new Random();
                Random random_number2 = new Random();
                int rand1 = random_number1.Next(0, 52);
                int rand2 = random_number2.Next(0, 52);
                temp.setRank(this.deck[rand1].getRank());
                temp.setSuit(this.deck[rand1].getSuit());
                this.deck[rand1].setRank(this.deck[rand2].getRank());
                this.deck[rand1].setSuit(this.deck[rand2].getSuit());
                this.deck[rand2].setRank(temp.getRank());
                this.deck[rand2].setSuit(temp.getSuit());
            }
        }
        public void buildDeck()
        {
            foreach(Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.Rank r in Enum.GetValues(typeof(Card.Rank)))
                {
                    Card c = new Card();
                    c.setRank(r);
                    c.setSuit(s);
                    this.addCard(c);
                }
            }
            
        }
    }
}
