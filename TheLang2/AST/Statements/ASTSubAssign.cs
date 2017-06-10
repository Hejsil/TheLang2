using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTSubAssign : ASTBinary
    {
        public ASTSubAssign(Position position) 
            : base(position)
        { }
    }
}