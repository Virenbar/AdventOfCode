using System.Diagnostics;

namespace AdventOfCode2025.Days
{
    public abstract class TDay : TDay<List<string>>
    {
        public TDay(int day) : base(day) { }
    }

    public abstract class TDay<T> : IDay where T : class
    {
        protected T Input;
        protected T InputTest;
        private readonly Stopwatch SW = new();

        public TDay(int day) => Day = day;

        public int Day { get; private set; }
        protected abstract long TestOneResult { get; }
        protected abstract long TestTwoResult { get; }

        public (string PartOne, string PartTwo) Solve()
        {
            LoadDay();
            var T1 = CalculatePartOne(InputTest) == TestOneResult;
            var T2 = CalculatePartTwo(InputTest) == TestTwoResult;
            if (!(T1 && T2))
            {
                var P1 = T1 ? "passed" : "failed";
                var P2 = T2 ? "passed" : "failed";
                return ($"Test One: {P1}", $"Test Two: {P2}");
            }

            var A1 = SolvePart(CalculatePartOne);
            var A2 = SolvePart(CalculatePartTwo);
            return ($"Part One: {A1})", $"Part Two: {A2})");
        }

        protected abstract long CalculatePartOne(T input);

        protected abstract long CalculatePartTwo(T input);

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
            if (typeof(T) == typeof(string))
            {
                Input = (T)(object)TryRead(InputPath());
                InputTest = (T)(object)TryRead(InputPath(InputType.Test));
            }
            else if (typeof(T) == typeof(List<string>))
            {
                Input = (T)(object)TryRead(InputPath()).SplitToList();
                InputTest = (T)(object)TryRead(InputPath(InputType.Test)).SplitToList();
            }
        }

        private Result SolvePart(Func<T, long> Part)
        {
            SW.Restart();
            var answer = Part(Input).ToString();
            SW.Stop();
            return new(answer, SW.Elapsed);
        }

        private enum InputType
        {
            Normal,
            Test,
            TestA
        }
    }

    internal record struct Result(string Answer, TimeSpan Time)
    {
        public override readonly string ToString() => $"{Answer} ({Time})";
    }
}