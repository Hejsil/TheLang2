using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTGreaterThanEqual : ASTBinary
    {
        public ASTGreaterThanEqual(Position position) 
            : base(position)
        { }
    }
}