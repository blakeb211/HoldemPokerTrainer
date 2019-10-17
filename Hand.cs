using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public partial class Hand : IComparable<Hand>
    {
        public List<Card> Cards { get; private set; }
        public int PrimeId { get; private set; }
        public int Rank { get; private set; }
        public string Name { get; private set; }
        public List<HandType> Outs { get; private set; }

        public Hand(string handStr)
        {
            this.Cards = new List<Card>(5);

        }

        public void AssignName()
        {
            if (Rank.Equals(default))
                throw new InvalidOperationException("Cannot assign hand a name before it's rank has been assigned.");
            Name = PokerLib.Lookups.RankToNameDict[Rank];
        }


        public void AssignRank()
        {
            if (CheckFlush())
            {
                Rank = PokerLib.Lookups.FlushDict[PrimeId];
                return;
            }

            Rank = PokerLib.Lookups.NonFlushDict[PrimeId];
            return;
        }

        private bool CheckFlush()
        {
            if (Cards[0].Suit == Cards[1].Suit && Cards[1].Suit == Cards[2].Suit
                 && Cards[2].Suit == Cards[3].Suit && Cards[3].Suit == Cards[4].Suit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public void AssignPrimeId()
        {
            if (PrimeId == default)
            {
                return;
            }
            else
            {
                int temp = 1;
                for (int i = 0; i < Cards.Count; i++)
                {
                    temp *= Cards[i].GetPrimeId();
                }
                this.PrimeId = temp;
            }
        }

        public void Sort()
        {


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

        public int CompareTo(Hand other)
        {
            if (other.Equals(null))
                throw new ArgumentNullException($"{nameof(other)} passed to CompareTo and is null");

            if (this.Rank.Equals(default) || other.Rank.Equals(default))
                throw new InvalidOperationException($"Cannot Compare this hand to {nameof(other)} until both have been assigned a Rank");


        }
    }
}
