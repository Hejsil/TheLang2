using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheLang.Syntax;
using TheLang2.AST;

namespace TheLang2.Syntax
{
    public class Parser : Scanner
    {
        public static readonly Dictionary<Type, OpInfo> OperatorInfo =
            new Dictionary<Type, OpInfo>
            {
                { typeof(ASTParenthese), new OpInfo(int.MinValue, Associativity.LeftToRight) },

                { typeof(ASTDot), new OpInfo(0, Associativity.LeftToRight) },

                { typeof(ASTCall), new OpInfo(1, Associativity.LeftToRight) },
                { typeof(ASTIndexing), new OpInfo(1, Associativity.LeftToRight) },
                
                { typeof(ASTNot), new OpInfo(2, Associativity.RightToLeft) },

                { typeof(ASTTimes), new OpInfo(4, Associativity.LeftToRight) },
                { typeof(ASTDivide), new OpInfo(4, Associativity.LeftToRight) },
                { typeof(ASTModulo), new OpInfo(4, Associativity.LeftToRight) },

                { typeof(ASTAdd), new OpInfo(5, Associativity.LeftToRight) },
                { typeof(ASTSub), new OpInfo(5, Associativity.LeftToRight) },

                { typeof(ASTLessThan), new OpInfo(6, Associativity.LeftToRight) },
                { typeof(ASTLessThanEqual), new OpInfo(6, Associativity.LeftToRight) },
                { typeof(ASTGreaterThan), new OpInfo(6, Associativity.LeftToRight) },
                { typeof(ASTGreaterThanEqual), new OpInfo(6, Associativity.LeftToRight) },

                { typeof(ASTEqual), new OpInfo(7, Associativity.LeftToRight) },
                { typeof(ASTNotEqual), new OpInfo(7, Associativity.LeftToRight) },

                { typeof(ASTAnd), new OpInfo(8, Associativity.LeftToRight) },
                { typeof(ASTOr), new OpInfo(8, Associativity.LeftToRight) },
            };

        public Parser(string fileName)
            : base(fileName)
        {
        }

        public Parser(TextReader stream)
            : base(stream)
        {
        }

        public ASTFile ParseFile()
        {
            var nodes = new List<ASTNode>();
            var startToken = PeekToken();

            while (!EatToken(TokenKind.EndOfFile))
            {
                var node = ParseDeclarationOrProcedure();
                if (node == null)
                    return null;

                nodes.Add(node);
            }
            
            return new ASTFile(startToken.Position) { Nodes = nodes };
        }

        private ASTProcedure ParseProcedure()
        {
            if (!EatToken(TokenKind.Identifier))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var identifier = EatenToken;

            // Eat the two next colons
            if (!EatToken(TokenKind.Colon) || !EatToken(TokenKind.Colon))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            if (!EatToken(TokenKind.ParenthesesLeft))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var arguments = new List<ASTSymbol>();
            var first = true;

            while (!EatToken(TokenKind.ParenthesesRight))
            {
                if (!first && !EatToken(TokenKind.Comma))
                {
                    var peek = PeekToken();
                    // TODO: Error
                    return null;
                }

                if (!EatToken(TokenKind.Identifier))
                {
                    var peek = PeekToken();
                    // TODO: Error
                    return null;
                }

                arguments.Add(new ASTSymbol(EatenToken.Position) { Name = EatenToken.Value });

                first = false;
            }

            if (!EatToken(TokenKind.Arrow))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var scope = ParseScope();

            if (scope == null)
                return null;

            return new ASTProcedure(identifier.Position)
            {
                Name = identifier.Value,
                Arguments = arguments,
                Scope = scope
            };
        }

        private ASTScope ParseScope()
        {
            if (!EatToken(TokenKind.CurlyLeft))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var top = EatenToken;

            var statements = new List<ASTNode>();

            while (!EatToken(TokenKind.CurlyRight))
            {
                var statement = ParseStatement();
                if (statement == null)
                    return null;

                statements.Add(statement);
            }

            return new ASTScope(top.Position) { Statements = statements };
        }

