using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RussianTransliteration
{
    /// <summary>
    /// Transliterates Russian Cyrillic text to the Latin alphabet.
    /// </summary>
    public static class RussianTransliterator
    {
        #region Fields

        private static readonly Dictionary<string, string> VowelCombinations = new()
        {
            { "Ай", "Ay" },
            { "аЙ", "aY" },
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

        private static readonly Dictionary<char, char> SingleLetters = new()
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

        private static readonly Dictionary<string, string> DoubleLetters = new()
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

        private static readonly char[] VowelLetters = [ 'а', 'о', 'и', 'е', 'ё', 'э', 'ы', 'у', 'ю', 'я' ];

        #endregion

        #region Methods

        /// <summary>
        /// Transliterates the specified Russian Cyrillic text to the Latin alphabet.
        /// </summary>
        /// <remarks>This method is obsolete. Use <see cref="ToLatin"/> instead.</remarks>
        /// <param name="value">The text to transliterate.</param>
        /// <returns>The transliterated text, or <see langword="null"/> if <paramref name="value"/> is <see langword="null"/>.</returns>
        [Obsolete("Use " + nameof(ToLatin) + " instead.")]
        [return: NotNullIfNotNull(nameof(value))]
        public static string? GetTransliteration(string? value) => ToLatin(value);

        /// <summary>
        /// Transliterates the specified Russian Cyrillic text to the Latin alphabet.
        /// </summary>
        /// <remarks>Letter case is preserved. Characters other than Russian Cyrillic letters are left unchanged.</remarks>
        /// <param name="value">The text to transliterate.</param>
        /// <returns>The transliterated text, or <see langword="null"/> if <paramref name="value"/> is <see langword="null"/>.</returns>
        [return: NotNullIfNotNull(nameof(value))]
        public static string? ToLatin(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var builder = new StringBuilder(value);

            // 'х' becomes 'h' after a vowel
            // Ах... -> Ah...
            // Рх... -> Rkh...
            for (var i = 1; i < builder.Length; i++)
            {
                if (char.ToLower(builder[i]) == 'х' && VowelLetters.Contains(char.ToLower(builder[i - 1])))
                {
                    builder[i] = char.IsLower(builder[i]) ? 'h' : 'H';
                }
            }

            // Removing the soft and hard signs
            builder.Replace("ь", string.Empty);
            builder.Replace("Ь", string.Empty);
            builder.Replace("ъ", string.Empty);
            builder.Replace("Ъ", string.Empty);

            // Replacing combinations of a vowel followed by 'й'
            foreach (var combination in VowelCombinations)
            {
                builder.Replace(combination.Key, combination.Value);
            }

            // Uppercase letters that map to several Latin letters are fully uppercased inside uppercase words
            // ЯГА -> YAGA, not YaGA
            for (var i = 0; i < builder.Length; i++)
            {
                foreach (var letter in DoubleLetters)
                {
                    if (builder[i].ToString().Equals(letter.Key.ToUpper()) && (i < builder.Length - 1 && char.IsLetter(builder[i + 1]) && char.IsUpper(builder[i + 1]) || i == builder.Length - 1))
                    {
                        builder.Insert(i, letter.Value.ToUpper());
                        builder.Remove(i + letter.Value.Length, 1);
                    }
                }
            }

            // Replacing single letters
            foreach (var letter in SingleLetters)
            {
                builder.Replace(letter.Key, letter.Value);
                builder.Replace(char.ToUpper(letter.Key), char.ToUpper(letter.Value));
            }

            // Replacing the remaining letters that map to several Latin letters, capitalizing only the first one
            // Яга -> Yaga, not YAga
            foreach (var letter in DoubleLetters)
            {
                builder.Replace(letter.Key, letter.Value);
                builder.Replace(letter.Key.ToUpper(), $"{char.ToUpper(letter.Value[0])}{letter.Value.Substring(1)}");
            }

            return builder.ToString();
        }

        #endregion
    }
}
