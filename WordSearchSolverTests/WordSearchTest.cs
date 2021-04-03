using System;
using System.Collections.Generic;
using WordSearchSolver;
using Xunit;
using static WordSearchSolverTests.Helpers.TestUtil;

namespace WordSearchSolverTests
{
    public class WordSearchTest
    {
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
        
        // .GetWord()

        [Fact]
        public void GetWord_GivenOutOfBoundsStart_ThrowsProperException()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var l = new WordLocation(3, 4, -1, -1, 1);
            var e = Assert.Throws<ArgumentException>(() => w.GetWord(l));
            Assert.Equal(WordSearch.LocationOutOfBoundsError, e.Message);
        }

        [Fact]
        public void GetWord_GivenTooLongWord_ThrowsProperException()
        {
            var w = new WordSearch(ThreeByFourGrid());
            var l = new WordLocation(0, 0, 1, 1, 10);
            var e = Assert.Throws<ArgumentException>(() => w.GetWord(l));
            Assert.Equal(WordSearch.LocationOutOfBoundsError, e.Message);
        }

        [Fact]
        public void GetWord_GivenOneLengthWordLocation_ReturnsTheSingleCharacter()
        {
            var w = new WordSearch(ThreeByFourGrid());
            w.Chars[1, 1] = 'A';
            var l = new WordLocation(1, 1, 1, 1, 1);
            Assert.Equal("A", w.GetWord(l));
        }

        [Fact]
        public void GetWord_GivenTypicalWordLocation_ReturnsProperWord()
        {
            var w = new WordSearch(new[,]
            {
                {'a', 'b', 'c', 'd'},
                {'e', 'f', 'g', 'h'},
                {'i', 'j', 'k', 'l'},
                {'m', 'n', 'o', 'p'},
                {'q', 'r', 's', 't'}
            });
            var l = new WordLocation(2, 1, 1, 1, 3);
            Assert.Equal("jot", w.GetWord(l));
        }
        
        // .Parse()
        
        [Fact]
        public void Parse_WithNoLines_ThrowsAppropriateException()
        {
            var e = Assert.Throws<ArgumentException>(() => WordSearch.Parse(new List<string>()));
            Assert.Equal($"{WordSearch.EmptyGridError} (Parameter 'lines')", e.Message);
        }

        [Fact]
        public void Parse_WithOneLineAndAllKeepable_ParsesCorrectly()
        {
            var w = WordSearch.Parse(new List<string> { "abcd" });
            
            Assert.Equal(1, w.Height);
            Assert.Equal(4, w.Width);
            
            Assert.Equal('a', w.Chars[0, 0]);
            Assert.Equal('b', w.Chars[0, 1]);
            Assert.Equal('c', w.Chars[0, 2]);
            Assert.Equal('d', w.Chars[0, 3]);
        }

        [Fact]
        public void Parse_WithMultipleLinesAndAllKeepable_ParsesCorrectly()
        {
            var w = WordSearch.Parse(new List<string>
            {
                "abcd",
                "efgh",
                "ijkl",
                "mnop",
                "qrst"
            });
            
            Assert.Equal(5, w.Height);
            Assert.Equal(4, w.Width);
            
            Assert.Equal('a', w.Chars[0, 0]);
            Assert.Equal('k', w.Chars[2, 2]);
            Assert.Equal('t', w.Chars[4, 3]);
        }

        [Fact]
        public void Parse_WithOneLineWithExtraneousChars_ParsesCorrectly()
        {
            var w = WordSearch.Parse(new List<string> {"a  b c\tde  "});
            
            Assert.Equal(1, w.Height);
            Assert.Equal(5, w.Width);
            
            Assert.Equal('a', w.Chars[0, 0]);
            Assert.Equal('b', w.Chars[0, 1]);
            Assert.Equal('c', w.Chars[0, 2]);
            Assert.Equal('d', w.Chars[0, 3]);
            Assert.Equal('e', w.Chars[0, 4]);
        }

        [Fact]
        public void Parse_WithMultipleLinesWithExtraneousCharsNoExtraneousLines_ParsesCorrectly()
        {
            var w = WordSearch.Parse(new List<string>
            {
                "a\tbcd",
                "ef\tg   h",
                " i j k l",
                "\tm\tn\to\tp",
                "q     r    s  t   "
            });
            
            Assert.Equal(5, w.Height);
            Assert.Equal(4, w.Width);
            
            Assert.Equal('a', w.Chars[0, 0]);
            Assert.Equal('k', w.Chars[2, 2]);
            Assert.Equal('t', w.Chars[4, 3]);
        }

        [Fact]
        public void Parse_WithMultipleLinesWithExtraneousCharsAndLines_ParsesCorrectly()
        {
            var w = WordSearch.Parse(new List<string>
            {
                "  ",
                "a\tbcd",
                "",
                "ef\tg   h",
                "\t  ",
                "   ",
                "",
                " i j k l",
                "\tm\tn\to\tp",
                "\t\t\t",
                "q     r    s  t   "
            });
            
            Assert.Equal(5, w.Height);
            Assert.Equal(4, w.Width);
            
            Assert.Equal('a', w.Chars[0, 0]);
            Assert.Equal('k', w.Chars[2, 2]);
            Assert.Equal('t', w.Chars[4, 3]);
        }

        [Fact]
        public void Parse_WithJaggedGridGreaterThanFirst_ThrowsProperException()
        {
            var input = new List<string>
            {
                "a b c d",
                "e f g h",
                "i j k l m",
                "n o m q"
            };
            var e = Assert.Throws<FormatException>(() => WordSearch.Parse(input));
            Assert.Equal(WordSearch.JaggedGridError, e.Message);
        }
        
        [Fact]
        public void Parse_WithJaggedGridLessThanFirst_ThrowsProperException()
        {
            var input = new List<string>
            {
                "a b c d",
                "e f g h",
                "i j k",
                "n o m q"
            };
            var e = Assert.Throws<FormatException>(() => WordSearch.Parse(input));
            Assert.Equal(WordSearch.JaggedGridError, e.Message);
        }
    }
}