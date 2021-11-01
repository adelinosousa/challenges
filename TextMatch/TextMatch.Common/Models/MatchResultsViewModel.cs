namespace TextMatch.Common.Models
{
    public class MatchResultsViewModel
    {
        public string Results { get; private set; }

        public MatchResultsViewModel(string results)
        {
            Results = results;
        }
    }
}
