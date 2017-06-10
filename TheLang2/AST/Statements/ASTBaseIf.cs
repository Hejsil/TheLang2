using System.Collections.Generic;
using TheLang2.AST.Expressions;
using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public abstract class ASTBaseIf : ASTNode
    {
        protected ASTBaseIf(Position position)
            : base(position)
        { }

        public IEnumerable<ASTSymbol> PayLoad { get; set; }
        public ASTNode Condition { get; set; }
    }
}
