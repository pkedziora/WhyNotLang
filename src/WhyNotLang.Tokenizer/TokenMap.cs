using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public class TokenMap
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
                {"(", TokenType.LeftParen},
                {")", TokenType.RightParen},
                {"+", TokenType.Plus},
                {"-", TokenType.Minus},
                {"*", TokenType.Multiply},
                {"/", TokenType.Divide},
                {"=", TokenType.Assign},
                {"!", TokenType.Not},
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