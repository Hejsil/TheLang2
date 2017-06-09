using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTProcedure : ASTNode
    {
        public ASTProcedure(Position position)
            : base(position)
        { }

        public string Name { get; set; }
        public IEnumerable<ASTSymbol> Arguments { get; set; }
        public ASTNode Scope { get; set; }
    }
}