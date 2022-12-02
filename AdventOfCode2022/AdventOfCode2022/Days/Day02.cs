using System.Text;

namespace AdventOfCode2022.Days
{
    public class Day02 : BaseDay
    {
        #region Overrides

        public Day02() : base(2) { }

        protected override string SolvePartOne()
        {
            var R = CalculateScore(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CalculateScoreV2(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CalculateScore(LinesTest);
            return R == 15;
        }

        protected override bool TestPartTwo()
        {
            var R = CalculateScoreV2(LinesTest);
            return R == 12;
        }

        #endregion Overrides

        private static int CalculateScore(List<string> rounds)
        {
            var Rounds = rounds.Select(R => Round.Decode(R));
            var Score = Rounds.Sum(R => R.GetScore());
            return Score;
        }

        private static int CalculateScoreV2(List<string> rounds)
        {
            var Rounds = rounds.Select(R => Round.DecodeV2(R));
            var Score = Rounds.Sum(R => R.GetScore());
            return Score;
        }

        private class Round
        {
            private static readonly Dictionary<char, RPS> Decoder = new()
            {
                {'A', RPS.Rock},
                {'B', RPS.Paper},
                {'C', RPS.Scissors},
                {'X', RPS.Rock},
                {'Y', RPS.Paper},
                {'Z', RPS.Scissors},
            };

            private static readonly Dictionary<(RPS, char), RPS> DecoderV2 = new()
            {
                {(RPS.Rock,'X'), RPS.Scissors},
                {(RPS.Paper,'X'), RPS.Rock},
                {(RPS.Scissors,'X'), RPS.Paper},
                {(RPS.Rock,'Y'), RPS.Rock},
                {(RPS.Paper,'Y'), RPS.Paper},
                {(RPS.Scissors,'Y'), RPS.Scissors},
                {(RPS.Rock,'Z'), RPS.Paper},
                {(RPS.Paper,'Z'), RPS.Scissors},
                {(RPS.Scissors,'Z'), RPS.Rock}
            };

            private static readonly Dictionary<(RPS, RPS), int> Results = new()
            {
                {(RPS.Rock, RPS.Paper), 0},
                {(RPS.Paper, RPS.Scissors), 0},
                {(RPS.Scissors, RPS.Rock), 0},
                {(RPS.Rock, RPS.Rock), 3},
                {(RPS.Paper, RPS.Paper), 3},
                {(RPS.Scissors, RPS.Scissors), 3},
                {(RPS.Rock, RPS.Scissors), 6},
                {(RPS.Paper, RPS.Rock), 6},
                {(RPS.Scissors, RPS.Paper), 6}
            };

            public Round(RPS first, RPS second)
            {
                FirstPlayer = first;
                SecondPlayer = second;
            }

            private RPS FirstPlayer { get; set; }
            private RPS SecondPlayer { get; set; }

            public static Round Decode(string round)
            {
                var Play = Decoder[round[0]];
                var Reason = Decoder[round[2]];
                return new Round(Reason, Play);
            }

            public static Round DecodeV2(string round)
            {
                var Play = Decoder[round[0]];
                var Reason = DecoderV2[(Play, round[2])];
                return new Round(Reason, Play);
            }

            public int GetScore()
            {
                var score = (int)FirstPlayer;
                score += Results[(FirstPlayer, SecondPlayer)];
                return score;
            }
        }

        private enum RPS
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }
    }
}