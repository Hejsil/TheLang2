﻿using System.Collections.Generic;
using TheLang2.Syntax;

namespace TheLang2.AST.Expressions
{
    public class ASTStruct : ASTNode
    {
        public ASTStruct(Position position)
            : base(position)
        { }

        public IEnumerable<Field> Fields { get; set; }

        public class Field
        {
            public Field(Position position) => Position = position;

            public Position Position { get; }
            public string Name { get; set; }
            public ASTNode Value { get; set; }
        }
    }
}
