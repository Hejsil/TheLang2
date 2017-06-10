using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
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