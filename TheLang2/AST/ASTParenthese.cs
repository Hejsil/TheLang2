using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTParenthese : ASTUnary
    {
        public ASTParenthese(Position position) 
            : base(position)
        { }
    }
}