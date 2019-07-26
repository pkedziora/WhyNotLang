using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class ArrayExpression : IExpression
    {
        public Token Name { get; }
        public IExpression IndexExpression { get;  }
        
        public ExpressionType Type => ExpressionType.Array;
        
        
        public ArrayExpression(Token name, IExpression indexExpression)
        {
            Name = name;
            IndexExpression = indexExpression;
        }

        public override bool Equals(object obj)
        {
            var expression = obj as ArrayExpression;
            if (expression == null)
            {
                return false;
            }
            
            return Name.Equals(expression.Name) &&
                   IndexExpression.Equals(expression.IndexExpression) &&
                   Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + IndexExpression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}