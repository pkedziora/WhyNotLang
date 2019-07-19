using System;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public abstract class VariableStatementBase
    {
        public Token Variable { get; }
        public IExpression Value { get; }

        public abstract StatementType Type { get; }

        public VariableStatementBase(Token variable, IExpression value)
        {
            Variable = variable;
            Value = value;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as VariableStatementBase;
            if (statement == null)
            {
                return false;
            }

            return Variable.Equals(statement.Variable) &&
                   Value.Equals(statement.Value) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Variable.GetHashCode();
                hash = hash * 23 + Value.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}