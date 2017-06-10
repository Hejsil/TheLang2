using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTInteger : ASTNode
    {
        public ASTInteger(Position position) : base(position)
        {
        }

        public long Value { get; set; }
    }
}