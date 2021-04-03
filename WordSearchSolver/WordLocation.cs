using System;

namespace WordSearchSolver
{
    /// <summary>
    /// An immutable representation of the location of a word in a <see cref="WordSearch"/>.
    /// </summary>
    public class WordLocation
    {
        /// <summary>
        /// The error message to use when an invalid direction integer is provided.
        /// </summary>
        public const string InvalidDirectionIntegerError = "Direction integers must be -1, 0, or 1!";

        /// <summary>
        /// The error message to use when both direction integers are zero, which would result in a word that did not
        /// extend in *any* direction.
        /// </summary>
        public const string NonDirectionalError = "Both the x and y directions cannot be zero!";

        /// <summary>
        /// The error message to use when the length of the word is less than or equal to zero.
        /// </summary>
        public const string InvalidLengthError = "The length of the word must be positive!";

        /// <summary>
        /// The error message to use when the starting row or column is negative.
        /// </summary>
        public const string InvalidStartCoordinateError = "The starting row and column of the word must not be negative!";
        
        /// <summary>
        /// The row in which the word begins.
        /// </summary>
        public int StartRow { get; }
        
        /// <summary>
        /// The column in which the word begins.
        /// </summary>
        public int StartCol { get; }
        
        /// <summary>
        /// An integer representing the horizontal direction in which the word points. -1 represents "left", 1
        /// represents "right", and 0 represents a word that points neither left nor right.
        /// </summary>
        public int DirectionX { get; }

        /// <summary>
        /// An integer representing the vertical direction in which the word points. -1 represents "up", 1
        /// represents "down", and 0 represents a word that points neither up nor down.
        /// </summary>
        public int DirectionY { get; }
        
        /// <summary>
        /// The number of characters that the word spans in its direction. In other words, its length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Determines the row in which the word ends.
        /// </summary>
        public int EndRow => StartRow + DirectionY * (Length - 1);

        /// <summary>
        /// Determines the column in which the words ends.
        /// </summary>
        public int EndCol => StartCol + DirectionX * (Length - 1);

        /// <summary>
        /// Constructs a new <see cref="WordLocation"/> with the given starting row and column, direction, and length.
        /// </summary>
        /// <param name="startRow">The row in which the word begins.</param>
        /// <param name="startCol">The column in which the word begins.</param>
        /// <param name="directionX">An integer representing the horizontal direction in which the word points. -1
        /// represents "left", 1 represents "right", and 0 represents a word that points neither left nor right.</param>
        /// <param name="directionY">An integer representing the vertical direction in which the word points. -1
        /// represents "up", 1 represents "down", and 0 represents a word that points neither up nor down.</param>
        /// <param name="length">The length of the word.</param>
        public WordLocation(int startRow, int startCol, int directionX, int directionY, int length)
        {
            ValidateDirection(directionX, directionY);

            // Ensure that the length is positive.
            if (length <= 0) throw new ArgumentException(InvalidLengthError, nameof(length));

            // Ensure that the start coordinates are in bounds.
            if (startRow < 0) throw new ArgumentOutOfRangeException(nameof(startRow), InvalidStartCoordinateError);
            if (startCol < 0) throw new ArgumentOutOfRangeException(nameof(startCol), InvalidStartCoordinateError);
            
            StartRow = startRow;
            StartCol = startCol;
            DirectionX = directionX;
            DirectionY = directionY;
            Length = length;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({StartCol}, {StartRow}) through ({EndCol}, {EndRow})";
        }

        /// <summary>
        /// Determines whether the specified character location is contained in this word location.
        /// </summary>
        /// <param name="row">The row of the location to check.</param>
        /// <param name="col">The column of the location to check.</param>
        /// <returns>True if the specified character location is contained within this word location; false
        /// otherwise.</returns>
        public bool Contains(int row, int col)
        {
            // If the given point is the starting point itself, return true.
            if (row == StartRow && col == StartCol) return true;
            
            // Calculate the distances between this point and the starting point of this word location.
            var xDistance = col - StartCol;
            var yDistance = row - StartRow;

            // The given point is contained within this word location if :
            // 1) It is possible to form a valid horizontal, vertical, or 45-degree diagonal line between the given
            //    point and the starting point,
            // 2) That line points in the same direction as the word, and
            // 3) The distance between the given point and the starting point is less than or equal to the word's
            //    length.
            return LiesOnValidLine(xDistance, yDistance)
                   && LiesOnSameLine(xDistance, yDistance)
                   && WithinLength(xDistance, yDistance);
        }

        /// <summary>
        /// Determines whether the given (potentially) negative distance is within the range of this word.
        /// </summary>
        /// <param name="xDistance">The x distance (i.e. column-wise).</param>
        /// <param name="yDistance">The y distance (i.e. row-wise).</param>
        /// <returns>True if the given distance is less than or equal to this word's length; false otherwise.</returns>
        private bool WithinLength(int xDistance, int yDistance)
        {
            return Math.Max(Math.Abs(xDistance), Math.Abs(yDistance)) < Length;
        }

        /// <summary>
        /// Determines whether the line formed by the given x/y distances points in the same direction as this word.
        /// </summary>
        /// <param name="xDistance">The x distance (i.e. column-wise).</param>
        /// <param name="yDistance">The y distance (i.e. row-wise).</param>
        /// <returns>True if the given distances form a line pointing in the same direction as this word; false
        /// otherwise.</returns>
        private bool LiesOnSameLine(int xDistance, int yDistance)
        {
            return Math.Sign(yDistance) == DirectionY && Math.Sign(xDistance) == DirectionX;
        }

        /// <summary>
        /// Validates the direction represented by the given direction integers.
        /// </summary>
        /// <param name="directionX">The x direction.</param>
        /// <param name="directionY">The y direction.</param>
        /// <exception cref="ArgumentException">If the direction is invalid.</exception>
        private static void ValidateDirection(int directionX, int directionY)
        {
            // Ensure that both direction integers are -1, 0, or 1.
            
            if (directionX != -1 && directionX != 0 && directionX != 1)
                throw new ArgumentException(InvalidDirectionIntegerError, nameof(directionX));
            
            if (directionY != -1 && directionY != 0 && directionY != 1)
                throw new ArgumentException(InvalidDirectionIntegerError, nameof(directionY));
            
            // Ensure that the direction integers don't create a non-direction.

            if (directionX == 0 && directionY == 0)
                throw new ArgumentException(NonDirectionalError);
        }

        /// <summary>
        /// Determines whether the line formed by the given x/y distances is a valid line for a word search grid (i.e.
        /// is completely horizontal, completely vertical, or a 45-degree diagonal).
        /// </summary>
        /// <param name="xDistance">The x distance (i.e. column-wise).</param>
        /// <param name="yDistance">The y distance (i.e. row-wise).</param>
        /// <returns>True if the line formed by the given distances is valid; false otherwise.</returns>
        private static bool LiesOnValidLine(int xDistance, int yDistance)
        {
            return yDistance == 0 || xDistance == 0 || Math.Abs(yDistance) == Math.Abs(xDistance);
        }
    }
}