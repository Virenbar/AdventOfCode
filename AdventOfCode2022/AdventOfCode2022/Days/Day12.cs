namespace AdventOfCode2022.Days
{
    public class Day12 : BaseDay
    {
        #region Overrides

        public Day12() : base(12) { }

        protected override string SolvePartOne()
        {
            var R = FindPathSteps(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindPathStepsMinimum(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindPathSteps(RawTest);
            return R == 31;
        }

        protected override bool TestPartTwo()
        {
            var R = FindPathStepsMinimum(RawTest);
            return R == 29;
        }

        #endregion Overrides

        private static int FindPathSteps(string heightmap)
        {
            var Hill = new Hill(heightmap);
            var Steps = Hill.FindPath();
            return Steps;
        }

        private static int FindPathStepsMinimum(string heightmap)
        {
            var Hill = new Hill(heightmap);
            var Steps = Hill.FindPathMin();
            return Steps;
        }

        private class Hill
        {
            private readonly Point End;
            private readonly Dictionary<Point, int> Heights = new();
            private readonly Point Start;

            public Hill(string heightmap)
            {
                var Rows = heightmap.SplitToList();
                for (int Y = 0; Y < Rows.Count; Y++)
                {
                    for (int X = 0; X < Rows[0].Length; X++)
                    {
                        var Char = Rows[Y][X];
                        var Height = Char % 32;
                        if (Char == 'S')
                        {
                            Height = 1;
                            Start = new(X, Y);
                        }
                        if (Char == 'E')
                        {
                            Height = 26;
                            End = new(X, Y);
                        }
                        Heights.Add(new(X, Y), Height);
                    }
                }
            }

            public int FindPath() => FindPath(Start, End);

            public int FindPathMin()
            {
                var Starts = Heights.Where(KV => KV.Value == 1).Select(KV => KV.Key);
                var Paths = Starts.Select(S => FindPath(S, End));
                return Paths.Min();
            }

            /// <summary>
            /// https://en.wikipedia.org/wiki/Breadth-first_search
            /// </summary>
            private int FindPath(Point start, Point end)
            {
                Dictionary<Point, Point> Parents = new();
                Queue<Point> Q = new();
                HashSet<Point> Explored = new() { start };
                Q.Enqueue(start);
                while (Q.Count > 0)
                {
                    var Current = Q.Dequeue();
                    if (Current == end)
                    {
                        List<Point> Path = new();
                        var Parent = end;
                        while (Parent != start)
                        {
                            Path.Add(Parent);
                            Parent = Parents[Parent];
                        }
                        return Path.Count;
                    }
                    foreach (var P in Neighbors(Current))
                    {
                        if (Explored.Contains(P)) { continue; }
                        Explored.Add(P);
                        Parents[P] = Current;
                        Q.Enqueue(P);
                    }
                }
                return int.MaxValue;
            }

            private List<Point> Neighbors(Point P)
            {
                List<Point> N = new();
                foreach (var (X, Y) in new[] { (1, 0), (0, 1), (-1, 0), (0, -1) })
                {
                    var T = new Point(P.X + X, P.Y + Y);
                    if (!Heights.ContainsKey(T)) { continue; }
                    if (Heights[T] <= Heights[P] + 1) { N.Add(T); }
                }
                return N;
            }

            private record Point(int X, int Y);
        }
    }
}