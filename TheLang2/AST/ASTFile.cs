using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTFile : ASTNode
    {
        public ASTFile(Position position)
            : base(position)
        { }

        public IEnumerable<ASTNode> Nodes { get; set; }
    }
}