using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Cmd
{
    public class ExecutorFactory
    {
        public static Executor CreateExecutor(ProgramState programState)
        {
            var iterator = new StatementIterator(CreateParser());
            return new Executor(iterator, new StatementExecutorMap(iterator, new ExpressionEvaluator(programState, new BuiltinFunctionEvaluator(programState)), programState));
        }

        private static TokenIterator CreateTokenIterator()
        {
            return new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap()));
        }

        private static ExpressionParser CreateExpressionParser(ITokenIterator tokenIterator = null)
        {
            tokenIterator = tokenIterator ??
                            new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap()));
            return new ExpressionParser(tokenIterator);
        }
        
        private static Parser.Parser CreateParser()
        {
            var tokenIterator = CreateTokenIterator();
            var expressionParser = CreateExpressionParser(tokenIterator);
            return new Parser.Parser(tokenIterator, new StatementParserMap(tokenIterator, expressionParser));
        }
    }
}