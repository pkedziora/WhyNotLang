using System.Collections.Generic;
using System.Linq;

namespace WhyNotLang.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private readonly ITokenReader _tokenReader;
        private readonly ITokenFactory _tokenFactory;
        public Tokenizer(ITokenReader tokenReader, ITokenFactory tokenFactory)
        {
            _tokenReader = tokenReader;
            _tokenFactory = tokenFactory;
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
                if (_tokenFactory.Map.ContainsKey(identifierOrKeyword.token.Value))
                {
                    // It's a keyword
                    var keyword = new Token(_tokenFactory.Map[identifierOrKeyword.token.Value], identifierOrKeyword.token.Value);
                    identifierOrKeyword = (keyword, identifierOrKeyword.endIndex);
                }

                return identifierOrKeyword;
            }

            var tokenStr = input[index].ToString();
            var matchingTokens = _tokenFactory.GetTokenTypesStartingWith(tokenStr);

            if (matchingTokens.Any())
            {
                while (matchingTokens.Any() && index + 1 < input.Length)
                {
                    var newTokenStr = tokenStr + input[++index];

                    matchingTokens = _tokenFactory.GetTokenTypesStartingWith(newTokenStr);
                    if (matchingTokens.Count > 0)
                    {
                        tokenStr = newTokenStr;
                        endIndex = index;
                    }
                }
            
                if (_tokenFactory.Map.ContainsKey(tokenStr))
                {
                    var token = _tokenFactory.Map[tokenStr];
                    return (new Token(token, tokenStr), endIndex);
                }
            }

            return (new Token(TokenType.Invalid, tokenStr), endIndex);
        }
    }
}