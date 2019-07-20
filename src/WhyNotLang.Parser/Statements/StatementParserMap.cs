using System;
using System.Collections.Generic;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class StatementParserMap : IStatementParserMap
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public StatementParserMap(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
           
        }

        public IStatementParser GetStatementParser(Parser parser)
        {
            switch (_tokenIterator.CurrentToken.Type)
            {
                case TokenType.Var:
                    return new VariableDeclarationParser(_tokenIterator, _expressionParser);
                
                case TokenType.Identifier:
                    if (_tokenIterator.PeekToken(1).Type == TokenType.LeftParen)
                    {
                        return new FunctionCallParser(_tokenIterator, _expressionParser);
                    }
                    
                    return new VariableAssignmentParser(_tokenIterator, _expressionParser);
                
                case TokenType.If:
                    return new IfStatementParser(_tokenIterator, _expressionParser, parser);
                
                case TokenType.Begin:
                    return new BlockStatementParser(_tokenIterator, _expressionParser, parser);
                
                case TokenType.While:
                    return new WhileStatementParser(_tokenIterator, _expressionParser, parser);
                
                case TokenType.Function:
                    return new FunctionDeclarationParser(_tokenIterator, _expressionParser, parser);
            }
            
            throw new ArgumentException("Parser not found for current token");
        }
    }
}