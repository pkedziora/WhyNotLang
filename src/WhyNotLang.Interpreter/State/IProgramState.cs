using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.State
{
    public interface IProgramState
    {
        Scope CurrentScope { get; }
        FunctionDeclarationStatement GetFunction(string identifier);
        void DeclareFunction(string identifier, FunctionDeclarationStatement statement);
    }
}