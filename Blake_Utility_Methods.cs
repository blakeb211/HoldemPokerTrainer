using System;
using System.Collections.Generic;

namespace PokerConsoleApp
{
    class Blake_Utility_Methods
    {
        public static List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();
            Random r = new Random();
            int randomIndex;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }
            return randomList; //return the new random list
        }
        public static void GetKeyPress()
        {
            Console.WriteLine("Press a key to continue...");
            Console.ReadLine();
        }
        public static bool Ask_User_For_Quit_Signal()
        {
            Console.WriteLine("Press \"Q\" to quit...");
            string sInput = Console.ReadLine();
            if (sInput == "q" || sInput == "Q")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string Trim_To_End(string s_to_trim, string s_substr)
        {
            int i_substr = s_to_trim.LastIndexOf(s_substr);
            string ret_string = s_to_trim.Substring(0, i_substr);
            ret_string += "\n";
            return ret_string;
        }

    }
}
