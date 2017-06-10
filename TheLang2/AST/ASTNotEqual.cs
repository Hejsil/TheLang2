using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTNotEqual : ASTBinary
    {
        public ASTNotEqual(Position position) : base(position)
        {
        }
    }
}