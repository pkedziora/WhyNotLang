using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.State
{
    public class Scope
    {
        public string Name { get; }

        private Dictionary<string, ExpressionValue> _variableValues;

        public Scope(string name)
        {
            Name = name;
            _variableValues = new Dictionary<string, ExpressionValue>();
        }

        public ExpressionValue GetVariable(string identifier)
        {
            if (!_variableValues.ContainsKey(identifier))
            {
                throw new ArgumentException($"Variable {identifier} is not defined");
            }
            
            return _variableValues[identifier];
        }
        
        public bool IsVariableDefined(string identifier)
        {
            return _variableValues.ContainsKey(identifier);
        }
        
        public void DeclareVariable(string identifier, ExpressionValue value)
        {
            if (_variableValues.ContainsKey(identifier))
            {
                throw new ArgumentException($"Variable {identifier} has already been declared");
            }
            
            _variableValues.Add(identifier, value);
        }
        
        public void AssignVariable(string identifier, ExpressionValue value)
        {
            if (!_variableValues.ContainsKey(identifier))
            {
                throw new ArgumentException($"Variable {identifier} is not defined");
            }
            
            _variableValues[identifier] = value;
        }
    }
}