using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTRangeLoop : ASTNode
    {
        public ASTRangeLoop(Position position) 
            : base(position)
        { }

        public ASTNode RangeStart { get; set; }
        public ASTNode RangeEnd { get; set; }
        public ASTScope Scope { get; set; }
    }
}