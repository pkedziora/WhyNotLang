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
                {"begin", TokenType.Begin},
                {"end", TokenType.End},
                {"if", TokenType.If},
                {"else", TokenType.Else},
                {"while", TokenType.While},
                {"function", TokenType.Function},
                {"return", TokenType.Return},
                {"=", TokenType.Assign},
                {"(", TokenType.LeftParen},
                {")", TokenType.RightParen},
                {"+", TokenType.Plus},
                {"-", TokenType.Minus},
                {"*", TokenType.Multiply},
                {"/", TokenType.Divide},
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