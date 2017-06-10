using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTAnd : ASTBinary
    {
        public ASTAnd(Position position) 
            : base(position)
        { }
    }
}