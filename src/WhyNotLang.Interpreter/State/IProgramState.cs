using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.State
{
    public interface IProgramState
    {
        Scope CurrentScope { get; }
        FunctionDeclarationStatement GetFunction(string identifier);
        void DeclareFunction(string identifier, FunctionDeclarationStatement statement);
        ExpressionValue GetVariable(string identifier);
        bool IsVariableDefined(string identifier);
        void DeclareVariable(string identifier, ExpressionValue value);
        void AssignVariable(string identifier, ExpressionValue value);
        Scope AddScope(string name, bool isFunctionScope = false);
        void RemoveScope();
    }
}