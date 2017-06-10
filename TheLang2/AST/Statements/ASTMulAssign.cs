using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTMulAssign : ASTBinary
    {
        public ASTMulAssign(Position position) 
            : base(position)
        { }
    }
}