using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class VariableAssignmentParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public VariableAssignmentParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }
        
        public IStatement Parse()
        {
            var variableName = _tokenIterator.CurrentToken;
            if (variableName.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("identifier expected", _tokenIterator.CurrentToken.LineNumber);    
            }
            
            _tokenIterator.GetNextToken();
            if (_tokenIterator.CurrentToken.Type != TokenType.Assign)
            {
                throw new WhyNotLangException(":= expected", _tokenIterator.CurrentToken.LineNumber);
            }

            _tokenIterator.GetNextToken();
            var expression = _expressionParser.ParseNextExpression();
            var statement = new VariableAssignmentStatement(variableName, expression, variableName.LineNumber);

            return statement;
        }
    }
}