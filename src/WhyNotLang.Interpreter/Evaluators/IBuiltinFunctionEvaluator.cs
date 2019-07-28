using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IBuiltinFunctionEvaluator
    {
        Task<ExpressionValue> Eval(string functionName, List<ExpressionValue> argumentValues);
    }
}