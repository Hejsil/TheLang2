using System;
using System.Collections.Generic;
using System.Text;
using TheLang2.AST;
using TheLang2.AST.Expressions;
using TheLang2.AST.Statements;
using TheLang2.Semantics.Typechecking.Types;
using TheLang2.Syntax;

namespace TheLang2.Semantics.Typechecking
{
    public class TypeChecker
    {
        public Dictionary<string, List<BaseType[]>> Dictionary { get; set; }

        public bool Visit(dynamic node) => Visit(node);

        public bool Visit(ASTAdd node) { throw new NotImplementedException(); }
        public bool Visit(ASTAnd node) { throw new NotImplementedException(); }
        public bool Visit(ASTArray node) { throw new NotImplementedException(); }
        public bool Visit(ASTCall node) { throw new NotImplementedException(); }
        public bool Visit(ASTDivide node) { throw new NotImplementedException(); }
        public bool Visit(ASTDot node) { throw new NotImplementedException(); }
        public bool Visit(ASTEqual node) { throw new NotImplementedException(); }
        public bool Visit(ASTFloat node) { throw new NotImplementedException(); }
        public bool Visit(ASTGreaterThan node) { throw new NotImplementedException(); }
        public bool Visit(ASTGreaterThanEqual node) { throw new NotImplementedException(); }
        public bool Visit(ASTIndexing node) { throw new NotImplementedException(); }
        public bool Visit(ASTInteger node) { throw new NotImplementedException(); }
        public bool Visit(ASTLessThan node) { throw new NotImplementedException(); }
        public bool Visit(ASTLessThanEqual node) { throw new NotImplementedException(); }
        public bool Visit(ASTModulo node) { throw new NotImplementedException(); }
        public bool Visit(ASTMul node) { throw new NotImplementedException(); }
        public bool Visit(ASTNew node) { throw new NotImplementedException(); }
        public bool Visit(ASTNot node) { throw new NotImplementedException(); }
        public bool Visit(ASTNotEqual node) { throw new NotImplementedException(); }
        public bool Visit(ASTOr node) { throw new NotImplementedException(); }
        public bool Visit(ASTParenthese node) { throw new NotImplementedException(); }
        public bool Visit(ASTString node) { throw new NotImplementedException(); }
        public bool Visit(ASTStruct node) { throw new NotImplementedException(); }
        public bool Visit(ASTSub node) { throw new NotImplementedException(); }
        public bool Visit(ASTSymbol node) { throw new NotImplementedException(); }
        
        public bool Visit(ASTAddAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTBreak node) { throw new NotImplementedException(); }
        public bool Visit(ASTContinue node) { throw new NotImplementedException(); }
        public bool Visit(ASTDeclaration node) { throw new NotImplementedException(); }
        public bool Visit(ASTDelete node) { throw new NotImplementedException(); }
        public bool Visit(ASTDivAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTIf node) { throw new NotImplementedException(); }
        public bool Visit(ASTIfElseSequence node) { throw new NotImplementedException(); }
        public bool Visit(ASTLoop node) { throw new NotImplementedException(); }
        public bool Visit(ASTModAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTMulAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTProcedure node) { throw new NotImplementedException(); }
        public bool Visit(ASTRangeLoop node) { throw new NotImplementedException(); }
        public bool Visit(ASTReturn node) { throw new NotImplementedException(); }
        public bool Visit(ASTScope node) { throw new NotImplementedException(); }
        public bool Visit(ASTSubAssign node) { throw new NotImplementedException(); }
        public bool Visit(ASTSwitch node) { throw new NotImplementedException(); }
        
        public bool Visit(ASTFile node) { throw new NotImplementedException(); }
        public bool Visit(ASTProgram node) { throw new NotImplementedException(); }
    }
}
