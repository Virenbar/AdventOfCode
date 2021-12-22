using AdventOfCode2021.Types;
using System;
using System.Collections.Generic;

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

			public void FindPath()
			{
				SortedSet<PointF> Open = new(new PointFComparer()) { new PointF(Start, H(Start)) };
				DefaultDictionary<Point, int> GScore = new(10000);
				DefaultDictionary<Point, int> FScore = new(10000);
				GScore[Start] = 0;
				FScore[Start] = H(Start);

				while (Open.Count > 0)
				{
					var CurrentF = Open.Min;
					var Current = CurrentF.Point;
					if (Current == End)
					{
						MinRisk = GScore[Current];
						return;
					}

					Open.Remove(CurrentF);
					foreach (var P in Neighbors(Current))
					{
						var G = GScore[Current] + D(P);
						if (G < GScore[P])
						{
							GScore[P] = G;
							FScore[P] = G + H(P);
							var F = new PointF(P, FScore[P]);
							if (!Open.Contains(F)) { Open.Add(F); }
						}
					}
				}
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
		private record PointF(Point Point, int F);
		private class PointFComparer : IComparer<PointF>
		{
			public int Compare(PointF x, PointF y)
			{
				Point PX = x.Point, PY = y.Point;
				if (PX.X == PY.X && PX.Y == PY.Y) { return 0; }

				var D = x.F - y.F;
				if (D != 0) { return D; }

				var dX = PX.X - PY.X;
				if (dX != 0) { return dX; }
				return PX.Y - PY.Y;
			}
		}
	}
}