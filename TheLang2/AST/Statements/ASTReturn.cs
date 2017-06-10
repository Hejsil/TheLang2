using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTReturn : ASTNode
    {
        public ASTReturn(Position position) 
            : base(position)
        { }

        public ASTNode Expression { get; set; }
    }
}