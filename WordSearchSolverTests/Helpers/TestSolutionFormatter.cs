using System.Collections.Generic;
using System.Linq;
using WordSearchSolver;

namespace WordSearchSolverTests.Helpers
{
    public class TestSolutionFormatter : ISolutionFormatter
    {
        public int Length { get; init; } = 1;
        
        public FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c)
        {
            return new(solutions.Any() ? new string('*', Length) : c.ToString());
        }
    }
}