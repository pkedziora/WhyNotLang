using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public interface ITokenMap
    {
        Dictionary<string, TokenType> Map { get; }
        Dictionary<string, TokenType> GetTokensStartingWith(string prefix);
        Token CreateToken(TokenType type, string val = null);
    }
}