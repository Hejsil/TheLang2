using TheLang2.AST;

namespace TheLang2.Syntax
{
    public abstract class ASTBinary : ASTNode
    {
        protected ASTBinary(Position position)
            : base(position)
        { }

        public ASTNode Left { get; set; }
        public ASTNode Right { get; set; }
    }
}