namespace AdventOfCode2025.Days
{
    public class Day07 : TDay
    {
        public Day07() : base(7) { }

        protected override long TestOneResult => 21;
        protected override long TestTwoResult => 40;

        protected override long CalculatePartOne(List<string> diagram)
        {
            var M = new Manifold(diagram);
            M.Process();
            return M.SplitCount;
        }

        protected override long CalculatePartTwo(List<string> diagram)
        {
            var M = new Manifold(diagram);
            M.Process();
            return M.Timelines;
        }

        private class Manifold
        {
            private readonly Dictionary<int, long> Beams = new();
            private readonly List<HashSet<int>> Splitters = new();

            public Manifold(List<string> diagram)
            {
                Beams.Add(diagram[0].IndexOf('S'), 1);
                foreach (var line in diagram.Skip(1))
                {
                    var set = Enumerable.Range(0, line.Length).Where(I => line[I] == '^').ToHashSet();
                    Splitters.Add(set);
                }
            }

            public long SplitCount { get; private set; }
            public long Timelines => Beams.Values.Sum();

            public void Process()
            {
                foreach (var S in Splitters)
                {
                    foreach (var (beam, count) in Beams.ToList())
                    {
                        if (S.Contains(beam))
                        {
                            Beams.Remove(beam);
                            Beams[beam - 1] = Beams.GetValueOrDefault(beam - 1) + count;
                            Beams[beam + 1] = Beams.GetValueOrDefault(beam + 1) + count;
                            SplitCount++;
                        }
                    }
                }
            }
        }
    }
}