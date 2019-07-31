using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class ArrayDeclarationStatement : IStatement
    {
        public Token ArrayName { get; }
        public IExpression IndexExpression { get; }
        public int LineNumber { get; }
        public StatementType Type => StatementType.ArrayDeclarationStatement;
        public bool IsGlobal { get; }
        
        public ArrayDeclarationStatement(Token arrayName, IExpression indexExpression, bool isGlobal = false, int lineNumber = 0)
        {
            ArrayName = arrayName;
            IndexExpression = indexExpression;
            LineNumber = lineNumber;
            IsGlobal = isGlobal;
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
                   IsGlobal.Equals(statement.IsGlobal) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ArrayName.GetHashCode();
                hash = hash * 23 + IndexExpression.GetHashCode();
                hash = hash * 23 + IsGlobal.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}