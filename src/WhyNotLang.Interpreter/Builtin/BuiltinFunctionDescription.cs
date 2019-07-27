using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.Builtin
{
    public class BuiltinFunctionDescription
    {
        public string FunctionName { get; }
        public List<ExpressionValueTypes> Parameters { get; }
        public Func<List<ExpressionValue>, ExpressionValue> Implementation { get; }

        public BuiltinFunctionDescription(string functionName, List<ExpressionValueTypes> parameters,
            Func<List<ExpressionValue>, ExpressionValue> implementation)
        {
            FunctionName = functionName;
            Parameters = parameters;
            Implementation = implementation;
        }
    }
}