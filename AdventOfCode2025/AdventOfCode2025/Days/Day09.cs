using MoreLinq;

namespace AdventOfCode2025.Days
{
    public class Day09 : TDay
    {
        public Day09() : base(9) { }

        protected override long TestOneResult => 50;
        protected override long TestTwoResult => 24;

        protected override long CalculatePartOne(List<string> tiles)
        {
            var floor = new Floor(tiles);
            return floor.FindLargest();
        }

        protected override long CalculatePartTwo(List<string> tiles)
        {
            var floor = new Floor(tiles);
            return floor.FindLargestValid();
        }

        private class Floor
        {
            private readonly List<Tile> RedTiles = new();

            public Floor(List<string> tiles)
            {
                foreach (var tile in tiles)
                {
                    var p = tile.Split(',').Select(int.Parse).ToList();
                    RedTiles.Add(new Tile(p[0], p[1]));
                }
            }

            public long FindLargest()
            {
                var rectangles = Rectangles();
                return rectangles.First().Area;
            }

            public long FindLargestValid()
            {
                var rectangles = Rectangles();
                return rectangles.First(IsValid).Area;
            }

            private static long Area((Tile first, Tile second) pair)
            {
                Tile P1 = pair.first, P2 = pair.second;
                return (long)(Math.Abs(P1.X - P2.X) + 1) * (Math.Abs(P1.Y - P2.Y) + 1);
            }

            private bool IsValid(Rectangle rectangle)
            {
                var (T1, T2, _) = rectangle;
                foreach (var (p, q) in RedTiles.Lead(1, RedTiles[0], (p, q) => (p, q)))
                {
                    if (p.X == q.X)
                    {
                        if (
                            p.X.Between(Math.Min(T1.X, T2.X) + 1, Math.Max(T1.X, T2.X) - 1)
                            && Math.Min(T1.Y, T2.Y) < Math.Max(p.Y, q.Y)
                            && Math.Max(T1.Y, T2.Y) > Math.Min(p.Y, q.Y)
                        )
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (
                            p.Y.Between(Math.Min(T1.Y, T2.Y) + 1, Math.Max(T1.Y, T2.Y) - 1)
                            && Math.Min(T1.X, T2.X) < Math.Max(p.X, q.X)
                            && Math.Max(T1.X, T2.X) > Math.Min(p.X, q.X)
                        )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private List<Rectangle> Rectangles()
            {
                return RedTiles
                     .Pairs()
                     .Select(P => new Rectangle(P.First, P.Second, Area: Area(P)))
                     .OrderByDescending(P => P.Area)
                     .ToList();
            }

            private record Rectangle(Tile First, Tile Second, long Area);
            private record Tile(int X, int Y);
        }
    }
}