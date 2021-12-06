using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day06 : BaseDay
	{
		#region Overrides

		public Day06() : base(6) { }

		protected override string SolvePartOne()
		{
			var R = new Lanternfishes(Raw.ToIntList());
			R.Simulate(80);
			return R.Count.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = new Lanternfishes(Raw.ToIntList());
			R.Simulate(256);
			return R.Count.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = new Lanternfishes(RawTest.ToIntList());
			R.Simulate(80);
			return R.Count == 5934;
		}

		protected override bool TestPartTwo()
		{
			var R = new Lanternfishes(RawTest.ToIntList());
			R.Simulate(256);
			return R.Count == 26984457539;
		}

		#endregion Overrides

		private class Lanternfishes
		{
			private readonly Dictionary<int, long> Fishes = new();

			public Lanternfishes(List<int> fishes)
			{
				for (int i = 0; i < 9; i++) { Fishes.Add(i, 0); }
				fishes.ForEach(F => Fishes[F]++);
			}

			public long Count => Fishes.Sum(F => F.Value);

			public void Simulate(int days)
			{
				for (int day = 0; day < days; day++)
				{
					var ZeroFishes = Fishes[0];
					for (int i = 1; i < 9; i++)
					{
						Fishes[i - 1] = Fishes[i];
						Fishes[i] = 0;
					}
					Fishes[6] += ZeroFishes;
					Fishes[8] += ZeroFishes;
				}
			}
		}
	}
}