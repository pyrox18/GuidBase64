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

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsUrlUnsafeBase64String(Guid input, string expected)
            {
                expected = expected.Replace("-", "+").Replace("_", "/");

                var base64Guid = new Base64Guid(input, options => options.UseStandardBase64Encoding());

                var result = base64Guid.ToString();

                Assert.Equal(expected, result);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsBase64StringWithPadding(Guid input, string expected)
            {
                expected += "==";
                var base64Guid = new Base64Guid(input, options => options.UsePadding());

                var result = base64Guid.ToString();

                Assert.Equal(expected, result);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64StringData))]
            public void ReturnsUrlUnsafeBase64StringWithPadding(Guid input, string expected)
            {
                expected = expected.Replace("-", "+").Replace("_", "/") + "==";

                var base64Guid = new Base64Guid(input, options =>
                {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                });

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
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64GuidFromUrlUnsafeInput(Guid expected, string input)
            {
                input = input.Replace("-", "+").Replace("_", "/");

                var result = Base64Guid.Parse(input, options => options.UseStandardBase64Encoding());

                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64GuidFromInputWithPadding(Guid expected, string input)
            {
                input += "==";

                var result = Base64Guid.Parse(input, options => options.UsePadding());

                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsBase64GuidData))]
            public void ReturnsBase64GuidFromUrlUnsafeInputWithPadding(Guid expected, string input)
            {
                input = input.Replace("-", "+").Replace("_", "/") + "==";

                var result = Base64Guid.Parse(input, options =>
                {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                });

                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringLengthIsInvalidData))]
            public void ThrowsWhenStringLengthIsInvalid(string input)
            {
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringLengthIsInvalidData))]
            public void ThrowsWhenStringLengthIsInvalidForInputWithPadding(string input)
            {
                input += "==";
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input, options => options.UsePadding()));
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringContentIsInvalidData))]
            public void ThrowsWhenStringContentIsInvalid(string input)
            { 
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input));
            }

            [Theory]
            [MemberData(nameof(ThrowsWhenStringContentIsInvalidData))]
            public void ThrowsWhenStringContentIsInvalidForUrlUnsafeInput(string input)
            {
                input = input.Replace("+", "-").Replace("/", "_");
                Assert.Throws<FormatException>(() => Base64Guid.Parse(input, options => options.UseStandardBase64Encoding()));
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
            [MemberData(nameof(ReturnsTrueWithBase64GuidResultData))]
            public void ReturnsTrueWithBase64GuidResultFromUrlUnsafeInput(Guid expected, string input)
            {
                input = input.Replace("-", "+").Replace("_", "/");

                var result = Base64Guid.TryParse(input, options => options.UseStandardBase64Encoding(), out Base64Guid output);

                Assert.Equal(expected, output.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsTrueWithBase64GuidResultData))]
            public void ReturnsTrueWithBase64GuidResultFromInputWithPadding(Guid expected, string input)
            {
                input += "==";

                var result = Base64Guid.TryParse(input, options => options.UsePadding(), out Base64Guid output);

                Assert.Equal(expected, output.Guid);
            }

            [Theory]
            [MemberData(nameof(ReturnsTrueWithBase64GuidResultData))]
            public void ReturnsTrueWithBase64GuidResultFromUrlUnsafeInputWithPadding(Guid expected, string input)
            {
                input = input.Replace("-", "+").Replace("_", "/") + "==";

                var result = Base64Guid.TryParse(input, options =>
                {
                    options.UseStandardBase64Encoding();
                    options.UsePadding();
                }, out Base64Guid output);

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
            [MemberData(nameof(ReturnsFalseWhenStringLengthIsInvalidData))]
            public void ReturnsFalseWhenStringLengthIsInvalidForInputWithPadding(string input)
            {
                input += "==";

                var result = Base64Guid.TryParse(input, options => options.UsePadding(), out Base64Guid output);

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

            [Theory]
            [MemberData(nameof(ReturnsFalseWhenStringContentIsInvalidData))]
            public void ReturnsFalseWhenStringContentIsInvalidForUrlUnsafeInput(string input)
            {
                input = input.Replace("+", "-").Replace("/", "_");

                var result = Base64Guid.TryParse(input, options => options.UseStandardBase64Encoding(), out Base64Guid output);

                Assert.False(result);
                Assert.Equal(default(Base64Guid), output);
            }
        }
    }
}
