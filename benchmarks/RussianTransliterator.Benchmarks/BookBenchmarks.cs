using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.IO;
using System.Text;

namespace RussianTransliteration.Benchmarks
{
    /// <summary>
    /// Measures the speed and memory usage of <see cref="RussianTransliterator.ToLatin"/> on the whole "War and Peace".
    /// </summary>
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net48)]
    public class BookBenchmarks
    {
        #region Fields

        private const string BookResourceName = "Voyna i mir.txt";

        private string _book = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the book from the embedded resource.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            using (var stream = typeof(BookBenchmarks).Assembly.GetManifestResourceStream(BookResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"The embedded resource '{BookResourceName}' is not found.");
                }

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    _book = reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Transliterates the book.
        /// </summary>
        /// <returns>The transliterated book.</returns>
        [Benchmark]
        public string ToLatin()
        {
            return _book.ToLatin();
        }

        #endregion
    }
}
