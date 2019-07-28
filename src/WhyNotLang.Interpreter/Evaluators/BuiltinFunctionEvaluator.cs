using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class BuiltinFunctionEvaluator : IBuiltinFunctionEvaluator
    {
        private readonly IProgramState _programState;

        public BuiltinFunctionEvaluator(IProgramState programState)
        {
            _programState = programState;
        }

        public async Task<ExpressionValue> Eval(string functionName, List<ExpressionValue> argumentValues)
        {
            if (!_programState.BuiltinFunctionCollection.FunctionDescriptions.TryGetValue(functionName, out var functionDescription))
            {
                throw new ArgumentException($"Unexpected builtin function name {functionName}");
            }

            return await functionDescription.Implementation(argumentValues);
        }
    }
}