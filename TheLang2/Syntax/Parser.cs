using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheLang2.AST;

namespace TheLang2.Syntax
{
    public class Parser : Scanner
    {
        public static readonly Dictionary<Type, OpInfo> OperatorInfo =
            new Dictionary<Type, OpInfo>
            {
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

        private ASTNode ParseExpression()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}