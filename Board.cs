using ConsoleTables;
namespace PokerConsoleApp
{
    public class Board
    {
        // note that NUMBER_OF_PLAYERS IS IN BOTH PROGRAM.cs and 
        // BOARD.cs and needs to be fixed so that it only appears once.

        int NUMBER_OF_PLAYERS;
        public Card[] flop_cards = new Card[3];
        public Card turn_card = new Card();
        public Card river_card = new Card();
        public Player[] players;
        public Deck deck;
        public Board(int player_count)
        {
            // Board constructor
            deck = new Deck();
            deck.Shuffle();
            NUMBER_OF_PLAYERS = player_count;
            players = new Player[player_count];
            for (int i = 0; i < player_count; i++)
            {
                this.players[i] = new Player();
            }
        }
        public override string ToString()
        {
            string ret_string;
            var table = new ConsoleTable("flop", "turn", "river");
            table.AddRow($"{flop_cards[0].ToString()} {flop_cards[1].ToString()} {flop_cards[2].ToString()}", turn_card.ToString(), river_card.ToString());
            var table2 = new ConsoleTable("player", "hole cards");
            for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
            {
                table2.AddRow(i.ToString(), $"{this.players[i].hole[0].ToString()} {this.players[i].hole[1].ToString()}");
            }
            ret_string = table2.ToString() + "\n" + table.ToString();

            return ret_string;
        }
        public void Deal_Cards(int player_count)
        {
            // deal players their hole cards
            for (int player_index = 0; player_index < player_count; player_index++)
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
        public void Get_New_Deck()
        {
            Deck deck2 = new Deck();
            deck2.Shuffle();
            this.deck = deck2;
        }
    }
}
