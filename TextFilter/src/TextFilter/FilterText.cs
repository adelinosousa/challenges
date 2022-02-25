using System.Text;
using System.Text.RegularExpressions;
using TextFilter.Filters;
using TextFilter.Reader;

namespace TextFilter
{
    public class FilterText
    {
        private const string wordPattern = @"\w+";

        private readonly Regex wordRegex = new(wordPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IReader _reader;
        private readonly IEnumerable<ITextFilter> _textFilters;

        public FilterText(IReader reader, IEnumerable<ITextFilter> textFilters)
        {
            _reader = reader;
            _textFilters = textFilters;
        }

        public string FilterWords(string filePath)
        {
            var result = new StringBuilder();

            foreach (var line in _reader.Read(filePath))
            {
                var filteredLine = line;

                foreach (Match match in wordRegex.Matches(line))
                {
                    foreach (var wordFilter in _textFilters)
                    {
                        if (wordFilter.Filter(match.Value))
                        {
                            filteredLine = Regex.Replace(filteredLine, @$"\b{match.Value}\b", string.Empty);
                            break;
                        }
                    }
                }

                result.Append(filteredLine);   
            }

            return result.ToString();
        }
    }
}
