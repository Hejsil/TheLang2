using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTMul : ASTBinary
    {
        public ASTMul(Position position) 
            : base(position)
        { }
    }
}