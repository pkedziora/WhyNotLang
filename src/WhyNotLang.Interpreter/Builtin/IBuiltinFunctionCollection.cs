using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;

namespace WhyNotLang.Interpreter.Builtin
{
    public interface IBuiltinFunctionCollection
    {
        Dictionary<string, BuiltinFunctionDescription> FunctionDescriptions { get; }
        void DeclareBuiltinFunctions(IProgramState programState);

        void Add(string functionName, Func<List<ExpressionValue>, ExpressionValue> implementation);
    }
}