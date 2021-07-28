using System;

namespace SudokuCollective.Data.Resiliency
{
    internal static class CacheStrategy
    {
        internal static DateTime Light 
        { 
            get
            {
                return DateTime.Now.AddMinutes(5);
            } 
        }
        internal static DateTime Medium
        {
            get
            {
                return DateTime.Now.AddMinutes(15);
            }
        }
        internal static DateTime Heavy
        {
            get
            {
                return DateTime.Now.AddDays(1);
            }
        }
    }
}
