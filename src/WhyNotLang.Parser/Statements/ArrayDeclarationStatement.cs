using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class ArrayDeclarationStatement : IStatement
    {
        public Token ArrayName { get; }
        public IExpression IndexExpression { get; }
        public StatementType Type => StatementType.ArrayDeclarationStatement;

        public ArrayDeclarationStatement(Token arrayName, IExpression indexExpression)
        {
            ArrayName = arrayName;
            IndexExpression = indexExpression;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as ArrayDeclarationStatement;
            if (statement == null)
            {
                return false;
            }

            return ArrayName.Equals(statement.ArrayName) &&
                   IndexExpression.Equals(statement.IndexExpression) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ArrayName.GetHashCode();
                hash = hash * 23 + IndexExpression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}