using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTCall : ASTUnary
    {
        public ASTCall(Position position)
            : base(position)
        { }

        public IEnumerable<ASTNode> Arguments { get; set; }
    }
}
