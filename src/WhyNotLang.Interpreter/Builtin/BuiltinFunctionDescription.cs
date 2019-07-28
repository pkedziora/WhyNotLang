using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.Builtin
{
    public class BuiltinFunctionDescription
    {
        public string FunctionName { get; }
        public Func<List<ExpressionValue>, Task<ExpressionValue>> Implementation { get; }

        public BuiltinFunctionDescription(string functionName, Func<List<ExpressionValue>, Task<ExpressionValue>> implementation)
        {
            FunctionName = functionName;
            Implementation = implementation;
        }
    }
}