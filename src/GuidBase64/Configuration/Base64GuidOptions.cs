namespace GuidBase64.Configuration
{
    internal struct Base64GuidOptions
    {
        internal bool _standardBase64Encoding;
        internal bool _padding;

        internal bool StandardBase64Encoding
        {
            get { return _standardBase64Encoding;  }
            set { _standardBase64Encoding = value; }
        }

        internal bool Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        internal Base64GuidOptions(bool standardBase64Encoding, bool padding)
        {
            _standardBase64Encoding = standardBase64Encoding;
            _padding = padding;
        }
    }
}
