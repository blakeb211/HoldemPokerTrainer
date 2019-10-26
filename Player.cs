using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Player
    {
        public List<Card> Hole { get; set; }

        public bool IsWinner { get; set; }

        public Hand BestHand { get; set; }

        public Player()
        {
            this.IsWinner = false;
            this.Hole = new List<Card>(2);

        }

        public override string ToString()
        {
            return "" + Hole[0].ToString() + " " + Hole[1].ToString();
        }
    }

}
