using System;

namespace PS_Akka_DotNet
{
    public static class DisplayHelper
    {
        public static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
        }

        public static void WriteInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {text}");
        }

        public static void WriteResult(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {text}");
        }
    }
}
