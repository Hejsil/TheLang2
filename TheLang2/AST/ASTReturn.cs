using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTReturn : ASTNode
    {
        public ASTReturn(Position position) : base(position)
        {
        }

        public ASTNode Expression { get; set; }
    }
}