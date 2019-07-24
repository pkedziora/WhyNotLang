using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.State
{
    public class ProgramState : IProgramState
    {
        public List<Scope> Scopes { get; }
        public Scope CurrentScope => Scopes.LastOrDefault();
        private Dictionary<string, FunctionDeclarationStatement> _functions;

        public ProgramState()
        {
            Scopes = new List<Scope>();
            AddScope("Main", true);
            _functions = new Dictionary<string, FunctionDeclarationStatement>();
            BuiltinFunctionEvaluator.DeclareBuiltinFunctions(this);
        }

        public Scope AddScope(string name, bool isFunctionScope = false)
        {
            Scopes.Add(new Scope(name, isFunctionScope));
            return CurrentScope;
        }

        public void RemoveScope()
        {
            Scopes.RemoveAt(Scopes.Count - 1);
        }

        public FunctionDeclarationStatement GetFunction(string identifier)
        {
            if (!_functions.ContainsKey(identifier))
            {
                throw new ArgumentException($"Function {identifier} is not defined");
            }
            
            return _functions[identifier];
        }
        
        public void DeclareFunction(string identifier, FunctionDeclarationStatement statement)
        {
            if (_functions.ContainsKey(identifier))
            {
                throw new ArgumentException($"Function {identifier} has already been declared");
            }
            
            _functions.Add(identifier, statement);
        }
        
        public ExpressionValue GetVariable(string identifier)
        {
            var scope = FindScopeForVariable(identifier);

            if (scope != null)
            {
                return scope.VariableValues[identifier];
            }
            
            throw new ArgumentException($"Variable {identifier} is not defined");
        }
        
        public bool IsVariableDefined(string identifier)
        {
            return FindScopeForVariable(identifier) != null;
        }
        
        public void DeclareVariable(string identifier, ExpressionValue value)
        {
            if (CurrentScope.VariableValues.ContainsKey(identifier))
            {
                throw new ArgumentException($"Variable {identifier} has already been declared");
            }
            
            CurrentScope.VariableValues.Add(identifier, value);
        }
        
        public void AssignVariable(string identifier, ExpressionValue value)
        {
            var scope = FindScopeForVariable(identifier);

            if (scope != null)
            {
                scope.VariableValues[identifier] = value;
                return;
            }

            throw new ArgumentException($"Variable {identifier} is not defined");
        }

        private Scope FindScopeForVariable(String identifier)
        {
            var scopeIndex = Scopes.Count;
            do
            {
                scopeIndex--;
                if (Scopes[scopeIndex].VariableValues.ContainsKey(identifier))
                {
                    return Scopes[scopeIndex];
                }
            } while (!Scopes[scopeIndex].IsFunctionScope);

            return null;
        }
    }
}