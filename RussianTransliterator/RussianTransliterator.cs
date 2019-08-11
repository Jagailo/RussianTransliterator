using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RussianTransliterator
{
    /// <summary>
    /// Cyrillic transliterator to latin
    /// </summary>
    public static class RussianTransliterator
    {
        #region Fields

        private static readonly Dictionary<string, string> vowelCombinations = new Dictionary<string, string>
        {
            { "Ай", "Ay" },
            { "aЙ", "aY" },
            { "ай", "ay" },
            { "АЙ", "AY" },
            { "Ей", "Ey" },
            { "еЙ", "eY" },
            { "ей", "ey" },
            { "ЕЙ", "EY" },
            { "Ий", "Y" },
            { "иЙ", "y" },
            { "ий", "y" },
            { "ИЙ", "Y" },
            { "Ой", "Oy" },
            { "оЙ", "oY" },
            { "ой", "oy" },
            { "ОЙ", "OY" },
            { "Уй", "Uy" },
            { "уЙ", "uY" },
            { "уй", "uy" },
            { "УЙ", "UY" },
            { "Ый", "Yu" },
            { "ыЙ", "yU" },
            { "ый", "yu" },
            { "ЫЙ", "YU" },
            { "Юй", "Yuy" },
            { "юЙ", "yuY" },
            { "юй", "yuy" },
            { "ЮЙ", "YUY" },
            { "Яй", "Yay" },
            { "яЙ", "yaY" },
            { "яй", "yay" },
            { "ЯЙ", "YAY" },
            { "Эй", "Ey" },
            { "эЙ", "eY" },
            { "эй", "ey" },
            { "ЭЙ", "EY" }
        };

        private static readonly Dictionary<char, char> singleLetters = new Dictionary<char, char>
        {
            { 'а', 'a' },
            { 'б', 'b' },
            { 'в', 'v' },
            { 'г', 'g' },
            { 'д', 'd' },
            { 'е', 'e' },
            { 'ё', 'e' },
            { 'з', 'z' },
            { 'и', 'i' },
            { 'й', 'y' },
            { 'к', 'k' },
            { 'л', 'l' },
            { 'м', 'm' },
            { 'н', 'n' },
            { 'о', 'o' },
            { 'п', 'p' },
            { 'р', 'r' },
            { 'с', 's' },
            { 'т', 't' },
            { 'у', 'u' },
            { 'ф', 'f' },
            { 'ы', 'y' },
            { 'э', 'e' }
        };

        private static readonly Dictionary<string, string> doubleLetters = new Dictionary<string, string>
        {
            { "ж", "zh" },
            { "х", "kh" },
            { "ц", "ts" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "shch" },
            { "ю", "yu" },
            { "я", "ya" }
        };

        private static readonly char[] vowelsLetters = { 'а', 'о', 'и', 'е', 'ё', 'э', 'ы', 'у', 'ю', 'я' };

        #endregion

        #region Methods

        /// <summary>
        /// Converts the specified cyrillic string to latin string.
        /// </summary>
        /// <param name="value">Cyrillic string</param>
        /// <returns>Latin string</returns>
        public static string GetTransliteration(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var builder = new StringBuilder(value);

            // 'х' = 'h' if standing after vowels
            // Ах... -> Ah...
            // Рх... -> Rkh...
            for (var i = 1; i < builder.Length; i++)
            {
                if (char.ToLower(builder[i]) == 'х' && vowelsLetters.Contains(char.ToLower(builder[i - 1])))
                {
                    builder[i] = char.IsLower(builder[i]) ? 'h' : 'H';
                }
            }

            // Removing the unpronounceable letters
            builder.Replace("ь", string.Empty);
            builder.Replace("Ь", string.Empty);
            builder.Replace("ъ", string.Empty);
            builder.Replace("Ъ", string.Empty);

            // Replacing vowel combinations with the letter 'й'
            foreach (var combination in vowelCombinations)
            {
                builder.Replace(combination.Key, combination.Value);
            }

            // Uppercase for cyrillic letters with 2+ latin letters
            // ЯГА -> YAGA not ЯГА -> YaGA
            for (var i = 0; i < builder.Length; i++)
            {
                foreach (var letter in doubleLetters)
                {
                    if (builder[i].ToString().Equals(letter.Key.ToUpper()) && (i < builder.Length - 1 && char.IsLetter(builder[i + 1]) && char.IsUpper(builder[i + 1]) || i == builder.Length - 1))
                    {
                        builder.Insert(i, letter.Value.ToUpper());
                        builder.Remove(i + letter.Value.Length, 1);
                    }
                }
            }

            // Replacing single letters
            foreach (var letter in singleLetters)
            {
                builder.Replace(letter.Key, letter.Value);
                builder.Replace(char.ToUpper(letter.Key), char.ToUpper(letter.Value));
            }

            // Replacing double letters with a single letter in uppercase
            // Яга -> Yaga not Яга -> YAga
            foreach (var letter in doubleLetters)
            {
                builder.Replace(letter.Key, letter.Value);
                builder.Replace(letter.Key.ToUpper(), $"{char.ToUpper(letter.Value[0])}{letter.Value.Substring(1)}");
            }

            return builder.ToString();
        }

        #endregion
    }
}
