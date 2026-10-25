namespace RussianTransliteration.Benchmarks
{
    /// <summary>
    /// The kind of text passed to the transliterator in benchmarks.
    /// </summary>
    public enum InputKind
    {
        /// <summary>
        /// An article title in sentence case.
        /// </summary>
        Title,

        /// <summary>
        /// An article title in uppercase, which exercises the case logic for letters that map to several Latin letters.
        /// </summary>
        UpperCaseTitle,

        /// <summary>
        /// A long archive file name in mixed case that exercises all transliteration rules at once.
        /// </summary>
        ArchiveName,

        /// <summary>
        /// A text without Cyrillic letters, which shows the cost of a call that changes nothing.
        /// </summary>
        Latin
    }
}
