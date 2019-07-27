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

        public void Add(BuiltinFunctionDescription description)
        {
            FunctionDescriptions[description.FunctionName] = description;
        }

        public void Add(string functionName, List<ExpressionValueTypes> parameters,
            Func<List<ExpressionValue>, ExpressionValue> implementation)
        {
            var functionDescription = new BuiltinFunctionDescription(functionName, parameters, implementation);
            Add(functionDescription);
        }

        public BuiltinFunctionCollection()
        {
            FunctionDescriptions = new Dictionary<string, BuiltinFunctionDescription>();
            Add("ToString",
                new List<ExpressionValueTypes> {ExpressionValueTypes.Number},
                arguments =>
                {
                    var number = arguments.Single();
                    if (number.Type != ExpressionValueTypes.Number)
                    {
                        throw new Exception("Number expected");
                    }

                    return new ExpressionValue(number.Value.ToString(), ExpressionValueTypes.String);
                }
            );

            Add("ToNumber",
                new List<ExpressionValueTypes> {ExpressionValueTypes.String},
                arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new Exception("String expected");
                    }

                    return new ExpressionValue(int.Parse((string) str.Value), ExpressionValueTypes.Number);
                });

            Add("Writeln",
                new List<ExpressionValueTypes>() {ExpressionValueTypes.String},
                arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new Exception("String expected");
                    }

                    Console.WriteLine(str.Value);
                    return ExpressionValue.Empty;
                });

            Add("Readln",
                new List<ExpressionValueTypes>(),
                arguments =>
                {
                    var str = Console.ReadLine();
                    return new ExpressionValue(str, ExpressionValueTypes.String);
                });
        }

        public void DeclareBuiltinFunctions(IProgramState programState)
        {
            foreach (var description in FunctionDescriptions)
            {
                programState.DeclareFunction(description.Key, new FunctionDeclarationStatement(
                    new Token(TokenType.Identifier, description.Key),
                    description.Value.Parameters
                        .Select((p, index) => new Token(TokenType.Identifier, $"p{index.ToString()}")).ToList()));
            }
        }
    }
}