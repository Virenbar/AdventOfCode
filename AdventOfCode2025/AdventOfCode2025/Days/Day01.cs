namespace AdventOfCode2025.Days
{
    public partial class Day01 : TDay
    {
        public Day01() : base(1) { }

        protected override long TestOneResult => 3;
        protected override long TestTwoResult => 6;

        protected override long CalculatePartOne(List<string> sequnce)
        {
            var dial = new Dial();
            dial.Rotate(sequnce);
            return dial.ZeroCount;
        }

        protected override long CalculatePartTwo(List<string> sequnce)
        {
            var dial = new Dial();
            dial.Rotate(sequnce);
            return dial.ZeroClick;
        }

        private partial class Dial
        {
            private int Position = 50;

            public int ZeroClick { get; private set; }
            public int ZeroCount { get; private set; }

            public void Rotate(List<string> sequnce)
            {
                foreach (var op in sequnce)
                {
                    var dir = op[0];
                    var number = int.Parse(op.AsSpan()[1..]);
                    var (position, zero) = dir switch
                    {
                        'R' => NextPosition(Position, number),
                        'L' => NextPosition(Position, -number),
                        _ => throw new InvalidDataException()
                    };
                    Position = position;
                    ZeroClick += zero;
                    if (Position == 0) { ZeroCount++; }
                }
            }

            private static (int, int) NextPosition(int current, int rotation)
            {
                var position = (current + rotation) % 100;
                if (position < 0) { position += 100; }
                var zero = rotation switch
                {
                    > 0 => (current + rotation) / 100,
                    < 0 => (-(current + rotation) / 100) + (current > 0 && rotation <= -current ? 1 : 0),
                    _ => 0
                };
                return (position, zero);
            }
        }
    }
}