using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTAssign : ASTBinary
    {
        public ASTAssign(Position position) 
            : base(position)
        { }
    }
}