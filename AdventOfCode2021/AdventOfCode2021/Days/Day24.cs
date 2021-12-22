using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Days
{
	public class Day24 : BaseDay
	{
		#region Overrides

		public Day24() : base(24) { }

		protected override string SolvePartOne()
		{
			var MF = new ModelFinder(Lines);
			var R = MF.FindModel();
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var R = CalculatePartTwo(Lines);
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var R = CalculatePartOne(LinesTest);
			return R == 0;
		}

		protected override bool TestPartTwo()
		{
			var R = CalculatePartTwo(LinesTest);
			return R == 0;
		}

		private static int CalculatePartOne(List<string> _)
		{
			return 0;
		}

		private static int CalculatePartTwo(List<string> _)
		{
			return 0;
		}

		#endregion Overrides

		private enum OP
		{
			inp,
			add,
			mul,
			div,
			mod,
			eql
		}

		private class ALU
		{
			private readonly List<Instruction> Program = new();
			private readonly Dictionary<char, int> Vars = new();
			private Queue<int> Input;

			public ALU(List<string> program)
			{
				foreach (var I in program)
				{
					var O = I.Split(' ');
					var P = (OP)Enum.Parse(typeof(OP), O[0], true);
					var A =char.ToUpper( O[1][0]);
					if (O.Length <= 2)
					{
						Program.Add(new Instruction(P, A, ' ', null));
					}
					else if (int.TryParse(O[2], out int Value))
					{
						Program.Add(new Instruction(P, A, ' ', Value));
					}
					else
					{
						var B =char.ToUpper( O[2][0]);
						Program.Add(new Instruction(P, A, B, null));
					}
				}
			}

			public int W => Vars['W'];
			public int X => Vars['X'];
			public int Y => Vars['Y'];
			public int Z => Vars['Z'];

			public int CheckModel(long model)
			{
				Vars['W'] = 0; Vars['X'] = 0; Vars['Y'] = 0; Vars['Z'] = 0;
				var Digits = model.ToString().Select(c => (int)char.GetNumericValue(c));
				Input = new(Digits);
				Program.ForEach(I => Execute(I));
				return Z;
			}

			private void Execute(Instruction I)
			{
				switch (I.Operation)
				{
					case OP.inp:
						Vars[I.A] = Input.Dequeue();
						break;

					case OP.add:
						Vars[I.A] += I.IsValue ? I.Value.Value : Vars[I.B];
						break;

					case OP.mul:
						Vars[I.A] *= I.IsValue ? I.Value.Value : Vars[I.B];
						break;

					case OP.div:
						Vars[I.A] /= I.IsValue ? I.Value.Value : Vars[I.B];
						break;

					case OP.mod:
						Vars[I.A] %= I.IsValue ? I.Value.Value : Vars[I.B];
						break;

					case OP.eql:
						Vars[I.A] = Vars[I.A] == (I.IsValue ? I.Value.Value : Vars[I.B]) ? 1 : 0;
						break;

					default:
						break;
				}
			}
		}
		private class ModelFinder
		{
			private long Model = 100_000_000_000_000;
			private readonly ALU MONAD;

			public ModelFinder(List<string> program) => MONAD = new(program);

			public long FindModel()
			{
				while (true)
				{
					Model--;
					if (Model.ToString().Contains('0')) { continue; }
					MONAD.CheckModel(Model);
					if (MONAD.Z == 0) { return Model; }
				}
			}
		}
		private record Instruction(OP Operation, char A, char B, int? Value)
		{
			public bool IsValue => Value.HasValue;
		}
	}
}