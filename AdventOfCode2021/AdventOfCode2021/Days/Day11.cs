using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day11 : BaseDay
	{
		#region Overrides

		public Day11() : base(11) { }

		protected override string SolvePartOne()
		{
			var Cave = new OctopusCave(Lines);
			Cave.Simulate();
			var R = Cave.TotalFlashes;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var Cave = new OctopusCave(Lines);
			var R = Cave.SimulateFlash();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var Cave = new OctopusCave(LinesTest);
			Cave.Simulate();
			var R = Cave.TotalFlashes;
			return R == 1656;
		}

		protected override bool TestPartTwo()
		{
			var Cave = new OctopusCave(LinesTest);
			var R = Cave.SimulateFlash();
			return R == 195;
		}

		#endregion Overrides

		private class Octopus
		{
			public List<Octopus> Adjacent = new();

			public Octopus(int energy) => Energy = energy;

			public int Energy { get; set; }
			public int Flashes { get; private set; }
			public bool IsFlashed { get; private set; }

			public void Reset()
			{
				if (IsFlashed)
				{
					Energy = 0;
					IsFlashed = false;
				}
			}

			public void TryFlash()
			{
				if (!IsFlashed && Energy > 9)
				{
					IsFlashed = true;
					Flashes++;
					Adjacent.ForEach(O => O.Energy++);
					Adjacent.ForEach(O => O.TryFlash());
				}
			}
		}

		private class OctopusCave
		{
			private readonly Dictionary<Point, Octopus> Octopuses = new();

			public OctopusCave(List<string> octopuses)
			{
				for (int Y = 0; Y < octopuses.Count; Y++)
				{
					for (int X = 0; X < octopuses[Y].Length; X++)
					{
						Octopuses.Add(new Point(X, Y), new Octopus(int.Parse(octopuses[Y][X].ToString())));
					}
				}
				foreach (var (P, O) in Octopuses) { FindAdjacent(P, O); }
			}

			public int TotalFlashes => Octopuses.Values.Sum(O => O.Flashes);

			private Octopus this[Point P] => Octopuses.ContainsKey(P) ? Octopuses[P] : null;

			public void FindAdjacent(Point P, Octopus octopus)
			{
				for (int X = -1; X <= 1; X++)
				{
					for (int Y = -1; Y <= 1; Y++)
					{
						if (X == 0 && Y == 0) { continue; }
						var O = this[new Point(P.X + X, P.Y + Y)];
						if (O != null) { octopus.Adjacent.Add(O); }
					}
				}
			}

			public void Simulate()
			{
				for (int i = 0; i < 100; i++) { Step(); }
			}

			public int SimulateFlash()
			{
				var i = 1;
				while (true)
				{
					Step();
					if (Octopuses.Values.All(O => O.IsFlashed)) { return i; }
					i++;
				}
			}

			private void Step()
			{
				foreach (var O in Octopuses.Values.Where(F => F.IsFlashed)) { O.Reset(); }
				foreach (var O in Octopuses.Values) { O.Energy++; }
				foreach (var O in Octopuses.Values) { O.TryFlash(); }
			}
		}

		private record Point(int X, int Y);
	}
}