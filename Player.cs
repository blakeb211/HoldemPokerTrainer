using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Player
    {
        public List<Card> Hole { get; set; }

        public bool? IsWinner { get; set; }

        public Hand BestHand { get; set; }

        public Player()
        {
            this.IsWinner = null;
            this.Hole = new List<Card>(2) { };
            this.Hole[0] = new Card();
            this.Hole[1] = new Card();
        }

        public override string ToString()
        {
            string ret_string;
            ret_string = "" + hole[0] + " " + hole[1];
            return ret_string;
        }
    }

}
