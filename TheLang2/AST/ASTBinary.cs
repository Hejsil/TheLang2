using TheLang2.Syntax;

namespace TheLang2.AST
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