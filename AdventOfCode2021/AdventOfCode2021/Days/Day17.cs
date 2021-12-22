using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Days
{
	public class Day17 : BaseDay
	{
		#region Overrides

		public Day17() : base(17) { }

		protected override string SolvePartOne()
		{
			var PL = new ProbeLauncher(Raw);
			PL.Launch();
			var R = PL.MaxY;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var PL = new ProbeLauncher(Raw);
			PL.Launch();
			var R = PL.HitCount;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var PL = new ProbeLauncher(RawTest);
			PL.Launch();
			var R = PL.MaxY;
			return R == 45;
		}

		protected override bool TestPartTwo()
		{
			var PL = new ProbeLauncher(RawTest);
			PL.Launch();
			var R = PL.HitCount;
			return R == 112;
		}

		#endregion Overrides

		private class ProbeLauncher
		{
			private static readonly Regex R = new(@"=(?<x1>-?\d+)\.\.(?<x2>-?\d+).+=(?<y1>-?\d+)\.\.(?<y2>-?\d+)");
			private readonly List<int> MaxYList = new();
			private readonly int X1, X2, Y1, Y2;

			public ProbeLauncher(string target)
			{
				var M = R.Match(target);
				X1 = int.Parse(M.Groups["x1"].Value);
				X2 = int.Parse(M.Groups["x2"].Value);
				Y1 = int.Parse(M.Groups["y1"].Value);
				Y2 = int.Parse(M.Groups["y2"].Value);
			}

			public int HitCount { get; private set; }

			public int MaxY => MaxYList.Max();

			public void Launch()
			{
				for (int y = -1000; y < 1000; y++)
				{
					for (int x = 1; x < 1000; x++)
					{
						Launch(x, y);
					}
				}
			}

			private void Launch(int dX, int dY)
			{
				int X = 0, Y = 0;
				int MY = 0;

				while (X <= X2 && Y >= Y1)
				{
					X += dX;
					Y += dY;

					MY = Math.Max(MY, Y);

					if (dX > 0) { dX--; }
					else if (dX < 0) { dX++; }

					dY--;

					if ((X >= X1 && X <= X2) && (Y >= Y1 && Y <= Y2))
					{
						MaxYList.Add(MY);
						HitCount++;
						return;
					}
				}
			}
		}
	}
}