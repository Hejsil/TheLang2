using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTInteger : ASTNode
    {
        public ASTInteger(Position position) 
            : base(position)
        { }

        public long Value { get; set; }
    }
}