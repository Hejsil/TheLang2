using System.Collections.Generic;
using TheLang2.AST;

namespace TheLang2.Syntax
{
    public class ASTArray : ASTNode
    {
        public ASTArray(Position position) 
            : base(position)
        { }

        public ASTNode Index { get; set; }
        public IEnumerable<ASTNode> Values { get; set; }
    }
}