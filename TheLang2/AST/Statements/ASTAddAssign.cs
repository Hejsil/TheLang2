using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTAddAssign : ASTBinary
    {
        public ASTAddAssign(Position position) 
            : base(position)
        { }
    }
}