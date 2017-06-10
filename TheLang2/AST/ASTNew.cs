using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTNew : ASTUnary
    {
        public ASTNew(Position position) 
            : base(position)
        { }
    }
}