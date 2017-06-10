using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTDivAssign : ASTBinary
    {
        public ASTDivAssign(Position position) 
            : base(position)
        { }
    }
}