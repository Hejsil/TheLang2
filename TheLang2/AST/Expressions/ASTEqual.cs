using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTEqual : ASTBinary
    {
        public ASTEqual(Position position) 
            : base(position)
        { }
    }
}