using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class UnaryExpression : IExpression
    {
        public IExpression Inner { get;  }
        public Token Operator { get; }
        
        public ExpressionType Type => ExpressionType.Unary;
    }
}