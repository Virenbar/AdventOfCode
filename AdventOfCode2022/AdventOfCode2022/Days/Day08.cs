using AdventOfCode2022.Types;

namespace AdventOfCode2022.Days
{
    public class Day08 : BaseDay
    {
        #region Overrides

        public Day08() : base(8) { }

        protected override string SolvePartOne()
        {
            var R = CountVisibleTrees(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindHighestScore(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CountVisibleTrees(RawTest);
            return R == 21;
        }

        protected override bool TestPartTwo()
        {
            var R = FindHighestScore(RawTest);
            return R == 8;
        }

        #endregion Overrides

        private static int CountVisibleTrees(string trees)
        {
            var Patch = new TreePatch(trees);
            return Patch.CountVisible();
        }

        private static int FindHighestScore(string trees)
        {
            var Patch = new TreePatch(trees);
            return Patch.FindMaxScore();
        }

        private class TreePatch
        {
            private record Point(int X, int Y);

            private readonly int MaxX;
            private readonly int MaxY;
            private readonly Dictionary<Point, int> Trees = new();

            public TreePatch(string trees)
            {
                var rows = trees.SplitToList();
                MaxY = rows.Count - 1;
                MaxX = rows[0].Length - 1;
                for (int Y = 0; Y < rows.Count; Y++)
                {
                    var row = rows[Y];
                    for (int X = 0; X < row.Length; X++)
                    {
                        Trees[new(X, Y)] = int.Parse(row[X].ToString());
                    }
                }
            }

            public int CountVisible() => Trees.Count(KV => IsVisible(KV.Key));

            public int FindMaxScore() => Trees.Max(KV => GetScore(KV.Key));

            private int GetScore(Point tree)
            {
                var Height = Trees[tree];
                int Top = 0, Right = 0, Bottom = 0, Left = 0;
                for (int X = tree.X - 1; X >= 0; X--)
                {
                    Left++;
                    if (Trees[new(X, tree.Y)] >= Height) { break; }
                }
                for (int X = tree.X + 1; X <= MaxX; X++)
                {
                    Right++;
                    if (Trees[new(X, tree.Y)] >= Height) { break; }
                }
                for (int Y = tree.Y - 1; Y >= 0; Y--)
                {
                    Top++;
                    if (Trees[new(tree.X, Y)] >= Height) { break; }
                }
                for (int Y = tree.Y + 1; Y <= MaxY; Y++)
                {
                    Bottom++;
                    if (Trees[new(tree.X, Y)] >= Height) { break; }
                }
                return Top * Right * Bottom * Left;
            }

            private bool IsVisible(Point tree)
            {
                var Height = Trees[tree];
                var Tall = Trees.Where(KV => KV.Value >= Height).ToList();
                var Left = !Tall.Where(KV => KV.Key.X < tree.X && KV.Key.Y == tree.Y).Any();
                var Right = !Tall.Where(KV => KV.Key.X > tree.X && KV.Key.Y == tree.Y).Any();
                var Top = !Tall.Where(KV => KV.Key.X == tree.X && KV.Key.Y < tree.Y).Any();
                var Bottom = !Tall.Where(KV => KV.Key.X == tree.X && KV.Key.Y > tree.Y).Any();
                return Top || Right || Bottom || Left;
            }
        }
    }
}