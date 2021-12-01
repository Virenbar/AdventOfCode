using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day01 : BaseDay
	{
		public Day01() : base(1) { }

		protected override string SolvePartOne()
		{
			var M = Lines.ToIntList();
			return CountIncreases(M).ToString();
		}

		protected override string SolvePartTwo()
		{
			var M = Lines.ToIntList();
			return CountIncreasesSum(M).ToString();
		}

		protected override bool TestPartOne()
		{
			var M = LinesTest.ToIntList();
			return CountIncreases(M) == 7;
		}

		protected override bool TestPartTwo()
		{
			var M = LinesTest.ToIntList();
			return CountIncreasesSum(M) == 5;
		}

		private static int CountIncreases(List<int> Measurments)
		{
			var C = 0;
			_ = Measurments.Aggregate((Prev, Curent) =>
			{
				if (Curent > Prev) { C++; }
				return Curent;
			});

			return C;
		}

		private static int CountIncreasesSum(List<int> Measurments)
		{
			var C = 0;
			_ = Measurments.Aggregate(0, (Index, _) =>
			{
				var S1 = Measurments.Skip(Index).Take(3).Sum();
				var S2 = Measurments.Skip(Index + 1).Take(3).Sum();
				if (S2 > S1) { C++; }
				return Index + 1;
			});

			return C;
		}
	}
}