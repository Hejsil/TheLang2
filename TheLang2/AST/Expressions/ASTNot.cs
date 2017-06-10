using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTNot : ASTUnary
    {
        public ASTNot(Position position) 
            : base(position)
        { }
    }
}