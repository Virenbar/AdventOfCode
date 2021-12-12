using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day12 : BaseDay
	{
		#region Overrides

		public Day12() : base(12) { }

		protected override string SolvePartOne()
		{
			var CM = new CavesMap(Lines);
			var R = CM.CountPaths();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var CM = new CavesMap(Lines);
			var R = CM.CountPaths(true);
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var CM = new CavesMap(LinesTest);
			var R = CM.CountPaths();
			return R == 10;
		}

		protected override bool TestPartTwo()
		{
			var CM = new CavesMap(LinesTest);
			var R = CM.CountPaths(true);
			return R == 36;
		}

		#endregion Overrides

		private class Cave
		{
			public readonly Dictionary<string, Cave> Connections = new();

			public Cave(string name)
			{
				Name = name;
				IsSmall = char.IsLower(name[0]);
			}

			public bool IsSmall { get; private set; }
			public string Name { get; private set; }
		}
		private class CavesMap
		{
			private readonly Dictionary<string, Cave> Caves = new();

			public CavesMap(List<string> connections)
			{
				foreach (var connection in connections)
				{
					var C = connection.Split('-');
					string C0 = C[0], C1 = C[1];
					if (!Caves.ContainsKey(C0)) { Caves.Add(C0, new Cave(C0)); }
					if (!Caves.ContainsKey(C1)) { Caves.Add(C1, new Cave(C1)); }
					Caves[C0].Connections.Add(C1, Caves[C1]);
					Caves[C1].Connections.Add(C0, Caves[C0]);
				}
			}

			public int CountPaths(bool twice = false)
			{
				var Start = Caves["start"];
				var End = Caves["end"];
				return GetPaths(Start, End, twice).ToList().Count;
			}

			private static IEnumerable<string> GetPaths(Cave start, Cave end, bool twice)
			{
				var EndPaths = new List<string>();
				var Paths = new Queue<(Cave Cave, string Path, bool Twice)>();
				Paths.Enqueue((start, start.Name, false));

				while (Paths.Count > 0)
				{
					var (Cave, Path, Twice) = Paths.Dequeue();
					if (Cave == end)
					{
						EndPaths.Add(Path);
						continue;
					}

					foreach (var C in Cave.Connections.Values)
					{
						if (!twice)
						{
							//Part One
							if (!(C.IsSmall && Path.Contains(C.Name))) { Paths.Enqueue((C, $"{Path}-{C.Name}", false)); }
						}
						else
						{
							//Part Two
							if (!C.IsSmall)
							{
								Paths.Enqueue((C, $"{Path}-{C.Name}", Twice));
							}
							else if (!Twice)
							{
								if (!Path.Contains(C.Name))
								{
									Paths.Enqueue((C, $"{Path}-{C.Name}", false));
								}
								else if (C != start && C != end)
								{
									Paths.Enqueue((C, $"{Path}-{C.Name}", true));
								}
							}
							else
							{
								if (!Path.Contains(C.Name)) { Paths.Enqueue((C, $"{Path}-{C.Name}", true)); }
							}
						}
					}
				}
				return EndPaths;
			}
		}
	}
}