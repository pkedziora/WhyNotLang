using System;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Extensions
{
    public static class TokenExtensions
    {
        public static Precedence GetPrecedence(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Or:
                    return Precedence.Or;
                
                case TokenType.And:
                    return Precedence.And;
                
                case TokenType.LessThan:
                case TokenType.GreaterThan:
                case TokenType.LessThanOrEqual:
                case TokenType.GreaterThanOrEqual:
                case TokenType.Equal:
                case TokenType.NotEqual:
                    return Precedence.Comparison;
                
                case TokenType.Plus:
                case TokenType.Minus:
                    return Precedence.AddSub;
                
                case TokenType.Multiply:
                case TokenType.Divide:
                    return Precedence.MulDiv;
            }

            return Precedence.None;
        }
    }
}