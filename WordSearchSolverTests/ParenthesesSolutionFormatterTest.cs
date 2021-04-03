using System;
using WordSearchSolver;
using Xunit;
using static WordSearchSolverTests.Helpers.TestUtil;

namespace WordSearchSolverTests
{
    public class ParenthesesSolutionFormatterTest
    {
        [Fact]
        public void UsedWithFormatter_WithTypicalWordSearchWithValidSpacing_FormatsCorrectly()
        {
            var f = new WordSearchFormatter(new ParenthesesSolutionFormatter(), 2);
            var locations = new[]
            {
                new WordLocation(0, 3, 1, 1, 3),
                new WordLocation(1, 2, 0, 1, 4),
                new WordLocation(1, 1, 1, 1, 3)
            };
            var expected = "a  b  c (d) e  f\n" +
                           "g (h)(i) j (k) l\n" +
                           "m  n (o) p  q (r)\n" +
                           "s  t (u)(v) w  x\n" +
                           "y  z (a) b  c  d";
            Assert.Equal(expected, f.Format(SixByFiveWordSearch(), locations));
        }

        [Fact]
        public void UsedWithFormatter_WithTooLittleSpace_ThrowsProperException()
        {
            var f = new WordSearchFormatter(new ParenthesesSolutionFormatter(), 0);
            var locations = new[]
            {
                new WordLocation(0, 3, 1, 1, 3),
                new WordLocation(1, 2, 0, 1, 4),
                new WordLocation(1, 1, 1, 1, 3)
            };
            var e = Assert.Throws<FormatException>(() => f.Format(SixByFiveWordSearch(), locations));
            Assert.Equal(WordSearchFormatter.TooLittleSpacingError, e.Message);
        }
    }
}