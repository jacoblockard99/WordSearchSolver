using System.Collections.Generic;
using System.Collections.Immutable;

namespace WordSearchSolver
{
    /// <summary>
    /// Defines a class that is capable of "solving" a word search.
    /// </summary>
    public interface ISolver
    {
        WordSearch WordSearch { get; }
        IEnumerable<string> Words { get; }

        /// <summary>
        /// Solves the word search by finding the locations of each word.
        /// </summary>
        /// <returns>
        /// An immutable dictionary with the words as keys and their locations as values. If a word could not be found
        /// its associated location is null.
        /// </returns>
        ImmutableDictionary<string, ImmutableList<WordLocation>> Solve();
    }
}