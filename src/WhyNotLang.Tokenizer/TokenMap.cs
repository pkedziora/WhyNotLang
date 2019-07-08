using System.Collections.Generic;

namespace WhyNotLang.Tokenizer
{
    public class TokenMap
    {
        public Dictionary<string, TokenType> GetTokenMap()
        {
            return new Dictionary<string, TokenType>
            {
                {"", TokenType.Identifier},
                {"", TokenType.String},
                {"", TokenType.Number},
                {"var", TokenType.Var},
                {"=", TokenType.Assign},
                {"begin", TokenType.Begin},
                {"end", TokenType.End},
                {"if", TokenType.If},
                {"else", TokenType.Else},
                {"while", TokenType.While},
                {"+", TokenType.Plus},
                {"-", TokenType.Minus},
                {"*", TokenType.Multiply},
                {"/", TokenType.Divide},
                {"<", TokenType.LessThan},
                {"<=", TokenType.LessThanOrEqual},
                {">", TokenType.GreaterThan},
                {">=", TokenType.GreaterThanOrEqual},
                {"=", TokenType.Equal},
                {"!=", TokenType.NotEqual},
                {"(", TokenType.LeftParen},
                {")", TokenType.RightParen},
                {"function", TokenType.Function},
                {"return", TokenType.Return}
            };
        }
    }
}