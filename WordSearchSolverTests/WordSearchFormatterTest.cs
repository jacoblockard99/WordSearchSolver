using System;
using System.Linq;
using WordSearchSolver;
using WordSearchSolverTests.Helpers;
using Xunit;
using static WordSearchSolverTests.Helpers.TestUtil;

namespace WordSearchSolverTests
{
    public class WordSearchFormatterTest
    {
        // .Format()

        [Fact]
        public void Format_WithDefaultSpacing_ReturnsProperString()
        {
            var expected = "a b c\nd e f\ng h i\nj k l";
            Assert.Equal(expected, WordSearchFormatter.Default.Format(ThreeByFourWordSearch()));
        }

        [Fact]
        public void Format_WithNoSpacing_ReturnsUnSpacedString()
        {
            var f = new WordSearchFormatter(new DummySolutionFormatter(), 0);
            var expected = "abc\ndef\nghi\njkl";
            Assert.Equal(expected,f.Format(ThreeByFourWordSearch()));
        }

        [Fact]
        public void Format_WithTwoAndOneSpacing_ReturnsProperlySpacedString()
        {
            var f = new WordSearchFormatter(new DummySolutionFormatter(), 2, 1);
            var expected = "a  b  c\n\nd  e  f\n\ng  h  i\n\nj  k  l";
            Assert.Equal(expected, f.Format(ThreeByFourWordSearch()));
        }
        
        [Fact]
        public void Format_WithThreeAndTwoSpacing_ReturnsProperlySpacedString()
        {
            var f = new WordSearchFormatter(new DummySolutionFormatter(), 3, 2);
            var expected = "a   b   c\n\n\nd   e   f\n\n\ng   h   i\n\n\nj   k   l";
            Assert.Equal(expected, f.Format(ThreeByFourWordSearch()));
        }

        [Fact]
        public void FormatWithSolutions_WithSingleCharFormatterWithOneSolution_FormatsCorrectly()
        {
            var f = new WordSearchFormatter(new TestSolutionFormatter());
            var expected = "* b c\nd * f\ng h *\nj k l";
            var location = new WordLocation(0, 0, 1, 1, 3);
            Assert.Equal(expected, f.Format(ThreeByFourWordSearch(), new []{location}));
        }

        [Fact]
        public void FormatWithSolutions_WithSingleCharFormatterWithManySolutions_FormatsCorrectly()
        {
            var f = new WordSearchFormatter(new TestSolutionFormatter(), 0);
            var expected = "***de*\ng*ij*l\nm*o*qr\nstuvwx\nyz***d";
            var words = new[] {"abc", "bhn", "fkp"};
            var locations = new IterativeSolver(SixByFiveWordSearch(), words).Solve().Values.SelectMany(l => l);
            Assert.Equal(expected, f.Format(SixByFiveWordSearch(), locations));
        }

        [Fact]
        public void FormatWithSolutions_WithSingleCharFormatterTooLittleSpacing_ThrowsProperException()
        {
            var f = new WordSearchFormatter(new ParenthesesSolutionFormatter(), 0);
            var location = new WordLocation(0, 0, 1, 1, 3);
            var e = Assert.Throws<FormatException>(() => f.Format(ThreeByFourWordSearch(), new[] {location}));
            Assert.Equal(WordSearchFormatter.TooLittleSpacingError, e.Message);
        }

        [Fact]
        public void FormatWithSolutions_WithLongFormatterTooLittleSpacing_ThrowsProperException()
        {
            var f = new WordSearchFormatter(new TestSolutionFormatter {Length = 21}, 9);
            var location = new WordLocation(0, 0, 1, 1, 3);
            var e = Assert.Throws<FormatException>(() => f.Format(ThreeByFourWordSearch(), new[] {location}));
            Assert.Equal(WordSearchFormatter.TooLittleSpacingError, e.Message);
        }

        [Fact]
        public void FormatWithSolutions_WithLongFormatter_FormatsCorrectly()
        {
            var f = new WordSearchFormatter(new TestSolutionFormatter {Length = 3}, 3);
            var location = new WordLocation(0, 0, 1, 1, 3);
            var expected = "*** b   c\n" +
                           "d  ***  f\n" +
                           "g   h  ***\n" +
                           "j   k   l";
            Assert.Equal(expected, f.Format(ThreeByFourWordSearch(), new[]{location}));
        }

        [Fact]
        public void FormatWithSolutions_WithEvenFormatter_PlacesSpaceAtBeginning()
        {
            var f = new WordSearchFormatter(new TestSolutionFormatter {Length = 2});
            var location = new WordLocation(0, 0, 1, 1, 3);
            var expected = "**b c\n" + 
                           "d** f\n" +
                           "g h**\n" +
                           "j k l";
            Assert.Equal(expected, f.Format(ThreeByFourWordSearch(), new []{location}));
        }
    }
}