        private ASTNode ParseDeclarationOrProcedure()
        {
            if (PeekTokenIs(TokenKind.Identifier) && PeekTokenIs(TokenKind.Colon, 1))
                return PeekTokenIs(TokenKind.Colon, 2) ? ParseProcedure() : ParseDeclaration();

            var peek = PeekToken();
            // TODO: Error
            return null;
        }

        private ASTNode ParseStatement()
        {
            var peek = PeekToken();

            switch (peek.Kind)
            {
                case TokenKind.Identifier:
                    return ParseDeclarationOrProcedure();

                case TokenKind.KeywordIf:
                    return ParseIfElseSequence();

                case TokenKind.KeywordLoop:
                    return ParseLoop();

                case TokenKind.KeywordBreak:
                    EatToken();
                    return new ASTBreak(EatenToken.Position);

                case TokenKind.KeywordContinue:
                    EatToken();
                    return new ASTContinue(EatenToken.Position);

                case TokenKind.KeywordNew:
                {
                    EatToken();
                    var expr = ParseExpression();
                    return expr != null ? new ASTDelete(EatenToken.Position) { Expression = expr } : null;
                }

                case TokenKind.KeywordDelete:
                {
                    EatToken();
                    var expr = ParseExpression();
                    return expr != null ? new ASTDelete(EatenToken.Position) { Expression = expr } : null;
                }

                default:
                    // TODO: Error
                    return null;
            }
        }

        private ASTNode ParseCase()
        {
            if (!EatToken(TokenKind.KeywordCase))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var expr = ParseExpression();
            if (expr == null)
                return null;

            var scope = ParseScope();
            if (scope == null)
                return null;

            return new ASTCase(EatenToken.Position) { Expression = expr, Scope = scope };
        }

        private ASTNode ParseLoop()
        {
            if (!EatToken(TokenKind.KeywordLoop))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var top = EatenToken;

            ASTNode expr = null;
            if (!PeekTokenIs(TokenKind.CurlyLeft))
            {
                expr = ParseExpression();
                if (expr == null)
                    return null;
            }

            // TODO: Parse payload

            var scope = ParseScope();
            if (scope == null)
                return null;

            return new ASTLoop(top.Position) { Condition = expr, Scope = scope };
        }

        private ASTNode ParseIfElseSequence()
        {
            var top = PeekToken();
            var ifs = new List<ASTBaseIf>();

            do
            {
                if (!EatToken(TokenKind.KeywordIf))
                {
                    var peek = PeekToken();
                    // TODO: Error
                    return null;
                }

                var ifTop = EatenToken;

                var condition = ParseExpression();
                if (condition == null)
                    return null;

                // TODO: Parse payload

                // Parse switch
                if (PeekTokenIs(TokenKind.CurlyLeft) && PeekTokenIs(TokenKind.KeywordCase, 1))
                {
                    var cases = new List<ASTCase>();

                    EatToken();

                    while (!EatToken(TokenKind.CurlyRight))
                    {
                        if (!EatToken(TokenKind.KeywordCase))
                        {
                            var peek = PeekToken();
                            // TODO: Error
                            return null;
                        }

                        var caseTop = EatenToken;

                        ASTNode expr = null;
                        if (!PeekTokenIs(TokenKind.CurlyLeft))
                        {
                            expr = ParseExpression();
                            if (expr == null)
                                return null;
                        }

                        var scope = ParseScope();
                        if (scope == null)
                            return null;

                        cases.Add(new ASTCase(caseTop.Position) { Expression = expr, Scope = scope });
                    }

                    ifs.Add(new ASTSwitch(ifTop.Position) { Condition = condition, Cases = cases });
                }
                else
                {
                    var scope = ParseScope();
                    if (scope == null)
                        return null;

                    ifs.Add(new ASTIf(ifTop.Position) { Condition = condition, Scope = scope });
                }
            } while (EatToken(TokenKind.KeywordElse) && PeekTokenIs(TokenKind.KeywordIf));


            if (EatenToken.Kind != TokenKind.KeywordElse)
                return new ASTIfElseSequence(top.Position) { Ifs = ifs, FinalElse = null };

            {
                var scope = ParseScope();
                if (scope == null)
                    return null;

                return new ASTIfElseSequence(top.Position) { Ifs = ifs, FinalElse = scope };
            }
        }

