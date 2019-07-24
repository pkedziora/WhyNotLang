using WhyNotLang.Interpreter.Evaluators.ExpressionValues;

namespace WhyNotLang.Interpreter
{
    public interface IExecutor
    {
        void Initialise(string program);
        ExpressionValue ExecuteNext();
        void ResetPosition();
        ExpressionValue ExecuteAll();
    }
}