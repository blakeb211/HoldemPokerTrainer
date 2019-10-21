namespace PokerConsoleApp
{

    internal enum GameState
    {
        HOLE_CARDS_DEALT,
        FLOP_DEALT,
        TURN_DEALT,
        RIVER_DEALT,
        GAME_OVER
    };
    public enum HandType
    {
        SF = 8,
        FourOfKind = 7,
        FH = 6,
        F = 5,
        S = 4,
        ThreeP = 3,
        TwoP = 2,
        OneP = 1,
        HC = 0,
    };

    public enum SuitType
    {
        H = 1,
        D = 2,
        S = 3,
        C = 4
    };
    public enum RankType
    {
        TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8,
        NINE = 9, TEN = 10, J = 11, Q = 12, K = 13, A = 14
    };
}