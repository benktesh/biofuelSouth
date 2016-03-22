namespace BiofuelSouth.Helpers
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Calculates a cumulative value for decimal numbers
        /// </summary>
        /// <param name="numbers">Numbers to sum</param>
        /// <returns>Cumulative sum</returns>
        public static System.Collections.Generic.IEnumerable<decimal> CumulativeSum(
           this System.Collections.Generic.IEnumerable<decimal> numbers)
        {

            decimal summedNumber = 0;

            foreach (decimal number in numbers)
            {
                summedNumber = summedNumber + number;
                yield return summedNumber;
            }
        }
        public static System.Collections.Generic.IEnumerable<double> CumulativeSum(
           this System.Collections.Generic.IEnumerable<double> numbers)
        {

            double summedNumber = 0;

            foreach (double number in numbers)
            {
                summedNumber = summedNumber + number;
                yield return summedNumber;
            }
        }

        public static System.Collections.Generic.IEnumerable<int> CumulativeSum(
         this System.Collections.Generic.IEnumerable<int> numbers)
        {

            int summedNumber = 0;

            foreach (int number in numbers)
            {
                summedNumber = summedNumber + number;
                yield return summedNumber;
            }
        }
    }
}