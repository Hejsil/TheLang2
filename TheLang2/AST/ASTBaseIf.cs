using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTBaseIf : ASTNode
    {
        public ASTBaseIf(Position position)
            : base(position)
        { }

        public IEnumerable<ASTSymbol> PayLoad { get; set; }
        public ASTNode Condition { get; set; }
    }
}
