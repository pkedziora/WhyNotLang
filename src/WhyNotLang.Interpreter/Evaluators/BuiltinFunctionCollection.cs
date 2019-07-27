using System;
using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class BuiltinFunctionCollection : IBuiltinFunctionCollection
    {
        public Dictionary<string, BuiltinFunctionDescription> FunctionDescriptions { get; }

        public BuiltinFunctionCollection()
        {
            FunctionDescriptions =
                new Dictionary<string, BuiltinFunctionDescription>()
                {
                    {
                        "ToString", new BuiltinFunctionDescription("ToString",
                            new List<ExpressionValueTypes>() {ExpressionValueTypes.Number},
                            arguments =>
                            {
                                var number = arguments.Single();
                                if (number.Type != ExpressionValueTypes.Number)
                                {
                                    throw new Exception("Number expected");
                                }

                                return new ExpressionValue(number.Value.ToString(), ExpressionValueTypes.String);
                            })
                    },
                    {
                        "ToNumber", new BuiltinFunctionDescription("ToNumber",
                            new List<ExpressionValueTypes>() {ExpressionValueTypes.Number},
                            arguments =>
                            {
                                var str = arguments.Single();
                                if (str.Type != ExpressionValueTypes.String)
                                {
                                    throw new Exception("String expected");
                                }

                                return new ExpressionValue(int.Parse((string) str.Value), ExpressionValueTypes.Number);
                            })
                    },
                    {
                        "Writeln", new BuiltinFunctionDescription("Writeln",
                            new List<ExpressionValueTypes>() {ExpressionValueTypes.Number},
                            arguments =>
                            {
                                var str = arguments.Single();
                                if (str.Type != ExpressionValueTypes.String)
                                {
                                    throw new Exception("String expected");
                                }

                                Console.WriteLine(str.Value);
                                return ExpressionValue.Empty;
                            })
                    },
                    {
                        "Readln", new BuiltinFunctionDescription("Readln",
                            new List<ExpressionValueTypes>() {ExpressionValueTypes.Number},
                            arguments =>
                            {
                                var str = Console.ReadLine();
                                return new ExpressionValue(str, ExpressionValueTypes.String);
                            })
                    }
                };
        }
        
        public void DeclareBuiltinFunctions(IProgramState programState)
        {
            programState.DeclareFunction("ToString",
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "ToString"),
                    new List<Token>() {new Token(TokenType.Identifier, "number")}));

            programState.DeclareFunction("ToNumber",
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "ToNumber"),
                    new List<Token>() {new Token(TokenType.Identifier, "str")}));

            programState.DeclareFunction("Writeln",
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "Writeln"),
                    new List<Token>() {new Token(TokenType.Identifier, "str")}));

            programState.DeclareFunction("Readln",
                new FunctionDeclarationStatement(new Token(TokenType.Identifier, "Readln"),
                    new List<Token>()));
        }
    }
}