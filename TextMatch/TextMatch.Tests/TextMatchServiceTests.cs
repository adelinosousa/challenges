using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextMatch.Common.Models;
using TextMatch.Common.Services;

namespace TextMatch.Tests
{
    [TestClass]
    public class TextMatchServiceTests
    {
        const string Text = "Polly put the kettle on, polly put the kettle on, polly put the kettle on we’ll all have tea";

        [TestMethod]
        public void TestMultipleCaseInsensitive()
        {
            var result = new MatchResultsViewModel(TextMatchService.FindMatches(Text, "Polly")).Results;
            Assert.AreEqual("1,26,51", result);
        }

        [TestMethod]
        public void TestMultiple()
        {
            var result = new MatchResultsViewModel(TextMatchService.FindMatches(Text, "ll")).Results;
            Assert.AreEqual("3,28,53,78,82", result);
        }

        [TestMethod]
        public void TestNotFound()
        {
            var result = new MatchResultsViewModel(TextMatchService.FindMatches(Text, "X")).Results;
            Assert.AreEqual("", result);

            result = new MatchResultsViewModel(TextMatchService.FindMatches(Text, "Polx")).Results;
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TestSpace()
        {
            var result = new MatchResultsViewModel(TextMatchService.FindMatches(Text, " ")).Results;
            Assert.AreEqual("6,10,14,21,25,31,35,39,46,50,56,60,64,71,74,80,84,89", result);
        }
    }
}
