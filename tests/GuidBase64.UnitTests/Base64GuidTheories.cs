using System;
using System.Collections.Generic;
using Xunit;
using GuidBase64;

namespace GuidBase64.UnitTests
{
    public class Base64GuidTheories
    {
        public static IEnumerable<object[]> Base64GuidPairData =>
            new List<object[]>
            {
                new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA" },
                new object[] { new Guid("c6a44c9f-763a-4524-8c0b-04c599f001a6"), "n0ykxjp2JEWMCwTFmfABpg" },
                new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "_____________________w" }
            };

        public class ToStringMethod
        {
            public static IEnumerable<object[]> ReturnsBase64StringData => Base64GuidPairData;

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsBase64String(Guid input, string expected)
            {
                var base64Guid = new Base64Guid(input);

                var result = base64Guid.ToString();

                Assert.Equal(expected, result);
            }
        }

        public class ParseStaticMethod
        {
            public static IEnumerable<object[]> ReturnsBase64GuidData => Base64GuidPairData;

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64Guid(Guid expected, string input)
            {
                var result = Base64Guid.Parse(input);

                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [InlineData("123456789012345678901")]
            [InlineData("12345678901234567890123")]
            public void ThrowsWhenStringLengthIsInvalid(string input)
            {
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }

            [Theory]
            [InlineData("abcdefghijABCDEFGHIJ1=")]
            [InlineData(":bcdefghijABCDEFGHIJ1=")]
            [InlineData(":bcdefghijABCDEFGHIJ12")]
            public void ThrowsWhenStringContentIsInvalid(string input)
            { 
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }
        }
    }
}
