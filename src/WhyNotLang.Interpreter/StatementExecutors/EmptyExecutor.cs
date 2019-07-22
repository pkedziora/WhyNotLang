using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class EmptyExecutor : IStatementExecutor
    {
        public ExpressionValue Execute()
        {
            return ExpressionValue.Empty;
        }
    }
}