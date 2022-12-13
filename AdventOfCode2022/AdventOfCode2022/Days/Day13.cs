using MoreLinq;
using System.Text.Json;

namespace AdventOfCode2022.Days
{
    public class Day13 : BaseDay
    {
        #region Overrides

        public Day13() : base(13) { }

        protected override string SolvePartOne()
        {
            var R = FindRightPackets(Lines);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindDecoderKey(Lines);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindRightPackets(LinesTest);
            return R == 13;
        }

        protected override bool TestPartTwo()
        {
            var R = FindDecoderKey(LinesTest);
            return R == 140;
        }

        #endregion Overrides

        private static int FindDecoderKey(List<string> signal)
        {
            var Signal = new DistressSignal(signal);
            return Signal.DecoderKey();
        }

        private static int FindRightPackets(List<string> signal)
        {
            var Signal = new DistressSignal(signal);
            return Signal.RightPackets();
        }

        private class DistressSignal
        {
            private readonly List<Packet> Packets;

            public DistressSignal(List<string> signal)
            {
                Packets = signal.Where(P => !string.IsNullOrEmpty(P)).Select(P => Packet.Parse(P)).ToList();
            }

            public int DecoderKey()
            {
                var ID = 1;
                var D1 = Packet.Parse("[[2]]");
                var D2 = Packet.Parse("[[6]]");
                var Ordered = Packets.Concat(new[] { D1, D2 }).Order().ToDictionary(P => P, _ => ID++);
                return Ordered[D1] * Ordered[D2];
            }

            public int RightPackets()
            {
                var PacketPairs = Packets.Batch(2).Select(P => (Left: P.First(), Right: P.Last())).Index(1);
                return PacketPairs.Where(KV => KV.Value.Left.CompareTo(KV.Value.Right) < 0).Sum(P => P.Key);
            }
        }
        private abstract record Packet : IComparable<Packet>
        {
            public static Packet Parse(string packet) => Parse(JsonSerializer.Deserialize<JsonElement>(packet));
            private static Packet Parse(JsonElement element)
            {
                return element.ValueKind == JsonValueKind.Number
                    ? new IntPacket(element.GetInt32())
                    : new ListPacket(element.EnumerateArray().Select(Parse).ToList());
            }
            public int CompareTo(Packet other) => Compare(this, other);

            public static int Compare(Packet left, Packet right)
            {
                return (left, right) switch
                {
                    (Packet _, null) => +1,
                    (null, Packet _) => -1,
                    (IntPacket L, IntPacket R) => L.Value.CompareTo(R.Value),
                    (IntPacket L, ListPacket R) => new ListPacket(new() { L }).CompareTo(R),
                    (ListPacket L, IntPacket R) => L.CompareTo(new ListPacket(new() { R })),
                    (ListPacket L, ListPacket R) => L.Packets.ZipLongest(R.Packets, Compare)
                            .SkipWhile(x => x == 0)
                            .FirstOrDefault(),
                    _ => throw new InvalidOperationException()
                };
            }
        }
        private record IntPacket(int Value) : Packet;
        private record ListPacket(List<Packet> Packets) : Packet;
    }
}