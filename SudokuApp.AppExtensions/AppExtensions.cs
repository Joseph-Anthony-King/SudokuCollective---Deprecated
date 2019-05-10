using System;
using System.Collections.Generic;

namespace SudokuApp.AppExtensions {
    public class AppExtensions {

        public static Random generateRandomNumber = new Random();

        public static void Shuffle<T>(List<T> list) {
            
            var _randomShuffle = generateRandomNumber;

            for (int i = list.Count; i > 1; i--)
            {
                // Pick a random element to swap
                int j = _randomShuffle.Next(9);
                int k = _randomShuffle.Next(9);
                // Swap
                T tmp = list[j];
                list[j] = list[k];
                list[k] = tmp;
            }
        }
    }
}
