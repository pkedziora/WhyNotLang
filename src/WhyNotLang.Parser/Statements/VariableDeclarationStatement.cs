using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class VariableDeclarationStatement : VariableStatementBase, IStatement
    {
        public override StatementType Type => StatementType.VariableDeclarationStatement;

        public VariableDeclarationStatement(Token variable, IExpression value) : base(variable, value)
        {
        }
    }
}