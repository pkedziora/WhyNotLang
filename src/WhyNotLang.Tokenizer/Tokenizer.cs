using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhyNotLang.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private TokenMap _map;
        public Tokenizer()
        {
            _map = new TokenMap();
        }
        
        public IList<Token> GetTokens(string input)
        {
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
            var index = startIndex;
            var endIndex = index;
            
            if (char.IsDigit(input[index]))
            {
                return ReadNumber(input, index);
            }

            if (IsIdentifierChar(input[index]))
            {
                var identifierOrKeyword = ReadIdentifier(input, index);
                if (_map.Map.ContainsKey(identifierOrKeyword.token.Value))
                {
                    // It's a keyword
                    identifierOrKeyword.token.Type = _map.Map[identifierOrKeyword.token.Value];
                }

                return identifierOrKeyword;
            }

            if (input[index] == '"')
            {
                return ReadString(input, index);
            }
            
            
            var str = input[index].ToString();
            var tokens = _map.GetTokensStartingWith(str);

            if (tokens.Any())
            {
                while (tokens.Any() && index + 1 < input.Length)
                {
                    var newStr = str + input[++index];

                    var currentTokens = _map.GetTokensStartingWith(newStr);
                    if (currentTokens.Count > 0)
                    {
                        tokens = currentTokens;
                        str = newStr;
                        endIndex = index;
                    }
                }
            
                if (_map.Map.ContainsKey(str))
                {
                    var token = _map.Map[str];
                    return (new Token(token, str), endIndex);
                }
            }

            return (new Token(TokenType.Invalid, str), endIndex);
        }

        private bool IsIdentifierChar(char ch)
        {
            return char.IsDigit(ch) || char.IsLetter(ch) || ch == '_';
        }

        private (Token token, int endIndex) ReadIdentifier(string input, int startIndex)
        {
            if (!char.IsLetter(input[startIndex]) && input[startIndex] != '_')
            {
                return (new Token(TokenType.Invalid, input[startIndex].ToString()), startIndex);
            }
            
            var index = startIndex;
            var sb = new StringBuilder();
            while (index < input.Length && IsIdentifierChar(input[index]))
            {
                sb.Append(input[index]);
                index++;
            }

            index--;

            var tokenStr = sb.ToString();

            return (new Token(TokenType.Identifier, tokenStr), index);
        }
        
        private (Token token, int endIndex) ReadNumber(string input, int startIndex)
        {
            var index = startIndex;
            var sb = new StringBuilder();
            while (index < input.Length && char.IsDigit(input[index]))
            {
                sb.Append(input[index]);
                index++;
            }

            index--;

            var tokenStr = sb.ToString();

            return (new Token(TokenType.Number, tokenStr), index);
        }
        
        private (Token token, int endIndex) ReadString(string input, int startIndex)
        {
            var index = startIndex;
            if (input[index++] != '"')
            {
                throw new Exception("Invalid string");
            }
            var sb = new StringBuilder();
            while (index < input.Length && input[index] != '"')
            {
                sb.Append(input[index]);
                index++;
            }
            
            if (input[index] != '"')
            {
                throw new Exception("Invalid string");
            }

            var tokenStr = sb.ToString();
            
            return (new Token(TokenType.String, tokenStr), index);
        }
    }
}