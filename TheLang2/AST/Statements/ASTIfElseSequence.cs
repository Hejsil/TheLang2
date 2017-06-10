using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST.Statements
{
    public class ASTIfElseSequence : ASTNode
    {
        public ASTIfElseSequence(Position position) 
            : base(position)
        { }

        public IEnumerable<ASTBaseIf> Ifs { get; set; }
        public ASTScope FinalElse { get; set; }
    }
}