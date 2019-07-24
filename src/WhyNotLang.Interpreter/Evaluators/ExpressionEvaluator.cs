using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IProgramState _programState;
        private readonly IBuiltinFunctionEvaluator _builtinEvaluator;

        public ExpressionEvaluator(IProgramState programState, IBuiltinFunctionEvaluator builtinEvaluator)
        {
            _programState = programState;
            _builtinEvaluator = builtinEvaluator;
        }
        
        public ExpressionValue Eval(IExpression expression)
        {
            var evaluator = GetExpressionEvaluator(expression);
            return evaluator.Eval(expression);
        }
        
        public IExpressionEvaluator GetExpressionEvaluator(IExpression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Value:
                    return new ValueExpressionEvaluator(_programState);
                case ExpressionType.Unary:
                    return new UnaryExpressionEvaluator(this);
                case ExpressionType.Binary:
                    return new BinaryExpressionEvaluator(this);
                case ExpressionType.Function:
                    return new FunctionExpressionEvaluator(this, _builtinEvaluator, _programState);
            }
            
            throw new ArgumentException($"Parser not found for expression {expression.Type}");
        }
    }
}