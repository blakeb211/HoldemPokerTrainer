using static PokerConsoleApp.CustomTypes;

namespace PokerConsoleApp
{
    class BoardMethods
    {
        public void Deal_Cards(Board b)
        {
            // deal hole cards
            for (int player_index = 0; player_index < player_count; player_index++)
            {
                for (int hole_card_index = 0; hole_card_index < 2; hole_card_index++)
                {
                    this.players[player_index].hole[hole_card_index] = CardMethods.RemoveCard(deck);
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
    }
}
