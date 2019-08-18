namespace PokerConsoleApp
{
    public class Player
    {
        public Card[] hole = new Card[2];

        public bool Won_The_Hand { get; set; }
        public Hand best_hand = new Hand();
        public Player()
        {
            this.Won_The_Hand = false;
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
