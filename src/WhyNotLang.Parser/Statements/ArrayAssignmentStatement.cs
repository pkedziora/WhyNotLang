using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class ArrayAssignmentStatement : IStatement
    {
        public Token ArrayName { get; }
        public IExpression IndexExpression { get; }
        public IExpression ValExpression { get; }
        public StatementType Type => StatementType.ArrayAssignmentStatement;

        public ArrayAssignmentStatement(Token arrayName, IExpression indexExpression, IExpression valExpression)
        {
            ArrayName = arrayName;
            IndexExpression = indexExpression;
            ValExpression = valExpression;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as ArrayAssignmentStatement;
            if (statement == null)
            {
                return false;
            }

            return ArrayName.Equals(statement.ArrayName) &&
                   IndexExpression.Equals(statement.IndexExpression) &&
                   ValExpression.Equals(statement.ValExpression) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ArrayName.GetHashCode();
                hash = hash * 23 + IndexExpression.GetHashCode();
                hash = hash * 23 + ValExpression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}