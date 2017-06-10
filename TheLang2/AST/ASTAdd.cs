using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTAdd : ASTBinary
    {
        public ASTAdd(Position position) 
            : base(position)
        { }
    }
}