namespace WhyNotLang.Interpreter.Evaluators.ExpressionValues
{
    public class ExpressionValue
    {
        public object Value { get; }
        public ExpressionValueTypes Type { get; }

        public static ExpressionValue Empty => new ExpressionValue(0, ExpressionValueTypes.Empty);
        public ExpressionValue(object value, ExpressionValueTypes type)
        {
            Value = value;
            Type = type;
        }
        
        public override bool Equals(object obj)
        {
            var expressionValue = obj as ExpressionValue;
            if (expressionValue == null)
            {
                return false;
            }
            
            return Value.Equals(expressionValue.Value) &&
                   Type == expressionValue.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Value.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}