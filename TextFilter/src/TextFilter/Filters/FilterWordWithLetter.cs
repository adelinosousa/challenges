namespace TextFilter.Filters
{
    public class FilterWordWithLetter : ITextFilter
    {
        private readonly char _letter;

        public FilterWordWithLetter(char letter)
        {
            _letter = letter;
        }

        public bool Filter(string word)
        {
            return word.Contains(_letter, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
