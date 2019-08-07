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
        }
    }
  
}
