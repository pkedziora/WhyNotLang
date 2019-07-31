using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Parser.Statements
{
    public class FunctionCallStatement : IStatement
    {
        public StatementType Type => StatementType.FunctionCallStatement;
        
        public FunctionExpression FunctionExpression { get; }
        public int LineNumber { get; }

        public FunctionCallStatement(FunctionExpression functionExpression, int lineNumber = 0)
        {
            FunctionExpression = functionExpression;
            LineNumber = lineNumber;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as FunctionCallStatement;
            if (statement == null)
            {
                return false;
            }
            
            return FunctionExpression.Equals(statement.FunctionExpression) &&
                   Type == statement.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + FunctionExpression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}