using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WordSearchSolver
{
    /// <summary>
    /// A class that is capable of formatting <see cref="WordSearch"/>s in various ways.
    /// </summary>
    public class WordSearchFormatter
    {
        /// <summary>
        /// The default <see cref="WordSearchFormatter"/>.
        /// </summary>
        public static readonly WordSearchFormatter Default = new(new DummySolutionFormatter());
        
        /// <summary>
        /// The error to use when too little spacing is set to properly use the provided <see cref="SolutionFormatter"/>.
        /// </summary>
        public const string TooLittleSpacingError = "There is not enough spacing set for the provided solution formatter!";
        
        /// <summary>
        /// The amount of space to place between each column.
        /// </summary>
        public int HorizontalSpacing { get; }
        
        /// <summary>
        /// The amount of space to place between each row.
        /// </summary>
        public int VerticalSpacing { get; }
        
        /// <summary>
        /// The <see cref="ISolutionFormatter"/> to use when determining how to format characters that are part of a
        /// solution.
        /// </summary>
        public ISolutionFormatter SolutionFormatter { get; }

        /// <summary>
        /// Constructs a new <see cref="WordSearchFormatter"/> with the given <see cref="ISolutionFormatter"/> and
        /// optional spacing.
        /// </summary>
        /// <param name="solutionFormatter">The <see cref="ISolutionFormatter"/> to use when determining how to
        /// format characters that are part of a solution.</param>
        /// <param name="hSpacing">The amount of space to place between columns.</param>
        /// <param name="vSpacing">The amount of space to place between rows.</param>
        public WordSearchFormatter(ISolutionFormatter solutionFormatter, int hSpacing = 1, int vSpacing = 0)
        {
            HorizontalSpacing = hSpacing;
            VerticalSpacing = vSpacing;
            SolutionFormatter = solutionFormatter;
        }

        /// <summary>
        /// Gets a <see cref="string"/> representation of this <see cref="WordSearch"/> by iterating over its grid of
        /// characters.
        /// </summary>
        /// <param name="wordSearch">The word search to format.</param>
        /// <returns>A <see cref="string"/> containing the formatted word search.</returns>
        public string Format(WordSearch wordSearch)
        {
            // Format like normal, with no solutions.
            return Format(wordSearch, Array.Empty<WordLocation>());
        }

        /// <summary>
        /// Gets a <see cref="string"/> representation of this <see cref="WordSearch"/> by iterating over its grid of
        /// characters, specially formatting characters that are part of one of the given solutions.
        /// </summary>
        /// <param name="wordSearch">The word search to format.</param>
        /// <param name="solutions">The list of solutions to format.</param>
        /// <returns>A <see cref="string"/> containing the formatted word search.</returns>
        public string Format(WordSearch wordSearch, IEnumerable<WordLocation> solutions)
        {
            var result = new StringBuilder();

            // Iterate over each row in the grid of characters.
            for (var row = 0; row < wordSearch.Height; row++)
            {
                // Format each row and append it to the result.
                result.Append(FormatRow(wordSearch, row, solutions));
                // After every row *except the last* add the appropriate vertical space.
                if (row < wordSearch.Height - 1)
                    result.Append(VerticalSpace());
            }

            return result.ToString();
        }
        /// <summary>
        /// Gets a <see cref="string"/> representation of a single row of the given <see cref="WordSearch"/>. Takes
        /// into account the given solutions.
        /// </summary>
        /// <param name="wordSearch">The word search to format.</param>
        /// <param name="row">The row to format.</param>
        /// <param name="solutions">The list of solutions to format.</param>
        /// <returns>A <see cref="string"/> containing the given row's characters.</returns>
        private string FormatRow(WordSearch wordSearch, int row, IEnumerable<WordLocation> solutions)
        {
            var result = new StringBuilder();
            
            // Iterate over each column in the row.
            for (var col = 0; col < wordSearch.Width; col++)
            {
                // Retrieve the current character.
                var c = wordSearch.Chars[row, col];
                // Format the current character depending on the solutions (if any) that it is a part of.
                var formatted = SolutionFormatter.FormatChar(FindContainingSolutions(row, col, solutions), row, col, c);
                
                // Append each character to the result.
                AppendSpacedChar(formatted, result, col == 0, col == wordSearch.Width - 1);
            }

            return result.ToString();
        }

        /// <summary>
        /// Generates a string for horizontal spacing given the current formatted character.
        /// </summary>
        /// <returns>A blank string with length equal to <see cref="HorizontalSpacing"/>.</returns>
        private void AppendSpacedChar(FormattedChar formatted, StringBuilder builder, bool start, bool end)
        {
            // Calculate the exact required spacing offset to make the character "centered". Naturally, this exact
            // value cannot be used in the case of formatted strings of even length.
            var exactOffset = (formatted.ActualLength - 1) / 2d;
            
            // Calculate the integral left offset by rounding the exact offset up. This ensures that when the string
            // can't be centered exactly, less space is placed left. Thus, when in doubt, the string will be more
            // towards the beginning.
            var leftOffset = (int) Math.Ceiling(exactOffset);
            // Calculate the integral right offset by rounding the exact offset down. This ensures that when the string
            // can't be centered exactly, more space is placed right.
            var rightOffset = (int) Math.Floor(exactOffset);
            
            // If this character is at the beginning of the grid, then we can't remove space from the left. Thus we need
            // to remove *all* the space from the right.
            if (start) rightOffset += leftOffset;

            // If either offset is greater than the horizontal spacing, then the string won't fit. Throw an
            // appropriate exception.
            if (leftOffset > HorizontalSpacing || rightOffset > HorizontalSpacing)
                throw new FormatException(TooLittleSpacingError);
            
            // When not at the beginning, remove the appropriate space from the left. We cannot, of course, do this
            // at the beginning.
            if (!start)
                builder.Length -= leftOffset;

            // Append the actual formatted string.
            builder.Append(formatted.Formatted);

            // When not at the end, add the requested horizontal spacing, offset as calculated. We do not, of course,
            // want any space at the end.
            if (!end)
                builder.Append(new string(' ', HorizontalSpacing - rightOffset));
        }

        /// <summary>
        /// Generates a string for vertical spacing.
        /// </summary>
        /// <returns>A string of newlines equal to <see cref="VerticalSpacing"/> plus one.</returns>
        private string VerticalSpace()
        {
            return new('\n', VerticalSpacing + 1);
        }

        /// <summary>
        /// Finds all the solution word locations in which the given character location lies.
        /// </summary>
        /// <param name="row">The row of the location to check.</param>
        /// <param name="col">The column of the location to check.</param>
        /// <param name="solutions">The list of all solutions to check.</param>
        /// <returns>A list of <see cref="WordLocation"/>s that the given character lies in.</returns>
        private static IEnumerable<WordLocation> FindContainingSolutions(int row, int col, IEnumerable<WordLocation> solutions)
        {
            return solutions.Where(s => s.Contains(row, col));
        }
    }
}