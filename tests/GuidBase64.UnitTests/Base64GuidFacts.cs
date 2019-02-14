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

        public class GetHashCodeMethod
        {
            [Fact]
            public void ReturnsEqualHashCodes()
            {
                var guid = Guid.NewGuid();
                var a = new Base64Guid(guid);
                var b = new Base64Guid(guid);

                Assert.Equal(a.GetHashCode(), b.GetHashCode());
            }
        }

        public class EqualsOperator
        {
            [Fact]
            public void ReturnsTrue()
            {
                var guid = new Guid();
                var a = new Base64Guid(guid);
                var b = new Base64Guid(guid);

                var result = a == b;

                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalse()
            {
                var a = new Base64Guid(Guid.Empty);
                var b = new Base64Guid(Guid.NewGuid());

                var result = a == b;

                Assert.False(result);
            }
        }

        public class NotEqualsOperator
        {
            [Fact]
            public void ReturnsFalse()
            {
                var guid = new Guid();
                var a = new Base64Guid(guid);
                var b = new Base64Guid(guid);

                var result = a != b;

                Assert.False(result);
            }

            [Fact]
            public void ReturnsTrue()
            {
                var a = new Base64Guid(Guid.Empty);
                var b = new Base64Guid(Guid.NewGuid());

                var result = a != b;

                Assert.True(result);
            }
        }

        public class EqualsMethod
        {
            [Fact]
            public void ReturnsTrue()
            {
                var a = Base64Guid.NewBase64Guid();
                object o = a;

                var result = a.Equals(o);

                Assert.True(result);
            }

            [Fact]
            public void ReturnsFalse()
            {
                var a = Base64Guid.NewBase64Guid();
                object o = a;
                var b = new Base64Guid();

                var result = b.Equals(o);

                Assert.False(result);
            }
        }
    }
}
