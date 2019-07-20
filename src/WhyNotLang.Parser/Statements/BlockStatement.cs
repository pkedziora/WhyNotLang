using System.Collections.Generic;
using System.Linq;

namespace WhyNotLang.Parser.Statements
{
    public class BlockStatement : IStatement
    {
        public List<IStatement> ChildStatements { get; }
        public StatementType Type => StatementType.BlockStatement;

        public BlockStatement(List<IStatement> childStatements)
        {
            ChildStatements = childStatements;
        }
        
        public override bool Equals(object obj)
        {
            var statement = obj as BlockStatement;
            if (statement == null)
            {
                return false;
            }

            return ChildStatements.SequenceEqual(statement.ChildStatements) &&
                   Type.Equals(statement.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ChildStatements.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                
                return hash;
            }
        }
    }
}