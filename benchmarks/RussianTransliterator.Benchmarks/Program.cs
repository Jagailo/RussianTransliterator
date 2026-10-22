using BenchmarkDotNet.Running;

namespace RussianTransliteration.Benchmarks
{
    internal static class Program
    {
        #region Methods

        private static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

        #endregion
    }
}
