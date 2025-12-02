namespace AdventOfCode2025.Days
{
    public class Day00 : BaseDay
    {
        #region Overrides

        public Day00() : base(0) { }

        protected override string SolvePartOne()
        {
            var R = CalculatePartOne(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CalculatePartTwo(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CalculatePartOne(LinesTest);
            return R == 0;
        }

        protected override bool TestPartTwo()
        {
            var R = CalculatePartTwo(LinesTest);
            return R == 0;
        }

        #endregion Overrides

        private static int CalculatePartOne(List<string> _)
        {
            return 0;
        }

        private static int CalculatePartTwo(List<string> _)
        {
            return 0;
        }
    }
}