using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public interface ITokenIterator
    {
        Token CurrentToken { get; }
        void InitTokens(string str);
        Token GetNextToken();
        Token PeekToken(int offset);
    }
}