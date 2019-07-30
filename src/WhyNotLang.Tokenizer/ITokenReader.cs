namespace WhyNotLang.Tokenizer
{
    public interface ITokenReader
    {
        (int newIndex, int newLineCount) SkipWhitespace(string input, int index);
        bool CanReadIdentifier(string input, int index);
        (Token token, int endIndex) ReadIdentifier(string input, int index);
        bool CanReadNumber(string input, int index);
        (Token token, int endIndex) ReadNumber(string input, int index);
        bool CanReadString(string input, int index);
        (Token token, int endIndex) ReadString(string input, int index);
    }
}