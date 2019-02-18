using System;
using Xunit;
using GuidBase64.Extensions;
using System.Collections.Generic;

namespace GuidBase64.UnitTests.Extensions
{
    public class GuidExtensionsTheories
    {
        public static IEnumerable<object[]> Base64GuidPairData =>
            new List<object[]>
            {
                new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA" },
                new object[] { new Guid("c6a44c9f-763a-4524-8c0b-04c599f001a6"), "n0ykxjp2JEWMCwTFmfABpg" },
                new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "_____________________w" }
            };

        public class ToBase64GuidMethod
        {
            public static IEnumerable<object[]> ReturnsBase64GuidData => Base64GuidPairData;
            public static IEnumerable<object[]> ReturnsBase64StringData => Base64GuidPairData;

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64Guid(Guid guid, string _)
            {
                var result = guid.ToBase64Guid();

                Assert.Equal(guid, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64GuidWithOptions(Guid guid, string _)
            {
                var result = guid.ToBase64Guid(options =>
                {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                });

                Assert.Equal(guid, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsBase64String(Guid guid, string expected)
            {
                var result = guid.ToBase64String();

                Assert.Equal(expected, result);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsUrlUnsafeBase64String(Guid guid, string expected)
            {
                expected = expected.Replace("-", "+").Replace("_", "/");

                var result = guid.ToBase64String(options => options.UseStandardBase64Encoding());

                Assert.Equal(expected, result);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsBase64StringWithPadding(Guid guid, string expected)
            {
                expected += "==";

                var result = guid.ToBase64String(options => options.UsePadding());

                Assert.Equal(expected, result);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsUrlUnsafeBase64StringWithPadding(Guid guid, string expected)
            {
                expected = expected.Replace("-", "+").Replace("_", "/") + "==";

                var result = guid.ToBase64String(options =>
                {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                });

                Assert.Equal(expected, result);
            }
        }
    }
}
