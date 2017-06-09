using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTNode
    {
        public ASTNode(Position position) => Position = position;

        public Position Position { get; }
    }
}
