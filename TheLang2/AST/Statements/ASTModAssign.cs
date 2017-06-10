using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTModAssign : ASTBinary
    {
        public ASTModAssign(Position position) 
            : base(position)
        { }
    }
}