using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTAdd : ASTBinary
    {
        public ASTAdd(Position position) 
            : base(position)
        { }
    }
}