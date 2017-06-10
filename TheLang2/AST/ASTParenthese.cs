using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTParenthese : ASTUnary
    {
        public ASTParenthese(Position position) 
            : base(position)
        { }
    }
}