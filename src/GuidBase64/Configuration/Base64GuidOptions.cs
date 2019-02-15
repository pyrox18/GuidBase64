namespace GuidBase64.Configuration
{
    internal class Base64GuidOptions
    {
        internal bool UrlSafe { get; set; }
        internal bool StripPadding { get; set; }

        internal Base64GuidOptions(bool urlSafe = true, bool stripPadding = true)
        {
            UrlSafe = urlSafe;
            StripPadding = stripPadding;
        }
    }
}
