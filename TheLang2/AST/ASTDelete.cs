using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTDelete : ASTNode
    {
        public ASTDelete(Position position) 
            : base(position)
        { }

        public ASTNode Expression { get; set; }
    }
}