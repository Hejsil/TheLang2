using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTDelete : ASTNode
    {
        public ASTDelete(Position position) 
            : base(position)
        { }

        public ASTNode Expression { get; set; }
    }
}