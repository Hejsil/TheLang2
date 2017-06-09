using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTCase : ASTNode
    {
        public ASTCase(Position position) 
            : base(position)
        { }
        
        public ASTNode Expression { get; set; }
        public ASTScope Scope { get; set; }
    }
}