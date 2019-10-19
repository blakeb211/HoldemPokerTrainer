using System.Collections.Generic;
using System.Diagnostics;

namespace PokerConsoleApp
{
    public class Player
    {
        public List<Card> Hole { get; set; }

        public bool IsWinner { get; set; }

        public Hand BestHand { get; set; }

        public Player(List<Card> holeCards)
        {
            Debug.Assert(holeCards.Count == 2);
            this.IsWinner = false;
            this.Hole = new List<Card>(2);
            this.Hole = holeCards;
        }

        public override string ToString()
        {
             return "" + Hole[0].ToString() + " " + Hole[1].ToString();
        }
    }

}
