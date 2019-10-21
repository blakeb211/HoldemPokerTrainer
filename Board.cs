using ConsoleTables;
using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Board
    {
        public List<Card> Cards { get; private set; }

        public List<Player> Players { get; private set; }

        public List<Card> Deck { get; set; }

        public int PlayerCount { get; private set; }

        public Board(int playerCount)
        {
            Cards = new List<Card>(5);
            Players = new List<Player>(playerCount);
            Deck = new List<Card>(52);

            for (int i = 0; i < playerCount; i++)
            {
                // initialize player
            }
        }
        public static List<Card> BuildDeck()
        {
            List<Card> deck = new List<Card>(52);
            foreach (var cr in Enum.GetValues(typeof(RankType)))

                foreach (var cs in Enum.GetValues(typeof(SuitType)))
                {
                    deck.Add(new Card((RankType)cr, (SuitType)cs));
                }
            return deck;
        }

        public void DealGame()
        {
            // deal hole cards
            for (int player_index = 0; player_index < Players.Count; player_index++)
            {
                for (int hole_card_index = 0; hole_card_index < 2; hole_card_index++)
                {
                    this.Players[player_index].Hole[hole_card_index] = Card.DealCard(Deck);
                }
            }

            // deal flop cards
            for (int flop_card_index = 0; flop_card_index < 3; flop_card_index++)
            {
                this.Cards[flop_card_index] = Card.DealCard(Deck);
            }

            // deal turn card
            this.Cards[3] = Card.DealCard(Deck);
            // deal river card
            this.Cards[4] = Card.DealCard(Deck);
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
