using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuApp.AppExtensions {
    public static class AppExtensions {

        public static void Shuffle<T>(List<T> list, Random generateRandomNumber) {

            var _randomShuffle = generateRandomNumber;

            for (int i = list.Count; i > 1; i--)
            {
                // Pick a random element to swap
                int j = _randomShuffle.Next(0, list.Count);
                int k = _randomShuffle.Next(0, list.Count);
                // Swap
                T tmp = list[j];
                list[j] = list[k];
                list[k] = tmp;
            }
        }

        public static bool ContainsAnySimilarElements(this IEnumerable<int> aList, IEnumerable<int> bList) {
            bool result = false;

            foreach (int a in aList) {
                if (bList.Contains(a)) {
                    result = true;
                }
            }

            return result;
        }
    }
}
