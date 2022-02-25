namespace TextFilter.Filters
{
    public class FilterWordWithMiddleVowel : ITextFilter
    {
        private readonly IEnumerable<char> vowels = new List<char>() { 'a', 'e', 'i', 'o', 'u' };

        public bool Filter(string word)
        {
            if (word.Length % 2 == 0)
            {
                var middleLetters = word.Substring((word.Length / 2) - 1, 2);

                if (middleLetters.Any(v => vowels.Contains(char.ToLowerInvariant(v))))
                    return true;
            }
            else
            {
                var middleLetter = word[(int)Math.Floor(word.Length / 2d)];

                if (vowels.Contains(char.ToLowerInvariant(middleLetter)))
                    return true;
            }

            return false;
        }
    }
}
