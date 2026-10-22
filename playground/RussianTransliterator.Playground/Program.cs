using System;

namespace RussianTransliteration.Playground
{
    internal static class Program
    {
        #region Methods

        private static void Main()
        {
            while (true)
            {
                Console.Write("Source: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                Console.WriteLine($"Result: {input.ToLatin()}");
                Console.WriteLine();
            }
        }

        #endregion
    }
}
