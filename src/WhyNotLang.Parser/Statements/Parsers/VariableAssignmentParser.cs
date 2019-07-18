namespace WhyNotLang.Parser.Statements.Parsers
{
    public class VariableAssignmentParser : IStatementParser
    {
        private readonly IExpressionParser _expressionParser;

        public VariableAssignmentParser(IExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
        }
        
        public IStatement Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}