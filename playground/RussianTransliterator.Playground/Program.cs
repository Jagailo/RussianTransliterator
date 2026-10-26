using System;
using TextCopy;

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

                var result = input.ToLatin();
                ClipboardService.SetText(result);
                Console.WriteLine($"Result: {result}");
                Console.WriteLine();
            }
        }

        #endregion
    }
}
