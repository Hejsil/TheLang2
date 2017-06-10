using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTLessThanEqual : ASTBinary
    {
        public ASTLessThanEqual(Position position) 
            : base(position)
        { }
    }
}