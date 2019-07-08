using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public interface ITokenizer
    {
        IList<Token> GetTokens(string input);
    }
}