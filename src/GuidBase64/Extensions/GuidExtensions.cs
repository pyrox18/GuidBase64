using GuidBase64.Configuration;
using System;

namespace GuidBase64.Extensions
{
    public static class GuidExtensions
    {
        public static Base64Guid ToBase64Guid(this Guid guid)
        {
            return new Base64Guid(guid);
        }

        public static Base64Guid ToBase64Guid(this Guid guid, Action<Base64GuidOptionsBuilder> options)
        {
            return new Base64Guid(guid, options);
        }

        public static string ToBase64String(this Guid guid)
        {
            return new Base64Guid(guid).ToString();
        }

        public static string ToBase64String(this Guid guid, Action<Base64GuidOptionsBuilder> options)
        {
            return new Base64Guid(guid, options).ToString();
        }
    }
}
