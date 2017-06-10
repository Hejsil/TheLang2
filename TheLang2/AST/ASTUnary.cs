using TheLang2.AST;

namespace TheLang2.Syntax
{
    public abstract class ASTUnary : ASTNode
    {
        protected ASTUnary(Position position)
            : base(position)
        { }

        public ASTNode Child { get; set; }
    }
}