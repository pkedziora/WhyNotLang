using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public interface ITokenMap
    {
        Dictionary<string, TokenType> Map { get; }
        Dictionary<string, TokenType> GetTokensStartingWith(string prefix);
    }
}