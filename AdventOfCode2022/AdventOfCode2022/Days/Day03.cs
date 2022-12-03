namespace AdventOfCode2022.Days
{
    public class Day03 : BaseDay
    {
        #region Overrides

        public Day03() : base(3) { }

        protected override string SolvePartOne()
        {
            var R = SumRucksackPriority(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = SumGroupPriority(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = SumRucksackPriority(LinesTest);
            return R == 157;
        }

        protected override bool TestPartTwo()
        {
            var R = SumGroupPriority(LinesTest);
            return R == 70;
        }

        #endregion Overrides

        protected static int SumGroupPriority(List<string> rucksacks)
        {
            var Groups = rucksacks.Chunk(3).Select(G => (R1: G[0], R2: G[1], R3: G[2]));
            var Badges = Groups.Select(G => G.R1.Intersect(G.R2).Intersect(G.R3).First()).ToList();
            return Badges.Sum(B => GetPriority(B));
        }

        protected static int SumRucksackPriority(List<string> rucksacks)
        {
            var Rucksacks = rucksacks.Select(R => new Rucksack(R));
            return Rucksacks.Sum(R => GetPriority(R.GetCommonItem()));
        }

        private static int GetPriority(char item)
        {
            var Priority = item % 32;
            return char.IsLower(item) ? Priority : Priority + 26;
        }

        private class Rucksack
        {
            public Rucksack(string content)
            {
                var Count = content.Length;
                First = content[0..(Count / 2)].ToList();
                Second = content[(Count / 2)..Count].ToList();
            }

            public List<char> First { get; set; }
            public List<char> Second { get; set; }

            public char GetCommonItem() => First.Intersect(Second).First();
        }
    }
}