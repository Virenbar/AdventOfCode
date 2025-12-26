using MoreLinq.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public partial class Day05 : TDay<string>
    {
        public Day05() : base(5) { }

        protected override long TestOneResult => 3;
        protected override long TestTwoResult => 14;

        protected override long CalculatePartOne(string database)
        {
            var DB = new Database(database);
            return DB.CountFresh();
        }

        protected override long CalculatePartTwo(string database)
        {
            var DB = new Database(database);
            return DB.CountID();
        }

        private partial class Database
        {
            private readonly IEnumerable<long> ingredients;
            private readonly IEnumerable<(long low, long high)> ranges;

            public Database(string database)
            {
                var R = RangeRegex();

                var (ListOne, ListTwo) = database.SplitToTwoList();
                ranges = ListOne
                    .Select(S => R.Match(S))
                    .Select(M => (low: long.Parse(M.Groups["low"].Value), high: long.Parse(M.Groups["high"].Value)));
                ingredients = ListTwo.Select(long.Parse);
            }

            public int CountFresh() => ingredients.Count(IsFresh);

            public long CountID()
            {
                return ranges
                    .OrderBy(R => R.low)
                    .Aggregate((count: 0L, max: 0L), (agr, range) =>
                    {
                        if (agr.max >= range.low) { range.low = agr.max + 1; }
                        if (agr.max >= range.high) { range.high = agr.max; }
                        agr.count += range.high - range.low + 1;
                        return (agr.count, range.high);
                    }).count;
            }

            [GeneratedRegex(@"(?<low>\d+)-(?<high>\d+)")]
            private static partial Regex RangeRegex();

            private bool IsFresh(long ID) => ranges.Any(R => R.low <= ID && ID <= R.high);
        }
    }
}