        private ASTNode ParseDeclaration()
        {
            if (!EatToken(TokenKind.Identifier))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var identifier = EatenToken;

            // Eat the two next colons
            if (!EatToken(TokenKind.Colon))
            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }

            var expr = ParseExpression();
            if (expr == null)
                return null;

            return new ASTDeclaration(identifier.Position) { Name = identifier.Value, Value = expr };
        }

        private ASTNode ParseExpression()
        {
            var top = ParseUnary();
            if (top == null)
                return null;

            // TODO: This is close, but no quite correct
            while (EatToken(IsBinaryOperator))
            {
                var op = MakeBinaryOperator(EatenToken);

                var right = ParseUnary();
                if (right == null)
                    return null;

                OpInfo opInfo;
                if (!OperatorInfo.TryGetValue(op.GetType(), out opInfo))
                {
                    // TODO: Better error
                    return null;
                }

                ASTNode prev = null;
                var current = top;
                OpInfo currentInfo;

                // The loop that ensures that the operators upholds their priority.
                while (OperatorInfo.TryGetValue(current.GetType(), out currentInfo) &&
                       opInfo.Priority <= currentInfo.Priority)
                {
                    var unary = current as ASTUnary;
                    var binary = current as ASTBinary;

                    if (unary != null)
                    {
                        if (opInfo.Priority == currentInfo.Priority)
                            break;

                        prev = current;
                        current = unary.Child;
                    }
                    else if ((opInfo.Priority != currentInfo.Priority ||
                              opInfo.Associativity == Associativity.RightToLeft) &&
                             binary != null)
                    {
                        prev = current;
                        current = binary.Right;
                    }
                    else
                    {
                        break;
                    }
                }

                op.Right = right;
                op.Left = current;

                {
                    var unary = prev as ASTUnary;
                    var binary = prev as ASTBinary;

                    if (unary != null)
                        unary.Child = op;
                    else if (binary != null)
                        binary.Right = op;
                    else
                        prev = op;
                }

                top = prev;
            }

            return top;
        }

        private ASTNode ParseUnary()
        {
            ASTUnary topPrefix = null, leafPrefix = null;

            for (;;)
            {
                ASTUnary res;

                if (PeekTokenIs(TokenKind.ExclamationMark))
                {
                    EatToken();
                    res = new ASTNot(PeekToken().Position);
                }
                else
                {
                    break;
                }

                if (topPrefix == null)
                {
                    topPrefix = res;
                    leafPrefix = res;
                }
                else
                {
                    leafPrefix.Child = res;
                    leafPrefix = res;
                }
            }


            var result = ParseTerm();
            if (result == null)
                return null;


            ASTUnary topPostfix = null, leafPostfix = null;

            for (;;)
            {
                ASTUnary res;

                if (EatToken(TokenKind.SquareLeft))
                {
                    var expr = ParseExpression();
                    if (expr == null)
                        return null;

                    if (!EatToken(TokenKind.SquareRight))
                    {
                        var peek = PeekToken();
                        // TODO: Error
                        return null;
                    }

                    res = new ASTIndexing(PeekToken().Position) { Value = expr };
                }
                else if (EatToken(TokenKind.ParenthesesLeft))
                {
                    var start = EatenToken;

                    var arguments = new List<ASTNode>();
                    while (!EatToken(TokenKind.ParenthesesRight))
                    {
                        if (arguments.Count != 0 && !EatToken(TokenKind.Comma))
                        {
                            var peek = PeekToken();
                            // TODO: Error
                            return null;
                        }

                        var argument = ParseExpression();
                        if (argument == null)
                            return null;

                        arguments.Add(argument);
                    }

                    res = new ASTCall(start.Position) { Arguments = arguments };
                }
                else
                {
                    break;
                }

                if (topPostfix == null)
                {
                    leafPostfix = res;
                    topPostfix = res;
                }
                else
                {
                    res.Child = topPostfix;
                    topPostfix = res;
                }
            }

            if (leafPostfix != null)
            {
                leafPostfix.Child = result;
                result = topPostfix;
            }

            if (leafPrefix != null)
            {
                leafPrefix.Child = result;
                result = topPrefix;
            }

            return result;
        }

