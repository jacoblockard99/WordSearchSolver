using WordSearchSolver;

namespace WordSearchSolverTests.Helpers
{
    public static class TestUtil
    {
        public static WordSearch ThreeByFourWordSearch()
        {
            return new(ThreeByFourGrid());
        }
        
        public static char[,] ThreeByFourGrid()
        {
            return new[,]
            {
                {'a', 'b', 'c'},
                {'d', 'e', 'f'},
                {'g', 'h', 'i'},
                {'j', 'k', 'l'}
            };
        }

        public static WordSearch SixByFiveWordSearch()
        {
            return new(SixByFiveGrid());
        }

        public static char[,] SixByFiveGrid()
        {
            return new[,]
            {
                {'a', 'b', 'c', 'd', 'e', 'f'},
                {'g', 'h', 'i', 'j', 'k', 'l'},
                {'m', 'n', 'o', 'p', 'q', 'r'},
                {'s', 't', 'u', 'v', 'w', 'x'},
                {'y', 'z', 'a', 'b', 'c', 'd'}
            };
        }
    }
}