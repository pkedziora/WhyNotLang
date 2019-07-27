using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Cmd
{
    public static class ServiceProviderExtensions
    {
        public static void AddConsoleInputOutput(this IServiceProvider serviceProvider)
        {
            var functionCollection = serviceProvider.GetRequiredService<IBuiltinFunctionCollection>();
            functionCollection.Add("Writeln",
                arguments =>
                {
                    var str = arguments.Single();
                    if (str.Type != ExpressionValueTypes.String)
                    {
                        throw new Exception("String expected");
                    }

                    Console.WriteLine((string)str.Value);
                    return ExpressionValue.Empty;
                });

            functionCollection.Add("Readln",
                arguments =>
                {
                    var str = Console.ReadLine();
                    return new ExpressionValue(str, ExpressionValueTypes.String);
                });
        }
    }
}