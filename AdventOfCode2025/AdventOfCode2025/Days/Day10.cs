using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public class Day10 : TDay
    {
        public Day10() : base(10) { }

        protected override long TestOneResult => 7;
        protected override long TestTwoResult => 33;

        protected override long CalculatePartOne(List<string> machines)
        {
            var M = new Machines(machines);
            return M.PartOne();
        }

        protected override long CalculatePartTwo(List<string> machines)
        {
            var M = new Machines(machines);
            return M.PartTwo();
        }

        private class Machines
        {
            private readonly List<Problem> Problems;

            public Machines(List<string> machines)
            {
                Problems = machines.Select(Parse).ToList();
            }

            public long PartOne()
            {
                return Problems.Select(P =>
                        SinglePresses(P)
                        .Where(press => Enumerable.SequenceEqual(P.Target, press.JoltageChange.Select(i => i % 2)))
                        .Min(press => press.ButtonCount))
                    .Sum();
            }

            public long PartTwo()
            {
                return Problems
                    .Select(P => Solve(P.Joltage, SinglePresses(P), new Dictionary<string, int>()))
                    .Sum();
            }

            record Problem(int[] Target, int[] Buttons, int[] Joltage);

            record SinglePress(int ButtonCount, int[] JoltageChange);

            private static Problem Parse(string machine)
            {
                // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
                var parts = machine.Split(" ").ToArray();

                var target = parts[0]
                    .Trim("[]".ToCharArray())
                    .Select(ch => ch == '#' ? 1 : 0)
                    .ToArray();

                var buttons = parts[1..^1]
                    .Select(part =>
                        Regex.Matches(part, @"\d+")
                        .Select(m => int.Parse(m.Value))
                        .Aggregate(0, (acc, d) => acc | (1 << d)))
                    .ToArray();

                var joltage = Regex.Matches(parts[^1], @"\d+")
                    .Select(M => int.Parse(M.Value))
                    .ToArray();

                return new Problem(target, buttons, joltage);
            }

            private static List<SinglePress> SinglePresses(Problem p)
            {
                var presses = new List<SinglePress>();

                foreach (var buttonMask in Enumerable.Range(0, 1 << p.Buttons.Length))
                {
                    var joltageChange = new int[p.Joltage.Length];
                    var buttonCount = 0;

                    for (int ibutton = 0; ibutton < p.Buttons.Length; ibutton++)
                    {
                        if ((buttonMask >> ibutton) % 2 == 1)
                        {
                            buttonCount++;
                            var button = p.Buttons[ibutton];
                            for (int ijoltage = 0; ijoltage < p.Joltage.Length; ijoltage++)
                            {
                                if ((button >> ijoltage) % 2 == 1)
                                {
                                    joltageChange[ijoltage]++;
                                }
                            }
                        }
                    }
                    presses.Add(new SinglePress(buttonCount, joltageChange));
                }
                return presses;
            }

            private static int Solve(int[] joltages, List<SinglePress> singlePresses, Dictionary<string, int> cache)
            {
                if (joltages.All(jolt => jolt == 0)) { return 0; }

                var key = string.Join("-", joltages);
                if (!cache.ContainsKey(key))
                {
                    var best = 10_000_000;
                    foreach (var singlePress in singlePresses)
                    {
                        var buttonCount = singlePress.ButtonCount;
                        var joltageChange = singlePress.JoltageChange;

                        var evens = Enumerable.Range(0, joltages.Length)
                            .All(i => joltages[i] >= joltageChange[i] && (joltages[i] - joltageChange[i]) % 2 == 0);

                        if (evens)
                        {
                            var subProblem = Enumerable.Range(0, joltages.Length)
                                .Select(i => (joltages[i] - joltageChange[i]) / 2)
                                .ToArray();

                            best = Math.Min(best, buttonCount + 2 * Solve(subProblem, singlePresses, cache));
                        }
                    }
                    cache[key] = best;
                }
                return cache[key];
            }
        }
    }
}