using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTLoop : ASTNode
    {
        public ASTLoop(Position position) 
            : base(position)
        { }

        public ASTNode Condition { get; set; }
        public ASTScope Scope { get; set; }
    }
}