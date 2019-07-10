using System.Collections.Generic;
using System.Linq;

namespace WhyNotLang.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private readonly ITokenReader _tokenReader;
        private readonly ITokenMap _tokenMap;
        public Tokenizer(ITokenReader tokenReader, ITokenMap tokenMap)
        {
            _tokenReader = tokenReader;
            _tokenMap = tokenMap;
        }
        
        public IList<Token> GetTokens(string input)
        {
            input = input.Trim();
            var tokens = new List<Token>();
            var index = 0;
            while (index < input.Length)
            {
                var result = GetNextToken(input, index);
                index = result.endIndex + 1;
                tokens.Add(result.token);
            }

            return tokens;
        }

        private (Token token, int endIndex) GetNextToken(string input, int startIndex)
        {
            var index = _tokenReader.SkipWhitespace(input, startIndex);
            var endIndex = index;
            
            if (_tokenReader.CanReadNumber(input, index))
            {
                return _tokenReader.ReadNumber(input, index);
            }
            
            if (_tokenReader.CanReadString(input, index))
            {
                return _tokenReader.ReadString(input, index);
            }

            if (_tokenReader.CanReadIdentifier(input, index))
            {
                var identifierOrKeyword = _tokenReader.ReadIdentifier(input, index);
                if (_tokenMap.Map.ContainsKey(identifierOrKeyword.token.Value))
                {
                    // It's a keyword
                    var keyword = new Token(_tokenMap.Map[identifierOrKeyword.token.Value], identifierOrKeyword.token.Value);
                    identifierOrKeyword = (keyword, identifierOrKeyword.endIndex);
                }

                return identifierOrKeyword;
            }

            var tokenStr = input[index].ToString();
            var matchingTokens = _tokenMap.GetTokensStartingWith(tokenStr);

            if (matchingTokens.Any())
            {
                while (matchingTokens.Any() && index + 1 < input.Length)
                {
                    var newTokenStr = tokenStr + input[++index];

                    matchingTokens = _tokenMap.GetTokensStartingWith(newTokenStr);
                    if (matchingTokens.Count > 0)
                    {
                        tokenStr = newTokenStr;
                        endIndex = index;
                    }
                }
            
                if (_tokenMap.Map.ContainsKey(tokenStr))
                {
                    var token = _tokenMap.Map[tokenStr];
                    return (new Token(token, tokenStr), endIndex);
                }
            }

            return (new Token(TokenType.Invalid, tokenStr), endIndex);
        }
    }
}