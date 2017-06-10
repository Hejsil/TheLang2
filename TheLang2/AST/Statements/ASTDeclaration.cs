using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTDeclaration : ASTNode
    {
        public ASTDeclaration(Position position) 
            : base(position)
        { }

        public string Name { get; set; }
        public ASTNode Value { get; set; }
    }
}