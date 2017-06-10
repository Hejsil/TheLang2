using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTSub : ASTBinary
    {
        public ASTSub(Position position) 
            : base(position)
        { }
    }
}