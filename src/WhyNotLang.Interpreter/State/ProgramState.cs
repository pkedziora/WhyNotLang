using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.State
{
    public class ProgramState : IProgramState
    {
        public Stack<Scope> Scopes { get; }
        public Scope CurrentScope => Scopes.Peek();
        public Dictionary<string, FunctionDeclarationStatement> _functions;

        public ProgramState()
        {
            Scopes = new Stack<Scope>();
            Scopes.Push(new Scope("Main"));
            _functions = new Dictionary<string, FunctionDeclarationStatement>();
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