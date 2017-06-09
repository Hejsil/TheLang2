using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTSymbol : ASTNode
    {
        public ASTSymbol(Position position)
            : base(position)
        { }

        public string Name { get; set; }
    }
}
