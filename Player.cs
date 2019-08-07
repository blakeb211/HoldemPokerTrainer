using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    public class Player
    {
        public Card[] hole = new Card[2];
        
        bool won_the_hand;
        Hand best_hand = new Hand();
        public Player()
        {
            this.won_the_hand = false;
            this.hole[0] = new Card();
            this.hole[1] = new Card();
        }
        public override string ToString()
        {
            string ret_string;
            ret_string = "" + hole[0] + " " + hole[1];
            return ret_string;
        }
    }
  
}
