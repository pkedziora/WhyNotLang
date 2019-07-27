using System.Collections.Generic;
using WhyNotLang.Interpreter.State;

namespace WhyNotLang.Interpreter.Evaluators
{
    public interface IBuiltinFunctionCollection
    {
        Dictionary<string, BuiltinFunctionDescription> FunctionDescriptions { get; }
        void DeclareBuiltinFunctions(IProgramState programState);
    }
}