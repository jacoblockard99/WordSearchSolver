using System.Collections.Generic;

namespace WordSearchSolver
{
    public class DummySolutionFormatter : ISolutionFormatter
    {
        public FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c)
        {
            return new(c.ToString());
        }
    }
}