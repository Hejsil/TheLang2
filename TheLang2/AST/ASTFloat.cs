using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTFloat : ASTNode
    {
        public ASTFloat(Position position) : base(position)
        {
        }

        public double Value { get; set; }
    }
}