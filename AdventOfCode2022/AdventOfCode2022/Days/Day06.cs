namespace AdventOfCode2022.Days
{
    public class Day06 : BaseDay
    {
        #region Overrides

        public Day06() : base(6) { }

        protected override string SolvePartOne()
        {
            var R = FindPacket(Raw);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindMessage(Raw);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = FindPacket(RawTest);
            return R == 7;
        }

        protected override bool TestPartTwo()
        {
            var R = FindMessage(RawTest);
            return R == 19;
        }

        #endregion Overrides

        private static int FindMessage(string dataStream)
        {
            var Buffer = new Buffer(dataStream);
            return Buffer.FindMessageStart();
        }

        private static int FindPacket(string dataStream)
        {
            var Buffer = new Buffer(dataStream);
            return Buffer.FindPacketStart();
        }

        private class Buffer
        {
            private readonly string DataStream;

            public Buffer(string stream)
            {
                DataStream = stream;
            }

            public int FindMessageStart() => FindMarker(14);

            public int FindPacketStart() => FindMarker(4);

            private int FindMarker(int length)
            {
                for (int i = 0; i < DataStream.Length; i++)
                {
                    var Marker = DataStream[i..(i + length)];
                    if (Marker.ToHashSet().Count == length) { return i + length; }
                }
                return DataStream.Length;
            }
        }
    }
}