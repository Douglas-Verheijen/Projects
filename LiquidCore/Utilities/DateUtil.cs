namespace Liquid.Utilities
{
    public static class DateUtil
    {
        /// <summary>
        ///     Finds the number of days per month. Does not include leap years
        /// </summary>
        /// <param name="x">The month</param>
        /// <returns>The number of days</returns>
        public static int DaysPerMonth(int month)
        {
            return 28 + (month / 4) % 2 + 2 % month + 1 / month * 2 ;
        }

        /// <summary>
        ///     Finds the number of days per month. Includes leap years
        /// </summary>
        /// <param name="x">The month</param>
        /// <param name="x">The year</param>
        /// <returns>The number of days</returns>
        public static int DaysPerMonth(int month, int year)
        {
            var offset = month == 2 & year % 4 == 0 ? 1 : 0;
            return DaysPerMonth(month) + offset;
        }
    }
}
