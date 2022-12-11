namespace AdventOfCode2022.Days
{
    public class Day10 : BaseDay
    {
        #region Overrides

        public Day10() : base(10) { }

        protected override string SolvePartOne()
        {
            var R = SignalStrengthSum(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = DrawImage(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = SignalStrengthSum(LinesTest);
            return R == 13140;
        }

        protected override bool TestPartTwo()
        {
            var R = DrawImage(LinesTest);
            return R == 0;
        }

        #endregion Overrides

        private static int SignalStrengthSum(List<string> instructions)
        {
            var VS = new VideoSystem(instructions);
            VS.Execute();
            return VS.FindSignalStrength();
        }

        private static int DrawImage(List<string> instructions)
        {
            var VS = new VideoSystem(instructions);
            VS.Execute();
            VS.DrawImage();
            return 0;
        }

        private class VideoSystem
        {
            private readonly List<string> Instructions;
            private readonly Dictionary<int, int> StrengthX = new();
            private int Cycle = 0;
            private int X = 1;

            public VideoSystem(List<string> instructions)
            {
                Instructions = instructions;
                StrengthX[0] = X;
            }

            public void Execute()
            {
                foreach (var instruction in Instructions)
                {
                    _ = instruction.Split() switch
                    {
                        ["noop"] => Noop(),
                        ["addx", var value] => AddX(int.Parse(value)),
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            public int FindSignalStrength()
            {
                var Sum = 0;
                for (int i = 20; i <= 220; i += 40)
                {
                    Sum += i * StrengthX[i];
                }
                return Sum;
            }

            private bool AddX(int value)
            {
                Cycle++;
                StrengthX[Cycle] = X;
                Cycle++;
                StrengthX[Cycle] = X;
                X += value;
                return true;
            }

            private bool Noop()
            {
                Cycle++;
                StrengthX[Cycle] = X;
                return true;
            }

            public void DrawImage()
            {
                for (int i = 1; i <= 240; i++)
                {
                    var X = StrengthX[i];
                    var CRT = (i - 1) % 40;
                    var Pixel = CRT == X - 1 || CRT == X || CRT == X + 1 ? '#' : '.';
                    Console.Write(Pixel);
                    if (CRT == 39) { Console.WriteLine(); }
                }
                Console.WriteLine();
            }
        }
    }
}