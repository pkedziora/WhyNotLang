using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class EmptyExecutor : IStatementExecutor
    {
        public async Task<ExpressionValue> Execute()
        {
            return ExpressionValue.Empty;
        }
    }
}