using System.Collections.Generic;

namespace AdventOfCode2021.Days
{
	public class Day18 : BaseDay
	{
		#region Overrides

		public Day18() : base(18) { }

		protected override string SolvePartOne()
		{
			var H = new HomeWork(Lines);
			var R = H.Magnitude();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var H = new HomeWork(Lines);
			var R = H.MagnitudeOfTwo();
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var H = new HomeWork(LinesTest);
			var R = H.Magnitude();
			return R == 4140;
		}

		protected override bool TestPartTwo()
		{
			var H = new HomeWork(LinesTest);
			var R = H.MagnitudeOfTwo();
			return R == 3993;
		}

		#endregion Overrides

		private class FishNumber
		{
			public FishNumber Left, Right, Parent;
			public int Value;

			public FishNumber(int value) => Value = value;

			public FishNumber(FishNumber copy, FishNumber parent)
			{
				Left = copy.Left != null ? new FishNumber(copy.Left, this) : null;
				Right = copy.Right != null ? new FishNumber(copy.Right, this) : null;
				Value = copy.Value;
				Parent = parent;
			}

			public static void ExplodeLeft(FishNumber number)
			{
				FishNumber Parent = number.Parent;
				FishNumber temp = number;
				while (Parent != null)
				{
					if (Parent.Left != temp)
					{
						Parent = Parent.Left;
						break;
					}
					temp = Parent;
					Parent = Parent.Parent;
				}
				while (Parent != null)
				{
					if (Parent.IsValue())
					{
						Parent.Value += number.Left.Value;
						break;
					}
					Parent = Parent.Right;
				}
				number.Left = null;
			}

			public static void ExplodeRight(FishNumber number)
			{
				FishNumber Parent = number.Parent;
				FishNumber temp = number;
				while (Parent != null)
				{
					if (Parent.Right != temp)
					{
						Parent = Parent.Right;
						break;
					}
					temp = Parent;
					Parent = Parent.Parent;
				}
				while (Parent != null)
				{
					if (Parent.IsValue())
					{
						Parent.Value += number.Right.Value;
						break;
					}
					Parent = Parent.Left;
				}
				number.Right = null;
			}

			public static FishNumber operator +(FishNumber left, FishNumber right)
			{
				FishNumber Number = new(0);
				Number.Left = new FishNumber(left, Number);
				Number.Right = new FishNumber(right, Number);
				Number.Reduce();
				return Number;
			}

			public FishNumber FindNestedFour(int level = 0)
			{
				FishNumber left = Left?.FindNestedFour(level + 1);
				if (left != null) { return left; }
				FishNumber right = Right?.FindNestedFour(level + 1);
				if (right != null) { return right; }
				return Left != null && Right != null && level >= 4 ? this : null;
			}

			public FishNumber FindSplit()
			{
				FishNumber left = Left != null ? Left.FindSplit() : Value > 9 ? this : null;
				if (left != null) { return left; }
				FishNumber right = Right != null ? Right.FindSplit() : Value > 9 ? this : null;
				if (right != null) { return right; }
				return null;
			}

			public bool IsValue() => Left == null && Right == null;

			public int Magnitude()
			{
				if (IsValue()) { return Value; }
				return Left.Magnitude() * 3 + Right.Magnitude() * 2;
			}

			private void Reduce()
			{
				FishNumber Number = FindNestedFour();
				if (Number == null) { return; }

				ExplodeLeft(Number);
				ExplodeRight(Number);
				Number.Value = 0;
				Reduce();

				while ((Number = FindSplit()) != null)
				{
					Number.Left = new FishNumber(Number.Value / 2) { Parent = Number };
					Number.Right = new FishNumber((Number.Value + 1) / 2) { Parent = Number };
					Reduce();
				}
			}
		}
		private class HomeWork
		{
			private readonly List<FishNumber> Numbers = new();

			public HomeWork(List<string> numbers)
			{
				foreach (var number in numbers)
				{
					var NR = new NumberReader(number);
					Numbers.Add(NR.ReadSnailfish());
				}
			}

			public int Magnitude()
			{
				FishNumber Number = Numbers[0];
				for (int i = 1; i < Numbers.Count; i++)
				{
					Number += Numbers[i];
				}
				return Number.Magnitude();
			}

			public int MagnitudeOfTwo()
			{
				int R = 0;
				for (int i = 0; i < Numbers.Count; i++)
				{
					FishNumber snailfish = Numbers[i];
					for (int j = i + 1; j < Numbers.Count; j++)
					{
						FishNumber Sum = snailfish + Numbers[j];
						int Magnitude = Sum.Magnitude();
						if (Magnitude > R)
						{
							R = Magnitude;
						}
						Sum = Numbers[j] + snailfish;
						Magnitude = Sum.Magnitude();
						if (Magnitude > R)
						{
							R = Magnitude;
						}
					}
				}
				return R;
			}
		}
		private class NumberReader
		{
			private readonly string data;
			private int index;

			public NumberReader(string input) => data = input;

			public FishNumber ReadSnailfish(FishNumber parent = null)
			{
				FishNumber Number = new(0) { Parent = parent };
				while (!(index >= data.Length))
				{
					switch (data[index])
					{
						case '[': index++; Number.Left = ReadSnailfish(Number); break;
						case ',': index++; Number.Right = ReadSnailfish(Number); break;
						case ']': return Number;
						default: Number.Value = int.Parse(data[index].ToString()); ; return Number;
					}
					index++;
				}
				return Number;
			}
		}
	}
}