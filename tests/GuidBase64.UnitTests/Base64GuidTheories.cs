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

        public static IEnumerable<object[]> InvalidStringLengthData =>
            new List<object[]>
            {
                new object[] { "123456789012345678901" },
                new object[] { "12345678901234567890123" }
            };

        public static IEnumerable<object[]> InvalidStringContentData =>
            new List<object[]>
            {
                new object[] { "abcdefghijABCDEFGHIJ1=" },
                new object[] { ":bcdefghijABCDEFGHIJ1=" },
                new object[] { ":bcdefghijABCDEFGHIJ12" }
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

        public class ToByteArrayMethod
        {
            public static IEnumerable<object[]> ReturnsByteArrayData => Base64GuidPairData;

            [Theory]
            [MemberData(nameof(ReturnsByteArrayData))]
            public void ReturnsByteArray(Guid guid, string encoded)
            {
                var base64Guid = new Base64Guid(encoded);

                var result = base64Guid.ToByteArray();
                Assert.Equal(guid.ToByteArray(), result);
            }
        }

        public class ParseStaticMethod
        {
            public static IEnumerable<object[]> ReturnsBase64GuidData => Base64GuidPairData;
            public static IEnumerable<object[]> ThrowsWhenStringLengthIsInvalidData => InvalidStringLengthData;
            public static IEnumerable<object[]> ThrowsWhenStringContentIsInvalidData => InvalidStringContentData;

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64Guid(Guid expected, string input)
            {
                var result = Base64Guid.Parse(input);

                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringLengthIsInvalidData))]
            public void ThrowsWhenStringLengthIsInvalid(string input)
            {
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringContentIsInvalidData))]
            public void ThrowsWhenStringContentIsInvalid(string input)
            { 
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }
        }

        public class TryParseStaticMethod
        {
            public static IEnumerable<object[]> ReturnsTrueWithBase64GuidResultData => Base64GuidPairData;
            public static IEnumerable<object[]> ReturnsFalseWhenStringLengthIsInvalidData => InvalidStringLengthData;
            public static IEnumerable<object[]> ReturnsFalseWhenStringContentIsInvalidData => InvalidStringContentData;

            [Theory]
            [MemberData(nameof(ReturnsTrueWithBase64GuidResultData))]
            public void ReturnsTrueWithBase64GuidResult(Guid expected, string input)
            {
                var result = Base64Guid.TryParse(input, out Base64Guid output);

                Assert.True(result);
                Assert.Equal(expected, output.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsFalseWhenStringLengthIsInvalidData))]
            public void ReturnsFalseWhenStringLengthIsInvalid(string input)
            {
                var result = Base64Guid.TryParse(input, out Base64Guid output);

                Assert.False(result);
                Assert.Equal(default(Base64Guid), output);
            }

            [Theory]
            [MemberData(nameof(ReturnsFalseWhenStringContentIsInvalidData))]
            public void ReturnsFalseWhenStringContentIsInvalid(string input)
            { 
                var result = Base64Guid.TryParse(input, out Base64Guid output);

                Assert.False(result);
                Assert.Equal(default(Base64Guid), output);
            }
        }
    }
}
