using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IBuiltinFunctionEvaluator
    {
        ExpressionValue Eval(string functionName, List<ExpressionValue> parameterValues);
    }
}