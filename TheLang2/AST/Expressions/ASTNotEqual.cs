using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTNotEqual : ASTBinary
    {
        public ASTNotEqual(Position position) 
            : base(position)
        { }
    }
}