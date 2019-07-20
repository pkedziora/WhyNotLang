using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Parser.Statements
{
    public class WhileStatement : IStatement
    {
        public IExpression Condition { get; }
        public IStatement Body { get; }
        public StatementType Type => StatementType.WhileStatement;

        public WhileStatement(IExpression condition, IStatement body)
        {
            Condition = condition;
            Body = body;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as WhileStatement;
            if (statement == null)
            {
                return false;
            }
            
            return Condition.Equals(statement.Condition) && 
                   Body.Equals(statement.Body) &&
                   Type == statement.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Condition.GetHashCode();
                hash = hash * 23 + Body.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}