using System;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class StatementExecutorFactory : IStatementExecutorFactory
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public StatementExecutorFactory(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }

        public IStatementExecutor CreateStatementExecutor(StatementType statementType, IExecutor mainExecutor)
        {
            switch (statementType)
            {
                case StatementType.VariableDeclarationStatement:
                    return new VariableDeclarationExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.VariableAssignmentStatement:
                    return new VariableAssignmentExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.FunctionDeclarationStatement:
                    return new FunctionDeclarationStatementExecutor(mainExecutor);
                case StatementType.IfStatement:
                    return new IfStatementExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.BlockStatement:
                    return new BlockStatementExecutor(mainExecutor);
                case StatementType.WhileStatement:
                    return new WhileStatementExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.FunctionCallStatement:
                    return new FunctionCallStatementExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.ReturnStatement:
                    return new ReturnStatementExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.ArrayDeclarationStatement:
                    return new ArrayDeclarationExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.ArrayAssignmentStatement:
                    return new ArrayAssignmentExecutor(_expressionEvaluator, mainExecutor);
                case StatementType.EmptyStatement:
                    return new EmptyExecutor();
            }
            
            throw new ArgumentException("Executor not found for current statement");
        }
    }
}