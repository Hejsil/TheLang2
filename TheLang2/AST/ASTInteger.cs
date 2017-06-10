using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTInteger : ASTNode
    {
        public ASTInteger(Position position) : base(position)
        {
        }

        public long Value { get; set; }
    }
}