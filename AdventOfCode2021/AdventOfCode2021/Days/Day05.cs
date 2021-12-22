using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Days
{
	public class Day05 : BaseDay
	{
		private static readonly Regex VentR = new(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");

		#region Overrides

		public Day05() : base(5) { }

		protected override string SolvePartOne()
		{
			var R = new VentsDiagram(Lines).CountOverlaps;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = new VentsDiagram(Lines, false).CountOverlaps;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = new VentsDiagram(LinesTest).CountOverlaps;
			return R == 5;
		}

		protected override bool TestPartTwo()
		{
			var R = new VentsDiagram(LinesTest, false).CountOverlaps;
			return R == 12;
		}

		#endregion Overrides

		private class VentsDiagram
		{
			private readonly Dictionary<(int X, int Y), int> VentsCount = new();

			public VentsDiagram(List<string> vents, bool skip = true)
			{
				foreach (var vent in vents)
				{
					var M = VentR.Match(vent);
					var X1 = int.Parse(M.Groups["x1"].Value);
					var Y1 = int.Parse(M.Groups["y1"].Value);
					var X2 = int.Parse(M.Groups["x2"].Value);
					var Y2 = int.Parse(M.Groups["y2"].Value);
					if (skip && X1 != X2 && Y1 != Y2) { continue; }
					var DX = Math.Sign(X2 - X1);
					var DY = Math.Sign(Y2 - Y1);
					for ((int X, int Y) V = (X1, Y1); V.X != X2 + DX || V.Y != Y2 + DY; V.X += DX, V.Y += DY)
					{
						if (!VentsCount.ContainsKey(V)) { VentsCount[V] = 0; }
						VentsCount[V]++;
					}
				}
			}

			public int CountOverlaps => VentsCount.Where(V => V.Value >= 2).Count();
		}
	}
}