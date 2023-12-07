using MoreLinq;

namespace AdventOfCode2023.Days
{
    public class Day01 : BaseDay
    {
        #region Overrides

        public Day01() : base(1) { }

        protected override string SolvePartOne()
        {
            var R = FindValue(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindValueReal(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindValue(LinesTest);
            return R == 142;
        }

        protected override bool TestPartTwo()
        {
            var R = FindValueReal(LinesTestA);
            return R == 281;
        }

        #endregion Overrides

        private static readonly Dictionary<string, char> Digits = new()
            {
                {"one",'1' },
                {"two",'2' },
                {"three",'3' },
                {"four",'4' },
                {"five",'5' },
                {"six",'6' },
                {"seven",'7' },
                {"eight",'8' },
                {"nine",'9' }
            };

        private static int FindValue(List<string> document)
        {
            var sum = 0;
            foreach (var line in document)
            {
                var first = line.First(char.IsDigit);
                var last = line.Last(char.IsDigit);
                sum += int.Parse($"{first}{last}");
            }
            return sum;
        }

        private static int FindValueReal(List<string> document)
        {
            document = document.Select(ParseDigits).ToList();
            return FindValue(document);
        }

        private static string ParseDigits(string line)
        {
            var digits = line.WindowLeft(5)
                .Select(X => char.IsDigit(X[0]) ? X[0] : Digits.FirstOrDefault(D => X.StartsWith(D.Key)).Value)
                .Where(C => C != default);
            return string.Concat(digits);
        }
    }
}