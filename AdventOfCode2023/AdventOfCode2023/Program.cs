using AdventOfCode2023.Days;

namespace AdventOfCode2023
{
    internal class Program
    {
        private static void Main()
        {
            BaseDay Day = new Day01();
            var (PartOne, PartTwo) = Day.Solve();
            Console.WriteLine(PartOne);
            Console.WriteLine(PartTwo);
        }
    }
}