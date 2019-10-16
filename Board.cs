using ConsoleTables;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Board
    {
        public List<Card> Cards { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Card> Deck { get; private set; }

        public Board(int playerCount)
        {
            // Board constructor
            Cards = new List<Card>(5);
            Players = new List<Player>(playerCount);
            Deck = new List<Card>(52);

            for (int i = 0; i < playerCount; i++)
            {
                // initialize player
            }
        }
        public void DealCard()
        {
            // deal hole cards
            for (int player_index = 0; player_index < Players.Count; player_index++)
            {
                for (int hole_card_index = 0; hole_card_index < 2; hole_card_index++)
                {
                    this.Players[player_index].hole[hole_card_index] = CardMethods.RemoveCard(deck);
                }
            }

            // deal flop cards
            for (int flop_card_index = 0; flop_card_index < 3; flop_card_index++)
            {
                this.flop[flop_card_index] = CardMethods.RemoveCard(deck);
            }

            // deal turn card
            this.turn_card = CardMethods.RemoveCard(deck);
            // deal river card
            this.river_card = CardMethods.RemoveCard(deck);
        }
        public override string ToString()
        {
            string ret_string;
            var table = new ConsoleTable("flop", "turn", "river");
            table.AddRow($"{Cards[0].ToString()} {Cards[1].ToString()} {Cards[2].ToString()}", Cards[3].ToString(), Cards[4].ToString());
            ret_string = UtilityMethods.Trim_To_End(table.ToString(), "Count:");
            return ret_string;
        }

    }
}
