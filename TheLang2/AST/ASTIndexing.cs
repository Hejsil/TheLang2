using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTIndexing : ASTUnary
    {
        public ASTIndexing(Position position) : base(position)
        {
        }

        public ASTNode Value { get; set; }
    }
}