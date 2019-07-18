namespace WhyNotLang.Parser.Statements.Parsers
{
    public class VariableDeclarationParser : IStatementParser
    {
        private readonly IExpressionParser _expressionParser;

        public VariableDeclarationParser(IExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
        }
        
        public IStatement Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}