using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class VariableAssignmentStatement : VariableStatementBase, IStatement
    {
        public override StatementType Type => StatementType.VariableAssignmentStatement;

        public VariableAssignmentStatement(Token variableName, IExpression expression) : base(variableName, expression)
        {
        }
        
    }
}