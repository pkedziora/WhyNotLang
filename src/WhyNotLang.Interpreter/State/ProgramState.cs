using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = FindScopeForIdentifier(identifier);

            if (result.scope != null)
            {
                if (result.isArray)
                {
                    throw new ArgumentException($"{identifier} is an array. Variable expected");    
                }
                
                return result.scope.VariableValues[identifier];
            }
            
            throw new ArgumentException($"Variable {identifier} is not defined");
        }
        
        public ExpressionValue GetArrayItem(string identifier, int index)
        {
            var result = FindScopeForIdentifier(identifier);

            if (result.scope != null)
            {
                if (!result.isArray)
                {
                    throw new ArgumentException($"{identifier} is variable. Array expected");    
                }
                
                if (index >= result.scope.Arrays[identifier].Length)
                {
                    throw new ArgumentException($"Index {index} is out of range"); 
                }
                
                return result.scope.Arrays[identifier][index];
            }
            
            throw new ArgumentException($"Array {identifier} is not defined");
        }
        
        public ExpressionValue GetArrayReference(string identifier)
        {
            var result = FindScopeForIdentifier(identifier);

            if (result.scope != null)
            {
                if (!result.isArray)
                {
                    throw new ArgumentException($"{identifier} is variable. Array expected");    
                }
                
                return new ExpressionValue(result.scope.Arrays[identifier], ExpressionValueTypes.ArrayReference);
            }
            
            throw new ArgumentException($"Array {identifier} is not defined");
        }
        
        public void AssignArrayItem(string identifier, int index, ExpressionValue value)
        {
            var result = FindScopeForIdentifier(identifier);

            if (result.scope != null)
            {
                if (!result.isArray)
                {
                    throw new ArgumentException($"{identifier} is variable. Array expected");    
                }

                if (index >= result.scope.Arrays[identifier].Length)
                {
                    throw new ArgumentException($"Index {index} is out of range"); 
                }
                
                result.scope.Arrays[identifier][index] = value;
                
                return;
            }
            
            throw new ArgumentException($"Array {identifier} is not defined");
        }
        
        public ExpressionValue[] DeclareArray(string identifier, int size)
        {
            if (!CanBeDeclaredInCurrentScope(identifier))
            {
                throw new ArgumentException($"{identifier} has already been declared");
            }

            var array = new ExpressionValue[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = new ExpressionValue(0, ExpressionValueTypes.Number);
            }
            
            CurrentScope.Arrays.Add(identifier, array);

            return array;
        }

        private bool CanBeDeclaredInCurrentScope(string identifier)
        {
            return !CurrentScope.VariableValues.ContainsKey(identifier) &&
                   !CurrentScope.Arrays.ContainsKey(identifier) &&
                   !_functions.ContainsKey(identifier);
        }
        
        public bool IsVariableDefined(string identifier)
        {
            var result = FindScopeForIdentifier(identifier);
            return result.scope != null && result.scope.VariableValues.ContainsKey(identifier);
        }
        
        public bool IsArrayDefined(string identifier)
        {
            var result = FindScopeForIdentifier(identifier);
            return result.scope != null && result.scope.Arrays.ContainsKey(identifier);
        }
        
        public void DeclareVariable(string identifier, ExpressionValue value)
        {
            if (!CanBeDeclaredInCurrentScope(identifier))
            {
                throw new ArgumentException($"{identifier} has already been declared");
            }
            
            CurrentScope.VariableValues.Add(identifier, value);
        }
        
        public void DeclareArrayByReference(string identifier, ExpressionValue expressionValue)
        {
            if (!CanBeDeclaredInCurrentScope(identifier))
            {
                throw new ArgumentException($"{identifier} has already been declared");
            }

            if (expressionValue.Type != ExpressionValueTypes.ArrayReference)
            {
                throw new ArgumentException($"{identifier} is not an array reference");
            }
            
            CurrentScope.Arrays[identifier] = (ExpressionValue[]) expressionValue.Value;
        }
        
        public void AssignVariable(string identifier, ExpressionValue value)
        {
            var result = FindScopeForIdentifier(identifier);

            if (result.scope != null)
            {
                if (result.isArray)
                {
                    throw new ArgumentException($"{identifier} is an array. Variable expected");    
                }
                
                result.scope.VariableValues[identifier] = value;
                return;
            }

            throw new ArgumentException($"Variable {identifier} is not defined");
        }

        private (Scope scope, bool isArray) FindScopeForIdentifier(string identifier)
        {
            var scopeIndex = Scopes.Count;
            do
            {
                scopeIndex--;
                if (Scopes[scopeIndex].VariableValues.ContainsKey(identifier))
                {
                    return (Scopes[scopeIndex], false);
                }
                
                if (Scopes[scopeIndex].Arrays.ContainsKey(identifier))
                {
                    return (Scopes[scopeIndex], true);
                }
            } while (!Scopes[scopeIndex].IsFunctionScope);

            return (null, false);
        }
    }
}