using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public partial class Day11 : BaseDay
    {
        #region Overrides

        public Day11() : base(11) { }

        protected override string SolvePartOne()
        {
            var R = PlayGame20(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = PlayGame10000(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = PlayGame20(RawTest);
            return R == 10605;
        }

        protected override bool TestPartTwo()
        {
            var R = PlayGame10000(RawTest);
            return R == 2713310158;
        }

        #endregion Overrides

        private static long PlayGame10000(string monkeys)
        {
            var Game = new KeppAway(monkeys);
            Game.Play(10000, false);
            return Game.MonkeyBusinessLevel();
        }

        private static long PlayGame20(string monkeys)
        {
            var Game = new KeppAway(monkeys);
            Game.Play(20);
            return Game.MonkeyBusinessLevel();
        }

        private class KeppAway
        {
            private readonly List<Monkey> Monkeys;
            private readonly Dictionary<int, Monkey> MonkeysID;

            public KeppAway(string monkeys)
            {
                Monkeys = monkeys.SplitBlocksToList().Select(M => new Monkey(M)).ToList();
                MonkeysID = Monkeys.ToDictionary(M => M.ID, M => M);
            }

            public long MonkeyBusinessLevel()
            {
                var Top = Monkeys.OrderByDescending(M => M.Inspections).ToList();
                return (long)Top[0].Inspections * Top[1].Inspections;
            }

            public void Play(int count) => Play(count, true);

            public void Play(int count, bool divide)
            {
                for (int i = 0; i < count; i++)
                {
                    Monkeys.ForEach(M => M.TakeTurn(MonkeysID, divide));
                }
            }
        }
        private partial class Monkey
        {
            private readonly int False;
            private readonly Func<long, long> Operation;
            private readonly int True;

            public Monkey(string monkey)
            {
                var M = monkey.SplitToList();
                ID = int.Parse(RDigits().Match(M[0]).Value);
                Items = new(RDigits().Matches(M[1]).Select(M => long.Parse(M.Value)));
                Test = int.Parse(RDigits().Match(M[3]).Value);
                True = int.Parse(RDigits().Match(M[4]).Value);
                False = int.Parse(RDigits().Match(M[5]).Value);
                var O = ROperation().Match(M[2]).Groups[1].Value.Split(' ');
                Operation = O switch
                {
                    ["*", "old"] => N => N * N,
                    ["*", var value] => N => N * int.Parse(value),
                    ["+", var value] => N => N + int.Parse(value),
                    _ => throw new InvalidOperationException()
                };
            }

            public int ID { get; }
            public int Inspections { get; private set; }
            public Queue<long> Items { get; }
            public int Test { get; }

            public void TakeTurn(Dictionary<int, Monkey> monkeys, bool divide)
            {
                while (Items.Any())
                {
                    var Item = Items.Dequeue();
                    Item = Operation(Item);
                    Item = divide ? Item / 3 : Item % monkeys.Aggregate(1, (A, KV) => A * KV.Value.Test);
                    monkeys[Item % Test == 0 ? True : False].Items.Enqueue(Item);
                    Inspections++;
                }
            }

            [GeneratedRegex(@"\d+")]
            private static partial Regex RDigits();

            [GeneratedRegex(@"old ([*+] (old|\d+))")]
            private static partial Regex ROperation();
        }
    }
}