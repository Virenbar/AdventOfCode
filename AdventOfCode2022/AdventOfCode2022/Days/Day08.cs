using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Days
{
	public class Day08 : BaseDay
	{
		#region Overrides

		public Day08() : base(8) { }

		protected override string SolvePartOne()
		{
			var Displays = Lines.Select(D => new Display(D));
			var R = Displays.Sum(D => D.CountSimpleDigits());
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var Displays = Lines.Select(D => new Display(D));
			var R = Displays.Sum(D => D.ParceNumber());
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var Displays = LinesTest.Select(D => new Display(D));
			var R = Displays.Sum(D => D.CountSimpleDigits());
			return R == 26;
		}

		protected override bool TestPartTwo()
		{
			var Displays = LinesTest.Select(D => new Display(D));
			var R = Displays.Sum(D => D.ParceNumber());
			return R == 61229;
		}

		#endregion Overrides

		private class Display
		{
			private readonly List<HashSet<char>> Outputs;

			private readonly List<Signal> Signals;

			public Display(string display)
			{
				var D = display.Split('|');
				Signals = Map(D[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(O => O.ToHashSet()).ToList());
				Outputs = D[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(O => O.ToHashSet()).ToList();
			}

			public static List<Signal> Map(List<HashSet<char>> signals)
			{
				var D1 = signals.Where(S => S.Count == 2).First();
				var D4 = signals.Where(S => S.Count == 4).First();
				var D7 = signals.Where(S => S.Count == 3).First();
				var D8 = signals.Where(S => S.Count == 7).First();
				signals.RemoveAll(S => new[] { D1, D4, D7, D8 }.Contains(S));
				var BD = D4.Except(D1).ToHashSet();

				var D023 = signals.Where(S => !S.IsSupersetOf(BD));
				var D0 = D023.Where(S => S.Count == 6).First();
				var D2 = D023.Where(S => S.Count == 5 && !S.IsSupersetOf(D1)).First();
				var D3 = D023.Where(S => S.Count == 5 && S.IsSupersetOf(D1)).First();

				var D569 = signals.Where(S => S.IsSupersetOf(BD));
				var D5 = D569.Where(S => S.Count == 5).First();
				var D6 = D569.Where(S => S.Count == 6 && !S.IsSupersetOf(D1)).First();
				var D9 = D569.Where(S => S.IsSupersetOf(D1)).First();

				return new List<Signal> {
					new Signal(D0, 0),
					new Signal(D1, 1),
					new Signal(D2, 2),
					new Signal(D3, 3),
					new Signal(D4, 4),
					new Signal(D5, 5),
					new Signal(D6, 6),
					new Signal(D7, 7),
					new Signal(D8, 8),
					new Signal(D9, 9)
				};
			}

			public int CountSimpleDigits()
			{
				return OutputDigits().Where(D => new[] { 1, 4, 7, 8 }.Contains(D)).Count();
			}

			public int ParceNumber()
			{
				string Number = OutputDigits().Aggregate("", (N, D) => N + D.ToString());
				return int.Parse(Number);
			}

			private List<int> OutputDigits()
			{
				var S = Outputs.Select(O => Signals.Where(S => S.Pattern.SetEquals(O)).First());
				return S.Select(s => s.Digit).ToList();
			}
		}
		private class Signal
		{
			public Signal(HashSet<char> pattern, int digit)
			{
				Pattern = pattern;
				Digit = digit;
			}

			public int Digit { get; private set; } = -1;
			public HashSet<char> Pattern { get; private set; }
		}
	}
}