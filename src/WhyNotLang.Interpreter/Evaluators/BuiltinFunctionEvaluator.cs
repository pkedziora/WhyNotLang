using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class BuiltinFunctionEvaluator : IBuiltinFunctionEvaluator
    {
        private readonly IProgramState _programState;

        public BuiltinFunctionEvaluator(IProgramState programState)
        {
            _programState = programState;
        }
        
        public ExpressionValue Eval(string functionName, List<ExpressionValue> parameterValues)
        {
            switch (functionName)
            {
                case "ToString":
                    return Functions.ToString(parameterValues.Single());
                case "ToNumber":
                    return Functions.ToNumber(parameterValues.Single());
            }
            
            throw new ArgumentException("Unexpected builtin function name");
        }
        
        public static void DeclareBuiltinFunctions(IProgramState programState)
        {
            programState.DeclareFunction("ToString", 
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "ToString"),
                    new List<Token>() {new Token(TokenType.Identifier, "number")}));
            
            programState.DeclareFunction("ToNumber", 
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "ToNumber"),
                    new List<Token>() {new Token(TokenType.Identifier, "str")}));
        }
    }
}