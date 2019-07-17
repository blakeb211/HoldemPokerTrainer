using System;
using System.Collections.Generic;
using System.Text;

namespace PokerConsoleApp
{
    class Hand
    {
        private List<Card> cards = new List<Card> { };
        public Hand()
        {
            Console.WriteLine($"hand capacity = {cards.Capacity}");
            cards.Capacity = 5;
            Console.WriteLine($"hand capacity = {cards.Capacity}");

        }
    }
}
