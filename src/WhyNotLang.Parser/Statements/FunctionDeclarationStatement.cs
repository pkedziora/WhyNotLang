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
        public int LineNumber { get; }

        public bool IsBuiltin { get; }

        public FunctionDeclarationStatement(Token name, List<Token> parameters, BlockStatement body, int lineNumber = 0)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            LineNumber = lineNumber;
        }

        public FunctionDeclarationStatement(Token name, List<Token> parameters)
        {
            Name = name;
            Parameters = parameters;
            IsBuiltin = true;
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
                   (Body?.Equals(statement?.Body) ?? true) &&
                   IsBuiltin.Equals(statement.IsBuiltin) &&
                   Type == statement.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Parameters.GetHashCode();
                hash = hash * 23 + (Body?.GetHashCode() ?? 0);
                hash = hash * 23 + IsBuiltin.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}