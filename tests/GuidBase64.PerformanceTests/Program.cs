using BenchmarkDotNet.Running;
using GuidBase64.PerformanceTests.Benchmarks;

namespace GuidBase64.PerformanceTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Base64GuidToString>();
            var summary2 = BenchmarkRunner.Run<Base64GuidNewFromString>();
        }
    }
}
