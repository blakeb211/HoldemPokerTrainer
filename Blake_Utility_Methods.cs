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
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            int randomIndex = 0;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

    }
}
