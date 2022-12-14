using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public partial class Day14 : BaseDay
    {
        #region Overrides

        public Day14() : base(14) { }

        protected override string SolvePartOne()
        {
            var R = CountSand(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CountSandWithFloor(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CountSand(LinesTest);
            return R == 24;
        }

        protected override bool TestPartTwo()
        {
            var R = CountSandWithFloor(LinesTest);
            return R == 93;
        }

        #endregion Overrides

        private static int CountSand(List<string> paths)
        {
            var Cave = new SandCave(paths);
            Cave.FillSand();
            return Cave.SandCount;
        }

        private static int CountSandWithFloor(List<string> paths)
        {
            var Cave = new SandCave(paths);
            Cave.FillSandToSource();
            return Cave.SandCount;
        }

        [GeneratedRegex("(?<X>\\d+),(?<Y>\\d+)")]
        private static partial Regex RegexPoint();

        private class SandCave
        {
            private readonly int MaxY;
            private readonly HashSet<Point> Rock = new();
            private readonly Regex RPoint = RegexPoint();
            private readonly HashSet<Point> Sand = new();
            private readonly Point SandSource = new(500, 0);

            public SandCave(List<string> paths)
            {
                foreach (var path in paths)
                {
                    var Path = RPoint.Matches(path).Select(M => new Point(int.Parse(M.Groups["X"].Value), int.Parse(M.Groups["Y"].Value))).ToList();
                    for (int i = 0; i < Path.Count - 1; i++)
                    {
                        AddPath(Path[i], Path[i + 1]);
                    }
                }
                MaxY = Rock.Max(R => R.Y);
            }

            public int SandCount => Sand.Count;

            public void FillSand()
            {
                while (AddSand()) { }
            }

            public void FillSandToSource()
            {
                var FloorY = MaxY + 2;
                AddPath(new(-10000, FloorY), new(10000, FloorY));
                while (AddSand()) { }
            }

            private void AddPath(Point start, Point end)
            {
                var DX = Math.Sign(end.X - start.X);
                var DY = Math.Sign(end.Y - start.Y);
                Point rock = new(start.X, start.Y);
                while (rock != end)
                {
                    Rock.Add(rock);
                    rock = new(rock.X + DX, rock.Y + DY);
                }
                Rock.Add(end);
            }

            private bool AddSand()
            {
                Point sand = SandSource;
                while (true)
                {
                    if (sand.Y > MaxY + 10 || Sand.Contains(SandSource)) { return false; }
                    Point D = new(sand.X, sand.Y + 1);
                    Point L = new(D.X - 1, D.Y);
                    Point R = new(D.X + 1, D.Y);

                    if (IsFree(D)) { sand = D; }
                    else if (IsFree(L)) { sand = L; }
                    else if (IsFree(R)) { sand = R; }
                    else
                    {
                        Sand.Add(sand);
                        return true;
                    }
                }
            }

            private bool IsFree(Point point) => !(Rock.Contains(point) || Sand.Contains(point));
        }
        private record Point(int X, int Y);
    }
}