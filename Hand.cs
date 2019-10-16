using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public partial class Hand : IComparable<Hand>
    {
        public List<Card> Cards { get; private set; }
        public int PrimeId { get; private set; }
        public int? Rank { get; private set; }
        public string Name { get; private set; }
        public List<HandType> Outs { get; private set; }

        public Hand(string handStr)
        {
            this.Cards = new List<Card>(5);

        }

        public void AssignName()
        {


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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
