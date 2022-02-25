namespace TextFilter.Reader
{
    public interface IReader
    {
        IEnumerable<string> Read(string filePath);
    }
}
