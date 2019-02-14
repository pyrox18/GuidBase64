using System;

namespace GuidBase64
{
    public class Base64Guid
    {
        private Guid _guid;

        public Base64Guid() => _guid = new Guid();

        public Base64Guid(Guid guid) => _guid = guid;

        public Base64Guid(byte[] buffer) => _guid = new Guid(buffer);

        public override string ToString()
        {
            string enc = Convert.ToBase64String(_guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");

            return enc.Substring(0, 22);
        }

        public static Base64Guid NewBase64Guid() => new Base64Guid(Guid.NewGuid());

        public static Base64Guid FromBase64String(string encoded)
        {
            if (encoded.Length < 22 || encoded.Length > 22)
            {
                throw new ArgumentException("String length does not meet encoded GUID requirement (22 characters)");
            }

            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");

            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Base64Guid(buffer);
        }
    }
}
