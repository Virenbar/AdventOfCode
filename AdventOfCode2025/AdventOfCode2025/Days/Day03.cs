namespace AdventOfCode2025.Days
{
    public class Day03 : BaseDay
    {
        #region Overrides

        public Day03() : base(3) { }

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
            return R == 357;
        }

        protected override bool TestPartTwo()
        {
            var R = CalculatePartTwo(LinesTest);
            return R == 3121910778619;
        }

        #endregion Overrides

        private static int CalculatePartOne(List<string> batteries)
        {
            var Batteries = new Batteries(batteries);
            return Batteries.FindMax();
        }

        private static long CalculatePartTwo(List<string> batteries)
        {
            var Batteries = new Batteries(batteries);
            return Batteries.FindMaxOverride();
        }

        private class Batteries
        {
            private readonly int[][] Banks;

            public Batteries(List<string> banks)
            {
                Banks = banks.Select(B => B.Select(R => int.Parse(R.ToString())).ToArray()).ToArray();
            }

            public int FindMax()
            {
                return Banks.Select(B =>
                   {
                       var max = B[..^1].Max();
                       var maxFirst = Array.IndexOf(B, max);
                       var maxSecond = B[(maxFirst + 1)..].Max();
                       var number = (max * 10) + maxSecond;
                       return number;
                   }).Sum();
            }

            public long FindMaxOverride()
            {
                return Banks.Select(B =>
                    {
                        var list = B.ToList();
                        while (list.Count > 12)
                        {
                            var minFound = false;
                            for (var i = 0; !minFound && i < list.Count - 1; i++)
                            {
                                if (list[i] < list[i + 1])
                                {
                                    minFound = true;
                                    list.RemoveAt(i);
                                }
                            }

                            if (!minFound)
                                list.Remove(list.Min());
                        }

                        return long.Parse(string.Join("", list));
                    }).Sum();
            }
        }
    }
}