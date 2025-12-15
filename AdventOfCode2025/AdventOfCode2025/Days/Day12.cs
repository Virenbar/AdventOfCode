using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public partial class Day12 : TDay<string>
    {
        public Day12() : base(12) { }

        protected override long TestOneResult => 3;
        protected override long TestTwoResult => 0;

        protected override long CalculatePartOne(string input)
        {
            var presents = new Presents(input);
            return presents.Count();
        }

        protected override long CalculatePartTwo(string _)
        {
            return 0;
        }

        private partial class Presents
        {
            private List<int> Areas;
            private List<Region> Regions = new();

            public Presents(string input)
            {
                var blocks = input.SplitBlocksToList();
                Areas = blocks.SkipLast(1).Select(B => B.Count(C => C == '#')).ToList();

                foreach (var region in blocks.Last().SplitToList())
                {
                    var numbers = Digit().Matches(region).Select(M => int.Parse(M.Value)).ToList();
                    Regions.Add(new Region(numbers[0], numbers[1], numbers.Skip(2).ToArray()));
                }
            }

            public long Count()
            {
                var count = 0;
                foreach (var region in Regions)
                {
                    var area = Enumerable.Zip(region.Counts, Areas).Sum(P => P.First * P.Second);
                    if (area <= region.W * region.H) { count++; }
                }
                return count;
            }

            private record Region(int W, int H, int[] Counts);

            [GeneratedRegex(@"\d+")]
            private static partial Regex Digit();
        }
    }
}