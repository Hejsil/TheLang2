using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTIf : ASTBaseIf
    {
        public ASTIf(Position position) 
            : base(position)
        { }
        
        public ASTScope Scope { get; set; }
    }
}