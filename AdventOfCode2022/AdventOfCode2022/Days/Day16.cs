using SuperLinq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public partial class Day16 : BaseDay
    {
        #region Overrides

        public Day16() : base(16) { }

        protected override string SolvePartOne()
        {
            var R = CalculatePartOne(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = CalculatePartTwo(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CalculatePartOne(LinesTest);
            return R == 1651;
        }

        protected override bool TestPartTwo()
        {
            var R = CalculatePartTwo(LinesTest);
            return R == 1707;
        }

        #endregion Overrides

        private static int CalculatePartOne(List<string> valves)
        {
            var Cave = new ValveCave(valves);
            return Cave.FindMaxPressure();
        }

        private static int CalculatePartTwo(List<string> valves)
        {
            var Cave = new ValveCave(valves);
            return Cave.FindRateWithElephant();
        }

        [GeneratedRegex(@"(?<V>[A-Z]{2})\D*(?<R>\d+)[^A-Z]*(?<T>.*)")]
        private static partial Regex ValveRegex();

        private class Valve
        {
            public Valve(string info)
            {
                var Match = ValveRegex().Match(info);
                Name = Match.Groups["V"].Value;
                Rate = int.Parse(Match.Groups["R"].Value);
                Tunnels = Match.Groups["T"].Value.Split(", ").ToList();
            }

            public Valve(Valve valve)
            {
                Name = valve.Name;
                Rate = valve.Rate;
                Tunnels = valve.Tunnels;
            }

            public override string ToString() => $"{Name} {Rate}";

            public int Rate { get; }
            public string Name { get; }
            public List<string> Tunnels { get; }
        }

        private class ValveCave
        {
            private readonly Dictionary<string, Valve> Valves;
            private readonly ImmutableHashSet<(string id, int flow)> AllValves = ImmutableHashSet<(string id, int flow)>.Empty;
            private readonly Dictionary<(string from, string to), int> distanceMap = new();

            public ValveCave(List<string> valves)
            {
                Valves = valves.Select(V => new Valve(V)).ToDictionary(V => V.Name, StringComparer.Ordinal);

                foreach (var (key, _) in Valves)
                {
                    var map = SuperEnumerable.GetShortestPaths<string, int>(key, (f, c) => Valves[f].Tunnels.Select(t => (t, c + 1)));
                    foreach (var kvp in map)
                    { distanceMap[(key, kvp.Key)] = kvp.Value.cost; }
                }

                foreach (var v in Valves.Values.Where(v => v.Rate > 0).Select(v => (v.Name, v.Rate)))
                { AllValves = AllValves.Add(v); }
            }

            private record State(string Valve, ImmutableHashSet<(string id, int flow)> ClosedValves);

            private (int MaxPressure, State State) DoPart1(List<State> states, int timeRemaining)
            {
                var (valve, closedValves) = states[^1];
                if (timeRemaining <= 0) return (0, states[^1]);

                var remainingValves = closedValves
                    .Select(v =>
                    {
                        var distance = distanceMap[(valve, v.id)];
                        var timeOpen = timeRemaining - 1 - distance;
                        return (v, t: timeOpen, p: v.flow * timeOpen);
                    })
                    .OrderByDescending(x => x.p)
                    .ToList();

                var best = (pressure: 0, state: states[^1]);
                foreach (var (v, t, pressure) in remainingValves)
                {
                    states.Add(new(v.id, closedValves.Remove(v)));
                    var p = DoPart1(states, t);
                    p = (p.MaxPressure + pressure, p.State);
                    if (p.MaxPressure > best.pressure) { best = p; }
                    states.RemoveAt(states.Count - 1);
                }

                return best;
            }

            public int FindMaxPressure()
            {
                var You = DoPart1(new() { new("AA", AllValves) }, 30);
                return You.MaxPressure;
            }

            public int FindRateWithElephant()
            {
                var You = DoPart1(new() { new("AA", AllValves) }, 26);
                var Elephant = DoPart1(new() { new("AA", You.State.ClosedValves) }, 26);
                return You.MaxPressure + Elephant.MaxPressure;
            }
        }
    }
}