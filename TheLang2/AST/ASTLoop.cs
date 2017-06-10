using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTLoop : ASTNode
    {
        public ASTLoop(Position position) 
            : base(position)
        { }

        public ASTNode Condition { get; set; }
        public ASTScope Scope { get; set; }
    }
}