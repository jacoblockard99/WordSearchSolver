using System;
using System.Text;

namespace WordSearchSolver
{
    /// <summary>
    /// Represents a single word search, which is a grid of characters of arbitrary dimensions.
    /// </summary>
    public class WordSearch
    {
        /// <summary>
        /// The error message to use when a null character grid is provided for instantiation.
        /// </summary>
        public const string NullCharacterGridError = "The provided character grid cannot be null!";

        /// <summary>
        /// The error message to use when zero is provided for the width or height of the word search.
        /// </summary>
        public const string ZeroSizeError = "The character grid's dimensions must be greater than zero!";

        /// <summary>
        /// The character that is regarded as a "blank" entry in a word search.
        /// </summary>
        public const char BlankChar = ' ';
        
        /// <summary>
        /// A two-dimensional array of <see cref="char"/>s that represents the characters in this word search.
        /// </summary>
        public char[,] Chars { get; }

        /// <summary>
        /// Determines the width of the word search.
        /// </summary>
        public int Width => Chars.GetLength(1);
        
        /// <summary>
        /// Determines the height of the word search.
        /// </summary>
        public int Height => Chars.GetLength(0);

        /// <summary>
        /// Constructs a new <see cref="WordSearch"/> with the given array of characters.
        /// </summary>
        /// <param name="chars">The two-dimensional array of <see cref="char"/>s that represents the characters in this
        /// word search.</param>
        public WordSearch(char[,] chars)
        {
            // Ensure that chars is non-null.
            if (chars == null) throw new ArgumentNullException(nameof(chars), NullCharacterGridError);
            
            Chars = chars;
        }

        /// <summary>
        /// Constructs a new, blank <see cref="WordSearch"/> of the given width and height.
        /// </summary>
        /// <param name="width">The width of the word search.</param>
        /// <param name="height">The height of the word search.</param>
        public WordSearch(int width, int height) : this(BuildBlankGrid(width, height))
        {
            // Ensure valid dimensions.
            if (width <= 0)  throw new ArgumentOutOfRangeException(nameof(width),  ZeroSizeError);
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), ZeroSizeError);
        }

        /// <summary>
        /// Gets a <see cref="string"/> representation of this <see cref="WordSearch"/> by iterating over its grid of
        /// characters.
        /// </summary>
        /// <param name="hSpacing">The amount of horizontal space to add between rows. One by default.</param>
        /// <param name="vSpacing">The amount of vertical space to add between rows. Zero by default.</param>
        /// <returns>A <see cref="string"/> containing this word search's grid of characters.</returns>
        public string Format(int hSpacing = 1, int vSpacing = 0)
        {
            var result = new StringBuilder();
            // Store the requested amount of vertical space, by:
            //   1) starting with a single newline, which is required no matter what, and
            //   2) adding extra depending on the spacing parameter.
            var vSpace = new string('\n', vSpacing + 1);

            // Iterate over each row in the grid of characters.
            for (var row = 0; row < Height; row++)
            {
                // Format each row and append it to the result.
                result.Append(FormatRow(row, hSpacing));
                // After every row *except the last* add the appropriate newlines.
                if (row < Height - 1)
                    result.Append(vSpace);
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets a <see cref="string"/> representation of a single row of this <see cref="WordSearch"/>.
        /// </summary>
        /// <param name="row">The row to format.</param>
        /// <param name="spacing">The amount of space to add between each column.</param>
        /// <returns>A <see cref="string"/> containing the given row's characters.</returns>
        private string FormatRow(int row, int spacing)
        {
            var result = new StringBuilder();
            // Store the requested amount of horizontal space.
            var hSpace = new string(' ', spacing);
            
            // Iterate over each column in the row.
            for (var col = 0; col < Width; col++)
            {
                // Append each character to the result.
                result.Append(Chars[row, col]);
                // After every character *except the last* add the appropriate space.
                if (col < Width - 1)
                    result.Append(hSpace);
            }

            return result.ToString();
        }
    
        /// <summary>
        /// Builds a blank character grid of the given dimensions.
        /// </summary>
        /// <param name="width">The width of the character grid.</param>
        /// <param name="height">The height of the character grid.</param>
        /// <returns>A two-dimensional <see cref="char"/> array containing the blank-filled character grid.</returns>
        private static char[,] BuildBlankGrid(int width, int height)
        {
            var result = new char[height, width];
            
            for (var row = 0; row < height; row++)
                for (var col = 0; col < width; col++)
                    result[row, col] = BlankChar;
            
            return result;
        }
    }
}