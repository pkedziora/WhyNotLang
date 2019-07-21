using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        ExpressionValue Eval(IExpression expression);
    }
}