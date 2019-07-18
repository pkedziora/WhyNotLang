using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class VariableAssignmentStatement : IStatement
    {
        public Token Variable { get; }
        public IExpression Value { get; }
        
        public StatementType Type => StatementType.VariableAssignmentStatement;

        public VariableAssignmentStatement(Token variable, IExpression value)
        {
            Variable = variable;
            Value = value;
        }
    }
}