using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.State
{
    public class ProgramState : IProgramState
    {
        public Stack<Scope> Scopes { get; }
        public Scope CurrentScope => Scopes.Peek();
        private Dictionary<string, FunctionDeclarationStatement> _functions;

        public ProgramState()
        {
            Scopes = new Stack<Scope>();
            Scopes.Push(new Scope("Main"));
            _functions = new Dictionary<string, FunctionDeclarationStatement>();
            BuiltinFunctionEvaluator.DeclareBuiltinFunctions(this);
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
    }
}