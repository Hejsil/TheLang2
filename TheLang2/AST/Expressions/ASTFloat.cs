using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTFloat : ASTNode
    {
        public ASTFloat(Position position) 
            : base(position)
        { }

        public double Value { get; set; }
    }
}