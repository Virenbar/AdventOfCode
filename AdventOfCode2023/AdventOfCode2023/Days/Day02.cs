using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days
{
    public partial class Day02 : BaseDay
    {
        #region Overrides

        public Day02() : base(2) { }

        protected override string SolvePartOne()
        {
            var R = FindGames(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindGamesMinimal(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindGames(LinesTest);
            return R == 8;
        }

        protected override bool TestPartTwo()
        {
            var R = FindGamesMinimal(LinesTest);
            return R == 2286;
        }

        #endregion Overrides

        private static int FindGames(List<string> input)
        {
            var max = new GameResult(12, 13, 14);
            var games = input.Select(ParseGame).ToList();
            var sum = games.Where(G =>
                G.Results.TrueForAll(R =>
                    R.Red <= max.Red &&
                    R.Green <= max.Green &&
                    R.Blue <= max.Blue)
                ).Sum(G => G.ID);
            return sum;
        }

        private static int FindGamesMinimal(List<string> input)
        {
            var games = input.Select(ParseGame).ToList();
            var sum = games.Sum(G =>
                G.Results.Max(R => R.Red) *
                G.Results.Max(R => R.Green) *
                G.Results.Max(R => R.Blue));
            return sum;
        }

        private static Game ParseGame(string game)
        {
            var items = game.Split(':', ';');
            var id = int.Parse(RGame().Match(items[0]).Value);

            var results = new List<GameResult>();
            foreach (var item in items.Skip(1))
            {
                var red = int.TryParse(RRed().Match(item).Value, out var r) ? r : 0;
                var green = int.TryParse(RGreen().Match(item).Value, out var g) ? g : 0;
                var blue = int.TryParse(RBlue().Match(item).Value, out var b) ? b : 0;

                results.Add(new GameResult(red, green, blue));
            }

            return new Game(id, results);
        }

        [GeneratedRegex(@"\d*(?=\sblue)")]
        private static partial Regex RBlue();

        [GeneratedRegex(@"(?<=Game\s)\d*")]
        private static partial Regex RGame();

        [GeneratedRegex(@"\d*(?=\sgreen)")]
        private static partial Regex RGreen();

        [GeneratedRegex(@"\d*(?=\sred)")]
        private static partial Regex RRed();

        private record GameResult(int Red, int Green, int Blue);

        private record Game(int ID, List<GameResult> Results);
    }
}