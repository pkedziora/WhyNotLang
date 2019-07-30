namespace WhyNotLang.Tokenizer
{
    public static class TokenExtensions
    {
        public static Token AddLineNumber(this Token token, int lineNumber)
        {
            token.LineNumber = lineNumber;

            return token;
        }
    }
}