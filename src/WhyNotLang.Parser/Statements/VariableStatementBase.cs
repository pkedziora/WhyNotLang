using System;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public abstract class VariableStatementBase
    {
        public Token VariableName { get; }
        public IExpression Expression { get; }

        public abstract StatementType Type { get; }

        public VariableStatementBase(Token variableName, IExpression expression)
        {
            VariableName = variableName;
            Expression = expression;
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
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + VariableName.GetHashCode();
                hash = hash * 23 + Expression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}