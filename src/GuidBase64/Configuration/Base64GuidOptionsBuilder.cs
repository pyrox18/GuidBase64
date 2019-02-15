namespace GuidBase64.Configuration
{
    public class Base64GuidOptionsBuilder
    {
        private Base64GuidOptions _options;

        public Base64GuidOptionsBuilder() => _options = new Base64GuidOptions();

        public Base64GuidOptionsBuilder UseStandardBase64Encoding()
        {
            _options.UrlSafe = false;
            return this;
        }

        public Base64GuidOptionsBuilder UsePadding()
        {
            _options.StripPadding = false;
            return this;
        }

        internal Base64GuidOptions Build() => _options;
    }
}
