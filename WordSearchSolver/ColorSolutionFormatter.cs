using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WordSearchSolver;

namespace WordSearchSolver
{
    public class ColorSolutionFormatter : ISolutionFormatter
    {
        private static readonly int[] ColorList = {1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14};
        
        private IDictionary<WordLocation, string> Colors { get; }
        private ColorCycler Cycler { get; }

        public ColorSolutionFormatter()
        {
            Colors = new Dictionary<WordLocation, string>();
            Cycler = new ColorCycler(id => $"\u001b[38;5;{id}m", ColorList);
        }
        
        public FormattedChar FormatChar(IEnumerable<WordLocation> solutions, int row, int col, char c)
        {
            return solutions.Any()
                ? new FormattedChar($"{ColorCode(solutions.First())}{c}\u001b[0m", c.ToString())
                : new FormattedChar(c.ToString());
        }

        private string ColorCode(WordLocation location)
        {
            string code;
            
            if (Colors.ContainsKey(location))
            {
                code = Colors[location];
            } else
            {
                code = Cycler.NextColorCode();
                Colors[location] = code;
            }

            return code;
        }
    }
}