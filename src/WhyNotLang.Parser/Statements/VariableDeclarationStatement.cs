using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class VariableDeclarationStatement : VariableStatementBase, IStatement
    {
        public override StatementType Type => StatementType.VariableDeclarationStatement;

        public VariableDeclarationStatement(Token variableName, IExpression expression, bool isGlobal = false) : base(variableName, expression, isGlobal)
        {
        }
    }
}