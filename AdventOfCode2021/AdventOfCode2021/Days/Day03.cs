using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day03 : BaseDay
	{
		#region Overrides

		public Day03() : base(3) { }

		protected override string SolvePartOne()
		{
			var R = CalculatePower(Lines);
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = CalculateSupport(Lines);
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = CalculatePower(LinesTest);
			return R == 198;
		}

		protected override bool TestPartTwo()
		{
			var R = CalculateSupport(LinesTest);
			return R == 230;
		}

		#endregion Overrides

		private static int CalculatePower(List<string> BitsList)
		{
			var Count = new Dictionary<int, BitCount>();
			var Length = BitsList[0].Length;
			for (int i = 0; i < Length; i++) { Count[i] = new BitCount(); }
			foreach (var Bits in BitsList)
			{
				for (int i = 0; i < Length; i++)
				{
					Count[i].Add(Bits[i]);
				}
			}
			string Gamma = "", Epsilon = "";
			foreach (var KV in Count)
			{
				Gamma += KV.Value.Common;
				Epsilon += KV.Value.Uncommon;
			}

			return Convert.ToInt32(Gamma, 2) * Convert.ToInt32(Epsilon, 2);
		}

		private static int CalculateSupport(List<string> BitsList)
		{
			var O2 = Filter(BitsList, true);
			var CO2 = Filter(BitsList, false);
			return Convert.ToInt32(O2, 2) * Convert.ToInt32(CO2, 2);
		}

		private static string Filter(IEnumerable<string> BitsList, bool Common, int Index = 0)
		{
			if (BitsList.Count() == 1) { return BitsList.First(); }
			var Count = new BitCount();
			foreach (var Bits in BitsList)
			{
				Count.Add(Bits[Index]);
			}
			var FilterBit = Common ? Count.Common : Count.Uncommon;
			var Filtered = BitsList.Where(Bits => Bits[Index] == FilterBit);
			return Filter(Filtered, Common, Index + 1);
		}

		private class BitCount
		{
			public char Common => CountOne > CountZero ? '0' : '1';
			public int CountOne { get; set; }
			public int CountZero { get; set; }
			public char Uncommon => CountOne > CountZero ? '1' : '0';

			public void Add(char Bit) => _ = Bit == '0' ? CountOne++ : CountZero++;
		}
	}
}