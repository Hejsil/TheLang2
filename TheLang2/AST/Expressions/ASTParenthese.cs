using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTParenthese : ASTUnary
    {
        public ASTParenthese(Position position) 
            : base(position)
        { }
    }
}