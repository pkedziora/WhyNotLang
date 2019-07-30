using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public interface ITokenFactory
    {
        Dictionary<string, TokenType> Map { get; }
        Dictionary<string, TokenType> GetTokenTypesStartingWith(string prefix);
        Token CreateToken(TokenType type, string val = null, int lineNumber = 0);
    }
}