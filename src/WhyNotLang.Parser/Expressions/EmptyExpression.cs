namespace WhyNotLang.Parser.Expressions
{
    public class EmptyExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Empty;

        public override bool Equals(object obj)
        {
            var expression = obj as EmptyExpression;
            if (expression == null)
            {
                return false;
            }

            return Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}