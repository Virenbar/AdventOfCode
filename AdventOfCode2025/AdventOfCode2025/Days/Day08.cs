namespace AdventOfCode2025.Days
{
    public class Day08 : TDay
    {
        public Day08() : base(8) { }

        protected override long TestOneResult => 40;
        protected override long TestTwoResult => 25272;

        protected override long CalculatePartOne(List<string> junctions)
        {
            var limit = junctions.Count == 20 ? 10 : 1000;
            var P = new Playground(junctions);
            P.Connect(limit);
            return P.Length;
        }

        protected override long CalculatePartTwo(List<string> junctions)
        {
            var limit = junctions.Count == 20 ? 10 : 1000;
            var P = new Playground(junctions);
            P.Connect(limit);
            return P.LastConnection;
        }

        private class Playground
        {
            private readonly HashSet<List<Junction>> Circuits;
            private readonly List<Junction> Junctions;

            public Playground(List<string> junctions)
            {
                Junctions = junctions.Select(J => new Junction(J)).ToList();
                Circuits = Junctions.Select(J => J.Circuit).ToHashSet();
            }

            public long LastConnection { get; private set; }
            public long Length { get; private set; }

            public void Connect(int limit)
            {
                var pairs = Junctions
                    .Pairs()
                    .OrderBy(Distance)
                    .ToList();

                var count = 0;
                foreach (var (First, Second) in pairs)
                {
                    if (First.Circuit != Second.Circuit)
                    {
                        var oldCircuit = Second.Circuit;
                        First.Circuit.AddRange(oldCircuit);
                        oldCircuit.ForEach(J => J.Circuit = First.Circuit);
                        Circuits.Remove(oldCircuit);
                    }

                    if (++count == limit)
                    {
                        Length = Circuits.OrderByDescending(C => C.Count).Take(3).Aggregate(1L, (A, C) => A * C.Count);
                    }
                    if (Circuits.Count == 1)
                    {
                        LastConnection = First.Position.X * Second.Position.X;
                        break;
                    }
                }
            }

            private static double Distance((Junction first, Junction second) pair)
            {
                Position P1 = pair.first.Position, P2 = pair.second.Position;
                return Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));
            }

            private class Junction
            {
                public Junction(string position)
                {
                    var p = position.Split(',').Select(int.Parse).ToList();
                    Position = new Position(p[0], p[1], p[2]);
                    Circuit = new() { this };
                }

                public List<Junction> Circuit { get; set; }
                public Position Position { get; }

                public override string ToString() => $"{Position.X}:{Position.Y}:{Position.Z}";
            }
            private record Position(int X, int Y, int Z);
        }
    }
}