using AdventOfCode2025.Days;

namespace AdventOfCode2025
{
    internal static class Program
    {
        private static void Main()
        {
            IDay Day = new Day12();
            var (PartOne, PartTwo) = Day.Solve();
            Console.WriteLine(PartOne);
            Console.WriteLine(PartTwo);
        }
    }
}