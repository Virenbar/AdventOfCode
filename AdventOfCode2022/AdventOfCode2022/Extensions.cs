namespace AdventOfCode2022
{
    public static class Extensions
    {
        public static List<string> SplitBlocksToList(this string input) => input.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None).ToList();

        public static List<string> SplitToList(this string input) => input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

        public static List<int> ToIntList(this string input) => input.Split(',').Select(int.Parse).ToList();

        public static List<int> ToIntList(this List<string> input) => input.Select(int.Parse).ToList();

        public static List<long> ToLongList(this List<string> input) => input.Select(long.Parse).ToList();
    }
}