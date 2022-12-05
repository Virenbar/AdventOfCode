using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public class Day05 : BaseDay
    {
        #region Overrides

        public Day05() : base(5) { }

        protected override string SolvePartOne()
        {
            var R = MoveCrates(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = MoveCratesV2(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = MoveCrates(RawTest);
            return R == "CMZ";
        }

        protected override bool TestPartTwo()
        {
            var R = MoveCratesV2(RawTest);
            return R == "MCD";
        }

        #endregion Overrides

        private static string MoveCrates(string drawing)
        {
            var Cargo = new Cargo(drawing);
            Cargo.CrateMover9000();
            return Cargo.Peek();
        }

        private static string MoveCratesV2(string drawing)
        {
            var Cargo = new Cargo(drawing);
            Cargo.CrateMover9001();
            return Cargo.Peek();
        }

        private class Cargo
        {
            private readonly Dictionary<int, Stack<char>> Stacks = new();
            private readonly List<Step> Steps;

            public Cargo(string drawing)
            {
                var input = drawing.SplitBlocksToList();
                var stacks = input[0].SplitToList();
                var steps = input[1].SplitToList();
                var LastRow = stacks.Count - 1;
                for (int i = 1; i < stacks[LastRow].Length; i++)
                {
                    if (stacks[LastRow][i] == ' ') { continue; }

                    var column = int.Parse(stacks[LastRow][i].ToString());
                    Stacks[column] = new Stack<char>();
                    for (int row = stacks.Count - 2; row >= 0; row--)
                    {
                        if (stacks[row][i] != ' ')
                        {
                            Stacks[column].Push(stacks[row][i]);
                        }
                    }
                }
                Steps = steps.Select(M => Regex.Matches(M, @"\d+").Select(M => int.Parse(M.Value)).ToList()).Select(M => new Step(M[0], M[1], M[2])).ToList();
            }

            private record Step(int Count, int From, int To);

            public void CrateMover9000()
            {
                foreach (var Step in Steps)
                {
                    for (int i = 0; i < Step.Count; i++)
                    {
                        Stacks[Step.To].Push(Stacks[Step.From].Pop());
                    }
                }
            }

            public void CrateMover9001()
            {
                var Crane = new Stack<char>();
                foreach (var Step in Steps)
                {
                    for (int i = 0; i < Step.Count; i++)
                    {
                        Crane.Push(Stacks[Step.From].Pop());
                    }
                    for (int i = 0; i < Step.Count; i++)
                    {
                        Stacks[Step.To].Push(Crane.Pop());
                    }
                }
            }

            public string Peek() => string.Join("", Stacks.Select(S => S.Value.Peek()));
        }
    }
}