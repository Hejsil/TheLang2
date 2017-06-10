using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTCall : ASTUnary
    {
        public ASTCall(Position position)
            : base(position)
        { }

        public IEnumerable<ASTNode> Arguments { get; set; }
    }
}
