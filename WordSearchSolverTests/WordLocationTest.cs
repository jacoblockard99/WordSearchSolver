using System;
using WordSearchSolver;
using Xunit;

namespace WordSearchSolverTests
{
    public class WordLocationTest
    {
        // Constructor

        [Fact]
        public void Constructor_WithZeroLength_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 1, 1, 0));
            Assert.Equal($"{WordLocation.InvalidLengthError} (Parameter 'length')", e.Message);
        }

        [Fact]
        public void Constructor_WithNegativeLength_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 1, 1, -1));
            Assert.Equal($"{WordLocation.InvalidLengthError} (Parameter 'length')", e.Message);
        }

        [Fact]
        public void Constructor_WithNegativeStartRow_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentOutOfRangeException>(() => new WordLocation(-1, 0, 1, 1, 1));
            Assert.Equal($"{WordLocation.InvalidStartCoordinateError} (Parameter 'startRow')", e.Message);
        }
        
        [Fact]
        public void Constructor_WithNegativeStartCol_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentOutOfRangeException>(() => new WordLocation(0, -1, 1, 1, 1));
            Assert.Equal($"{WordLocation.InvalidStartCoordinateError} (Parameter 'startCol')", e.Message);
        }

        [Fact]
        public void Constructor_WithTooHighDirectionX_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 100, 1, 1));
            Assert.Equal($"{WordLocation.InvalidDirectionIntegerError} (Parameter 'directionX')", e.Message);
        }
        
        [Fact]
        public void Constructor_WithTooLowDirectionX_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, -100, 1, 1));
            Assert.Equal($"{WordLocation.InvalidDirectionIntegerError} (Parameter 'directionX')", e.Message);
        }
        
        [Fact]
        public void Constructor_WithTooHighDirectionY_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 1, 100, 1));
            Assert.Equal($"{WordLocation.InvalidDirectionIntegerError} (Parameter 'directionY')", e.Message);
        }
        
        [Fact]
        public void Constructor_WithTooLowDirectionY_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 1, -100, 1));
            Assert.Equal($"{WordLocation.InvalidDirectionIntegerError} (Parameter 'directionY')", e.Message);
        }

        [Fact]
        public void Constructor_WithOnlyDirectionXZero_DoesNothing()
        {
            _ = new WordLocation(0, 0, 0, 1, 1);
        }

        [Fact]
        public void Constructor_WithOnlyDirectionYZero_DoesNothing()
        {
            _ = new WordLocation(0, 0, 1, 0, 1);
        }

        [Fact]
        public void Constructor_WithBothDirectionsZero_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new WordLocation(0, 0, 0, 0, 1));
            Assert.Equal(WordLocation.NonDirectionalError, e.Message);
        }
        
        // .EndRow

        [Fact]
        public void EndRow_WithLengthOne_ReturnsStartRow()
        {
            var w = new WordLocation(176, 276, 1, 1, 1);
            Assert.Equal(176, w.EndRow);
        }

        [Fact]
        public void EndRow_WhenPositiveDiagonal_ReturnsCorrectRow()
        {
            var w = new WordLocation(0, 1, 1, 1, 3);
            Assert.Equal(2, w.EndRow);
        }

        [Fact]
        public void EndRow_WhenBackwardsDiagonal_ReturnsCorrectRow()
        {
            var w = new WordLocation(3, 2, -1, -1, 3);
            Assert.Equal(1, w.EndRow);
        }

        [Fact]
        public void EndRow_WhenStraight_ReturnsCorrectRow()
        {
            var w = new WordLocation(2, 1, 1, 0, 4);
            Assert.Equal(2, w.EndRow);
        }
        
        // .EndCol

        [Fact]
        public void EndCol_WithLengthOne_ReturnsStartCol()
        {
            var w = new WordLocation(176, 276, 1, 1, 1);
            Assert.Equal(276, w.EndCol);
        }
        
        [Fact]
        public void EndCol_WhenPositiveDiagonal_ReturnsCorrectCol()
        {
            var w = new WordLocation(0, 1, 1, -1, 3);
            Assert.Equal(3, w.EndCol);
        }
        
        [Fact]
        public void EndCol_WhenBackwardsDiagonal_ReturnsCorrectCol()
        {
            var w = new WordLocation(3, 2, -1, -1, 3);
            Assert.Equal(0, w.EndCol);
        }
        
        [Fact]
        public void EndCol_WhenStraight_ReturnsCorrectCol()
        {
            var w = new WordLocation(2, 1, 1, 0, 4);
            Assert.Equal(4, w.EndCol);
        }
        
        // .Contains()

        [Fact]
        public void Contains_GivenStartingPoint_ReturnsTrue()
        {
            var w = new WordLocation(47, 39, 1, 1, 10);
            Assert.True(w.Contains(47, 39));
        }

        [Fact]
        public void Contains_RightWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, 1, 0, 6);
            Assert.True(w.Contains(100, 155));
        }

        [Fact]
        public void Contains_LeftWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, -1, 0, 6);
            Assert.True(w.Contains(100, 145));
        }

        [Fact]
        public void Contains_DownWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, 0, 1, 6);
            Assert.True(w.Contains(105, 150));
        }

        [Fact]
        public void Contains_UpWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, 0, -1, 6);
            Assert.True(w.Contains(95, 150));
        }

        [Fact]
        public void Contains_UpRightWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, 1, -1, 6);
            Assert.True(w.Contains(95, 155));
        }

        [Fact]
        public void Contains_DownLeftWordFiveAway_ReturnsTrue()
        {
            var w = new WordLocation(100, 150, -1, 1, 6);
            Assert.True(w.Contains(105, 145));
        }
        
        [Fact]
        public void Contains_RightWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 1, 0, 6);
            Assert.False(w.Contains(100, 156));
        }

        [Fact]
        public void Contains_LeftWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, -1, 0, 6);
            Assert.False(w.Contains(100, 144));
        }

        [Fact]
        public void Contains_DownWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 0, 1, 6);
            Assert.False(w.Contains(106, 150));
        }

        [Fact]
        public void Contains_UpWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 0, -1, 6);
            Assert.False(w.Contains(94, 150));
        }

        [Fact]
        public void Contains_UpRightWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 1, -1, 6);
            Assert.False(w.Contains(94, 156));
        }

        [Fact]
        public void Contains_DownLeftWordSixAway_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, -1, 1, 6);
            Assert.False(w.Contains(106, 144));
        }

        [Fact]
        public void Contains_InvalidLine_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 1, 1, 2);
            Assert.False(w.Contains(102, 153));
        }

        [Fact]
        public void Contain_OnOppositeStraightSide_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, -1, 0, 3);
            Assert.False(w.Contains(100, 151));
        }

        [Fact]
        public void Contains_OnOppositeDiagonalSide_ReturnsFalse()
        {
            var w = new WordLocation(100, 150, 1, 1, 3);
            Assert.False(w.Contains(99, 149));
        }
    }
}