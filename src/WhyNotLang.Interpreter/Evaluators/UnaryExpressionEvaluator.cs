using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class UnaryExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionEvaluator _mainEvaluator;

        public UnaryExpressionEvaluator(IExpressionEvaluator mainEvaluator)
        {
            _mainEvaluator = mainEvaluator;
        }
        
        public ExpressionValue Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Unary)
            {
                throw new ArgumentException("UnaryExpression expected");
            }
            
            var unaryExpression = expression as UnaryExpression;
            var innerExpressionValue = _mainEvaluator.Eval(unaryExpression.Inner);
            var result =
                CalculateValue(unaryExpression.Operator, innerExpressionValue);

            return result;
        }

        private ExpressionValue CalculateValue(Token op, ExpressionValue inner)
        {
            if (inner.Type != ExpressionValueTypes.Number)
            {
                throw new ArgumentException("Number expression expected");
            }
            
            return new ExpressionValue(CalculateNumberOperation(op, (int)inner.Value), ExpressionValueTypes.Number);
        }
        
        private int CalculateNumberOperation(Token op, int value)
        {
            switch (op.Type)
            {
                case TokenType.Plus:
                    return value;
                case TokenType.Minus:
                    return -value;
                case TokenType.Not:
                    return value != 0 ? 0 : 1;
            }
            
            throw new ArgumentException("Unsupported token type");
        }
    }
}