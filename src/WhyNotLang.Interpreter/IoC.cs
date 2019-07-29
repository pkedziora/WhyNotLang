using System;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Builtin;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter
{
    public static class IoC
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddWhyNotLang()
                .BuildServiceProvider();

            return serviceProvider;
        }

        public static IServiceCollection AddWhyNotLang(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IExecutor, Executor>()
                .AddSingleton<IStatementIterator, StatementIterator>()
                .AddSingleton<IStatementExecutorFactory, StatementExecutorFactory>()
                .AddSingleton<IParser, Parser.Parser>()
                .AddSingleton<ITokenIterator, TokenIterator>()
                .AddSingleton<ITokenizer, Tokenizer.Tokenizer>()
                .AddSingleton<ITokenReader, TokenReader>()
                .AddSingleton<ITokenFactory, TokenFactory>()
                .AddSingleton<IStatementParserFactory, StatementParserFactory>()
                .AddSingleton<IBuiltinFunctionEvaluator, BuiltinFunctionEvaluator>()
                .AddSingleton<IExpressionParser, ExpressionParser>()
                .AddSingleton<IStatementExecutorFactory, StatementExecutorFactory>()
                .AddSingleton<IStatementIterator, StatementIterator>()
                .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
                .AddSingleton<IProgramState, ProgramState>()
                .AddSingleton<IBuiltinFunctionCollection, BuiltinFunctionCollection>();

            return serviceCollection;
        }
    }
}