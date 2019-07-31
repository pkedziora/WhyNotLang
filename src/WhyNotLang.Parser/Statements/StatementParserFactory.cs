using System;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class StatementParserFactory : IStatementParserFactory
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public StatementParserFactory(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
           
        }

        public IStatementParser CreateStatementParser(Parser parser)
        {
            switch (_tokenIterator.CurrentToken.Type)
            {
                case TokenType.Var:
                case TokenType.Global:
                    if (_tokenIterator.PeekToken(2).Type == TokenType.LeftBracket)
                    {
                        return new ArrayDeclarationParser(_tokenIterator, _expressionParser);
                    }
                    
                    return new VariableDeclarationParser(_tokenIterator, _expressionParser);
                
                case TokenType.Identifier:
                    if (_tokenIterator.PeekToken(1).Type == TokenType.LeftParen)
                    {
                        return new FunctionCallParser(_tokenIterator, _expressionParser);
                    }
                    
                    if (_tokenIterator.PeekToken(1).Type == TokenType.LeftBracket)
                    {
                        return new ArrayAssignmentParser(_tokenIterator, _expressionParser);
                    }
                    
                    return new VariableAssignmentParser(_tokenIterator, _expressionParser);
                
                case TokenType.If:
                    return new IfStatementParser(_tokenIterator, _expressionParser, parser);
                
                case TokenType.Begin:
                    return new BlockStatementParser(_tokenIterator, parser);
                
                case TokenType.While:
                    return new WhileStatementParser(_tokenIterator, _expressionParser, parser);
                
                case TokenType.Function:
                    return new FunctionDeclarationParser(_tokenIterator, parser);
                
                case TokenType.Return:
                    return new ReturnStatementParser(_tokenIterator, _expressionParser);
            }
            
            throw new WhyNotLangException($"Unexpected token: {_tokenIterator.CurrentToken.Value}", _tokenIterator.CurrentToken.LineNumber);
        }
    }
}