using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day09 : BaseDay
	{
		#region Overrides

		public Day09() : base(9) { }

		protected override string SolvePartOne()
		{
			var H = new Heightmap(Lines);
			H.FindLowPoints();
			var R = H.SumRiskLevels;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var H = new Heightmap(Lines);
			H.FindLowPoints();
			H.FindBasins();
			var R = H.Top3Basins;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var H = new Heightmap(LinesTest);
			H.FindLowPoints();
			var R = H.SumRiskLevels;
			return R == 15;
		}

		protected override bool TestPartTwo()
		{
			var H = new Heightmap(LinesTest);
			H.FindLowPoints();
			H.FindBasins();
			var R = H.Top3Basins;
			return R == 1134;
		}

		#endregion Overrides

		private class Heightmap
		{
			private readonly List<HashSet<Point>> Basins = new();
			private readonly Dictionary<Point, int> LowPoints = new();
			private readonly Dictionary<Point, int> Points = new();

			public Heightmap(List<string> heightmap)
			{
				for (int Y = 0; Y < heightmap.Count; Y++)
				{
					for (int X = 0; X < heightmap[Y].Length; X++)
					{
						this[new Point(X, Y)] = int.Parse(heightmap[Y][X].ToString());
					}
				}
			}

			public int SumRiskLevels => LowPoints.Sum(P => P.Value + 1);
			public int Top3Basins => Basins.OrderByDescending(B => B.Count).Take(3).Aggregate(1, (M, B) => M * B.Count);

			private int this[Point P]
			{
				get => Points.ContainsKey(P) ? Points[P] : 10;
				set => Points[P] = value;
			}

			public void FindBasins()
			{
				foreach (var (LP, _) in LowPoints)
				{
					if (Basins.Any(B => B.Contains(LP))) { continue; }
					HashSet<Point> Basin = new();
					List<Point> NewPoints = new() { LP };
					while (NewPoints.Count > 0)
					{
						NewPoints.ForEach(P => Basin.Add(P));
						var Adjacent = NewPoints.SelectMany(NP => GetAdjacent(NP)).Distinct().Where(P => !Basin.Contains(P) && this[P] < 9).ToList();
						NewPoints.Clear();
						NewPoints.AddRange(Adjacent);
					}
					Basins.Add(Basin);
				}
			}

			public void FindLowPoints()
			{
				foreach (var (P, H) in Points)
				{
					var Adjacent = GetAdjacent(P);
					var IsLow = Adjacent.All(A => this[A] > H);
					if (IsLow) { LowPoints[P] = H; }
				}
			}

			private static List<Point> GetAdjacent(Point P) => new()
			{
				new Point(P.X + 1, P.Y),
				new Point(P.X - 1, P.Y),
				new Point(P.X, P.Y + 1),
				new Point(P.X, P.Y - 1)
			};

			private record Point(int X, int Y);
		}
	}
}