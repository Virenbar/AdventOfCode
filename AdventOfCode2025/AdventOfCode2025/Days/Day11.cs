using System.Text.RegularExpressions;

namespace AdventOfCode2025.Days
{
    public partial class Day11 : TDay
    {
        public Day11() : base(11) { }

        protected override bool AlternativeTestTwo => true;
        protected override long TestOneResult => 5;
        protected override long TestTwoResult => 2;

        protected override long CalculatePartOne(List<string> devices)
        {
            var rack = new Rack(devices);
            return rack.CountYOU();
        }

        protected override long CalculatePartTwo(List<string> devices)
        {
            var rack = new Rack(devices);
            return rack.CountSVR();
        }

        private partial class Rack
        {
            private readonly Dictionary<string, Device> Devices;
            private Dictionary<(string name, bool dac, bool), long> M;

            public Rack(List<string> devices)
            {
                Devices = devices.Select(D => InputRegex().Match(D))
                    .Select(M => new Device(M.Groups["from"].Value, M.Groups["to"].Captures.Select(C => C.Value).ToList()))
                    .ToDictionary(D => D.Name, D => D);
            }

            public long CountSVR()
            {
                M = new();
                return FindSVR_Recursive("svr", false, false);
            }

            public long CountYOU()
            {
                return GetDevices("you").Where(D => D.Outputs.Contains("out")).Count();
            }

            [GeneratedRegex(@"^(?<from>\w+):( (?<to>\w+))+$", RegexOptions.ExplicitCapture)]
            private static partial Regex InputRegex();

            private long FindSVR_Recursive(string name, bool dac, bool fft)
            {
                if (name == "out") { return dac && fft ? 1 : 0; }

                if (M.TryGetValue((name, dac, fft), out long count)) { return count; }

                return M[(name, dac, fft)] = Devices[name].Outputs.Sum(D => FindSVR_Recursive(D, dac || name == "dac", fft || name == "fft"));
            }

            private IEnumerable<Device> GetDevices(string name)
            {
                var control = Devices[name];
                var queue = new Queue<Device>();
                do
                {
                    foreach (var child in control.Outputs)
                    {
                        if (child == "out") { continue; }
                        queue.Enqueue(Devices[child]);
                    }
                    control = queue.Dequeue();
                    yield return control;
                } while (queue.Count > 0);
            }

            private record Device(string Name, List<string> Outputs);
        }
    }
}