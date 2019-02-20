using BenchmarkDotNet.Attributes;

namespace GuidBase64.PerformanceTests.Benchmarks
{
    [ClrJob, MonoJob, CoreJob, CoreRtJob]
    public class Base64GuidToString
    {
        private readonly Base64Guid _base64Guid = Base64Guid.NewBase64Guid();
        private readonly Base64Guid _base64GuidWithStandardEncoding = Base64Guid.NewBase64Guid(options => options.UseStandardBase64Encoding());
        private readonly Base64Guid _base64GuidWithPadding = Base64Guid.NewBase64Guid(options => options.UsePadding());
        private readonly Base64Guid _base64GuidWithAllOptions = Base64Guid.NewBase64Guid(options =>
        {
            options.UseStandardBase64Encoding();
            options.UsePadding();
        });

        [Benchmark]
        public string ConvertToString() => _base64Guid.ToString();

        [Benchmark]
        public string ConvertToStringWithStandardEncoding() => _base64GuidWithStandardEncoding.ToString();

        [Benchmark]
        public string ConvertToStringWithPadding() => _base64GuidWithPadding.ToString();

        [Benchmark]
        public string ConvertToStringWithStandardEncodingAndPadding() => _base64GuidWithAllOptions.ToString();
    }
}
