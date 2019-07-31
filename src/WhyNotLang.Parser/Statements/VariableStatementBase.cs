using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public abstract class VariableStatementBase
    {
        public Token VariableName { get; }
        public IExpression Expression { get; }
        public int LineNumber { get; }

        public abstract StatementType Type { get; }

        public bool IsGlobal { get; }
        
        public VariableStatementBase(Token variableName, IExpression expression, bool isGlobal = false, int lineNumber = 0)
        {
            VariableName = variableName;
            Expression = expression;
            LineNumber = lineNumber;
            IsGlobal = isGlobal;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as VariableStatementBase;
            if (statement == null)
            {
                return false;
            }

            return VariableName.Equals(statement.VariableName) &&
                   Expression.Equals(statement.Expression) &&
                   IsGlobal.Equals(statement.IsGlobal) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + VariableName.GetHashCode();
                hash = hash * 23 + Expression.GetHashCode();
                hash = hash * 23 + IsGlobal.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}