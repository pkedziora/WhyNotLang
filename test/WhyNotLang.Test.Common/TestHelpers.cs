using System.Linq;
using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Test.Common
{
    public class TestHelpers
    {
        private static readonly Tokenizer.Tokenizer _tokenizer = CreateTokenizer();

        public static TokenIterator CreateTokenIterator()
        {
            return new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap()));
        }

        public static ExpressionParser CreateExpressionParser(ITokenIterator tokenIterator = null)
        {
            tokenIterator = tokenIterator ??
                            new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap()));
            return new ExpressionParser(tokenIterator);
        }
        
        public static Parser.Parser CreateParser()
        {
            var tokenIterator = CreateTokenIterator();
            var expressionParser = CreateExpressionParser(tokenIterator);
            return new Parser.Parser(tokenIterator, new StatementParserMap(tokenIterator, expressionParser));
        }

        public static Executor CreateExecutor(ProgramState programState)
        {
            var iterator = new StatementIterator(CreateParser());
            return new Executor(iterator, new StatementExecutorMap(iterator, new ExpressionEvaluator(), programState));
        }
        
        public static BinaryExpression GetBinaryExpression(int a, string op, int b)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpressionWithIdentifiers(string a, string op, string b)
        {
            var left = new ValueExpression(new Token(TokenType.Identifier, a));
            var right = new ValueExpression(new Token(TokenType.Identifier, b));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpressionWithIdentifiers(string identifier, string op, int number)
        {
            var left = new ValueExpression(new Token(TokenType.Identifier, identifier));
            var right = new ValueExpression(new Token(TokenType.Number, number.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpressionWithStrings(string a, string op, string b)
        {
            var left = new ValueExpression(new Token(TokenType.String, a));
            var right = new ValueExpression(new Token(TokenType.String, b));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(IExpression left, string op, int b)
        {
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpressionWithIdentifiers(IExpression left, string op, string b)
        {
            var right = new ValueExpression(new Token(TokenType.Identifier, b));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(int a, string op, IExpression right)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpressionWithIdentifiers(string a, string op, IExpression right)
        {
            var left = new ValueExpression(new Token(TokenType.Identifier, a));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(IExpression left, string op, IExpression right)
        {
            return new BinaryExpression(left, GetToken(op), right);
        }

        public static UnaryExpression GetUnaryExpression(string op, int number)
        {
            return new UnaryExpression(GetValueExpression(number), GetToken(op));
        }
        
        public static UnaryExpression GetUnaryExpression(string op, string identifier)
        {
            return new UnaryExpression(GetValueExpression(identifier), GetToken(op));
        }
        
        public static UnaryExpression GetUnaryExpressionWithIdentifier(string op, string identifier)
        {
            return new UnaryExpression(new ValueExpression(GetToken(identifier)), GetToken(op));
        }

        public static ValueExpression GetValueExpression(int number)
        {
            return new ValueExpression(GetToken(number.ToString()));
        }
        
        public static ValueExpression GetValueExpression(string identifier)
        {
            return new ValueExpression(GetToken(identifier));
        }
        
        public static ValueExpression GetValueExpressionAsString(string str)
        {
            return new ValueExpression(new Token(TokenType.String, str));
        }
        
        public static UnaryExpression GetUnaryExpression(string op, IExpression inner)
        {
            return new UnaryExpression(inner, GetToken(op));
        }

        public static VariableAssignmentStatement GetVariableAssignementStatement(string identifier, IExpression expression)
        {
            return new VariableAssignmentStatement(GetToken(identifier),
                expression);
        }
        
        public static VariableDeclarationStatement GetVariableDeclarationStatement(string identifier, IExpression expression)
        {
            return new VariableDeclarationStatement(GetToken(identifier),
                expression);
        }

        public static Token GetToken(string token)
        {
            return _tokenizer.GetTokens(token).FirstOrDefault();
        }

        private static Tokenizer.Tokenizer CreateTokenizer()
        {
            return new Tokenizer.Tokenizer(new TokenReader(), new TokenMap());
        }
    }
}