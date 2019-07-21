using System.Collections.Generic;
using System.Linq;

namespace WhyNotLang.Tokenizer
{
    public class TokenMap : ITokenMap
    {
        public Dictionary<string, TokenType> Map { get; }
        public TokenMap()
        {
            Map = GetTokenMap();
        }
        
        public Dictionary<string, TokenType> GetTokensStartingWith(string prefix)
        {
            var result = new Dictionary<string, TokenType>();
            foreach (var keyVal in Map)
            {
                if (keyVal.Key.StartsWith(prefix))
                {
                    result.Add(keyVal.Key, keyVal.Value);
                }
            }

            return result;
        }

        public Token CreateToken(TokenType type, string val = null)
        {
            switch (type)
            {
                case TokenType.Identifier:
                    return new Token(TokenType.Identifier, val);
                case TokenType.Number:
                    return new Token(TokenType.Number, val);
                case TokenType.String:
                    return new Token(TokenType.String, val);
                default:
                    var key = Map.First(keyVal => keyVal.Value == type).Key;
                    return new Token(type, key);
            }
        }

        private Dictionary<string, TokenType> GetTokenMap()
        {
            return new Dictionary<string, TokenType>
            {
                {"var", TokenType.Var},
                {"begin", TokenType.Begin},
                {"end", TokenType.End},
                {"if", TokenType.If},
                {"else", TokenType.Else},
                {"while", TokenType.While},
                {"function", TokenType.Function},
                {"return", TokenType.Return},
                {",", TokenType.Comma},
                {"(", TokenType.LeftParen},
                {")", TokenType.RightParen},
                {"+", TokenType.Plus},
                {"-", TokenType.Minus},
                {"*", TokenType.Multiply},
                {"/", TokenType.Divide},
                {":=", TokenType.Assign},
                {"!", TokenType.Not},
                {"and", TokenType.And},
                {"or", TokenType.Or},
                {"<", TokenType.LessThan},
                {">", TokenType.GreaterThan},
                {"<=", TokenType.LessThanOrEqual},
                {">=", TokenType.GreaterThanOrEqual},
                {"==", TokenType.Equal},
                {"!=", TokenType.NotEqual}
            };
        }
    }
}