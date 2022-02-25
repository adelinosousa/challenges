using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using TextFilter.Filters;
using TextFilter.Reader;

namespace TextFilter.Tests
{
    [TestFixture]
    public class FilterTextTests
    {
        private readonly IReader reader = Substitute.For<IReader>();

        private readonly string fileName = "test.file";

        [TestCase("what clean", " ")]
        [TestCase("currently rather", " rather")]
        [TestCase("the what", "the ")]
        public void FilterText_FilterWordMiddleVowelCorrectly(string content, string expectedResult)
        {
            reader.Read(fileName).Returns(new List<string> { content });

            var filterText = new FilterText(reader, new ITextFilter[]
            {
                new FilterWordWithMiddleVowel(),
            });

            var actualResult = filterText.FilterWords(fileName);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [TestCase(3, "what clean", " ")]
        [TestCase(3, "she rather", "she ")]
        [TestCase(3, "word. the", ". the")]
        [TestCase(2, "the sun was out", "   ")]
        [TestCase(1, "a pea", "a ")]
        public void FilterText_FilterWordMaxLengthCorrectly(int maxLength, string content, string expectedResult)
        {
            reader.Read(fileName).Returns(new List<string> { content });

            var filterText = new FilterText(reader, new ITextFilter[]
            {
                new FilterWordByMaxLength(maxLength),
            });

            var actualResult = filterText.FilterWords(fileName);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [TestCase('r', "what clean", "what clean")]
        [TestCase('t', "pool rather", "pool ")]
        [TestCase('p', "the pool", "the ")]
        public void FilterText_FilterWordWithLetterCorrectly(char letter, string content, string expectedResult)
        {
            reader.Read(fileName).Returns(new List<string> { content });

            var filterText = new FilterText(reader, new ITextFilter[]
            {
                new FilterWordWithLetter(letter),
            });

            var actualResult = filterText.FilterWords(fileName);

            Assert.AreEqual(actualResult, expectedResult);
        }
    }
}