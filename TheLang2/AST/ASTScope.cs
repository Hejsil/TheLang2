using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTScope : ASTNode
    {
        public ASTScope(Position position)
            : base(position)
        { }

        public IEnumerable<ASTNode> Statements { get; set; }
    }
}