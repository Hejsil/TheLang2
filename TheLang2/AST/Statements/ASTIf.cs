using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTIf : ASTBaseIf
    {
        public ASTIf(Position position) 
            : base(position)
        { }
        
        public ASTScope Scope { get; set; }
    }
}