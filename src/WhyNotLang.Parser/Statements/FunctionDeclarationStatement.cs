using System.Collections.Generic;
using System.Linq;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class FunctionDeclarationStatement : IStatement
    {
        public StatementType Type => StatementType.FunctionDeclarationStatement;
        public Token Name { get; }
        public List<Token> Parameters { get; }
        
        public BlockStatement Body { get; }

        public FunctionDeclarationStatement(Token name, List<Token> parameters, BlockStatement body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as FunctionDeclarationStatement;
            if (statement == null)
            {
                return false;
            }
            
            return Name.Equals(statement.Name) && 
                   Parameters.SequenceEqual(statement.Parameters) &&
                   Body.Equals(statement.Body) &&
                   Type == statement.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Parameters.GetHashCode();
                hash = hash * 23 + Body.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}