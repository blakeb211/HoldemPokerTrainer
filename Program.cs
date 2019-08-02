/*
 * Purpose: This program draws a random hand from the deck and displays the handtype. 
 *          Ultimately, it will use monte carlo to calculate the odds of winning for different hole cards. 
 * Input:   None. Player number is fixed at 4.
 * Output:  Print four hands, their hand types, and who wins           
 *
 *Todo: Add evaluator method
 *             Identify and print out correct hand type, e.g. four 5s and a queen kicker, full house, 3s over 2s. 
 *             
 *      Add testing 
 * 
 */
namespace PokerConsoleApp
{
    class Program
    {
        static void Main()
        {
            var d = new Deck();
            d.ShuffleDeck();
            Hand h = new Hand();
            for (int i = 0; i < 5; i++)
            {
                Card c = d.RemoveCard();
                h.AddCard(c);

            }
            h.PrintHand();
            h.EvaluateHandtype();
            h.DoSort();
            h.PrintHand();

        }
    }
}
