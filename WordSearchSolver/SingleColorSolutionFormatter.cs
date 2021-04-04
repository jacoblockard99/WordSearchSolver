using System.Collections.Generic;
using System.Linq;

namespace WordSearchSolver
{
    public class SingleColorSolutionFormatter : ISolutionFormatter
    {
        public FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c)
        {
            return solutions.Any()
                ? new FormattedChar($"\u001b[31m{c}\u001b[0m", c.ToString())
                : new FormattedChar(c.ToString());
        }
    }
}