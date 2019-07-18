using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class VariableDeclarationStatement : IStatement
    {
        public Token Variable { get; }
        public IExpression Value { get; }
        public StatementType Type => StatementType.VariableDeclarationStatement;

        public VariableDeclarationStatement(Token variable, IExpression value)
        {
            Variable = variable;
            Value = value;
        }
    }
}