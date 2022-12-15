using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days
{
    public partial class Day15 : BaseDay
    {
        #region Overrides

        public Day15() : base(15) { }

        protected override string SolvePartOne()
        {
            var R = CountEmptyPoints(Lines, 2000000);
            return R.ToString();
        }

        protected override string SolvePartTwo()
        {
            var R = FindFrequency(Lines, 4000000);
            return R.ToString();
        }

        protected override bool TestPartOne()
        {
            var R = CountEmptyPoints(LinesTest, 10);
            return R == 26;
        }

        protected override bool TestPartTwo()
        {
            var R = FindFrequency(LinesTest, 20);
            return R == 56000011;
        }

        #endregion Overrides

        private static int CountEmptyPoints(List<string> sensors, int row)
        {
            var Scan = new SensorsScan(sensors);
            return Scan.CountEmpty(row);
        }

        private static long FindFrequency(List<string> sensors, int size)
        {
            var Scan = new SensorsScan(sensors);
            return Scan.FindFrequency(size);
        }

        [GeneratedRegex("=(?<SX>.+),.+=(?<SY>.+):.+=(?<BX>.+),.+=(?<BY>.+)")]
        private static partial Regex SensorRegex();

        private class SensorsScan
        {
            private readonly HashSet<Point> Beacons = new();
            private readonly List<(Point Sensor, Point Beacon, int Distance)> Data = new();

            public SensorsScan(List<string> sensors)
            {
                foreach (var sensor in sensors)
                {
                    var Match = SensorRegex().Match(sensor);
                    Point Sensor = new(int.Parse(Match.Groups["SX"].Value), int.Parse(Match.Groups["SY"].Value));
                    Point Beacon = new(int.Parse(Match.Groups["BX"].Value), int.Parse(Match.Groups["BY"].Value));
                    var Distance = Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
                    Beacons.Add(Beacon);
                    Data.Add((Sensor, Beacon, Distance));
                }
            }

            public int CountEmpty(int row)
            {
                var MinX = Data.Select(D => D.Sensor.X - (D.Distance - Math.Abs(row - D.Sensor.Y))).Min();
                var MaxX = Data.Select(D => D.Sensor.X + (D.Distance - Math.Abs(row - D.Sensor.Y))).Max();

                var Zone = Enumerable.Range(MinX, MaxX - MinX + 1)
                    .Select(X => new Point(X, row))
                    .Where(P => Data.Any(D => Math.Abs(P.X - D.Sensor.X) + Math.Abs(P.Y - D.Sensor.Y) <= D.Distance))
                    .Where(P => !Beacons.Contains(P));
                return Zone.Count();
            }

            public long FindFrequency(int size)
            {
                foreach (var X in Enumerable.Range(0, size + 1))
                {
                    var Ranges = Data
                        .Select(D => (D.Sensor, YDistance: D.Distance - Math.Abs(X - D.Sensor.X)))
                        .Where(D => D.YDistance >= 0)
                        .Select(D => (MinY: D.Sensor.Y - D.YDistance, MaxY: D.Sensor.Y + D.YDistance))
                        .OrderBy(D => D.MinY)
                        .ToList();
                    var Y = 0;
                    foreach (var (MinY, MaxY) in Ranges)
                    {
                        if (MinY <= Y + 1) { Y = Math.Max(MaxY, Y); }
                        else { return X * 4000000L + Y + 1; }
                    }
                }
                return 0;
            }
        }
        private record Point(int X, int Y);
    }
}