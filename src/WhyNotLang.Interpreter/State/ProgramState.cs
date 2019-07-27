using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.State
{
    public class ProgramState : IProgramState
    {
        public IBuiltinFunctionCollection BuiltinFunctionCollection { get; }
        public Scope GlobalScope => _scopes.FirstOrDefault();
        public Scope CurrentScope => _scopes.LastOrDefault();

        private readonly List<Scope> _scopes;
        private readonly Dictionary<string, FunctionDeclarationStatement> _functions;
        public ProgramState(IBuiltinFunctionCollection builtinFunctionCollection)
        {
            BuiltinFunctionCollection = builtinFunctionCollection;
            _scopes = new List<Scope>();
            AddScope("Global", true);
            AddScope("Main");
            _functions = new Dictionary<string, FunctionDeclarationStatement>();
            builtinFunctionCollection.DeclareBuiltinFunctions(this);
        }

        public Scope AddScope(string name, bool isFunctionScope = false)
        {
            _scopes.Add(new Scope(name, isFunctionScope));
            return CurrentScope;
        }

        public void RemoveScope()
        {
            _scopes.RemoveAt(_scopes.Count - 1);
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

        private bool CanBeDeclaredInScope(string identifier, Scope scope)
        {
            return !scope.VariableValues.ContainsKey(identifier) &&
                   !scope.Arrays.ContainsKey(identifier) &&
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
        
        public ExpressionValue[] DeclareArray(string identifier, int size, bool isGlobal = false)
        {
            var scope = isGlobal ? GlobalScope : CurrentScope;
            if (!CanBeDeclaredInScope(identifier, scope))
            {
                throw new ArgumentException($"{identifier} has already been declared");
            }

            var array = new ExpressionValue[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = new ExpressionValue(0, ExpressionValueTypes.Number);
            }
            
            scope.Arrays.Add(identifier, array);

            return array;
        }
        
        public void DeclareVariable(string identifier, ExpressionValue value, bool isGlobal = false)
        {
            var scope = isGlobal ? GlobalScope : CurrentScope;
            if (!CanBeDeclaredInScope(identifier, scope))
            {
                throw new ArgumentException($"{identifier} has already been declared");
            }
            
            scope.VariableValues.Add(identifier, value);
        }
        
        public void DeclareArrayByReference(string identifier, ExpressionValue expressionValue)
        {
            if (!CanBeDeclaredInScope(identifier, CurrentScope))
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
            var scopeIndex = _scopes.Count;
            do
            {
                scopeIndex--;
                if (_scopes[scopeIndex].VariableValues.ContainsKey(identifier))
                {
                    return (_scopes[scopeIndex], false);
                }
                
                if (_scopes[scopeIndex].Arrays.ContainsKey(identifier))
                {
                    return (_scopes[scopeIndex], true);
                }
            } while (!_scopes[scopeIndex].IsFunctionScope);

            if (GlobalScope.VariableValues.ContainsKey(identifier))
            {
                return (GlobalScope, false);
            }
                
            if (GlobalScope.Arrays.ContainsKey(identifier))
            {
                return (GlobalScope, true);
            }
            
            return (null, false);
        }
    }
}