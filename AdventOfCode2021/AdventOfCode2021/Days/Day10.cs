using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day10 : BaseDay
	{
		#region Overrides

		public Day10() : base(10) { }

		protected override string SolvePartOne()
		{
			var SC = new SyntaxChecker(Lines);
			SC.Check();
			var R = SC.Score;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var SC = new SyntaxChecker(Lines);
			SC.Fix();
			var R = SC.Score;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var SC = new SyntaxChecker(LinesTest);
			SC.Check();
			var R = SC.Score;
			return R == 26397;
		}

		protected override bool TestPartTwo()
		{
			var SC = new SyntaxChecker(LinesTest);
			SC.Fix();
			var R = SC.Score;
			return R == 288957;
		}

		#endregion Overrides

		private static int CharScore(char C)
		{
			return C switch
			{
				')' => 3,
				']' => 57,
				'}' => 1197,
				'>' => 25137,
				'(' => 1,
				'[' => 2,
				'{' => 3,
				'<' => 4,
				_ => 0,
			};
		}

		private class SyntaxChecker
		{
			private readonly List<string> Lines;
			private readonly Stack<char> Stack = new();

			public SyntaxChecker(List<string> lines) => Lines = lines;

			public long Score { get; private set; }

			public void Check() => Score = Lines.Sum(L => CheckLine(L));

			public void Fix()
			{
				var Scores = Lines.Select(L => FixLine(L)).Where(S => S != 0).OrderBy(S => S).ToList();
				Score = Scores[Scores.Count / 2];
			}

			private int CheckChar(char C)
			{
				if (C is '(' or '[' or '{' or '<')
				{
					Stack.Push(C);
					return 0;
				}
				else
				{
					var O = Stack.Pop();
					var V = (O, C) switch
					{
						('(', ')') => 0,
						('[', ']') => 0,
						('{', '}') => 0,
						('<', '>') => 0,
						_ => CharScore(C),
					};
					return V;
				}
			}

			private int CheckLine(string Line)
			{
				Stack.Clear();
				foreach (char C in Line)
				{
					var Score = CheckChar(C);
					if (Score != 0) { return Score; }
				}
				return 0;
			}

			private long FixLine(string Line)
			{
				Stack.Clear();
				foreach (char C in Line)
				{
					var S = CheckChar(C);
					if (S != 0) { return 0; }
				}
				return Stack.Aggregate(0L, (S, C) => S * 5 + CharScore(C));
			}
		}
	}
}