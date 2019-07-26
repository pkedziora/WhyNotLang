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
        bool IsArrayDefined(string identifier);
        void DeclareVariable(string identifier, ExpressionValue value);
        void AssignVariable(string identifier, ExpressionValue value);
        Scope AddScope(string name, bool isFunctionScope = false);
        void RemoveScope();
        ExpressionValue GetArrayItem(string identifier, int index);
        void AssignArrayItem(string identifier, int index, ExpressionValue value);
        ExpressionValue[] DeclareArray(string identifier, int size);
        ExpressionValue GetArrayReference(string identifier);
        void DeclareArrayByReference(string identifier, ExpressionValue expressionValue);
    }
}