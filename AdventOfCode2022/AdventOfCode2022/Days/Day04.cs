using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public class Day04 : BaseDay
    {
        #region Overrides

        public Day04() : base(4) { }

        protected override string SolvePartOne()
        {
            var R = CountFullOverlaps(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CountOverlaps(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CountFullOverlaps(LinesTest);
            return R == 2;
        }

        protected override bool TestPartTwo()
        {
            var R = CountOverlaps(LinesTest);
            return R == 4;
        }

        #endregion Overrides

        private static int CountFullOverlaps(List<string> pairs)
        {
            var Pairs = pairs.Select(P => new Pair(P));
            return Pairs.Count(P => P.IsFullOverlapp());
        }

        private static int CountOverlaps(List<string> pairs)
        {
            var Pairs = pairs.Select(P => new Pair(P));
            return Pairs.Count(P => P.IsOverlapp());
        }

        private class Pair
        {
            private static readonly Regex R = new(@"\d+");

            public Pair(string pair)
            {
                var Numbers = R.Matches(pair).Select(M => int.Parse(M.Value)).ToList();
                for (int i = Numbers[0]; i <= Numbers[1]; i++) { First.Add(i); }
                for (int i = Numbers[2]; i <= Numbers[3]; i++) { Second.Add(i); }
            }

            public HashSet<int> First { get; } = new();
            public HashSet<int> Second { get; } = new();

            public bool IsFullOverlapp()
            {
                var Length = First.Intersect(Second).Count();
                return Length == First.Count || Length == Second.Count;
            }

            public bool IsOverlapp() => First.Intersect(Second).Any();
        }
    }
}