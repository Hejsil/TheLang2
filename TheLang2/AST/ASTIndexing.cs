using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTIndexing : ASTUnary
    {
        public ASTIndexing(Position position) : base(position)
        {
        }

        public ASTNode Value { get; set; }
    }
}