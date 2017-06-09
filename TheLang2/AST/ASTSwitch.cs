using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    internal class ASTSwitch : ASTBaseIf
    {
        public ASTSwitch(Position position) 
            : base(position)
        { }

        public List<ASTCase> Cases { get; set; }
    }
}