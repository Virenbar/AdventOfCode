namespace AdventOfCode2025.Days
{
    public class Day06 : TDay
    {
        public Day06() : base(6) { }

        protected override long TestOneResult => 4277556;
        protected override long TestTwoResult => 3263827;

        protected override long CalculatePartOne(List<string> worksheet)
        {
            var work = new Homework(worksheet);
            return work.Total();
        }

        protected override long CalculatePartTwo(List<string> worksheet)
        {
            var work = new Homework(worksheet);
            return work.TotalProper();
        }

        private class Homework
        {
            private readonly List<char> Operations;
            private readonly List<List<long>> Problems;
            private readonly List<List<long>> ProblemsProper;

            public Homework(List<string> work)
            {
                Operations = work.Last().Where(C => C != ' ').ToList();
                var numberLines = work.SkipLast(1).ToList();

                var numbers = numberLines
                    .Select(S => S.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
                Problems = Enumerable.Range(0, Operations.Count)
                    .Select(I => numbers.Select(P => P[I]).ToList())
                    .ToList();

                ProblemsProper = new List<List<long>>();
                var problem = new List<long>();
                for (int i = 0; i < numberLines[0].Length; i++)
                {
                    var number = string.Join("", numberLines.Select(L => L[i])).Trim();
                    if (string.IsNullOrEmpty(number))
                    {
                        ProblemsProper.Add(problem);
                        problem = new List<long>();
                    }
                    else
                    {
                        problem.Add(long.Parse(number));
                    }
                }
                ProblemsProper.Add(problem);
            }

            public long Total() => Solve(Problems);

            public long TotalProper() => Solve(ProblemsProper);

            private long Solve(List<List<long>> problems)
            {
                var total = 0L;
                for (int i = 0; i < Operations.Count; i++)
                {
                    var op = Operations[i];
                    var numbers = problems[i];
                    total += op switch
                    {
                        '+' => numbers.Aggregate(0L, (A, N) => A + N),
                        _ => numbers.Aggregate(1L, (A, N) => A * N)
                    };
                }
                return total;
            }
        }
    }
}