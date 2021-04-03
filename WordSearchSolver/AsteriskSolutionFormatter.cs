using System.Collections.Generic;
using System.Linq;

namespace WordSearchSolver
{
    public class AsteriskSolutionFormatter : ISolutionFormatter
    {
        public FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c)
        {
            return new(solutions.Any() ? "*" : c.ToString());
        }
    }
}