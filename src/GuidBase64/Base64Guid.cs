using GuidBase64.Configuration;
using GuidBase64.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GuidBase64
{
    [TypeConverter(typeof(Base64GuidTypeConverter))]
    public class Base64Guid : IEquatable<Base64Guid>
    {
        private readonly Base64GuidOptions _options;

        public Guid Guid { get; }

        public Base64Guid()
            : this(new Guid(), options => { }) { }

        public Base64Guid(Action<Base64GuidOptionsBuilder> options)
            : this(new Guid(), options) { }

        public Base64Guid(byte[] buffer)
            : this(new Guid(buffer), options => { }) { }

        public Base64Guid(byte[] buffer, Action<Base64GuidOptionsBuilder> options)
            : this(new Guid(buffer), options) { }

        public Base64Guid(Guid guid)
            : this(guid, options => { }) { }

        public Base64Guid(Guid guid, Action<Base64GuidOptionsBuilder> options)
        {
            _options = BuildOptions(options);
            Guid = guid;
        }

        public Base64Guid(string encoded)
            : this(encoded, options => { }) { }

        public Base64Guid(string encoded, Action<Base64GuidOptionsBuilder> options)
        {
            _options = BuildOptions(options);
            Guid = new Guid(ParseToByteArray(encoded, _options));
        }

        private Base64GuidOptions BuildOptions(Action<Base64GuidOptionsBuilder> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = new Base64GuidOptionsBuilder();
            options(builder);
            return builder.Build();
        }

        public byte[] ToByteArray()
        {
            return Guid.ToByteArray();
        }

        public override string ToString()
        {
            string enc = Convert.ToBase64String(Guid.ToByteArray());

            if (_options.UrlSafe)
            {
                enc = enc.Replace("/", "_");
                enc = enc.Replace("+", "-");
            }

            if (_options.StripPadding)
            {
                return enc.Substring(0, 22);
            }

            return enc;
        }

        public override bool Equals(object obj)
        {
            Base64Guid b;
            if (obj is null || !(obj is Base64Guid))
            {
                return false;
            }
            else
            {
                b = (Base64Guid)obj;
            }

            return Guid == b.Guid;
        }

        public static Base64Guid NewBase64Guid() => new Base64Guid(Guid.NewGuid());

        public static Base64Guid NewBase64Guid(Action<Base64GuidOptionsBuilder> options) =>
            new Base64Guid(Guid.NewGuid(), options);

        public static Base64Guid Parse(string encoded) => new Base64Guid(encoded);

        public static Base64Guid Parse(string encoded, Action<Base64GuidOptionsBuilder> options) =>
            new Base64Guid(encoded, options);

        public static bool TryParse(string encoded, out Base64Guid result)
        {
            try
            {
                result = new Base64Guid(encoded);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static bool TryParse(string encoded, Action<Base64GuidOptionsBuilder> options, out Base64Guid result)
        {
            try
            {
                result = new Base64Guid(encoded, options);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static bool operator ==(Base64Guid a, Base64Guid b)
        {
            return a.Guid == b.Guid;
        }

        public static bool operator !=(Base64Guid a, Base64Guid b)
        {
            return a.Guid != b.Guid;
        }

        private static byte[] ParseToByteArray(string encoded, Base64GuidOptions options)
        {
            if (encoded is null)
            {
                throw new ArgumentNullException(nameof(encoded));
            }

            if (options.StripPadding && (encoded.Length < 22 || encoded.Length > 22))
            {
                throw new FormatException($"{nameof(encoded)} is not 22 characters long");
            }
            else if (!options.StripPadding && (encoded.Length < 24 || encoded.Length > 24))
            {
                throw new FormatException($"{nameof(encoded)} is not 24 characters long");
            }

            Regex regex;
            if (options.UrlSafe && options.StripPadding)
            {
                regex = new Regex(@"^[a-zA-Z0-9-_]*$");
            }
            else if (!options.UrlSafe && options.StripPadding)
            {
                regex = new Regex(@"^[a-zA-Z0-9\+/]*$");
            }
            else if (options.UrlSafe && !options.StripPadding)
            {
                regex = new Regex(@"^[a-zA-Z0-9-_]*={0,2}$");
            }
            else
            {
                regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,2}$");
            }

            if (!regex.IsMatch(encoded))
            {
                throw new FormatException($"{nameof(encoded)} is not encoded correctly");
            }

            if (options.UrlSafe)
            {
                encoded = encoded.Replace("_", "/");
                encoded = encoded.Replace("-", "+");
            }

            if (!options.StripPadding)
            {
                return Convert.FromBase64String(encoded);
            }

            return Convert.FromBase64String(encoded + "==");
        }

        public bool Equals(Base64Guid other)
        {
            return other != null &&
                   Guid.Equals(other.Guid);
        }

        public override int GetHashCode()
        {
            return -737073652 + EqualityComparer<Guid>.Default.GetHashCode(Guid);
        }
    }
}
