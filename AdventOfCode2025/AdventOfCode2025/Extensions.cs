namespace AdventOfCode2025
{
    public static class Extensions
    {
        public static bool IsAllUnique(this IEnumerable<char> chars)
        {
            var Chars = new HashSet<char>();
            foreach (var Char in chars)
            {
                if (Chars.Contains(Char)) { return false; }
                else { Chars.Add(Char); }
            }
            return true;
        }

        public static List<string> SplitBlocksToList(this string input) => input.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None).ToList();

        public static List<string> SplitToList(this string input) => input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

        public static (List<string> ListOne, List<string> ListTwo) SplitToTwoList(this string input)
        {
            var I = input.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
            var P1 = I[0].SplitToList();
            var P2 = I[1].SplitToList();
            return (P1, P2);
        }

        public static List<int> ToIntList(this string input) => input.Split(',').Select(int.Parse).ToList();

        public static List<int> ToIntList(this List<string> input) => input.Select(int.Parse).ToList();

        public static List<long> ToLongList(this List<string> input) => input.Select(long.Parse).ToList();
    }
}