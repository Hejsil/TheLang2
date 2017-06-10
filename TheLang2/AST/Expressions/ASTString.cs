using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTString : ASTNode
    {
        public ASTString(Position position) 
            : base(position)
        { }

        public string Value { get; set; }
    }
}