using System;
using Xunit;
using GuidBase64;

namespace GuidBase64.UnitTests
{
    public class Base64GuidFacts
    {
        public class ParseMethod
        {
            [Fact]
            public void ThrowsWhenArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => Base64Guid.Parse(null));
            }
        }
    }
}
