using System.Globalization;
using System.Linq;

namespace RussianTransliteration.Tests
{
    /// <summary>
    /// Tests for <see cref="RussianTransliterator"/>.
    /// </summary>
    public class RussianTransliteratorTests
    {
        #region Input without Russian letters

        /// <summary>
        /// <see langword="null"/> is returned as is.
        /// </summary>
        [Fact]
        public void ToLatin_Null_ReturnsNull()
        {
            Assert.Null(((string?)null).ToLatin());
        }

        /// <summary>
        /// Text without Russian letters, including other Cyrillic alphabets, is returned as the same instance without allocations.
        /// </summary>
        /// <param name="value">The text without Russian letters.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Hello, world!")]
        [InlineData("2026-10-24 12:00")]
        [InlineData("ҐЇЄІЎ ґїєіў")]
        public void ToLatin_NoRussianLetters_ReturnsSameInstance(string value)
        {
            Assert.Same(value, value.ToLatin());
        }

        #endregion

        #region Letters

        /// <summary>
        /// Every letter of the alphabet is transliterated in both cases.
        /// </summary>
        /// <param name="value">The alphabet.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("абвгдеёжзийклмнопрстуфхцчшщъыьэюя", "abvgdeezhziyklmnoprstufkhtschshshchyeyuya")]
        [InlineData("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ", "ABVGDEEZHZIYKLMNOPRSTUFKHTSCHSHSHCHYEYUYA")]
        public void ToLatin_Alphabet_TransliteratesEveryLetter(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// Letters that map to several Latin letters are transliterated in lowercase words.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("борщ", "borshch")]
        [InlineData("цапля", "tsaplya")]
        [InlineData("чайник", "chaynik")]
        [InlineData("шуба", "shuba")]
        [InlineData("юла", "yula")]
        [InlineData("эскимо", "eskimo")]
        [InlineData("фиалка", "fialka")]
        public void ToLatin_LowercaseWord_TransliteratesLetters(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// Ё is transliterated as E in both cases.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("ёлочка", "elochka")]
        [InlineData("ёрш", "ersh")]
        [InlineData("Ёжик", "Ezhik")]
        [InlineData("ЁЖИК", "EZHIK")]
        [InlineData("ЕЁ", "EE")]
        public void ToLatin_Yo_TransliteratesAsE(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// The soft and hard signs are removed in both cases.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("гвоздь", "gvozd")]
        [InlineData("съезд", "sezd")]
        [InlineData("подъём", "podem")]
        [InlineData("ПОДЪЁМ", "PODEM")]
        [InlineData("вьюга", "vyuga")]
        [InlineData("ЪЬ", "")]
        public void ToLatin_SoftAndHardSigns_AreRemoved(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        #endregion

        #region Letter case

        /// <summary>
        /// In a capitalized word only the first Latin letter of a multi-letter replacement is uppercased.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("Жираф", "Zhiraf")]
        [InlineData("Чебурашка", "Cheburashka")]
        [InlineData("Щегол", "Shchegol")]
        [InlineData("Юпитер", "Yupiter")]
        [InlineData("Цирк", "Tsirk")]
        [InlineData("Вьюга", "Vyuga")]
        public void ToLatin_CapitalizedWord_UppercasesFirstLatinLetter(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// In an uppercase word all Latin letters of a multi-letter replacement are uppercased.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("ЖИРАФ", "ZHIRAF")]
        [InlineData("ЧЕБУРАШКА", "CHEBURASHKA")]
        [InlineData("ЩЕГОЛ", "SHCHEGOL")]
        [InlineData("ЮПИТЕР", "YUPITER")]
        [InlineData("ЦИРК", "TSIRK")]
        [InlineData("ВЬЮГА", "VYUGA")]
        [InlineData("ЦУМ", "TSUM")]
        public void ToLatin_UppercaseWord_UppercasesAllLatinLetters(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// At the end of an uppercase word the previous letter decides the case, whatever follows the word.
        /// </summary>
        /// <param name="value">The text.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("ЁРШ!", "ERSH!")]
        [InlineData("МЯЧ.", "MYACH.")]
        [InlineData("КОВШ И ЛОЖКА", "KOVSH I LOZHKA")]
        [InlineData("ПЛАЩ-ПАЛАТКА", "PLASHCH-PALATKA")]
        [InlineData("ТИШЬ, ГЛАДЬ", "TISH, GLAD")]
        [InlineData("МЫШЬ", "MYSH")]
        [InlineData("Мышь", "Mysh")]
        [InlineData("ЖЭК и ЦУМ", "ZHEK i TSUM")]
        public void ToLatin_EndOfUppercaseWord_UppercasesAllLatinLetters(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// A single-letter word follows the case of the neighbouring words, the next word first.
        /// </summary>
        /// <param name="value">The text.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("Я", "Ya")]
        [InlineData("Я сам", "Ya sam")]
        [InlineData("Я САМ", "YA SAM")]
        [InlineData("И Я ТОЖЕ", "I YA TOZHE")]
        [InlineData("Я, ВЫ И ОНИ", "YA, VY I ONI")]
        [InlineData("ГДЕ Я", "GDE YA")]
        [InlineData("Где Я", "Gde Ya")]
        [InlineData("Ю Я", "Yu Ya")]
        [InlineData("Ж2", "Zh2")]
        public void ToLatin_SingleLetterWord_FollowsNeighbouringWords(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// In a mixed-case word the case of each letter is preserved and the next letter decides the case of a multi-letter replacement.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("ЧаЩа", "ChaShcha")]
        [InlineData("чАЩА", "chASHCHA")]
        [InlineData("ЯNDEX", "YANDEX")]
        [InlineData("Яndex", "Yandex")]
        public void ToLatin_MixedCaseWord_PreservesCase(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        #endregion

        #region Letter 'х'

        /// <summary>
        /// Х becomes H after a Russian vowel.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("смех", "smeh")]
        [InlineData("эхо", "eho")]
        [InlineData("вихрь", "vihr")]
        [InlineData("Махаон", "Mahaon")]
        [InlineData("ОХОТА", "OHOTA")]
        [InlineData("Яхта", "Yahta")]
        [InlineData("ЯХТА", "YAHTA")]
        [InlineData("аХ", "aH")]
        public void ToLatin_KhaAfterVowel_BecomesH(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// Х becomes H after К, so КХ reads as KH instead of KKH.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("кхе-кхе", "khe-khe")]
        [InlineData("КХЕ", "KHE")]
        [InlineData("ЖКХ", "ZHKH")]
        public void ToLatin_KhaAfterKa_BecomesH(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// Х stays KH at the start of a word, after other consonants and after non-Russian letters, where H would form a different digraph.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("Хлеб", "Khleb")]
        [InlineData("ХЛЕБ", "KHLEB")]
        [InlineData("схватка", "skhvatka")]
        [InlineData("архив", "arkhiv")]
        [InlineData("АРХИВ", "ARKHIV")]
        [InlineData("aх", "akh")]
        public void ToLatin_KhaElsewhere_BecomesKh(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        #endregion

        #region Endings 'ий' and 'ый'

        /// <summary>
        /// ИЙ at the end of a word becomes Y in the case of И.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("синий", "siny")]
        [InlineData("Синий", "Siny")]
        [InlineData("СИНИЙ", "SINY")]
        [InlineData("синий-синий", "siny-siny")]
        [InlineData("Кий2", "Ky2")]
        public void ToLatin_IyAtEndOfWord_BecomesY(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// ИЙ inside a word is transliterated letter by letter.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("Бийск", "Biysk")]
        [InlineData("Вийон", "Viyon")]
        public void ToLatin_IyInsideWord_IsTransliteratedLetterByLetter(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// ЫЙ is transliterated letter by letter as YY.
        /// </summary>
        /// <param name="value">The word.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("сильный", "silnyy")]
        [InlineData("Добрый", "Dobryy")]
        [InlineData("ДОБРЫЙ", "DOBRYY")]
        public void ToLatin_Yy_IsTransliteratedLetterByLetter(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        #endregion

        #region Other characters

        /// <summary>
        /// Characters other than Russian letters are left unchanged, including other Cyrillic letters, emoji and combining marks.
        /// </summary>
        /// <param name="value">The text.</param>
        /// <param name="expected">The expected transliteration.</param>
        [Theory]
        [InlineData("Версия 2.0 (beta) — релиз!", "Versiya 2.0 (beta) — reliz!")]
        [InlineData("iPhone в Москве", "iPhone v Moskve")]
        [InlineData("Привет 👋 мир", "Privet 👋 mir")]
        [InlineData("Київ", "Kiїv")]
        [InlineData("Ґанок", "Ґanok")]
        [InlineData("что\u0300", "chto\u0300")]
        public void ToLatin_OtherCharacters_AreLeftUnchanged(string value, string expected)
        {
            Assert.Equal(expected, value.ToLatin());
        }

        /// <summary>
        /// A long text is transliterated entirely.
        /// </summary>
        [Fact]
        public void ToLatin_LongText_TransliteratesEntirely()
        {
            var value = string.Concat(Enumerable.Repeat("Щи да каша — пища наша. ", 10_000));
            var expected = string.Concat(Enumerable.Repeat("Shchi da kasha — pishcha nasha. ", 10_000));

            Assert.Equal(expected, value.ToLatin());
        }

        #endregion

        #region Culture

        /// <summary>
        /// The result does not depend on the current culture, in particular И is not transliterated as the Turkish İ.
        /// </summary>
        /// <param name="cultureName">The name of the current culture.</param>
        [Theory]
        [InlineData("tr-TR")]
        [InlineData("az-Latn-AZ")]
        [InlineData("en-US")]
        [InlineData("ru-RU")]
        public void ToLatin_AnyCulture_ReturnsSameResult(string cultureName)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureName);

                Assert.Equal("Ivan Ilich IVANOV", "Иван Ильич ИВАНОВ".ToLatin());
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        #endregion

        #region Obsolete API

        /// <summary>
        /// The obsolete <see cref="RussianTransliterator.GetTransliteration"/> returns the same result as <see cref="RussianTransliterator.ToLatin"/>.
        /// </summary>
        [Fact]
        public void GetTransliteration_ReturnsSameResultAsToLatin()
        {
            const string value = "Щи да каша — пища наша";

#pragma warning disable CS0618
            Assert.Equal(value.ToLatin(), RussianTransliterator.GetTransliteration(value));
#pragma warning restore CS0618
        }

        #endregion
    }
}
