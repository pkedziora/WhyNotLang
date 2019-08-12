using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Parser.Statements
{
    public class ReturnStatement : IStatement
    {
        public IExpression ReturnExpression { get; }
        public int LineNumber { get; }
        public StatementType Type => StatementType.ReturnStatement;

        public ReturnStatement(IExpression returnExpression, int lineNumber = 0)
        {
            ReturnExpression = returnExpression;
            LineNumber = lineNumber;
        }

        public override bool Equals(object obj)
        {
            var statement = obj as ReturnStatement;
            if (statement == null)
            {
                return false;
            }

            return ReturnExpression.Equals(statement.ReturnExpression) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ReturnExpression.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}