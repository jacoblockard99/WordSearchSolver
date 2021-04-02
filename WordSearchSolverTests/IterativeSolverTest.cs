using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WordSearchSolver;
using Xunit;

namespace WordSearchSolverTests
{
    public class IterativeSolverTest
    {
        private WordSearch ReadSample(string name)
        {
            return WordSearch.Parse(File.ReadAllLines($"../../../TestWordSearches/{name}.txt"));
        }

        private string[] ReadWords(string name)
        {
            return File.ReadAllLines($"../../../TestWordSearches/{name}Words.txt");
        }

        private void AssertWordLocation(int startRow, int startCol, int endRow, int endCol, WordLocation actual)
        {
            Assert.Equal(startRow, actual.StartRow);
            Assert.Equal(startCol, actual.StartCol);
            Assert.Equal(endRow, actual.EndRow);
            Assert.Equal(endCol, actual.EndCol);
        }
        
        // Constructor

        [Fact]
        public void Constructor_GivenBlankWord_ThrowsProperException()
        {
            var e = Assert.Throws<ArgumentException>(() => new IterativeSolver(new WordSearch(10, 10), new[] {""}));
            Assert.Equal($"{IterativeSolver.BlankWordError} (Parameter 'words')", e.Message);
        }
        
        // .Solve()

        [Fact]
        public void Solve_WithTypicalWordSearch_SolvesCorrectly()
        {
            var result = new IterativeSolver(ReadSample("HarryPotter"), ReadWords("HarryPotter")).Solve();
            
            AssertWordLocation(0, 5, 4, 5, result["HARRY"][0]);
            AssertWordLocation(4, 0, 9, 5, result["POTTER"][0]);
            AssertWordLocation(3, 8, 11, 8, result["JKROWLING"][0]);
            AssertWordLocation(4, 7, 9, 7, result["RONALD"][0]);
            AssertWordLocation(12, 0, 12, 6, result["WEASLEY"][0]);
            AssertWordLocation(4, 9, 11, 9, result["HERMIONE"][0]);
            AssertWordLocation(7, 10, 13, 10, result["GRANGER"][0]);
            AssertWordLocation(13, 1, 13, 8, result["HOGWARTS"][0]);
            AssertWordLocation(4, 11, 13, 11, result["WITCHCRAFT"][0]);
            AssertWordLocation(12, 7, 5, 0, result["WIZARDRY"][0]);
            AssertWordLocation(1, 6, 9, 6, result["VOLDEMORT"][0]);
            AssertWordLocation(0, 10, 4, 10, result["ALBUS"][0]);
            AssertWordLocation(5, 5, 5, 2, result["SCAR"][0]);
            AssertWordLocation(11, 0, 11, 5, result["HAGRID"][0]);
            AssertWordLocation(3, 0, 10, 7, result["DARKARTS"][0]);
            AssertWordLocation(0, 1, 0, 9, result["SLYTHERIN"][0]);
        }

        [Fact]
        public void Solve_WithMultipleInstances_SolvesCorrectly()
        {
            var result = new IterativeSolver(ReadSample("Multiple"), new List<string> {"KEYWORD"}).Solve();
            Assert.Equal(4, result["KEYWORD"].Count);
        }

        [Fact]
        public void Solve_WithOverlappingWordsWhenAllowed_FindsAll()
        {
            var result =
                new IterativeSolver(ReadSample("MultipleFromSameLocation"), ReadWords("MultipleFromSameLocation"), true)
                    .Solve();
            
            AssertWordLocation(9, 14, 8, 13, result["KE"][0]);
            AssertWordLocation(9, 14, 7, 12, result["KEY"][0]);
            AssertWordLocation(9, 14, 3, 8, result["KEYWORD"][0]);
            AssertWordLocation(9, 14, 2, 7, result["KEYWORDS"][0]);
            AssertWordLocation(9, 14, 0, 5, result["KEYWORDSER"][0]);
        }
        
        [Fact]
        public void Solve_WithOverlappingWordsWhenDisallowed_FindsOnlyShortest()
        {
            var result =
                new IterativeSolver(ReadSample("MultipleFromSameLocation"), ReadWords("MultipleFromSameLocation"))
                    .Solve();
            
            AssertWordLocation(9, 14, 8, 13, result["KE"][0]);
            Assert.Empty(result["KEY"]);
            Assert.Empty(result["KEYWORD"]);
            Assert.Empty(result["KEYWORDS"]);
            Assert.Empty(result["KEYWORDSER"]);
        }
    }
}