using TheLang2.AST;

namespace TheLang2.Syntax
{
    internal class ASTString : ASTNode
    {
        public ASTString(Position position) : base(position)
        {
        }

        public string Value { get; set; }
    }
}