using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class BuiltinFunctionEvaluator : IBuiltinFunctionEvaluator
    {
        private readonly IProgramState _programState;

        public BuiltinFunctionEvaluator(IProgramState programState)
        {
            _programState = programState;
        }

        public ExpressionValue Eval(string functionName, List<ExpressionValue> argumentValues)
        {
            if (!_programState.BuiltinFunctionCollection.FunctionDescriptions.TryGetValue(functionName, out var functionDescription))
            {
                throw new ArgumentException($"Unexpected builtin function name {functionName}");
            }

            return functionDescription.Implementation(argumentValues);
        }
    }
}