using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    
    public class Board
    {
        const int NUMBER_OF_PLAYERS = 4;
        public Card [] flop_cards = new Card[3];
        public Card turn_card = new Card();
        public Card river_card = new Card();
        public Player[] players = new Player[NUMBER_OF_PLAYERS];

        public Board()
        {
            // Board constructor
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
                    this.players[player_index].hole[hole_card_index] = cc;
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
