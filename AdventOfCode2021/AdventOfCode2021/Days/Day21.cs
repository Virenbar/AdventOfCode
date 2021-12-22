using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day21 : BaseDay
	{
		#region Overrides

		public Day21() : base(21) { }

		protected override string SolvePartOne()
		{
			var DD = new DiceGame(10, 8);
			var R = DD.Play();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var DD = new DiracGame(10, 8);
			var (P1, P2) = DD.Play();
			return $"{(P1 > P2 ? P1 : P2)}";
		}

		protected override bool TestPartOne()
		{
			var DD = new DiceGame(4, 8);
			var R = DD.Play();
			return R == 739785;
		}

		protected override bool TestPartTwo()
		{
			var DD = new DiracGame(4, 8);
			var R = DD.Play();
			return R == (444356092776315, 341960390180808);
		}

		#endregion Overrides

		private class DeterministicDice
		{
			private int Value;
			public int RollCount { get; private set; }

			public int Roll()
			{
				RollCount++;
				Value %= 100;
				Value++;
				return Value;
			}
		}
		private class DiceGame
		{
			private readonly DeterministicDice Dice = new();
			private readonly Queue<Player> Players = new();

			public DiceGame(int P1, int P2)
			{
				Players.Enqueue(new Player(P1));
				Players.Enqueue(new Player(P2));
			}

			public int Play()
			{
				while (Players.Max(P => P.Score) < 1000) { PlayTurn(); }
				return Players.Min(P => P.Score) * Dice.RollCount;
			}

			private void PlayTurn()
			{
				var CurrentPlayer = Players.Dequeue();
				var Roll = Dice.Roll() + Dice.Roll() + Dice.Roll();
				CurrentPlayer = CurrentPlayer.Move(Roll);

				Players.Enqueue(CurrentPlayer);
			}
		}
		private class DiracGame
		{
			private static ImmutableArray<(int Move, long Count)> QuantumMoves = Enumerable.Range(1, 3)
				.SelectMany(X => Enumerable.Range(1, 3), (T1, T2) => (T1, T2))
				.SelectMany(X => Enumerable.Range(1, 3), (T, T3) => (T.T1, T.T2, T3))
				.Select(T => T.T1 + T.T2 + T.T3)
				.GroupBy(T => T)
				.Select(G => (G.Key, G.LongCount()))
				.ToImmutableArray();

			private readonly Player P1;
			private readonly Player P2;

			public DiracGame(int P1, int P2)
			{
				this.P1 = new Player(P1);
				this.P2 = new Player(P2);
			}

			public (long P1, long P2) Play() => PlayTurn(P1, P2, 1);

			private (long P1, long P2) PlayTurn(Player P1, Player P2, int TurnCount)
			{
				if (P1.Score >= 21 || P2.Score >= 21) { return P1.Score > P2.Score ? (1, 0) : (0, 1); }

				if (TurnCount % 2 != 0)
				{
					var Results = QuantumMoves.Select(M => (Result: PlayTurn(P1.Move(M.Move), P2, TurnCount + 1), M.Count));
					var Result = Results.Aggregate((P1: 0L, P2: 0L), (A, T) => (A.P1 + T.Result.P1 * T.Count, A.P2 + T.Result.P2 * T.Count));
					return Result;
				}
				else
				{
					var Results = QuantumMoves.Select(M => (Result: PlayTurn(P1, P2.Move(M.Move), TurnCount + 1), M.Count));
					var Result = Results.Aggregate((P1: 0L, P2: 0L), (A, T) => (A.P1 + T.Result.P1 * T.Count, A.P2 + T.Result.P2 * T.Count));
					return Result;
				}
			}
		}
		private record Player(int Position, int Score = 0)
		{
			public Player Move(int roll)
			{
				var P = ((Position + roll - 1) % 10) + 1;
				return new Player(P, Score + P);
			}
		}
	}
}