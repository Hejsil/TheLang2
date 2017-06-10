using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTGreaterThanEqual : ASTBinary
    {
        public ASTGreaterThanEqual(Position position) : base(position)
        {
        }
    }
}