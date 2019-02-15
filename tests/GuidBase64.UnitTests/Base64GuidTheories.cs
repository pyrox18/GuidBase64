using System;
using System.Collections.Generic;
using Xunit;
using GuidBase64;
using GuidBase64.CommonTestData;

namespace GuidBase64.UnitTests
{
    public class Base64GuidTheories
    {
        public class ToStringMethod
        {
            public static IEnumerable<object[]> ReturnsBase64StringData => TestData.Base64GuidPairs;

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
            public static IEnumerable<object[]> ReturnsByteArrayData => TestData.Base64GuidPairs;

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
            public static IEnumerable<object[]> ReturnsBase64GuidData => TestData.Base64GuidPairs;
            public static IEnumerable<object[]> ThrowsWhenStringLengthIsInvalidData => TestData.InvalidLengthBase64GuidStrings;
            public static IEnumerable<object[]> ThrowsWhenStringContentIsInvalidData => TestData.InvalidContentBase64GuidStrings;

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
            public static IEnumerable<object[]> ReturnsTrueWithBase64GuidResultData => TestData.Base64GuidPairs;
            public static IEnumerable<object[]> ReturnsFalseWhenStringLengthIsInvalidData => TestData.InvalidLengthBase64GuidStrings;
            public static IEnumerable<object[]> ReturnsFalseWhenStringContentIsInvalidData => TestData.InvalidContentBase64GuidStrings;

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
