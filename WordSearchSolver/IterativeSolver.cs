using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Serialization;

namespace WordSearchSolver
{
    /// <summary>
    /// An implementation of <see cref="ISolver"/> that solves a word search for iterating over every character in the
    /// word search, trying words in all directions and lengths until a match is found.
    /// </summary>
    public class IterativeSolver : ISolver
    {
        /// <summary>
        /// The error message to use when an attempt is made to search for a blank word (i.e. an empty string).
        /// </summary>
        public const string BlankWordError = "Cannot search for a blank word!";
        
        /// <summary>
        /// The word search being solved by this <see cref="IterativeSolver"/>.
        /// </summary>
        public WordSearch WordSearch { get; }
        
        /// <summary>
        /// The list of words to find when solving the word search.
        /// </summary>
        public IEnumerable<string> Words { get; }
        
        /// <summary>
        /// Whether to try and find overlapping words (words that start from the same location and point in the same
        /// direction).
        /// </summary>
        public bool AllowOverlappingWords { get; }
        
        /// <summary>
        /// A list of matches found by this <see cref="IterativeSolver"/>.
        /// </summary>
        private IDictionary<string, IList<WordLocation>> Matches { get; set; }

        /// <summary>
        /// Constructs a new <see cref="IterativeSolver"/> with the given word search and list of words.
        /// </summary>
        /// <param name="wordSearch">The words search to solve.</param>
        /// <param name="words">The list of words to find.</param>
        /// <param name="allowOverlappingWords">Whether to try and find overlapping words (words that start from the
        /// same location and point in the same direction).</param>
        public IterativeSolver(WordSearch wordSearch, IEnumerable<string> words, bool allowOverlappingWords = false)
        {
            if (words.Any(s => !s.Any(WordSearch.IsKeepable)))
                throw new ArgumentException(BlankWordError, nameof(words));
            
            WordSearch = wordSearch;
            Words = words;
            AllowOverlappingWords = allowOverlappingWords;
        }
        
        /// <summary>
        /// Solves the word search by iterating over every character in the word search, trying words in all directions
        /// and all lengths.
        /// </summary>
        /// <returns>An immutable dictionary containing the result. The keys are the words that were searched for, and
        /// the values are lists of <see cref="WordLocation"/>s representing all the instances of that word.</returns>
        public ImmutableDictionary<string, ImmutableList<WordLocation>> Solve()
        {
            // Initialize the matches dictionary.
            Matches = new Dictionary<string, IList<WordLocation>>();
            
            // Add an entry for every search word.
            foreach (var word in Words)
                Matches.Add(word, new List<WordLocation>());
            
            // Iterate over every character in the word search.
            for (var row = 0; row < WordSearch.Height; row++)
                for (var col = 0; col < WordSearch.Width; col++)
                    // For each one, attempt to find a matching word starting from it.
                    TryCharacter(row, col);

            // Return an immutable version of the matches dictionary.
            return Matches.ToImmutableDictionary(p => p.Key, p => p.Value.ToImmutableList());
        }

        /// <summary>
        /// Attempts to find a matching word starting at the given location.
        /// </summary>
        /// <param name="row">The row of the starting location.</param>
        /// <param name="col">The column of the starting location.</param>
        private void TryCharacter(int row, int col)
        {
            // Iterate over every combination of x/y directions, skipping (0, 0).
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        // Check for a match in every valid direction.k
                        TryCharacterInDirection(row, col, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Recursively looks for a matching word from the given starting location in the given direction.
        /// </summary>
        /// <param name="row">The starting row.</param>
        /// <param name="col">The starting column.</param>
        /// <param name="directionX">The horizontal direction integer.</param>
        /// <param name="directionY">The vertical direction integer.</param>
        /// <param name="currentWord">The currently built word.</param>
        /// <param name="n">The current position in the word.</param>
        private void TryCharacterInDirection(int row, int col, int directionX, int directionY,
            string currentWord = "", int n = 0)
        {
            // Calculate the current row and column.
            var currentRow = row + directionY * n;
            var currentCol = col + directionX * n;

            // Return if we've reached the bounds of the word search in this direction.
            if (!WordSearch.InBounds(currentRow, currentCol)) return;

            // Add the character at the current row and column to the current word.
            currentWord += WordSearch.Chars[currentRow, currentCol];

            // Check if the word is a match.
            if (Words.Contains(currentWord))
            {
                // If so add the match and return.
                // Note that, due to the return, multiple words from the same location in the same direction
                // will not be found.
                // This is intentional as it will never happen in an actual word search.
                Matches[currentWord].Add(new WordLocation(row, col, directionX, directionY, n + 1));
                
                if (!AllowOverlappingWords) return;
            }

            // If not, extend the search one character.
            TryCharacterInDirection(row, col, directionX, directionY, currentWord, n + 1);
        }
    }
}