using System;
using System.Collections.Generic;
using System.Linq;
using Asker;
using Asker.Conversion.Converters;
using Asker.Validation.Validators;
using WordSearchSolver;

namespace WordSearchSolverConsole
{
    public class WordSearchSolverConsole
    {
        private static readonly IEnsuredAsker<bool> BooleanAsker = new GeneralAsker<bool>(
            new ValidatedConverter<string, bool>(
                new PresenceValidator(),
                new StringToBooleanConverter()
            )
        );

        private WordSearch WordSearch { get; }
        private IEnumerable<string> Words { get; }
        private bool AllowOverlapping { get; }
        private WordSearchFormatter Formatter { get; }

        public WordSearchSolverConsole(WordSearch wordSearch, IEnumerable<string> words, bool allowOverlapping,
            WordSearchFormatter formatter)
        {
            WordSearch = wordSearch;
            Words = words;
            AllowOverlapping = allowOverlapping;
            Formatter = formatter;
        }

        public bool Confirm()
        {
            Console.WriteLine($"Search Words: {string.Join(", ", Words)}.");
            Console.WriteLine();
            Console.WriteLine("Word Search:");
            Console.WriteLine(WordSearchFormatter.Default.Format(WordSearch));
            Console.WriteLine();

            return BooleanAsker.Ask("Look good?");
        }

        public void Launch()
        {
            var result = new IterativeSolver(WordSearch, Words, AllowOverlapping).Solve();
            
            Console.WriteLine("Solved:\n");
            Console.WriteLine(Formatter.Format(WordSearch, result.Values.SelectMany(list => list)));
            
            Console.WriteLine();
            Console.WriteLine("Coordinates of Solutions:\n");

            foreach (var word in Words)
            {
                var entry = $"'{word}': ";
                var space = new string(' ', entry.Length);
                var first = result[word].Any() ? result[word][0].ToString() : "[None found]";
                
                Console.WriteLine(entry + first);

                foreach (var location in result[word].Skip(1))
                {
                    Console.WriteLine(space + location);
                }
            }
        }
    }
}