namespace AdventOfCode2025.Days
{
    public class Day04 : TDay
    {
        public Day04() : base(4) { }

        protected override long TestOneResult => 13;
        protected override long TestTwoResult => 43;

        protected override long CalculatePartOne(List<string> rolls)
        {
            var Rolls = new PaperRolls(rolls);
            return Rolls.CountAcceseble();
        }

        protected override long CalculatePartTwo(List<string> rolls)
        {
            var Rolls = new PaperRolls(rolls);
            return Rolls.CountRemoved();
        }

        private class PaperRolls
        {
            private readonly List<(int DX, int DY)> Offsets;
            private readonly HashSet<Roll> Rolls = new();

            public PaperRolls(List<string> rolls)
            {
                Offsets = new List<(int DX, int DY)> { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) };

                for (int x = 0; x < rolls.Count; x++)
                {
                    for (int y = 0; y < rolls[x].Length; y++)
                    {
                        if (rolls[x][y] == '@') { Rolls.Add(new(x, y)); }
                    }
                }
            }

            public int CountAcceseble()
            {
                return Rolls.Count(IsAcceseble);
            }

            public int CountRemoved()
            {
                var total = 0;
                while (true)
                {
                    var removed = Rolls.RemoveWhere(IsAcceseble);
                    total += removed;
                    if (removed == 0) { break; }
                }
                return total;
            }

            private bool IsAcceseble(Roll roll)
            {
                return Offsets.Count(O => Rolls.Contains(new(roll.X + O.DX, roll.Y + O.DY))) < 4;
            }

            private record Roll(int X, int Y);
        }
    }
}