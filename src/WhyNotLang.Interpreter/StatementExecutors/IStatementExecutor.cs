using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutor
    {
        Task<ExpressionValue> Execute();
    }
}