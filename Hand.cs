using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static PokerLib.Lookups;

namespace PokerConsoleApp
{
    public partial class Hand
    {
        public List<Card> Cards { get; private set; }
        public int PrimeId { get; private set; }
        public int Rank { get; private set; }
        public string Name { get; private set; }
        public List<HandType> Outs { get; private set; }

        public Hand(string s)
        {
            while (s.Contains('-'))
            {
                s = s.Remove(s.IndexOf('-'), 1);
            }
            Debug.Assert(s.Length == 10);
            Cards = new List<Card>(5);

            for (int i = 0; i <= s.Length - 2; i += 2)
            {
                int rNum = 0;
                switch (s[i])
                {
                    case '2':
                        rNum = 2;
                        break;
                    case '3':
                        rNum = 3;
                        break;
                    case '4':
                        rNum = 4;
                        break;
                    case '5':
                        rNum = 5;
                        break;
                    case '6':
                        rNum = 6;
                        break;
                    case '7':
                        rNum = 7;
                        break;
                    case '8':
                        rNum = 8;
                        break;
                    case '9':
                        rNum = 9;
                        break;
                    case 'T':
                        rNum = 10;
                        break;
                    case 'J':
                        rNum = 11;
                        break;
                    case 'Q':
                        rNum = 12;
                        break;
                    case 'K':
                        rNum = 13;
                        break;
                    case 'A':
                        rNum = 14;
                        break;
                }

                _ = Char.TryParse(s[i + 1].ToString(), out char sChar);
                Cards.Add(new Card((RankType)rNum, Card.CharToSuit(sChar)));
                Debug.Assert(2 <= rNum && rNum <= 14);
            }

        }
        public Hand()
        {
            Cards = new List<Card>(5);
        }
        public Hand(List<Card> cards)
        {
            this.Cards = cards;
        }
        public void AssignRankAndName()
        {
            // get primeId used for hand rank lookup
            int _primeId;
            if (Rank != default)
            {
                return;
            }
            else
            {
                int _temp = 1;
                for (int i = 0; i < Cards.Count; i++)
                {
                    _temp *= Cards[i].GetPrimeIdForRankingHand();
                }
                _primeId = _temp;
                Console.WriteLine("Prime ID is : {0}", _primeId);
            }

            // look up and assign hand rank
            if (CheckFlush())
            {
                Console.WriteLine("Is a flush!");
                Rank = FlushDict[_primeId];
                return;
            }
            Console.WriteLine("Is not a flush!");
            Rank = NonFlushDict[_primeId];

            Name = RankToNameDict[Rank];
        }

        private bool CheckFlush()
        {
            if (Cards[0].Suit == Cards[1].Suit && Cards[1].Suit == Cards[2].Suit
                 && Cards[2].Suit == Cards[3].Suit && Cards[3].Suit == Cards[4].Suit)
            {
                return true;
            }
            return false;
        }

        public void Sort()
        {
            this.Cards = Cards.OrderByDescending(cards => cards.Rank)
            .ThenByDescending(cards => cards.Suit)
            .ToList();
        }
        public override string ToString()
        {
            string ret_string = "";
            for (int i = 0; i < 5; i++)
            {
                ret_string += " ";
                ret_string = ret_string + this.Cards[i].ToString();
                ret_string += " ";
            }
            return ret_string;
        }

        public bool Contains(Card other)
        {
            bool retVal = false;
            int otherHashcode = other.GetHashCode();
            foreach (var cc in Cards)
            {
                int thisHashCode = cc.GetHashCode();
                if (thisHashCode == otherHashcode)
                    return true;
            }
            return retVal;
        }
        public void Add(Card c)
        {
            if (!this.Contains(c))
                Cards.Add(c);
        }

        public int CompareTo(Hand other)
        {
            if (other.Equals(null))
                throw new ArgumentNullException($"{nameof(other)} passed to CompareTo and is null");

            if (this.Rank.Equals(default) || other.Rank.Equals(default))
                throw new InvalidOperationException($"Cannot Compare this hand to {nameof(other)} until both have been assigned a Rank");

            if (other.Rank > this.Rank)
            {
                return 1;
            }
            else if (other.Rank < this.Rank)
            {
                return -1;
            }
            else if (other.Rank == this.Rank)
            {
                return 0;
            }
            return -99;
        }
    }
}
