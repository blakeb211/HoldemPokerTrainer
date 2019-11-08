using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Player
    {
        public List<Card> Hole { get; }

        public bool IsWinner { get; set; }

        public Hand BestHand { get; set; }

        public float PreFlopOdds { get; set; }

        public float PostFlopOdds { get; set; }

        public Player()
        {
            this.IsWinner = false;
            this.Hole = new List<Card>(2);

        }

        public override string ToString()
        {
            return "" + Hole[0].ToString() + " " + Hole[1].ToString();
        }

        internal string GetHoleCardsString()
        {
            return $"{this.Hole[0]} {this.Hole[1]}";
        }
    }

}
