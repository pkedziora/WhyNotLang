using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementIterator
    {
        IStatement CurrentStatement{ get; }
        void InitStatements(string program);
        IStatement GetNextStatement();
        IStatement PeekStatement(int offset);
    }
}