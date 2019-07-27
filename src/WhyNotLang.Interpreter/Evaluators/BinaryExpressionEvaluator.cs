using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionEvaluator _mainEvaluator;

        public BinaryExpressionEvaluator(IExpressionEvaluator mainEvaluator)
        {
            _mainEvaluator = mainEvaluator;
        }
        
        public ExpressionValue Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Binary)
            {
                throw new ArgumentException("BinaryExpression expected");
            }
            
            var binaryExpression = expression as BinaryExpression;
            var leftExpressionValue = _mainEvaluator.Eval(binaryExpression.Left);
            var rightExpressionValue = _mainEvaluator.Eval(binaryExpression.Right);
            var result =
                CalculateValue(leftExpressionValue, binaryExpression.Operator, rightExpressionValue);

            return result;
        }

        private ExpressionValue CalculateValue(ExpressionValue left, Token op, ExpressionValue right)
        {
            if (left.Type != right.Type)
            {
                throw new ArgumentException("Left and right expressions need to be of same type");
            }

            switch (left.Type)
            {
                case ExpressionValueTypes.Number:
                    return new ExpressionValue(CalculateNumberOperation((int) left.Value, op, (int) right.Value), ExpressionValueTypes.Number);
                case ExpressionValueTypes.String:
                    if (op.Value == "==")
                    {
                        return new ExpressionValue(CastBoolToInt(left.Equals(right)), ExpressionValueTypes.Number);
                    }
                    
                    return new ExpressionValue(CalculateStringOperation((string)left.Value, op, (string)right.Value), ExpressionValueTypes.String);
            }
            
            throw new ArgumentException("Unsupported token type");
        }

        private string CalculateStringOperation(string left, Token op, string right)
        {
            if (op.Type != TokenType.Plus)
            {
                throw new ArgumentException("Only + and == operation supported for strings");
            }

            return left + right;
        }
        
        private int CalculateNumberOperation(int left, Token op, int right)
        {
            switch (op.Type)
            {
                case TokenType.Plus:
                    return left + right;
                case TokenType.Minus:
                    return left - right;
                case TokenType.Multiply:
                    return left * right;
                case TokenType.Divide:
                    return left / right;
                case TokenType.Equal:
                    return CastBoolToInt(left == right);
                case TokenType.NotEqual:
                    return CastBoolToInt(left != right);
                case TokenType.LessThan:
                    return CastBoolToInt(left < right);
                case TokenType.LessThanOrEqual:
                    return CastBoolToInt(left <= right);
                case TokenType.GreaterThan:
                    return CastBoolToInt(left > right);
                case TokenType.GreaterThanOrEqual:
                    return CastBoolToInt(left >= right);
                case TokenType.Or:
                    return CastBoolToInt(CastIntToBool(left) || CastIntToBool(right));
                case TokenType.And:
                    return CastBoolToInt(CastIntToBool(left) && CastIntToBool(right));
            }
            
            throw new ArgumentException("Unsupported token type");
        }

        private int CastBoolToInt(bool val)
        {
            return val ? 1 : 0;
        }
        
        private bool CastIntToBool(int val)
        {
            return val != 0;
        }
    }
}