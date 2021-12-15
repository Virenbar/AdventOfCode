using AdventOfCode2021.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
	public class Day15 : BaseDay
	{
		#region Overrides

		public Day15() : base(15) { }

		protected override string SolvePartOne()
		{
			var C = new Cave(Lines);
			C.FindPath();
			var R = C.MinRisk;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var C = new Cave(Lines, 5);
			C.FindPath();
			var R = C.MinRisk;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var C = new Cave(LinesTest);
			C.FindPath();
			var R = C.MinRisk;
			return R == 40;
		}

		protected override bool TestPartTwo()
		{
			var C = new Cave(LinesTest, 5);
			C.FindPath();
			var R = C.MinRisk;
			return R == 315;
		}

		#endregion Overrides

		private class Cave
		{
			private readonly Point End;
			private readonly Dictionary<Point, int> Risks = new();
			private readonly Point Start;

			public Cave(List<string> risks, int Size = 1)
			{
				var Width = risks[0].Length;
				var Height = risks.Count;
				Start = new Point(0, 0);
				End = new Point(Width * Size - 1, Height * Size - 1);
				for (int Y = 0; Y < risks.Count; Y++)
				{
					for (int X = 0; X < Width; X++)
					{
						var Risk = int.Parse(risks[Y][X].ToString());

						for (int dY = 0; dY < Size; dY++)
						{
							for (int dX = 0; dX < Size; dX++)
							{
								var R = (Risk + (dX + dY));
								if (R > 9) { R -= 9; }
								Risks.Add(new Point(X + Width * (dX), Y + Height * (dY)), R);
							}
						}
					}
				}
			}

			public int MinRisk { get; private set; }

			public void FindPath() => A_Star();

			private static List<Point> RestorePath(Dictionary<Point, Point> CameFrom, Point Current)
			{
				List<Point> Total = new() { Current };
				while (CameFrom.ContainsKey(Current))
				{
					Current = CameFrom[Current];
					Total.Insert(0, Current);
				}
				return Total;
			}

			private List<Point> A_Star()
			{
				HashSet<Point> Open = new() { Start };
				DefaultDictionary<Point, int> GScore = new(10000);
				GScore[Start] = 0;
				DefaultDictionary<Point, int> FScore = new(10000);
				FScore[Start] = H(Start);

				Dictionary<Point, Point> CameFrom = new();

				while (Open.Count > 0)
				{
					var M = Open.Min(P => FScore[P]);
					var Current = Open.Where(P => FScore[P] == M).First();
					if (Current == End)
					{
						MinRisk = GScore[Current];
						return RestorePath(CameFrom, Current);
					}

					Open.Remove(Current);
					foreach (var P in Neighbors(Current))
					{
						var G = GScore[Current] + D(P);
						if (G < GScore[P])
						{
							CameFrom[P] = Current;
							GScore[P] = G;
							FScore[P] = G + H(P);
							if (!Open.Contains(P)) { Open.Add(P); }
						}
					}
				}
				throw new Exception();
			}

			private int D(Point P) => Risks[P];

			private int H(Point P) => End.X - P.X + End.Y - P.Y;

			private List<Point> Neighbors(Point P)
			{
				List<Point> N = new();
				foreach (var (X, Y) in new[] { (1, 0), (0, 1), (-1, 0), (0, -1) })
				{
					var T = new Point(P.X + X, P.Y + Y);
					if (Risks.ContainsKey(T)) { N.Add(T); }
				}
				return N;
			}
		}
		private record Point(int X, int Y);
		private record FPoint(int Score, Point Point);
		private class PointComparer : IComparer<FPoint>
		{
			public int Compare(FPoint x, FPoint y) => Math.Sign(x.Score - y.Score);
		}
	}
}