using System;
using WordSearchSolver;
using Xunit;

namespace WordSearchSolverTests
{
    public class WordSearchTest
    {
        private static char[,] ThreeByFourGrid()
        {
            return new[,]
            {
                {'a', 'b', 'c'},
                {'d', 'e', 'f'},
                {'g', 'h', 'i'},
                {'j', 'k', 'l'}
            };
        }
        
        // Grid Constructor

        [Fact]
        public void GridConstructor_WithNullGrid_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new WordSearch(null));
            Assert.Equal($"{WordSearch.NullCharacterGridError} (Parameter 'chars')", e.Message);
        }

        // Blank Constructor

        [Fact]
        public void BlankConstructor_WithValidWidthAndHeight_CreatesProperGrid()
        {
            var w = new WordSearch(2, 3);
            
            Assert.Equal(2, w.Width);
            Assert.Equal(3, w.Height);

            foreach (var c in w.Chars)
                Assert.Equal(' ', c);
        }

        [Fact]
        public void BlankConstructor_WithZeroWidth_ThrowsProperException()
        {
            Assert.Throws<ArgumentOutOfRangeException>("width", () => new WordSearch(0, 5));
        }
        
        [Fact]
        public void BlankConstructor_WithZeroHeight_ThrowsProperException()
        {
            Assert.Throws<ArgumentOutOfRangeException>("height", () => new WordSearch(5, 0));
        }
        
        // .Width
        
        [Fact]
        public void Width_WithValidCharacterGrid_ReturnsWidth()
        {
            var w = new WordSearch(ThreeByFourGrid());
            Assert.Equal(3, w.Width);
        }
        
        // .Height

        [Fact]
        public void Height_WithValidCharacterGrid_ReturnsHeight()
        {
            var w = new WordSearch(ThreeByFourGrid());
            Assert.Equal(4, w.Height);
        }
        
        // .Format()

        [Fact]
        public void Format_WithDefaultSpacing_ReturnsProperString()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var expected = "a b c\nd e f\ng h i\nj k l";
            Assert.Equal(expected, w.Format());
        }

        [Fact]
        public void Format_WithNoSpacing_ReturnsUnSpacedString()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var expected = "abc\ndef\nghi\njkl";
            Assert.Equal(expected, w.Format(0, 0));
        }

        [Fact]
        public void Format_WithTwoAndOneSpacing_ReturnsProperlySpacedString()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var expected = "a  b  c\n\nd  e  f\n\ng  h  i\n\nj  k  l";
            Assert.Equal(expected, w.Format(2, 1));
        }
        
        [Fact]
        public void Format_WithThreeAndTwoSpacing_ReturnsProperlySpacedString()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var expected = "a   b   c\n\n\nd   e   f\n\n\ng   h   i\n\n\nj   k   l";
            Assert.Equal(expected, w.Format(3, 2));
        }
    }
}