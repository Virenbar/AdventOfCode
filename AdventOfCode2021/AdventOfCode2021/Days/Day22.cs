using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day22 : BaseDay
	{
		#region Overrides

		public Day22() : base(22) { }

		protected override string SolvePartOne()
		{
			var RC = new Reactor(Lines);
			var R = RC.Initialize();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var RC = new Reactor(Lines);
			var R = RC.InitializeFull();

			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var RC = new Reactor(LinesTest);
			var R = RC.Initialize();

			return R == 590784;
		}

		protected override bool TestPartTwo()
		{
			var RC = new Reactor(LinesTestA);
			var R = RC.InitializeFull();

			return R == 2758514936282235;
		}

		#endregion Overrides

		private static bool Contains(Cuboid a, Cuboid b) => a.MinX <= b.MinX && a.MaxX >= b.MaxX && a.MinY <= b.MinY && a.MaxY >= b.MaxY && a.MinZ <= b.MinZ && a.MaxZ >= b.MaxZ;

		private static bool Intersects(Cuboid a, Cuboid b) => a.MinX <= b.MaxX && a.MaxX >= b.MinX && a.MinY <= b.MaxY && a.MaxY >= b.MinY && a.MinZ <= b.MaxZ && a.MaxZ >= b.MinZ;

		private static List<Cuboid> Substract(Cuboid C1, Cuboid C2)
		{
			if (Contains(C2, C1)) { return new List<Cuboid>(); }

			if (!Intersects(C1, C2)) { return new List<Cuboid>(new[] { C1 }); }

			var XSplits = new[] { C2.MinX, C2.MaxX }.Where((x) => C1.MinX < x && x < C1.MaxX);
			var YSplits = new[] { C2.MinY, C2.MaxY }.Where((y) => C1.MinY < y && y < C1.MaxY);
			var ZSplits = new[] { C2.MinZ, C2.MaxZ }.Where((z) => C1.MinZ < z && z < C1.MaxZ);

			var XV = XSplits.Prepend(C1.MinX).Append(C1.MaxX).ToList();
			var YV = YSplits.Prepend(C1.MinY).Append(C1.MaxY).ToList();
			var ZV = ZSplits.Prepend(C1.MinZ).Append(C1.MaxZ).ToList();

			var Splitted = new List<Cuboid>();

			for (var i = 0; i < XV.Count - 1; i++)
			{
				for (var j = 0; j < YV.Count - 1; j++)
				{
					for (var k = 0; k < ZV.Count - 1; k++)
					{
						Splitted.Add(new Cuboid(XV[i], XV[i + 1], YV[j], YV[j + 1], ZV[k], ZV[k + 1]));
					}
				}
			}

			return Splitted.Where((c) => !Contains(C2, c)).ToList();
		}

		private class Reactor
		{
			private readonly HashSet<Cube> OnCubes = new();
			private readonly List<Cuboid> Steps;
			private List<Cuboid> Cuboids = new();

			public Reactor(List<string> steps)
			{
				Steps = steps.Select(S => new Cuboid(S)).ToList();
			}

			public int Initialize()
			{
				Steps.ForEach(S => ApplyStep(S));
				return OnCubes.Count;
			}

			public long InitializeFull()
			{
				foreach (var Step in Steps)
				{
					var Cube = new Cuboid(Step);
					Cuboids = Cuboids.SelectMany(C => Substract(C, Cube)).ToList();
					if (Cube.State) { Cuboids.Add(Cube); }
				}
				return Cuboids.Sum(C => C.Volume);
			}

			private void ApplyStep(Cuboid step)
			{
				if (step.MaxX - step.MinX > 1000 ||
					step.MaxY - step.MinY > 1000 ||
					step.MaxZ - step.MinZ > 1000) { return; }

				for (int X = step.MinX; X <= step.MaxX; X++)
				{
					for (int Y = step.MinY; Y <= step.MaxY; Y++)
					{
						for (int Z = step.MinZ; Z <= step.MaxZ; Z++)
						{
							SetState(new Cube(X, Y, Z), step.State);
						}
					}
				}
			}

			private void SetState(Cube cube, bool state)
			{
				if (state) { OnCubes.Add(cube); } else { OnCubes.Remove(cube); }
			}
		}
		private record Cube(int X, int Y, int Z);
		private record Cuboid
		{
			public bool State { get; init; }
			public int MinX { get; init; }
			public int MaxX { get; init; }
			public int MinY { get; init; }
			public int MaxY { get; init; }
			public int MinZ { get; init; }
			public int MaxZ { get; init; }

			public long Volume => Math.Abs((long)(MaxX - MinX) * (MaxY - MinY) * (MaxZ - MinZ));

			public Cuboid(string step)
			{
				var S = step.Split(new[] { ' ', 'x', 'y', 'z', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
				State = S[0] == "on";
				var X = S[1].Split("..");
				var Y = S[2].Split("..");
				var Z = S[3].Split("..");
				MinX = int.Parse(X[0]);
				MaxX = int.Parse(X[1]);
				MinY = int.Parse(Y[0]);
				MaxY = int.Parse(Y[1]);
				MinZ = int.Parse(Z[0]);
				MaxZ = int.Parse(Z[1]);
			}
			public Cuboid(Cuboid cube)
			{
				State = cube.State;
				MinX = cube.MinX;
				MaxX = cube.MaxX + 1;
				MinY = cube.MinY;
				MaxY = cube.MaxY + 1;
				MinZ = cube.MinZ;
				MaxZ = cube.MaxZ + 1;
			}
			public Cuboid(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
			{
				State = true;
				MinX = minX;
				MaxX = maxX;
				MinY = minY;
				MaxY = maxY;
				MinZ = minZ;
				MaxZ = maxZ;
			}
		}
	}
}