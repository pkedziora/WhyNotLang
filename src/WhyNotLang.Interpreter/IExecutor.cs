using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter
{
    public interface IExecutor
    {
        void Initialise(string program);
        Task<ExpressionValue> ExecuteNext();
        void ResetPosition();
        Task<ExpressionValue> ExecuteAll();
    }
}