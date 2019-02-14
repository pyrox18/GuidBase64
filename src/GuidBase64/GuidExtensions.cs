using System;

namespace GuidBase64
{
    public static class GuidExtensions
    {
        public static Base64Guid ToBase64Guid(this Guid guid)
        {
            return new Base64Guid(guid);
        }

        public static string ToBase64String(this Guid guid)
        {
            return new Base64Guid(guid).ToString();
        }
    }
}
