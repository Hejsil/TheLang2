using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheLang2.AST;
using TheLang2.AST.Expressions;
using TheLang2.AST.Statements;

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

                { typeof(ASTMul), new OpInfo(4, Associativity.LeftToRight) },
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
        
        private readonly Compiler _compiler;

        public Parser(string fileName, Compiler compiler)
            : base(fileName) => _compiler = compiler ?? throw new ArgumentNullException();

        public Parser(TextReader stream, Compiler compiler)
            : base(stream) => _compiler = compiler ?? throw new ArgumentNullException();

        public ASTFile ParseFile()
        {
            var nodes = new List<ASTNode>();
            var startToken = PeekToken();

            while (!EatToken(TokenKind.EndOfFile))
            {
                if (PeekTokenIs(TokenKind.Identifier) && PeekTokenIs(TokenKind.Colon, 1))
                {
                    var node = PeekTokenIs(TokenKind.Colon, 2) ? ParseProcedure() : ParseDeclaration();
                    if (node == null)
                        return null;

                    nodes.Add(node);
                }
                else if (EatToken(TokenKind.Directive))
                {
                    if (EatenToken.Value == "include")
                    {
                        if (EatToken(TokenKind.String))
                        {
                            var str = EatenToken;
                            string fileToAdd;

                            if (Path.IsPathRooted(str.Value))
                            {
                                fileToAdd = str.Value;
                            }
                            else
                            {
                                var directory = new FileInfo(str.Position.FileName).Directory.FullName;
                                fileToAdd = $"{directory}\\{str.Value}";
                            }

                            if (!File.Exists(fileToAdd))
                            {
                                Error($"Files does not exist: \"{str.Value}\"");
                                return null;
                            }

                            _compiler.AddFile(fileToAdd);

                        }
                        else
                        {
                            Error("A string has to perceed the '#include' directive.");
                            return null;
                        }
                    }
                    else
                    {
                        Error("Only '#include' directive can be used in global scope.");
                        return null;
                    }
                }
            }

            return new ASTFile(startToken.Position) { Nodes = nodes };
        }

        private ASTProcedure ParseProcedure()
        {
            if (!EatExpect(TokenKind.Identifier))
                return null;

            var identifier = EatenToken;

            // Eat the two next colons
            if (!EatExpect(TokenKind.Colon) ||
                !EatExpect(TokenKind.Colon) ||
                !EatExpect(TokenKind.ParenthesesLeft))
                return null;

            var arguments = new List<ASTSymbol>();
            var first = true;

            while (!EatToken(TokenKind.ParenthesesRight))
            {
                if (!first && !EatExpect(TokenKind.Comma))
                    return null;

                if (!EatExpect(TokenKind.Identifier))
                    return null;

                arguments.Add(new ASTSymbol(EatenToken.Position) { Name = EatenToken.Value });

                first = false;
            }

            if (!EatExpect(TokenKind.Arrow))
                return null;

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
            if (!EatExpect(TokenKind.CurlyLeft))
                return null;

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

        private ASTNode ParseStatement()
        {
            if (PeekTokenIs(TokenKind.Identifier) && PeekTokenIs(TokenKind.Colon, 1))
                return PeekTokenIs(TokenKind.Colon, 2) ? ParseProcedure() : ParseDeclaration();

            if (PeekTokenIs(TokenKind.KeywordIf))
                return ParseIfElseSequence();

            if (PeekTokenIs(TokenKind.KeywordLoop))
                return ParseLoop();

            if (EatToken(TokenKind.KeywordBreak))
                return new ASTBreak(EatenToken.Position);

            if (EatToken(TokenKind.KeywordContinue))
                return new ASTContinue(EatenToken.Position);

            if (EatToken(TokenKind.KeywordDelete))
            {
                var expr = ParseExpression();
                return expr != null ? new ASTDelete(EatenToken.Position) { Expression = expr } : null;
            }

            if (EatToken(TokenKind.KeywordReturn))
            {
                var expr = ParseExpression();
                return expr != null ? new ASTReturn(EatenToken.Position) { Expression = expr } : null;
            }

            {
                var expr = ParseExpression();
                if (expr == null)
                    return null;

                if (EatToken(TokenKind.Equal))
                    return MakeAssign(expr, pos => new ASTAssign(pos));
                if (EatToken(TokenKind.PlusEqual))
                    return MakeAssign(expr, pos => new ASTAddAssign(pos));
                if (EatToken(TokenKind.MinusEqual))
                    return MakeAssign(expr, pos => new ASTSubAssign(pos));
                if (EatToken(TokenKind.TimesEqual))
                    return MakeAssign(expr, pos => new ASTMulAssign(pos));
                if (EatToken(TokenKind.DivideEqual))
                    return MakeAssign(expr, pos => new ASTDivAssign(pos));
                if (EatToken(TokenKind.ModulusEqual))
                    return MakeAssign(expr, pos => new ASTModAssign(pos));
                
                return expr;
            }

            ASTNode MakeAssign(ASTNode left, Func<Position, ASTBinary> make)
            {
                var right = ParseExpression();
                if (right == null)
                    return null;

                var result = make(left.Position);
                result.Left = left;
                result.Right = right;

                return result;
            }
        }

        private ASTSwitch.Case ParseCase()
        {
            if (!EatExpect(TokenKind.KeywordCase))
                return null;

            var expr = ParseExpression();
            if (expr == null)
                return null;

            var scope = ParseScope();
            if (scope == null)
                return null;

            return new ASTSwitch.Case(EatenToken.Position) { Expression = expr, Scope = scope };
        }

        private ASTNode ParseLoop()
        {
            if (!EatExpect(TokenKind.KeywordLoop))
                return null;

            var top = EatenToken;

            ASTNode expr = null;
            if (!PeekTokenIs(TokenKind.CurlyLeft))
            {
                expr = ParseExpression();
                if (expr == null)
                    return null;
            }

            ASTNode rangeEnd = null;
            if (EatToken(TokenKind.DotDot))
            {
                rangeEnd = ParseExpression();
                if (rangeEnd == null)
                    return null;
            }

            // TODO: Parse payload

            var scope = ParseScope();
            if (scope == null)
                return null;
            
            if (rangeEnd == null)
                return new ASTLoop(top.Position) { Condition = expr, Scope = scope };


            return new ASTRangeLoop(top.Position) { RangeStart = expr, RangeEnd = rangeEnd, Scope = scope };
        }

        private ASTNode ParseIfElseSequence()
        {
            var top = PeekToken();
            var ifs = new List<ASTBaseIf>();

            do
            {
                if (!EatExpect(TokenKind.KeywordIf))
                    return null;

                var ifTop = EatenToken;

                var condition = ParseExpression();
                if (condition == null)
                    return null;

                // TODO: Parse payload

                // Parse switch
                if (PeekTokenIs(TokenKind.CurlyLeft) && PeekTokenIs(TokenKind.KeywordCase, 1))
                {
                    var cases = new List<ASTSwitch.Case>();

                    EatToken();

                    while (!EatToken(TokenKind.CurlyRight))
                    {
                        var cse = ParseCase();
                        if (cse == null)
                            return null;

                        cases.Add(cse);
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
            if (!EatExpect(TokenKind.Identifier))
                return null;

            var identifier = EatenToken;

            // Eat the two next colons
            if (!EatExpect(TokenKind.Colon))
                return null;

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
                    throw new NotImplementedException("Panic");

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

                if (EatToken(TokenKind.ExclamationMark))
                    res = new ASTNot(EatenToken.Position);
                else if (EatToken(TokenKind.KeywordNew))
                    res = new ASTNew(EatenToken.Position);
                else
                    break;

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

                    if (!EatExpect(TokenKind.SquareRight))
                        return null;

                    res = new ASTIndexing(PeekToken().Position) { Value = expr };
                }
                else if (EatToken(TokenKind.ParenthesesLeft))
                {
                    var start = EatenToken;

                    var arguments = new List<ASTNode>();
                    while (!EatToken(TokenKind.ParenthesesRight))
                    {
                        if (arguments.Count != 0 && !EatExpect(TokenKind.Comma))
                            return null;

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

                if (!EatExpect(TokenKind.ParenthesesLeft))
                    return null;

                var fields = new List<ASTStruct.Field>();

                var first = true;
                while (!EatToken(TokenKind.ParenthesesRight))
                {
                    if (!first && !EatExpect(TokenKind.Comma))
                        return null;

                    if (!EatExpect(TokenKind.Identifier))
                        return null;

                    var identifier = EatenToken;

                    if (!EatExpect(TokenKind.Colon))
                        return null;

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

                if (!EatExpect(TokenKind.ParenthesesRight))
                    return null;

                return new ASTParenthese(top.Position) { Child = expr };
            }

            if (EatToken(TokenKind.SquareLeft))
            {
                var top = EatenToken;

                var index = ParseExpression();
                if (index == null)
                    return null;

                if (!EatExpect(TokenKind.SquareRight) || !EatExpect(TokenKind.CurlyLeft))
                    return null;
                
                var values = new List<ASTNode>();

                var first = true;
                while (!EatToken(TokenKind.CurlyRight))
                {
                    if (!first && !EatExpect(TokenKind.Comma))
                        return null;

                    var expr = ParseExpression();
                    if (expr == null)
                        return null;

                    values.Add(expr);
                    first = false;
                }

                return new ASTArray(top.Position) { Index = index, Values = values };
            }
            
            Error(
                TokenKind.String, TokenKind.DecimalNumber, TokenKind.FloatNumber,
                TokenKind.Identifier, TokenKind.KeywordStruct, TokenKind.ParenthesesLeft
            );
            return null;
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
                    return new ASTMul(pos);
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

        public bool EatExpect(TokenKind kind, string hint = "")
        {
            if (EatToken(kind))
                return true;

            Error(hint, kind);
            return false;
        }

        public bool PeekExpect(TokenKind kind, int offset = 0, string hint = "")
        {
            if (PeekTokenIs(kind, offset))
                return true;

            Error(hint, kind);
            return false;
        }

        public void Error(params TokenKind[] kind) => Error("", kind);
        public void Error(string hint, params TokenKind[] kind)
        {
            var peek = PeekToken();
            Error($"Expected {string.Join(", ", kind)}, but got {peek.Kind}.");

            if (!string.IsNullOrEmpty(hint))
                Console.WriteLine($"Hint: {hint}");
        }

        public void Error(string message)
        {
            var peek = PeekToken();
            var position = peek.Position;

            Console.WriteLine($"{position.FileName}:{position.Line}:{position.Column}: Error: {message}");

            throw new NotImplementedException();
        }
        
        private static readonly TokenKind[] TokenKinds = (TokenKind[]) Enum.GetValues(typeof(TokenKind));

        private static readonly IEnumerable<TokenKind> BinaryOperators =
            TokenKinds
                .SkipWhile(t => t < TokenKind.Plus)
                .TakeWhile(t => t <= TokenKind.Dot);

        private static bool IsBinaryOperator(TokenKind kind) => BinaryOperators.Contains(kind);
    }
}