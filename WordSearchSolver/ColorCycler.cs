using System;
using System.Collections.Generic;
using System.Linq;

namespace WordSearchSolver
{
    public class ColorCycler
    {
        public Func<int, string> CodeGenerator { get; }
        public IList<int> Colors { get; }

        private int LastIndex { get; set; } = -1;

        public ColorCycler(Func<int, string> codeGenerator, IEnumerable<int> colors)
        {
            CodeGenerator = codeGenerator;
            Colors = colors.ToList();
        }

        public string NextColorCode()
        {
            LastIndex++;
            if (LastIndex == Colors.Count) LastIndex = 0;

            return CodeGenerator(Colors[LastIndex]);
        }
    }
}