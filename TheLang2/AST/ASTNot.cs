using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTNot : ASTUnary
    {
        public ASTNot(Position position) : base(position)
        {
        }
    }
}