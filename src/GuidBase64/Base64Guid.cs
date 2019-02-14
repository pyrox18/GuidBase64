using System;

namespace GuidBase64
{
    public class Base64Guid
    {
        public Guid Guid { get; }

        public Base64Guid() => Guid = new Guid();

        public Base64Guid(Guid guid) => Guid = guid;

        public Base64Guid(byte[] buffer) => Guid = new Guid(buffer);

        public Base64Guid(string encoded) => Guid = new Guid(ParseToByteArray(encoded));

        public byte[] ToByteArray()
        {
            return Guid.ToByteArray();
        }

        public override string ToString()
        {
            string enc = Convert.ToBase64String(Guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
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

        public static Base64Guid Parse(string encoded) => new Base64Guid(encoded);

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

        public static bool operator ==(Base64Guid a, Base64Guid b)
        {
            return a.Guid == b.Guid;
        }

        public static bool operator !=(Base64Guid a, Base64Guid b)
        {
            return a.Guid != b.Guid;
        }

        private static byte[] ParseToByteArray(string encoded)
        {
            if (encoded is null)
            {
                throw new ArgumentNullException(nameof(encoded));
            }

            if (encoded.Length < 22 || encoded.Length > 22)
            {
                throw new FormatException($"{nameof(encoded)} is not 22 characters long");
            }

            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");

            return Convert.FromBase64String(encoded + "==");
        }
    }
}
