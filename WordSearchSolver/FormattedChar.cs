using System.Globalization;

namespace WordSearchSolver
{
    /// <summary>
    /// Represents the result of formatting a character in a word search grid for its solutions.
    /// </summary>
    public class FormattedChar
    {
        /// <summary>
        /// The final formatted string.
        /// </summary>
        public string Formatted { get; }
        
        /// <summary>
        /// The actual length of the formatted string, regardless of special characters.
        /// </summary>
        public int ActualLength { get; }

        /// <summary>
        /// Constructs a new <see cref="FormattedChar"/> with the given formatted string and actual length.
        /// </summary>
        /// <param name="formatted">The final formatted string.</param>
        /// <param name="actualLength">The actual length of the formatted string, regardless of special
        /// characters.</param>
        public FormattedChar(string formatted, int actualLength)
        {
            Formatted = formatted;
            ActualLength = actualLength;
        }

        /// <summary>
        /// Constructs a new <see cref="FormattedChar"/> with the given formatted string and calculates its actual length
        /// from a provided actual string.
        /// </summary>
        /// <param name="formatted">The final formatted string.</param>
        /// <param name="actual">The actual string.</param>
        public FormattedChar(string formatted, string actual) : this(formatted, StringLength(actual))
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FormattedChar"/> with the given formatted string and calculates its length
        /// using <see cref="StringInfo"/>.
        /// </summary>
        /// <param name="formatted">The final formatted string.</param>
        public FormattedChar(string formatted) : this(formatted, formatted)
        {
        }

        /// <summary>
        /// Calculates the length of the given string using <see cref="StringInfo"/>.
        /// </summary>
        /// <param name="input">The string whose length should be calculated.</param>
        /// <returns>An integer containing the length of the given string.</returns>
        private static int StringLength(string input)
        {
            return new StringInfo(input).LengthInTextElements;
        }
    }
}