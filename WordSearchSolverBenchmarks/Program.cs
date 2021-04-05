using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WordSearchSolver;

namespace WordSearchSolverBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarks = new Dictionary<string, Action>
            {
            };

            var temp = new Stopwatch();
            temp.Start();
            temp.Stop();

            foreach (var (_, action) in benchmarks)
            {
                action();
            }

            foreach (var (name, action) in benchmarks)
            {
                var s = new Stopwatch();
                s.Start();
                action();
                s.Stop();
                Console.WriteLine($"'{name}': {s.ElapsedMilliseconds}");
            }
        }
        
        private static WordSearch ReadSample(string name)
        {
            return WordSearch.Parse(File.ReadAllLines($"../../../../WordSearchSolverTests/TestWordSearches/{name}.txt"));
        }

        private static string[] ReadWords(string name)
        {
            return File.ReadAllLines($"../../../../WordSearchSolverTests/TestWordSearches/{name}Words.txt");
        }
    }
}