using AdventOfCode2022.Types;
using System.Net.Security;

namespace AdventOfCode2022.Days
{
    public class Day07 : BaseDay
    {
        #region Overrides

        public Day07() : base(7) { }

        protected override string SolvePartOne()
        {
            var R = FindSum(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindDir(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindSum(LinesTest);
            return R == 95437;
        }

        protected override bool TestPartTwo()
        {
            var R = FindDir(LinesTest);
            return R == 24933642;
        }

        #endregion Overrides

        private static int FindDir(List<string> output)
        {
            var Files = new Filesystem(output);
            return Files.GetDir();
        }

        private static int FindSum(List<string> output)
        {
            var Files = new Filesystem(output);
            return Files.GetSum();
        }

        private class Filesystem
        {
            private readonly DefaultDictionary<string, int> Directories = new();

            public Filesystem(List<string> output)
            {
                var dirs = new Stack<string>();
                var CDRoot = () => { dirs = new(new[] { "." }); return true; };
                var CDIn = (string dir) => { dirs.Push(dir); return true; };
                var CDOut = () => { dirs.Pop(); return true; };
                var AddSize = (int size) =>
                {
                    var path = new List<string>();
                    foreach (var dir in dirs.Reverse())
                    {
                        path.Add(dir);
                        Directories[string.Join('/', path)] += size;
                    }
                    return true;
                };

                foreach (var line in output)
                {
                    _ = line.Split(' ') switch
                    {
                        ["$", "cd", "/"] => CDRoot(),
                        ["$", "cd", ".."] => CDOut(),
                        ["$", "cd", var dir] => CDIn(dir),
                        ["$", "ls"] => false,
                        ["dir", _] => false,
                        [var size, _] => AddSize(int.Parse(size)),
                        _ => false
                    };
                }
            }

            public int GetDir()
            {
                var need = 70_000_000 - 30_000_000;
                var current = Directories["."];
                return Directories.Where(KV => KV.Value >= current - need).Min(KV => KV.Value);
            }

            public int GetSum() => Directories.Select(KV => KV.Value).Where(V => V <= 100000).Sum();
        }
    }
}