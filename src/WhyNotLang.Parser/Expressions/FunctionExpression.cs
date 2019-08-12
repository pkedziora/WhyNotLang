using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class FunctionExpression : IExpression
    {
        public Token Name { get; }
        public List<IExpression> Parameters { get; }

        public ExpressionType Type => ExpressionType.Function;


        public FunctionExpression(Token name, List<IExpression> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public FunctionExpression(Token name, IExpression parameters)
        {
            Name = name;
            Parameters = new List<IExpression> { parameters };
        }

        public override bool Equals(object obj)
        {
            var expression = obj as FunctionExpression;
            if (expression == null)
            {
                return false;
            }

            return Parameters.SequenceEqual(expression.Parameters) &&
                   Name.Equals(expression.Name) &&
                   Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Parameters.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}