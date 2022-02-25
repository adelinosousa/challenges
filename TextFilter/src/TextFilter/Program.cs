using System.Diagnostics;
using TextFilter;
using TextFilter.Filters;
using TextFilter.Reader;

var stopWatch = Stopwatch.StartNew();

var reader = new FileReader();

var filters = new ITextFilter[]
{
    new FilterWordWithMiddleVowel(),
    new FilterWordByMaxLength(3),
    new FilterWordWithLetter('t')
};

var filterText = new FilterText(reader, filters);

var result = filterText.FilterWords("sample.txt");

stopWatch.Stop();

Console.WriteLine(result);
Console.WriteLine($"Processed in { stopWatch.ElapsedMilliseconds }ms");