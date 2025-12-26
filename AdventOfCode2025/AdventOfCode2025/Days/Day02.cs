using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public partial class Day02 : TDay<string>
    {
        public Day02() : base(2) { }

        protected override long TestOneResult => 1227775554;
        protected override long TestTwoResult => 4174379265;

        protected override long CalculatePartOne(string ranges)
        {
            var Finder = new IDRanges(ranges);
            return Finder.FindInvalid(InvalidID());
        }

        protected override long CalculatePartTwo(string ranges)
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