using AdventOfCode2021.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day14 : BaseDay
	{
		#region Overrides

		public Day14() : base(14) { }

		protected override string SolvePartOne()
		{
			var P = new Polymerization(Raw);
			P.Apply(10);
			var R = P.CountMinMax();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var P = new Polymerization(Raw);
			P.Apply(40);
			var R = P.CountMinMax();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var P = new Polymerization(RawTest);
			P.Apply(10);
			var R = P.CountMinMax();
			return R == 1588;
		}

		protected override bool TestPartTwo()
		{
			var P = new Polymerization(RawTest);
			P.Apply(40);
			var R = P.CountMinMax();
			return R == 2188189693529;
		}

		#endregion Overrides

		private class Polymerization
		{
			private readonly Dictionary<(char, char), char> Pairs = new();

			private DefaultDictionary<(char First, char Second), long> PairCount = new();

			public Polymerization(string polymer)
			{
				var Parts = polymer.Split("\r\n\r\n");
				var Polymer = Parts[0];
				for (int i = 0; i < Polymer.Length - 1; i++)
				{
					var Pair = (Polymer[i], Polymer[i + 1]);
					PairCount[Pair]++;
				}

				foreach (var Pair in Parts[1].SplitToList())
				{
					var P = Pair.Split(" -> ");
					Pairs.Add((P[0][0], P[0][1]), P[1][0]);
				}
			}

			public void Apply(int count)
			{
				for (int i = 0; i < count; i++) { Apply(); }
			}

			public long CountMinMax()
			{
				var T0 = PairCount.First();
				var T = PairCount.Select(P => (Char: P.Key.Second, P.Value)).GroupBy(C => C.Char).ToDictionary(K => K.Key, V => V.Sum(G => G.Value));
				T[T0.Key.First] += T0.Value;

				var (Min, Max) = T.Values.Aggregate((Min: long.MaxValue, Max: 0L), (S, G) => (Math.Min(S.Min, G), Math.Max(S.Max, G)));
				return Max - Min;
			}

			private void Apply()
			{
				DefaultDictionary<(char, char), long> P = new();
				foreach (var (Pair, Value) in PairCount)
				{
					if (Pairs.ContainsKey(Pair))
					{
						var Insert = Pairs[Pair];
						P[(Pair.First, Insert)] += Value;
						P[(Insert, Pair.Second)] += Value;
					}
					else
					{
						P[Pair] += Value;
					}
				}
				PairCount = P;
			}
		}
	}
}