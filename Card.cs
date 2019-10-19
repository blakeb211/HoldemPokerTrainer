using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    public class Card
    {
        public RankType Rank { get; private set; }

        public SuitType Suit { get; private set; }

        // primes used to rank and name hands
        public static readonly List<int> PrimeValForRanking = new List<int> { -1, -1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };

        public static readonly List<string> RankStr = new List<string> { "-1", "-1", "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };

        public static readonly List<string> SuitStr = new List<string> { "-1", "H", "D", "S", "C" };

        // primes used to save cards to database
        private static readonly List<ulong> CardTo52UniquePrimes = new List<ulong> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41,
                                                                      43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101,
                                                                      103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167,
                                                                      173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239 };

        public static Dictionary<Card, ulong> CardPrimeDict = BuildCardToPrimeDict();
        
        public static Card DealCard(List<Card> inputCards)
        {
            Card temp = inputCards[0];
            inputCards.RemoveAt(0);
            return temp;
        }
        public override int GetHashCode()
        {
            // Hashcodes are unique for each of 52 cards but are not primes
            return (int)Rank * 37 + (int)Suit * 17;
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }
        
        public bool Equals(Card other)
        {
            return other != null && this.Rank == other.Rank && this.Suit == other.Suit;
        }

        public Card(RankType cr, SuitType cs)
        {
            this.Suit = cs;
            this.Rank = cr;
        }

        public static string GetRankAsString(RankType cr)
        {
            return RankStr[(int)cr];
        }

        public static Dictionary<Card, ulong> BuildCardToPrimeDict()
        {
            Dictionary<Card, ulong> CardToPrimeDict = new Dictionary<Card, ulong>() { };

            int dictIndex = 0;
            foreach (RankType cr in Enum.GetValues(typeof(RankType)))
                foreach (SuitType cs in Enum.GetValues(typeof(SuitType)))
                {
                    Card c = new Card((RankType)cr, (SuitType)cs);
                    CardToPrimeDict.Add(c, Card.CardTo52UniquePrimes[dictIndex]);
                    dictIndex++;
                }
            return CardToPrimeDict;
        }

        public static SuitType CharToSuit(char c)
        {
            c = char.ToLower(c);
            switch (c)
            {
                case 'h':
                    return SuitType.H;
                case 'd':
                    return SuitType.D;
                case 's':
                    return SuitType.S;
                case 'c':
                    return SuitType.C;
                default:
                    throw new ArgumentException($"{c} is invalid character to pass {nameof(CharToSuit)}");
            }
        }

        public int GetPrimeIdForRankingHand()
        {
            return PrimeValForRanking[(int)this.Rank];
        }

        public override string ToString()
        {
            return RankStr[(int)Rank] + "-" + SuitStr[(int)Suit];
        }
    }

}