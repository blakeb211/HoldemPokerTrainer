using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            if (s == null)
                throw new ArgumentNullException($"{nameof(Hand)} method with parameter {nameof(s)}");
            // Remove dashes from string if they are present.
            // The fact that dashes are optional in the Hand(string) constructor makes it easier to 
            // type out and re-read tests that use the constructor a lot.

            while (s.Contains('-'))
            {
                s = s.Remove(s.IndexOf('-'), 1);
            }
            Trace.Assert(s.Length == 10);
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

        public Hand(List<Card> cards)
        {
            this.Cards = cards;
            this.Rank = default;
            this.Name = default;
        }

        public void AssignRankAndName()
        {
            // get primeId used for hand rank lookup
            int _primeId;
            int _temp = 1;
            for (int i = 0; i < Cards.Count; i++)
            {
                _temp *= Cards[i].GetPrimeIdForRankingHand();
            }
            _primeId = _temp;
            // look up and assign hand rank
            if (CheckFlush())
            {
                Rank = PokerLib.Lookups.FlushDict[_primeId];
            }
            else
            {
                Rank = PokerLib.Lookups.NonFlushDict[_primeId];
            }

            Name = PokerLib.Lookups.RankToNameDict[Rank];
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
            //
            // Summary:
            //     Compares the current instance with another object of the same type and returns
            //     an integer that indicates whether the current instance precedes, follows, or
            //     occurs in the same position in the sort order as the other object.
            //
            // Parameters:
            //   Hand:
            //     An object to compare with this instance.
            //
            // Returns:
            //     A value that indicates the relative order of the objects being compared. The
            //     return value has these meanings: 
            //     -1 This instance precedes other in the sort order. 
            //      0 This instance occurs in the same position in the sort order.
            //      1 This instance follows other in the sort order.
            //
            if (other == null)
                throw new ArgumentNullException($"{nameof(other)} passed to CompareTo and is null");

            int retVal = 0;
            if (other.Rank < this.Rank)
            {
                retVal = 1;
            }
            else if (other.Rank > this.Rank)
            {
                retVal = -1;
            }
            else if (other.Rank == this.Rank)
            {
                retVal = 0;
            }
            return retVal;
        }
    }
}
