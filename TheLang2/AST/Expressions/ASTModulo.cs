using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTModulo : ASTBinary
    {
        public ASTModulo(Position position) 
            : base(position)
        { }
    }
}