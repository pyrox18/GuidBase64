using System;
using System.Collections.Generic;
using System.Linq;

namespace GuidBase64.CommonTestData
{
    public class TestData
    {
        public static IEnumerable<object[]> Base64GuidPairs =>
            new List<object[]>
            {
                new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA" },
                new object[] { new Guid("c6a44c9f-763a-4524-8c0b-04c599f001a6"), "n0ykxjp2JEWMCwTFmfABpg" },
                new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "_____________________w" }
            };

        public static IEnumerable<object[]> InvalidLengthBase64GuidStrings =>
            new List<object[]>
            {
                new object[] { "123456789012345678901" },
                new object[] { "12345678901234567890123" }
            };

        public static IEnumerable<object[]> InvalidContentBase64GuidStrings =>
            new List<object[]>
            {
                new object[] { "abcdefghijABCDEFGHIJ1=" },
                new object[] { ":bcdefghijABCDEFGHIJ1=" },
                new object[] { ":bcdefghijABCDEFGHIJ12" }
            };

        public static IEnumerable<object[]> InvalidBase64GuidStrings =>
            InvalidContentBase64GuidStrings.Concat(InvalidLengthBase64GuidStrings);
    }
}
