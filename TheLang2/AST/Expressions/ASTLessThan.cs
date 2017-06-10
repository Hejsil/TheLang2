using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTLessThan : ASTBinary
    {
        public ASTLessThan(Position position) 
            : base(position)
        { }
    }
}