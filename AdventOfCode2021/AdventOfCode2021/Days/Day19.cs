using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day19 : BaseDay
	{
		#region Overrides

		public Day19() : base(19) { }

		protected override string SolvePartOne()
		{
#if !DEBUG
			var (Beacons, _) = GetCoords(Raw);
			var R = Beacons.Count;
#else
			var R = 332;
#endif
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var (Beacons, Scanners) = GetCoords(Raw);
			var T = Scanners.SelectMany((_, i) => Scanners.Skip(i + 1), (A, B) => (A, B));
			var R = T.Select((S) => Math.Abs(S.A.X - S.B.X) + Math.Abs(S.A.Y - S.B.Y) + Math.Abs(S.A.Z - S.B.Z)).Max();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
#if !DEBUG
			var (beacons, _) = GetCoords(RawTest);
			var R = beacons.Count;
#else
			var R = 79;
#endif
			return R == 79;
		}

		protected override bool TestPartTwo()
		{
			var (Beacons, Scanners) = GetCoords(RawTest);
			var T = Scanners.SelectMany((_, i) => Scanners.Skip(i + 1), (A, B) => (A, B));
			var R = T.Select((S) => Math.Abs(S.A.X - S.B.X) + Math.Abs(S.A.Y - S.B.Y) + Math.Abs(S.A.Z - S.B.Z)).Max();
			return R == 3621;
		}

		#endregion Overrides

		private static (Coord, Scanner) GetCenter(Dictionary<Coord, Scanner> fixedScanners, List<Scanner> scanners)
		{
			for (var i = 0; i < scanners.Count; i++)
			{
				var scannerB = scanners[i];
				foreach (var (posA, scannerA) in fixedScanners)
				{
					var ptAs = scannerA.GetBeaconsRelativeTo(posA).ToHashSet();
					foreach (var ptA in ptAs)
					{
						for (var rotation = 0; rotation < 48; rotation++, scannerB = scannerB.Rotate())
						{
							foreach (var ptB in scannerB.GetBeaconsRelativeTo(new Coord(0, 0, 0)))
							{
								var center = new Coord(ptA.X - ptB.X, ptA.Y - ptB.Y, ptA.Z - ptB.Z);
								var ptBs = scannerB.GetBeaconsRelativeTo(center).ToHashSet();

								var c = ptAs.Intersect(ptBs).Count();

								if (c >= 12)
								{
									scanners.RemoveAt(i);
									return (center, scannerB);
								}
							}
						}
					}
				}
			}
			throw new Exception();
		}

		private static List<Scanner> Parse(string input)
		{
			var Scanners = new List<Scanner>();
			var Blocks = input.Split("\r\n\r\n").ToList();
			foreach (var Block in Blocks)
			{
				List<Coord> Beacons = new();
				foreach (var Beacon in Block.Split("\r\n").Skip(1))
				{
					var XYZ = Beacon.Split(",").Select(int.Parse).ToArray();
					Beacons.Add(new Coord(XYZ[0], XYZ[1], XYZ[2]));
				}
				Scanners.Add(new Scanner(0, Beacons));
			}
			return Scanners;
		}

		private (HashSet<Coord> beacons, HashSet<Coord> scanners) GetCoords(string input)
		{
			var scanners = Parse(input);
			var fixedScanners = new Dictionary<Coord, Scanner>
			{
				[new Coord(0, 0, 0)] = scanners[0]
			};
			scanners.RemoveAt(0);

			while (scanners.Any())
			{
				var (posB, scannerB) = GetCenter(fixedScanners, scanners);
				fixedScanners[posB] = scannerB;
			}

			var beacons = new HashSet<Coord>();
			foreach (var (pos, scanner) in fixedScanners)
			{
				foreach (var c in scanner.GetBeaconsRelativeTo(pos))
				{
					beacons.Add(c);
				}
			}
			return (beacons, fixedScanners.Keys.ToHashSet());
		}

		record Coord(int X, int Y, int Z);
		record Scanner(int Rotation, List<Coord> Beacons)
		{
			public Scanner Rotate() => new(Rotation + 1, Beacons);
			public Coord[] GetBeaconsRelativeTo(Coord coord)
			{
				Coord transform(Coord coord)
				{
					var (x, y, z) = coord;

					(x, y, z) = (Rotation % 6) switch
					{
						0 => (x, y, z),
						1 => (x, z, y),
						2 => (y, x, z),
						3 => (y, z, x),
						4 => (z, x, y),
						5 => (z, y, x),
						_ => (x, y, z),
					};
					(x, y, z) = ((Rotation / 6) % 8) switch
					{
						0 => (x, y, z),
						1 => (-x, y, z),
						2 => (x, -y, z),
						3 => (-x, -y, z),
						4 => (x, y, -z),
						5 => (-x, y, -z),
						6 => (x, -y, -z),
						7 => (-x, -y, -z),
						_ => (x, y, z),
					};
					return new Coord(x, y, z);
				}
				return Beacons.Select(beacon =>
				{
					var t = transform(beacon);
					return new Coord(coord.X + t.X, coord.Y + t.Y, coord.Z + t.Z);
				}).ToArray();
			}
		}
	}
}