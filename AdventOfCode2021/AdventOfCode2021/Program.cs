using AdventOfCode2021.Days;
using System;

namespace AdventOfCode2021
{
	internal class Program
	{
		private static void Main()
		{
			var Day = new Day02();
			var (PartOne, PartTwo) = Day.Solve();
			Console.WriteLine(PartOne);
			Console.WriteLine(PartTwo);
		}
	}
}