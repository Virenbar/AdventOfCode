using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day04 : BaseDay
	{
		public Day04() : base(4) { }

		protected override string SolvePartOne()
		{
			var R = new Bingo(Raw).Play();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = new Bingo(Raw).PlayLast();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = new Bingo(RawTest).Play();
			return R == 4512;
		}

		protected override bool TestPartTwo()
		{
			var R = new Bingo(RawTest).PlayLast();
			return R == 1924;
		}

		private class Bingo
		{
			private readonly List<Board> Boards;
			private readonly List<int> Numbers;
			private int LastNumber = 0;
			private Board Winner = null;

			public Bingo(string bingo)
			{
				var b = bingo.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
				Numbers = b[0].Split(',').Select(int.Parse).ToList();
				Boards = b.Skip(1).Select(B => new Board(B)).ToList();
			}

			public int Play()
			{
				foreach (int Number in Numbers)
				{
					LastNumber = Number;
					Boards.ForEach(B => B.AddNumber(Number));
					Winner = Boards.FirstOrDefault(B => B.IsBingo);
					if (Winner != null) { break; }
				}
				return Winner.UnmarkedSum * LastNumber;
			}

			public int PlayLast()
			{
				foreach (int Number in Numbers)
				{
					LastNumber = Number;
					Boards.ForEach(B => B.AddNumber(Number));
					Winner = Boards.FirstOrDefault(B => B.IsBingo);
					if (Winner != null)
					{
						if (Boards.Count == 1) { break; }
						Boards.RemoveAll(B => B.IsBingo);
					}
				}
				return Winner.UnmarkedSum * LastNumber;
			}
		}
		private class Board
		{
			private readonly Dictionary<int, (int Row, int Column)> Marked = new();
			private readonly Dictionary<int, (int Row, int Column)> Unmarked = new();

			public Board(string board)
			{
				var Numbers = board.Split(new string[] { "\r\n", " " }, StringSplitOptions.RemoveEmptyEntries);
				for (int r = 0; r < 5; r++)
				{
					for (int c = 0; c < 5; c++)
					{
						Unmarked[int.Parse(Numbers[r * 5 + c])] = (r, c);
					}
				}
			}

			public bool IsBingo { get; private set; }
			public int UnmarkedSum => Unmarked.Keys.Sum();

			public void AddNumber(int number)
			{
				if (IsBingo) { return; }
				if (Unmarked.ContainsKey(number))
				{
					Marked.Add(number, Unmarked[number]);
					Unmarked.Remove(number);
					IsBingo = CheckBingo();
				}
			}

			private bool CheckBingo()
			{
				for (int i = 0; i < 5; i++)
				{
					if (Marked.Values.Where(V => V.Row == i).Count() == 5) { return true; }
					if (Marked.Values.Where(V => V.Column == i).Count() == 5) { return true; }
				}
				return false;
			}
		}
	}
}