using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter
{
    public interface IExecutor
    {
        void Initialise(string program);
        Task<ExpressionValue> ExecuteNext();
        void ResetPosition();
        Task<ExpressionValue> ExecuteAll();
        IProgramState ProgramState { get; }
        IExecutorContext CurrentContext { get; }
        void CreateNewContext(List<IStatement> statements);
        void LeaveContext();
    }
}