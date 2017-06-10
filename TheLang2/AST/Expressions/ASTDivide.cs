using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTDivide : ASTBinary
    {
        public ASTDivide(Position position) 
            : base(position)
        { }
    }
}