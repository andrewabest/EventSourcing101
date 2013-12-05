using System;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotAndrew(this string value)
        {
            return value.Equals("Andrew", StringComparison.InvariantCultureIgnoreCase) == false;
        }

        public static bool IsAndrew(this string value)
        {
            return !value.IsNotAndrew();
        }
    }
}