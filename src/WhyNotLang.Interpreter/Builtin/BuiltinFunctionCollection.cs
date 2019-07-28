using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Builtin
{
    public class BuiltinFunctionCollection : IBuiltinFunctionCollection
    {
        public Dictionary<string, BuiltinFunctionDescription> FunctionDescriptions { get; }

        public void Add(BuiltinFunctionDescription description)
        {
            FunctionDescriptions[description.FunctionName] = description;
        }

        public void Add(string functionName, Func<List<ExpressionValue>, Task<ExpressionValue>> implementation)
        {
            var functionDescription = new BuiltinFunctionDescription(functionName, implementation);
            Add(functionDescription);
        }
        
        public void DeclareBuiltinFunctions(IProgramState programState)
        {
            foreach (var description in FunctionDescriptions)
            {
                programState.DeclareFunction(description.Key, new FunctionDeclarationStatement(
                    new Token(TokenType.Identifier, description.Key), new List<Token>()));
            }
        }

        public BuiltinFunctionCollection()
        {
            FunctionDescriptions = new Dictionary<string, BuiltinFunctionDescription>();
            Add("ToString",
                async arguments =>
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
                async arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new Exception("String expected");
                    }

                    return new ExpressionValue(int.Parse((string) str.Value), ExpressionValueTypes.Number);
            });

            Add("Delay",
                async arguments =>
                {
                    var number = arguments.Single();
                    if (number.Type != ExpressionValueTypes.Number)
                    {
                        throw new Exception("Number expected");
                    }

                    await Task.Delay((int)number.Value);

                    return ExpressionValue.Empty;
                });
        }
    }
}