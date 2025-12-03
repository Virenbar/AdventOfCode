using System;
using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public partial class Day02 : BaseDay
    {
        #region Overrides

        public Day02() : base(2) { }

        protected override string SolvePartOne()
        {
            var R = CalculatePartOne(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CalculatePartTwo(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CalculatePartOne(RawTest);
            return R == 1227775554;
        }

        protected override bool TestPartTwo()
        {
            var R = CalculatePartTwo(RawTest);
            return R == 4174379265;
        }

        #endregion Overrides

        private static long CalculatePartOne(string ranges)
        {
            var Finder = new IDRanges(ranges);
            return Finder.FindInvalid(InvalidID());
        }

        private static long CalculatePartTwo(string ranges)
        {
            var Finder = new IDRanges(ranges);
            return Finder.FindInvalid(InvalidIDMore());
        }

        [GeneratedRegex(@"^(\d+)(\1)$")]
        private static partial Regex InvalidID();

        [GeneratedRegex(@"^(\d+)(\1)+$")]
        private static partial Regex InvalidIDMore();

        private partial class IDRanges
        {
            private readonly IEnumerable<(long low, long high)> ranges;

            public IDRanges(string ranges)
            {
                this.ranges = ParseRanges(ranges);
            }

            public long FindInvalid(Regex regex)
            {
                return ranges
                    .SelectMany(R => LongRange(R.low, R.high))
                    .Where(ID => regex.IsMatch(ID.ToString()))
                    .Sum();
            }

            private static IEnumerable<long> LongRange(long low, long high)
            {
                var value = low;
                while (value <= high)
                {
                    yield return value++;
                }
            }

            private static IEnumerable<(long low, long high)> ParseRanges(string ranges)
            {
                return RangeRegex()
                    .Matches(ranges)
                    .Select(M => (low: long.Parse(M.Groups["low"].Value), high: long.Parse(M.Groups["high"].Value)));
            }

            [GeneratedRegex(@"(?<low>\d+)-(?<high>\d+)")]
            private static partial Regex RangeRegex();
        }
    }
}