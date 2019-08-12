using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class ArrayAssignmentParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public ArrayAssignmentParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }

        public IStatement Parse()
        {
            var arrayName = _tokenIterator.CurrentToken;
            if (arrayName.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("identifier expected", _tokenIterator.CurrentToken.LineNumber);
            }

            //Parse brackets, should contain array index expression
            _tokenIterator.GetNextToken(); // Swallow arrayName
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftBracket)
            {
                throw new WhyNotLangException("[ expected", _tokenIterator.CurrentToken.LineNumber);
            }

            _tokenIterator.GetNextToken(); // Swallow [

            var arrayIndexExpression = _expressionParser.ParseNextExpression();

            if (_tokenIterator.CurrentToken.Type != TokenType.RightBracket)
            {
                throw new WhyNotLangException("] expected", _tokenIterator.CurrentToken.LineNumber);
            }
            _tokenIterator.GetNextToken(); // Swallow ]

            if (_tokenIterator.CurrentToken.Type != TokenType.Assign)
            {
                throw new WhyNotLangException(":= expected", _tokenIterator.CurrentToken.LineNumber);
            }
            _tokenIterator.GetNextToken(); // Swallow :=


            var arrayValExpression = _expressionParser.ParseNextExpression();

            var statement = new ArrayAssignmentStatement(arrayName, arrayIndexExpression, arrayValExpression, arrayName.LineNumber);

            return statement;
        }
    }
}