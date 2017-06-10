using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTSymbol : ASTNode
    {
        public ASTSymbol(Position position)
            : base(position)
        { }

        public string Name { get; set; }
    }
}
