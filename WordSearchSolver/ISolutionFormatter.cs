using System.Collections.Generic;

namespace WordSearchSolver
{
    /// <summary>
    /// Defines a class that is capable of formatting solutions on a word search grid.
    /// </summary>
    public interface ISolutionFormatter
    {
        /// <summary>
        /// Changes the given character to format solutions on a word search grid.
        /// </summary>
        /// <param name="solutions">A list of <see cref="WordLocation"/>s in which the character lies, if any.</param>
        /// <param name="row">The row of the character to format.</param>
        /// <param name="col">The column of the character to format.</param>
        /// <param name="c">The original character.</param>
        /// <returns>An instance of <see cref="FormatChar"/> representing the formatted character.</returns>
        FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c);
    }
}