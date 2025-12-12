using AdventOfCode2025.Days;

namespace AdventOfCode2025
{
    internal class Program
    {
        private static void Main()
        {
            IDay Day = new Day09();
            var (PartOne, PartTwo) = Day.Solve();
            Console.WriteLine(PartOne);
            Console.WriteLine(PartTwo);
        }
    }
}