using WhyNotLang.Interpreter.ExpressionValues;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        ExpressionValue Eval(IExpression expression);
    }
}