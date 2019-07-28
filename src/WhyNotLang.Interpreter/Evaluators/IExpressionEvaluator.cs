using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        Task<ExpressionValue> Eval(IExpression expression);
    }
}