using TheLang2.Syntax;

namespace TheLang2.AST
{
    public abstract class ASTUnary : ASTNode
    {
        protected ASTUnary(Position position)
            : base(position)
        { }

        public ASTNode Child { get; set; }
    }
}