using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutor
    {
        ExpressionValue Execute();
    }
}