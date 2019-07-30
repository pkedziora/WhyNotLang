using System;
using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class StatementExecutorFactory : IStatementExecutorFactory
    {
        private Dictionary<StatementType, IStatementExecutor> _executorsCache;
        private IExpressionEvaluator _expressionEvaluator;
        
        public StatementExecutorFactory()
        {
            _executorsCache = new Dictionary<StatementType, IStatementExecutor>();
        }

        public IStatementExecutor CreateOrGetFromCache(StatementType statementType, IExecutor mainExecutor)
        {
            if (!_executorsCache.TryGetValue(statementType, out var statementExecutor))
            {
                statementExecutor = CreateStatementExecutor(statementType, mainExecutor);
                _executorsCache[statementType] = statementExecutor;
            }

            return statementExecutor;
        }
        
        private IStatementExecutor CreateStatementExecutor(StatementType statementType, IExecutor mainExecutor)
        {
            _expressionEvaluator = _expressionEvaluator ?? new ExpressionEvaluator(mainExecutor);
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
            
            throw new WhyNotLangException("Executor not found for current statement");
        }
    }
}