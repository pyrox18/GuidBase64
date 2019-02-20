using BenchmarkDotNet.Attributes;

namespace GuidBase64.PerformanceTests.Benchmarks
{
    [ClrJob, MonoJob, CoreJob, CoreRtJob]
    public class Base64GuidNewFromString
    {
        private readonly string _encoded = "_____________________w";
        private readonly string _encodedStandard = "/////////////////////w";
        private readonly string _encodedPadding = "_____________________w==";
        private readonly string _encodedStandardPadding = "/////////////////////w==";

        [Benchmark]
        public string NewFromEncoded() => new Base64Guid(_encoded);

        [Benchmark]
        public string NewFromEncodedWithStandardEncoding() => new Base64Guid(_encodedStandard);

        [Benchmark]
        public string NewFromEncodedWithPadding() => new Base64Guid(_encodedPadding);

        [Benchmark]
        public string NewFromEncodedWithStandardEncodingAndPadding() => new Base64Guid(_encodedStandardPadding);
    }
}
