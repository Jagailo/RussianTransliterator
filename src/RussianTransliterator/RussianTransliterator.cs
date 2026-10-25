using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace RussianTransliteration
{
    /// <summary>
    /// Transliterates Russian Cyrillic text to the Latin alphabet.
    /// </summary>
    public static class RussianTransliterator
    {
        #region Fields

        private const string Vowels = "аеёиоуыэюя";

        // Indexed by the offset from 'а'; 'ё' is handled separately:
        //   а    б    в    г    д    е    ж     з    и    й    к    л    м    н    о    п
        //   р    с    т    у    ф    х     ц     ч     ш     щ       ъ   ы    ь   э    ю     я
        private static readonly string[] Letters =
        [
            "a", "b", "v", "g", "d", "e", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p",
            "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya"
        ];

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
        public static string? GetTransliteration(string? value)
        {
            return value.ToLatin();
        }

        /// <summary>
        /// Transliterates the specified Russian Cyrillic text to the Latin alphabet.
        /// </summary>
        /// <remarks>Letter case is preserved. Characters other than Russian Cyrillic letters are left unchanged.</remarks>
        /// <param name="value">The text to transliterate.</param>
        /// <returns>The transliterated text, or <see langword="null"/> if <paramref name="value"/> is <see langword="null"/>.</returns>
        [return: NotNullIfNotNull(nameof(value))]
        public static string? ToLatin(this string? value)
        {
            if (value == null)
            {
                return value;
            }

            var start = IndexOfRussianLetter(value);
            if (start < 0)
            {
                return value;
            }

            // Room for multi-letter replacements (ж -> zh, щ -> shch), so that a typical text fits without growing the builder
            var builder = new StringBuilder(value.Length + value.Length / 8);
            builder.Append(value, 0, start);
            for (var i = start; i < value.Length; i++)
            {
                var letter = value[i];
                var lowerLetter = ToLowerRussian(letter);
                if (lowerLetter == '\0')
                {
                    builder.Append(letter);

                    continue;
                }

                var isUpper = lowerLetter != letter;

                // Василий -> Vasily, but Бийск -> Biysk
                if (lowerLetter == 'и' && i + 1 < value.Length && ToLowerRussian(value[i + 1]) == 'й' && !IsLetterAt(value, i + 2))
                {
                    builder.Append(isUpper ? 'Y' : 'y');
                    i++;

                    continue;
                }

                var latin = lowerLetter == 'ё' ? "e" : Letters[lowerLetter - 'а'];

                // Ах -> Ah, ЖКХ -> ZHKH, but Рх -> Rkh: after other consonants 'h' would form a different digraph (сх -> sh)
                if (lowerLetter == 'х' && i > 0 && (ToLowerRussian(value[i - 1]) == 'к' || Vowels.IndexOf(ToLowerRussian(value[i - 1])) >= 0))
                {
                    latin = "h";
                }

                var isUpperContext = isUpper && latin.Length > 1 && IsUpperContext(value, i);
                for (var j = 0; j < latin.Length; j++)
                {
                    builder.Append(isUpper && (j == 0 || isUpperContext) ? char.ToUpperInvariant(latin[j]) : latin[j]);
                }
            }

            return builder.ToString();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Finds the first Russian letter in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The source text.</param>
        /// <returns>The index of the first Russian letter, or <c>-1</c> if there is none.</returns>
        private static int IndexOfRussianLetter(string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (IsRussianLetter(value[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether <paramref name="letter"/> is a letter of the Russian alphabet.
        /// </summary>
        /// <remarks>The range 'А'..'я' is the whole Russian alphabet except Ё.</remarks>
        /// <param name="letter">The character to check.</param>
        /// <returns><see langword="true"/> if <paramref name="letter"/> is a Russian letter; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsRussianLetter(char letter)
        {
            return letter is >= 'А' and <= 'я' or 'Ё' or 'ё';
        }

        /// <summary>
        /// Converts a Russian letter to lowercase by its code.
        /// </summary>
        /// <param name="letter">The character to convert.</param>
        /// <returns>The lowercase Russian letter, or <c>'\0'</c> if <paramref name="letter"/> is not a Russian letter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char ToLowerRussian(char letter)
        {
            if (!IsRussianLetter(letter))
            {
                return '\0';
            }

            return letter switch
            {
                >= 'А' and <= 'Я' => (char)(letter + ('а' - 'А')),
                'Ё' => 'ё',
                _ => letter
            };
        }

        /// <summary>
        /// Determines whether all Latin letters of an uppercase letter are uppercased: ЯГА -> YAGA, Яга -> Yaga, ПЛЯЖ МОРЯ -> PLYAZH MORYA.
        /// </summary>
        /// <remarks>The next letter of the same word decides, then the previous one. A single-letter word follows the neighbouring words.</remarks>
        /// <param name="value">The source text.</param>
        /// <param name="index">The index of the uppercase letter.</param>
        /// <returns><see langword="true"/> to uppercase all Latin letters; <see langword="false"/> to uppercase only the first one.</returns>
        private static bool IsUpperContext(string value, int index)
        {
            if (IsLetterAt(value, index + 1))
            {
                return char.IsUpper(value[index + 1]);
            }

            if (IsLetterAt(value, index - 1))
            {
                return char.IsUpper(value[index - 1]);
            }

            return IsUpperWord(value, index + 1, 1) ?? IsUpperWord(value, index - 1, -1) ?? false;
        }

        /// <summary>
        /// Determines whether the nearest word starting from <paramref name="start"/> in the <paramref name="step"/> direction is written in uppercase.
        /// </summary>
        /// <param name="value">The source text.</param>
        /// <param name="start">The index to start searching from.</param>
        /// <param name="step"><c>1</c> to search forward, <c>-1</c> to search backward.</param>
        /// <returns>
        /// <see langword="true"/> if the word is uppercase; <see langword="false"/> if it contains a lowercase letter;
        /// <see langword="null"/> if there is no word or it is a single uppercase letter, which says nothing about the case.
        /// </returns>
        private static bool? IsUpperWord(string value, int start, int step)
        {
            var i = start;
            while (i >= 0 && i < value.Length && !char.IsLetter(value[i]))
            {
                i += step;
            }

            var length = 0;
            for (; IsLetterAt(value, i); i += step, length++)
            {
                if (!char.IsUpper(value[i]))
                {
                    return false;
                }
            }

            return length > 1 ? true : null;
        }

        /// <summary>
        /// Determines whether <paramref name="index"/> is within <paramref name="value"/> and points to a letter.
        /// </summary>
        /// <param name="value">The source text.</param>
        /// <param name="index">The index to check, may be out of range.</param>
        /// <returns><see langword="true"/> if there is a letter at <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
        private static bool IsLetterAt(string value, int index)
        {
            return index >= 0 && index < value.Length && char.IsLetter(value[index]);
        }

        #endregion
    }
}