        private ASTNode ParseTerm()
        {
            if (EatToken(TokenKind.String))
                return new ASTString(EatenToken.Position) { Value = EatenToken.Value };

            if (EatToken(TokenKind.DecimalNumber))
                return new ASTInteger(EatenToken.Position) { Value = long.Parse(EatenToken.Value) };

            if (EatToken(TokenKind.FloatNumber))
                return new ASTFloat(EatenToken.Position) { Value = double.Parse(EatenToken.Value) };

            if (EatToken(TokenKind.Identifier))
                return new ASTSymbol(EatenToken.Position) { Name = EatenToken.Value };

            if (EatToken(TokenKind.KeywordStruct))
            {
                var top = EatenToken;

                if (!EatToken(TokenKind.ParenthesesLeft))
                {
                    var peek = PeekToken();
                    // TODO: Error
                    return null;
                }

                var fields = new List<ASTStruct.Field>();

                var first = true;
                while (!EatToken(TokenKind.ParenthesesRight))
                {
                    if (!first && !EatToken(TokenKind.Comma))
                    {
                        var peek = PeekToken();
                        // TODO: Error
                        return null;
                    }

                    if (!EatToken(TokenKind.Identifier))
                    {
                        var peek = PeekToken();
                        // TODO: Error
                        return null;
                    }

                    var identifier = EatenToken;
                    
                    if (!EatToken(TokenKind.Colon))
                    {
                        var peek = PeekToken();
                        // TODO: Error
                        return null;
                    }

                    var expr = ParseExpression();
                    if (expr == null)
                        return null;

                    fields.Add(new ASTStruct.Field(identifier.Position) { Name = identifier.Value, Value = expr });
                    first = false;
                }

                return new ASTStruct(top.Position) { Fields = fields };
            }

            if (EatToken(TokenKind.ParenthesesLeft))
            {
                var top = EatenToken;

                var expr = ParseExpression();
                if (expr == null)
                    return null;

                if (!EatToken(TokenKind.ParenthesesRight))
                {
                    var peek = PeekToken();
                    // TODO: Error
                    return null;
                }

                return new ASTParenthese(top.Position) { Child = expr };
            }

            {
                var peek = PeekToken();
                // TODO: Error
                return null;
            }
        }

        private static ASTBinary MakeBinaryOperator(Token token)
        {
            var kind = token.Kind;
            var pos = token.Position;
            switch (kind)
            {
                case TokenKind.Plus:
                    return new ASTAdd(pos);
                case TokenKind.Minus:
                    return new ASTSub(pos);
                case TokenKind.Times:
                    return new ASTTimes(pos);
                case TokenKind.Divide:
                    return new ASTDivide(pos);
                case TokenKind.EqualEqual:
                    return new ASTEqual(pos);
                case TokenKind.Modulo:
                    return new ASTModulo(pos);
                case TokenKind.LessThan:
                    return new ASTLessThan(pos);
                case TokenKind.LessThanEqual:
                    return new ASTLessThanEqual(pos);
                case TokenKind.GreaterThan:
                    return new ASTGreaterThan(pos);
                case TokenKind.GreaterThanEqual:
                    return new ASTGreaterThanEqual(pos);
                case TokenKind.ExclamationMarkEqual:
                    return new ASTNotEqual(pos);
                case TokenKind.And:
                    return new ASTAnd(pos);
                case TokenKind.Or:
                    return new ASTOr(pos);
                case TokenKind.Dot:
                    return new ASTDot(pos);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static readonly TokenKind[] TokenKinds = (TokenKind[])Enum.GetValues(typeof(TokenKind));
            private static readonly IEnumerable<TokenKind> BinaryOperators =
                TokenKinds
                    .SkipWhile(t => t < TokenKind.Plus)
                    .TakeWhile(t => t <= TokenKind.Dot);

            private static bool IsBinaryOperator(TokenKind kind) => BinaryOperators.Contains(kind);
    }
}