using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.CSharp.RuntimeBinder;

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
        /// The error message to use when an out-of-bounds word location is provided.
        /// </summary>
        public const string LocationOutOfBoundsError = "The provided word location is out of bounds of this word " +
                                                       "search!";

        /// <summary>
        /// The error message to use when an empty grid is parsed.
        /// </summary>
        public const string EmptyGridError = "The provided list of lines cannot be empty!";

        /// <summary>
        /// The error message to use when a jagged grid is parsed.
        /// </summary>
        public const string JaggedGridError = "The character grid cannot be jagged!";

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
        /// Determines whether the given coordinates are within the bounds of this word search.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>True if the given coordinate is in-bounds; false otherwise.</returns>
        public bool InBounds(int row, int col)
        {
            return row >= 0 && col >= 0 && row < Height && col < Width;
        }

        /// <summary>
        /// Gets the word in this <see cref="WordSearch"/> at the given <see cref="WordLocation"/>.
        /// </summary>
        /// <param name="location">A <see cref="WordLocation"/> instance representing the location of the word.</param>
        /// <returns>A <see cref="string"/> containing the requested word.</returns>
        public string GetWord(WordLocation location)
        {
            // Ensure that both the start and end locations of the word are in bounds.
            ValidateLocation(location.StartRow, location.StartCol);
            ValidateLocation(location.EndRow, location.EndCol);
            
            var result = new StringBuilder();
            
            // Iterate through each character in the requested word.
            for (var n = 0; n < location.Length; n++)
            {
                // For each character, calculate its row and column.
                var row = location.StartRow + location.DirectionY * n;
                var col = location.StartCol + location.DirectionX * n;
                
                // Append the calculated character to the result.
                result.Append(Chars[row, col]);
            }

            return result.ToString();
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
        /// Ensures the the given character location is not outside this word search.
        /// </summary>
        /// <param name="row">The row of the character.</param>
        /// <param name="col">The column of the character.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the given location is outside this word search.</exception>
        private void ValidateLocation(int row, int col)
        {
            if (row >= Height) throw new ArgumentException(LocationOutOfBoundsError);
            if (col >= Width)  throw new ArgumentException(LocationOutOfBoundsError);
        }

        /// <summary>
        /// Attempts to parse the given list of lines into a <see cref="WordSearch"/> object.
        /// </summary>
        /// <param name="lines">The list of lines to parse.</param>
        /// <returns>The parsed <see cref="WordSearch"/> object.</returns>
        /// <exception cref="ArgumentException">If the lines list is empty.</exception>
        /// <exception cref="FormatException">If the input was badly formatted.</exception>
        public static WordSearch Parse(IEnumerable<string> lines)
        {
            var chars = ParseCharacters(lines);

            // Check if any rows got added. If not, throw an appropriate exception.
            if (chars.Count == 0) throw new ArgumentException(EmptyGridError, nameof(lines));

            // Determine the height of the parsed word search.
            var height = chars.Count;
            // Determine the width of the parsed word search using the first row.
            var width = chars[0].Count;
            
            // Instantiate a new WordSearch that will be populated with the data from chars.
            var wordSearch = new WordSearch(width, height);

            // Iterate over ever row in the chars list.
            for (var row = 0; row < chars.Count; row++)
            {
                // If the current row's length is different from the first one, then the grid is jagged. Throw an
                // appropriate exception.
                if (chars[row].Count != width) throw new FormatException(JaggedGridError);
                
                // Iterate over each character in the current row, adding it to the word search.
                for (var col = 0; col < chars[row].Count; col++)
                    wordSearch.Chars[row, col] = chars[row][col];
            }

            return wordSearch;
        }

        /// <summary>
        /// Determines whether the given character should be kept in a parsed word search.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character should be kept; false otherwise.</returns>
        public static bool IsKeepable(char c)
        {
            return !char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Parses the given list of lines into a (potentially jagged) "two-dimensional" list of <see cref="char"/>s.
        /// </summary>
        /// <param name="lines">The list of lines to parse.</param>
        /// <returns>The parsed "two-dimensional" list of <see cref="char"/>s.</returns>
        private static List<List<char>> ParseCharacters(IEnumerable<string> lines)
        {
            // Initialize a "2-dimensional" list to hold the filtered characters.
            var chars = new List<List<char>>();

            // Iterate over each line in the lines list.
            foreach (var line in lines)
            {
                // Filter out any extraneous characters.
                var row = line.Where(IsKeepable).ToList();
                
                // If the row had at least one "keepable" character, add it to the chars list.
                if (row.Any()) chars.Add(row);
            }

            return chars;
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