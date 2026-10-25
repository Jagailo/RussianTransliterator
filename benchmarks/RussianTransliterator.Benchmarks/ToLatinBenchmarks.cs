using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;

namespace RussianTransliteration.Benchmarks
{
    /// <summary>
    /// Measures the speed and memory usage of <see cref="RussianTransliterator.ToLatin"/> on short strings.
    /// </summary>
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net48)]
    public class ToLatinBenchmarks
    {
        #region Fields

        private string _value = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the kind of text to transliterate.
        /// </summary>
        [ParamsAllValues]
        public InputKind Input { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Prepares the text for the current <see cref="Input"/>.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            _value = Input switch
            {
                InputKind.Title => "Как выбрать тариф ЖКХ в 2026 году: советы и примеры",
                InputKind.UpperCaseTitle => "КАК ВЫБРАТЬ ТАРИФ ЖКХ В 2026 ГОДУ: СОВЕТЫ И ПРИМЕРЫ",
                InputKind.ArchiveName => "Архив_ЮРИДИЧЕСКИЙ_отдел_ЖКХ_Щёлково_ОХРАНА_ТРУДА_Счета_и_ПЛАТЁЖ_за_Январь–Июнь_2026_(Шаблоны_ЦЕНЫ_v2)_ИТОГОВЫЙ.zip",
                InputKind.Latin => "How to choose a utility tariff in 2026: tips and examples",
                _ => throw new ArgumentOutOfRangeException(nameof(Input), Input, null)
            };
        }

        /// <summary>
        /// Transliterates the text.
        /// </summary>
        /// <returns>The transliterated text.</returns>
        [Benchmark]
        public string ToLatin()
        {
            return _value.ToLatin();
        }

        #endregion
    }
}
