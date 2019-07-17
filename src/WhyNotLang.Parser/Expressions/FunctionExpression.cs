using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class FunctionExpression : IExpression
    {
        public Token Name { get; }
        public IExpression Parameter { get;  }
        
        public ExpressionType Type => ExpressionType.Function;
        
        
        public FunctionExpression(Token name, IExpression parameter)
        {
            Name = name;
            Parameter = parameter;
        }
        
        public override bool Equals(object obj)
        {
            var expression = obj as FunctionExpression;
            if (expression == null)
            {
                return false;
            }
            
            return Parameter.Equals(expression.Parameter) && 
                   Name.Equals(expression.Name) &&
                   Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Parameter.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}