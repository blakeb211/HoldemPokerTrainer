namespace PokerConsoleApp
{

    public class Board
    {
        // note that NUMBER_OF_PLAYERS IS IN BOTH PROGRAM.cs and 
        // BOARD.cs and needs to be fixed so that it only appears once.

        const int NUMBER_OF_PLAYERS = 4;
        public Card[] flop_cards = new Card[3];
        public Card turn_card = new Card();
        public Card river_card = new Card();
        public Player[] players = new Player[NUMBER_OF_PLAYERS];

        public Board(int player_count)
        {
            // Board constructor
            for (int i = 0; i < player_count; i++)
            {
                this.players[i] = new Player();
            }
        }
        public override string ToString()
        {
            string ret_string;
            ret_string = "| " + flop_cards[0] + " " + flop_cards[1] + " " + flop_cards[2] + "   " + turn_card + " " + river_card + " |";
            ret_string += "\n\n";
            for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
            {
                ret_string += "Player " + i + " - Hole Cards" + "\n" + players[i] + "\n\n";
            }
            return ret_string;
        }
        public void Deal_Cards()
        {
            // TODO - add in burn cards
            Deck deck = new Deck();
            deck.Shuffle();
            // deal players their hole cards
            for (int player_index = 0; player_index < NUMBER_OF_PLAYERS; player_index++)
            {
                for (int hole_card_index = 0; hole_card_index < 2; hole_card_index++)
                {
                    // deal a card and remove it from the deck
                    this.players[player_index].hole[hole_card_index] = deck.RemoveCard();

                }
            }
            // burn a card
            deck.RemoveCard();
            // deal 3 flop cards
            for (int flop_card_index = 0; flop_card_index < 3; flop_card_index++)
            {
                this.flop_cards[flop_card_index] = deck.RemoveCard();
            }
            // burn a card
            deck.RemoveCard();
            // deal turn card
            this.turn_card = deck.RemoveCard();
            // burn a card
            deck.RemoveCard();
            // deal river card
            this.river_card = deck.RemoveCard();
        }
    }
}
