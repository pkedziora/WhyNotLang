using System;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class StatementExecutorMap : IStatementExecutorMap
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;

        public StatementExecutorMap(IStatementIterator statementIterator, IExpressionEvaluator expressionEvaluator)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
        }

        public IStatementExecutor GetStatementExecutor()
        {
            switch (_statementIterator.CurrentStatement.Type)
            {
                case StatementType.VariableDeclarationStatement:
                    return new VariableDeclarationExecutor();
            }
            
            throw new ArgumentException("Executor not found for current statement");
        }
    }
}