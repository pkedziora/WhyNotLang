using System.Collections.Generic;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementIterator
    {
        IStatement CurrentStatement { get; }
        void InitStatements(List<IStatement> statements);
        IStatement GetNextStatement();
        IStatement PeekStatement(int offset);
        void ResetPosition();
    }
}