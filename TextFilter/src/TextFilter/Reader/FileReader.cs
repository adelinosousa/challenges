namespace TextFilter.Reader
{
    public class FileReader : IReader
    {
        public IEnumerable<string> Read(string filePath)
        {
            return File.ReadLines(filePath);
        }
    }
}
