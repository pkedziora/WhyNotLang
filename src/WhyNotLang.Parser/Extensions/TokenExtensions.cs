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