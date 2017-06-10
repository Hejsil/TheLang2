using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTSwitch : ASTBaseIf
    {
        public ASTSwitch(Position position) 
            : base(position)
        { }

        public List<Case> Cases { get; set; }

        public class Case
        {
            public Case(Position position) => Position = position;

            public Position Position { get; }
            public ASTNode Expression { get; set; }
            public ASTScope Scope { get; set; }
        }
    }
}