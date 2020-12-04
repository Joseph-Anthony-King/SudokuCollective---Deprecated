﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuCollective.Core.Extensions
{
    public static class CoreExtensions
    {
        public static void Shuffle<T>(List<T> list, Random generateRandomNumber)
        {
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

        public static bool IsThisListEqual(this IEnumerable<int> aList, IEnumerable<int> bList)
        {
            bool result = false;

            foreach (int a in aList)
            {
                if (bList.Contains(a))
                {
                    result = true;
                }
            }

            return result;
        }
        public static IEnumerable<T> RemoveSubList<T>(this IEnumerable<T> list, IEnumerable<T> subList)
        {
            var result = new List<T>();

            foreach (var element in list)
            {
                if (!subList.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }

    }
}
