using System.Diagnostics;

namespace AdventOfCode2023.Days
{
    public abstract class BaseDay
    {
        protected List<string> Lines;
        protected List<string> LinesTest, LinesTestA;
        protected string Raw;
        protected string RawTest, RawTestA;
        private readonly Stopwatch SW = new();

        public BaseDay(int day) => Day = day;

        public int Day { get; private set; }

        public (string PartOne, string PartTwo) Solve()
        {
            LoadDay();
            var T1 = TestPartOne();
            var T2 = TestPartTwo();
            if (!(T1 && T2))
            {
                var P1 = T1 ? "passed" : "failed";
                var P2 = T2 ? "passed" : "failed";
                return ($"Test One: {P1}", $"Test Two: {P2}");
            }

            var A1 = SolvePart(SolvePartOne);
            var A2 = SolvePart(SolvePartTwo);
            return ($"Part One: {A1.Answer} ({A1.Time})", $"Part Two: {A2.Answer} ({A2.Time})");
        }

        protected abstract string SolvePartOne();

        protected abstract string SolvePartTwo();

        protected abstract bool TestPartOne();

        protected abstract bool TestPartTwo();

        private static string TryRead(string FilePath)
        {
            if (File.Exists(FilePath)) { return File.ReadAllText(FilePath); }
            return "";
        }

        private string InputPath(InputType Type = InputType.Normal)
        {
            return Type switch
            {
                InputType.Normal => $"Input/day-{Day:00}.txt",
                InputType.Test => $"InputTest/day-{Day:00}.txt",
                InputType.TestA => $"InputTest/day-{Day:00}A.txt",
                _ => throw new ArgumentException("Invalid Type"),
            };
        }

        private void LoadDay()
        {
            Raw = TryRead(InputPath());
            RawTest = TryRead(InputPath(InputType.Test));
            RawTestA = TryRead(InputPath(InputType.TestA));
            Lines = Raw.SplitToList();
            LinesTest = RawTest.SplitToList();
            LinesTestA = RawTestA.SplitToList();
        }

        private (string Answer, TimeSpan Time) SolvePart(Func<string> Part)
        {
            SW.Restart();
            var answer = Part();
            SW.Stop();
            return (answer, SW.Elapsed);
        }

        private enum InputType
        {
            Normal,
            Test,
            TestA
        }
    }
}