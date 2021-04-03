using System;

namespace WordSearchSolverConsole
{
    public static class ErrorHandler
    {
        public static void HandleIOExeption(string situation, Exception e)
        {
            Console.WriteLine($"There was a problem{FormatSituation(situation)}! The error was '{e.Message}'.");
        }

        public static void HandleUnknownException(Exception e)
        {
            HandleUnknownException("", e);
        }

        public static void HandleUnknownException(string situation, Exception e)
        {
            Console.WriteLine($"An unknown error occurred{FormatSituation(situation)}! The message was '{e.Message}'.");
        }

        private static string FormatSituation(string situation)
        {
            return situation == "" ? "" : " " + situation;
        }
    }
}