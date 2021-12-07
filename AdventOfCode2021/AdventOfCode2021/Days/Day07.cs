using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
	public class Day07 : BaseDay
	{
		#region Overrides

		public Day07() : base(7) { }

		protected override string SolvePartOne()
		{
			var R = new CrabSwarm(Raw.ToIntList()).FindLeastFuel();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = new CrabSwarm(Raw.ToIntList()).FindLeastFuelFix();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = new CrabSwarm(RawTest.ToIntList()).FindLeastFuel();
			return R == 37;
		}

		protected override bool TestPartTwo()
		{
			var R = new CrabSwarm(RawTest.ToIntList()).FindLeastFuelFix();
			return R == 168;
		}

		#endregion Overrides

		private class CrabSwarm
		{
			private readonly List<int> Crabs;

			public CrabSwarm(List<int> crabs) => Crabs = crabs;

			public int FindLeastFuel()
			{
				var MinFuel = int.MaxValue;
				var Max = Crabs.Max();
				for (int Y = Crabs.Min(); Y <= Max; Y++)
				{
					var Fuel = Crabs.Aggregate(0, (Sum, C) => Sum + Math.Abs(C - Y));
					MinFuel = Math.Min(MinFuel, Fuel);
				}
				return MinFuel;
			}

			public int FindLeastFuelFix()
			{
				var MinFuel = int.MaxValue;
				var Max = Crabs.Max();
				for (int Y = Crabs.Min(); Y <= Max; Y++)
				{
					var Fuel = Crabs.Aggregate(0, (Sum, C) =>
					{
						var F = Math.Abs(C - Y);
						return Sum + F * (F + 1) / 2;
					});
					MinFuel = Math.Min(MinFuel, Fuel);
				}
				return MinFuel;
			}
		}
	}
}