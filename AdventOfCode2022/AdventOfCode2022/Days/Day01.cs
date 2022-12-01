namespace AdventOfCode2022.Days
{
    public class Day01 : BaseDay
    {
        #region Overrides

        public Day01() : base(1) { }

        protected override string SolvePartOne()
        {
            var R = FindMaxCalories(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindMaxCaloriesTop3(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindMaxCalories(RawTest);
            return R == 24000;
        }

        protected override bool TestPartTwo()
        {
            var R = FindMaxCaloriesTop3(RawTest);
            return R == 45000;
        }

        private static int FindMaxCalories(string items)
        {
            var calories = items.SplitBlocksToList().Select(E => E.SplitToList().Select(int.Parse).Sum());
            return calories.Max();
        }

        private static int FindMaxCaloriesTop3(string items)
        {
            var calories = items.SplitBlocksToList().Select(E => E.SplitToList().Select(int.Parse).Sum());
            return calories.OrderByDescending(C => C).Take(3).Sum();
        }

        #endregion Overrides
    }
}