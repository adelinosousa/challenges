namespace TextMatch.Common.Services
{
    public class TextMatchService
    {
        public static string FindMatches(string text, string subtext)
        {
            var results = string.Empty;

            // Loop every character in the text parameter
            for (int i = 0; i < text.Length; i++)
            {
                // Loop every character in the subtext parameter
                for (int x = 0; x < subtext.Length; x++)
                {
                    // Check if we still have text to search
                    if (!((i + x) < text.Length))
                    {
                        break;
                    }

                    // Try to match every character to the end by using values
                    var t1 = (double)text[i + x];
                    var t2 = (double)subtext[x];
                    if (!(t1 == t2 || (t1 + 32) == t2 || (t2 + 32) == t1))
                    {
                        break;
                    }

                    // Have we reach the end?
                    if ((x + 1) == subtext.Length)
                    {
                        // Add comma and index to results and move search point to the end of the match
                        if (results.Length > 0)
                        {
                            results += "," + (i + 1);
                        }
                        else
                        {
                            results += (i + 1);
                        }
                        i = i + x;
                        break;
                    }
                }
            }

            return results;
        }
    }
}
