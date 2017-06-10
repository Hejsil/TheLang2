using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.Syntax;

namespace TheLang2.AST
{
    public class ASTProgram : ASTNode
    {
        public ASTProgram()
            : base(null)
        { }

        public IEnumerable<ASTFile> Files { get; set; }
    }
}
