using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day13 : BaseDay
	{
		#region Overrides

		public Day13() : base(13) { }

		protected override string SolvePartOne()
		{
			var P = new Paper(Raw);
			P.FoldOnce();
			var R = P.DotsCount;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var P = new Paper(Raw);
			P.Fold();
			return "";
		}

		protected override bool TestPartOne()
		{
			var P = new Paper(RawTest);
			P.FoldOnce();
			var R = P.DotsCount;
			return R == 17;
		}

		protected override bool TestPartTwo()
		{
			var P = new Paper(RawTest);
			P.Fold();
			return true;
		}

		#endregion Overrides

		private class Paper
		{
			private readonly HashSet<Point> Dots = new();
			private readonly List<Fold> Folds = new();

			public Paper(string paper)
			{
				var P = paper.Split("\r\n\r\n");
				foreach (var Dot in P[0].SplitToList())
				{
					var D = Dot.ToIntList();
					Dots.Add(new Point(D[0], D[1]));
				}
				foreach (var Fold in P[1].SplitToList())
				{
					var F = Fold[11..].Split('=');
					var A = F[0][0];
					var V = int.Parse(F[1]);
					Folds.Add(new Fold(A, V));
				}
			}

			public int DotsCount => Dots.Count;

			public void Fold()
			{
				Folds.ForEach(F => FoldPaper(F));
				var MX = Dots.Max(D => D.X) + 1;
				var MY = Dots.Max(D => D.Y) + 1;
				for (int Y = 0; Y < MY; Y++)
				{
					var L = "";
					for (int X = 0; X < MX; X++)
					{
						L += Dots.Contains(new Point(X, Y)) ? '#' : '.';
					}
					Console.WriteLine(L);
				}
				Console.WriteLine();
			}

			public void FoldOnce() => FoldPaper(Folds[0]);

			private void FoldPaper(Fold fold)
			{
				var FoldedDots = Dots.Where(D => (fold.Axis == 'x' ? D.X : D.Y) > fold.Value).ToList();
				foreach (var D in FoldedDots)
				{
					Point P = fold.Axis switch
					{
						'x' => new Point(2 * fold.Value - D.X, D.Y),
						'y' => new Point(D.X, 2 * fold.Value - D.Y),
						_ => null
					};
					Dots.Add(P);
					Dots.Remove(D);
				}
			}
		}
		private record Point(int X, int Y);

		private record Fold(char Axis, int Value);
	}
}