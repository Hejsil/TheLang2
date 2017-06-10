using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTGreaterThan : ASTBinary
    {
        public ASTGreaterThan(Position position) 
            : base(position)
        { }
    }
}