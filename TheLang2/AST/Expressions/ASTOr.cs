using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTOr : ASTBinary
    {
        public ASTOr(Position position) 
            : base(position)
        { }
    }
}