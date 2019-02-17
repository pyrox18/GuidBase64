using GuidBase64.Configuration;
using GuidBase64.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GuidBase64
{
    [TypeConverter(typeof(Base64GuidTypeConverter))]
    public struct Base64Guid : IEquatable<Base64Guid>
    {
        private readonly Base64GuidOptions _options;

        /// <summary>
        /// Gets the Guid object.
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class with an empty GUID
        /// and a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        public Base64Guid(Action<Base64GuidOptionsBuilder> optionsAction)
            : this(new Guid(), optionsAction) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class by using the
        /// specified array of bytes.
        /// </summary>
        /// <param name="buffer">A 16-element byte array containing values with which to initialize the GUID.</param>
        public Base64Guid(byte[] buffer)
            : this(new Guid(buffer), options => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class by using the
        /// specified array of bytes and a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="buffer">A 16-element byte array containing values with which to initialize the GUID.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        public Base64Guid(byte[] buffer, Action<Base64GuidOptionsBuilder> optionsAction)
            : this(new Guid(buffer), optionsAction) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class with an existing
        /// <see cref="System.Guid"/> instance.
        /// </summary>
        /// <param name="guid">The <see cref="System.Guid"/> instance to initialise the <see cref="Base64Guid"/> instance with.</param>
        public Base64Guid(Guid guid)
            : this(guid, options => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class with an existing
        /// <see cref="System.Guid"/> instance and a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="guid">The <see cref="System.Guid"/> instance to initialise the <see cref="Base64Guid"/> instance with.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        public Base64Guid(Guid guid, Action<Base64GuidOptionsBuilder> optionsAction)
        {
            _options = BuildOptions(optionsAction);
            Guid = guid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class by using the value
        /// represented by the specified string.
        /// </summary>
        /// <param name="encoded">
        /// A string that contains a Guid which satisfies the regular expression "^[a-zA-Z0-9-_]{22}$".
        /// </param>
        public Base64Guid(string encoded)
            : this(encoded, options => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class by using the value
        /// represented by the specified string with a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="encoded">
        /// <para>A string that contains a Guid which satisfies one of the following regular expressions:</para>
        /// <para>- "^[a-zA-Z0-9-_]{22}$" if <paramref name="optionsAction"/> is not configured.</para>
        /// <para>- "^[a-zA-Z0-9\+/]{22}$" if <paramref name="optionsAction"/> is configured with UseStandardBase64Encoding().</para>
        /// <para>- "^[a-zA-Z0-9-_]{22}==$" if <paramref name="optionsAction"/> is configured with UsePadding().</para>
        /// <para>- "^[a-zA-Z0-9\+/]{22}==$" if <paramref name="optionsAction"/> is configured with 
        /// UseStandardBase64Encoding() and UsePadding().</para>
        /// </param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        public Base64Guid(string encoded, Action<Base64GuidOptionsBuilder> optionsAction)
        {
            _options = BuildOptions(optionsAction);
            Guid = new Guid(ParseToByteArray(encoded, _options));
        }

        private static Base64GuidOptions BuildOptions(Action<Base64GuidOptionsBuilder> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = new Base64GuidOptionsBuilder();
            options(builder);
            return builder.Build();
        }

        /// <summary>
        /// Returns a 16-element byte array that contains the value of this instance.
        /// </summary>
        /// <returns>A 16-element byte array containing the value of this instance.</returns>
        public byte[] ToByteArray()
        {
            return Guid.ToByteArray();
        }

        /// <summary>
        /// Returns a string representation of the value of this instance in base 64 format.
        /// </summary>
        /// <returns>
        /// The value of this instance, formatted with the RFC 4648 Section 5 base 64 character set
        /// and with padding stripped. If the instance has been configured to use the standard
        /// base 64 encoding, the instance value will be formatted with the RFC 4648 Section 4 base
        /// 64 character set instead. If the instance has been configured to use padding, the instance value
        /// will not have its padding stripped.
        /// </returns>
        public override string ToString()
        {
            string enc = Convert.ToBase64String(Guid.ToByteArray());

            if (!_options.StandardBase64Encoding)
            {
                enc = enc.Replace("/", "_");
                enc = enc.Replace("+", "-");
            }

            if (!_options.Padding)
            {
                return enc.Substring(0, 22);
            }

            return enc;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Base64Guid"/>
        /// represent the same value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>
        /// true if <paramref name="obj"/> is a <see cref="Base64Guid"/> that has the same value
        /// as this instance; otherwise false.
        /// </returns>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class.
        /// </summary>
        /// <returns>A new <see cref="Base64Guid"/> object.</returns>
        public static Base64Guid NewBase64Guid() => new Base64Guid(Guid.NewGuid());

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64Guid"/> class with
        /// a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        /// <returns>A new <see cref="Base64Guid"/> object.</returns>
        public static Base64Guid NewBase64Guid(Action<Base64GuidOptionsBuilder> optionsAction) =>
            new Base64Guid(Guid.NewGuid(), optionsAction);

        /// <summary>
        /// Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid"/> object.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <returns>A <see cref="Base64Guid"/> instance that contains the parsed value.</returns>
        public static Base64Guid Parse(string encoded) => new Base64Guid(encoded);

        /// <summary>
        /// Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid"/> object
        /// with a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        /// <returns>A <see cref="Base64Guid"/> instance that contains the parsed value.</returns>
        public static Base64Guid Parse(string encoded, Action<Base64GuidOptionsBuilder> options) =>
            new Base64Guid(encoded, options);

        /// <summary>
        /// Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid"/> object.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="result">
        /// The object that will contain the parsed value. If the method returns true,
        /// result contains a valid <see cref="Base64Guid"/>. If the method returns false,
        /// result is null.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise false.</returns>
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

        /// <summary>
        /// Converts the base 64 string representation of a Guid to the equivalent <see cref="Base64Guid"/> object
        /// with a configurable <see cref="Base64GuidOptionsBuilder"/>.
        /// </summary>
        /// <param name="encoded">The string to convert.</param>
        /// <param name="optionsAction">The action to configure the <see cref="Base64GuidOptions"/> for the instance.</param>
        /// <param name="result">
        /// The object that will contain the parsed value. If the method returns true,
        /// result contains a valid <see cref="Base64Guid"/>. If the method returns false,
        /// result is null.
        /// </param>
        /// <returns>true if the parse operation was successful; otherwise false.</returns>
        public static bool TryParse(string encoded, Action<Base64GuidOptionsBuilder> optionsAction, out Base64Guid result)
        {
            try
            {
                result = new Base64Guid(encoded, optionsAction);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the values of two specified <see cref="Base64Guid"/> objects are equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are equal; otherwise false.</returns>
        public static bool operator ==(Base64Guid a, Base64Guid b)
        {
            return a.Guid == b.Guid;
        }

        /// <summary>
        /// Indicates whether the values of two specified <see cref="Base64Guid"/> objects are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns>true if a and b are not equal; otherwise false.</returns>
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

            if (!options.Padding && (encoded.Length < 22 || encoded.Length > 22))
            {
                throw new FormatException($"{nameof(encoded)} is not 22 characters long");
            }
            else if (options.Padding && (encoded.Length < 24 || encoded.Length > 24))
            {
                throw new FormatException($"{nameof(encoded)} is not 24 characters long");
            }

            Regex regex;
            if (!options.StandardBase64Encoding && !options.Padding)
            {
                regex = new Regex(@"^[a-zA-Z0-9-_]*$");
            }
            else if (options.StandardBase64Encoding && !options.Padding)
            {
                regex = new Regex(@"^[a-zA-Z0-9\+/]*$");
            }
            else if (!options.StandardBase64Encoding && options.Padding)
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

            if (!options.StandardBase64Encoding)
            {
                encoded = encoded.Replace("_", "/");
                encoded = encoded.Replace("-", "+");
            }

            if (options.Padding)
            {
                return Convert.FromBase64String(encoded);
            }

            return Convert.FromBase64String(encoded + "==");
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Base64Guid"/>
        /// represent the same value.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns>
        /// true if <paramref name="other"/> is a <see cref="Base64Guid"/> that has the same value
        /// as this instance; otherwise false.
        /// </returns>
        public bool Equals(Base64Guid other)
        {
            return other != null &&
                   Guid.Equals(other.Guid);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return -737073652 + EqualityComparer<Guid>.Default.GetHashCode(Guid);
        }
    }
}
