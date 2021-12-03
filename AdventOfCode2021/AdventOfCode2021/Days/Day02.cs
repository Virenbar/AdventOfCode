using System.Collections.Generic;

namespace AdventOfCode2021.Days
{
	public class Day02 : BaseDay
	{
		public Day02() : base(2) { }

		protected override string SolvePartOne()
		{
			var R = Calculate(Lines);
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = CalculateFixed(Lines);
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = Calculate(LinesTest);
			return R == 150;
		}

		protected override bool TestPartTwo()
		{
			var R = CalculateFixed(LinesTest);
			return R == 900;
		}

		private static int Calculate(List<string> commands)
		{
			var P = new Position();
			foreach (var C in commands)
			{
				var S = C.Split(' ');
				var Direction = S[0];
				var Value = int.Parse(S[1]);
				switch (Direction)
				{
					case "forward": P.HPos += Value; break;
					case "down": P.Depth += Value; break;
					case "up": P.Depth -= Value; break;
				}
			}
			return P.Depth * P.HPos;
		}

		private static int CalculateFixed(List<string> commands)
		{
			var P = new Position();
			foreach (var C in commands)
			{
				var S = C.Split(' ');
				var Direction = S[0];
				var Value = int.Parse(S[1]);
				switch (Direction)
				{
					case "forward":
						P.HPos += Value;
						P.Depth += P.Aim * Value;
						break;

					case "down": P.Aim += Value; break;
					case "up": P.Aim -= Value; break;
				}
			}
			return P.Depth * P.HPos;
		}

		private class Position
		{
			public int Aim { get; set; }
			public int Depth { get; set; }
			public int HPos { get; set; }
		}
	}
}