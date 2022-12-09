namespace AdventOfCode2022.Days
{
    public class Day09 : BaseDay
    {
        #region Overrides

        public Day09() : base(9) { }

        protected override string SolvePartOne()
        {
            var R = SimulateRope2(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = SimulateRope10(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = SimulateRope2(LinesTest);
            return R == 13;
        }

        protected override bool TestPartTwo()
        {
            var R = SimulateRope10(LinesTest);
            return R == 1;
        }

        #endregion Overrides

        private static int SimulateRope10(List<string> moves)
        {
            var Rope = new Rope(10, moves);
            Rope.Simulate();
            return Rope.TailCount;
        }

        private static int SimulateRope2(List<string> moves)
        {
            var Rope = new Rope(2, moves);
            Rope.Simulate();
            return Rope.TailCount;
        }

        private class Rope
        {
            private record Point(int X, int Y);
            private readonly List<Point> Knots = new();
            private readonly List<string> Moves;
            private readonly HashSet<Point> TailPositions = new();

            public Rope(int length, List<string> moves)
            {
                for (int i = 0; i < length; i++) { Knots.Add(new Point(0, 0)); }
                Moves = moves;
            }

            public int TailCount => TailPositions.Count;
            private Point Head => Knots[0];
            private Point Tail => Knots[^1];

            public void Simulate()
            {
                foreach (var move in Moves) { MoveHead(move[0], int.Parse(move[1..])); }
            }

            private void MoveHead(char dir, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    Knots[0] = dir switch
                    {
                        'U' => new(Head.X, Head.Y + 1),
                        'R' => new(Head.X + 1, Head.Y),
                        'D' => new(Head.X, Head.Y - 1),
                        'L' => new(Head.X - 1, Head.Y),
                        _ => throw new InvalidOperationException()
                    };
                    for (int k = 0; k < Knots.Count - 1; k++) { MoveKnot(k, k + 1); }
                    TailPositions.Add(Tail);
                }
            }

            private void MoveKnot(int parent, int child)
            {
                var Parent = Knots[parent];
                var Child = Knots[child];
                var DX = Parent.X - Child.X;
                var DY = Parent.Y - Child.Y;
                if (Math.Abs(DX) > 1 || Math.Abs(DY) > 1)
                {
                    Child = (Math.Abs(DX), Math.Abs(DY)) switch
                    {
                        (2, 2) => new(Parent.X - Math.Sign(DX), Parent.Y - Math.Sign(DY)),
                        (2, _) => new(Parent.X - Math.Sign(DX), Parent.Y),
                        (_, 2) => new(Parent.X, Parent.Y - Math.Sign(DY)),
                        _ => throw new InvalidOperationException()
                    };
                }

                Knots[child] = Child;
            }
        }
    }
}