using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTFloat : ASTNode
    {
        public ASTFloat(Position position) : base(position)
        {
        }

        public double Value { get; set; }
    }
}