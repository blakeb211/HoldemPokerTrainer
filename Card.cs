using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Card
    {
        public Rank Rank { get; private set; }

        public Suit Suit { get; private set; }

        public static readonly List<int> PrimeVal = new List<int> { -1, -1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };

        public static readonly List<string> RankStr = new List<string> { "-1", "-1", "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };

        public static readonly List<string> SuitStr = new List<string> { "-1", "H", "D", "S", "C" };
        public Card(Suit cs, Rank cr)
        {
            this.suit = cs;
            this.rank = cr;
        }

        public Card(string cardStr)
        {

        }

        public int GetPrimeId()
        {
            return PrimeVal[(int)this.Rank];
        }

        public static string GetRankAsString(Rank cr)
        {
            return RankStr[(int)cr];
        }


        public override string ToString()
        {
            return RankStr[(int)Rank] + "-" + SuitStr[(int)Suit];
        }
    }
}