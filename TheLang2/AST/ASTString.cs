using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTString : ASTNode
    {
        public ASTString(Position position) : base(position)
        {
        }

        public string Value { get; set; }
    }
}