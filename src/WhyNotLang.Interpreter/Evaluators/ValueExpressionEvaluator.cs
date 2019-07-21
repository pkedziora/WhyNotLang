using System;
using WhyNotLang.Interpreter.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ValueExpressionEvaluator : IExpressionEvaluator
    {
        public ExpressionValue Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Value)
            {
                throw new ArgumentException("ValueExpression expected");
            }
            
            var valueExpression = expression as ValueExpression;
            var token = valueExpression.Token;
            switch (valueExpression.Token.Type)
            {
                case TokenType.Number:
                    return new ExpressionValue(int.Parse(token.Value), ExpressionValueTypes.Number);
                case TokenType.String:
                    return new ExpressionValue(token.Value, ExpressionValueTypes.String);
            }
            
            throw new ArgumentException("Unsupported token type");
        }
    }
}