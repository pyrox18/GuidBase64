using System;
using Xunit;
using GuidBase64.Extensions;
using System.Collections.Generic;

namespace GuidBase64.UnitTests
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
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsBase64String(Guid guid, string expected)
            {
                var result = guid.ToBase64String();

                Assert.Equal(expected, result);
            }
        }
    }
}
