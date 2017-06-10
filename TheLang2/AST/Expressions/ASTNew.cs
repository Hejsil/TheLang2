using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTNew : ASTUnary
    {
        public ASTNew(Position position) 
            : base(position)
        { }
    }
}