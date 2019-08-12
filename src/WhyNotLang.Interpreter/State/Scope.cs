using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.State
{
    public class Scope
    {
        public string Name { get; }

        public Dictionary<string, ExpressionValue> VariableValues { get; }

        public Dictionary<string, ExpressionValue[]> Arrays { get; }
        public bool IsFunctionScope { get; }

        public Scope(string name, bool isFunctionScope = false)
        {
            Name = name;
            VariableValues = new Dictionary<string, ExpressionValue>();
            Arrays = new Dictionary<string, ExpressionValue[]>();
            IsFunctionScope = isFunctionScope;
        }
    }
}