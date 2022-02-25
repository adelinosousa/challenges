namespace TextFilter.Filters
{
    public class FilterWordByMaxLength : ITextFilter
    {
        private readonly int _maxLength;

        public FilterWordByMaxLength(int maxLength)
        {
            _maxLength = maxLength;
        }

        public bool Filter(string word)
        {
            return word.Length > _maxLength;
        }
    }
}
