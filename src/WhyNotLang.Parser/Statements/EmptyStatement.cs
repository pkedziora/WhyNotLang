namespace WhyNotLang.Parser.Statements
{
    public class EmptyStatement : IStatement
    {
        public StatementType Type => StatementType.EmptyStatement;
        
        public override bool Equals(object obj)
        {
            var statement = obj as EmptyStatement;
            if (statement == null)
            {
                return false;
            }

            return Type.Equals(statement.Type);
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