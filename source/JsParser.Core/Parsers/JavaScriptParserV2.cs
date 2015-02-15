using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint.Parser;
using Jint.Parser.Ast;
using JsParser.Core.Code;
using JsParser.Core.Helpers;

namespace JsParser.Core.Parsers
{
    public class JavaScriptParserV2: IJavascriptParser
    {
        public JSParserResult Parse(string code)
        {
            var jsp = new Jint.Parser.JavaScriptParser();
            var returnedResult = new JSParserResult();

            Program program = null;
            try
            {
                program = jsp.Parse(code);
            }
            catch (ParserException pex)
            {
                returnedResult.Errors.Add(new ErrorMessage()
                {
                    Message = pex.Message,
                    StartColumn = pex.Column,
                    StartLine = pex.LineNumber
                });
            }
            catch (Exception ex)
            {
                returnedResult.InternalErrors.Add(new ErrorMessage()
                {
                    Message = ex.Message,
                    StartColumn = 1,
                    StartLine = 1
                });
            }

            if (program == null)
            {
                return returnedResult;
            }

            ProcessStatements(program.Body, returnedResult.Nodes);

            return returnedResult;
        }

        private void ProcessStatements(IEnumerable<Statement> statements, Hierachy<CodeNode> currentNode)
        {
            foreach (var statement in statements)
            {
                ProcessStatement(statement, currentNode);
            }
        }

        private void ProcessStatement(Statement statement, Hierachy<CodeNode> nodes)
        {
            if (statement is FunctionDeclaration)
            {
                ProcessFunctionDefinition(nodes, statement.As<FunctionDeclaration>(), null);
                return;
            }

            if (statement is VariableDeclaration)
            {
                ProcessVariableDefinition(nodes, statement.As<VariableDeclaration>());
            }

            if (statement is ExpressionStatement)
            {
                ProcessExpression(nodes, statement.As<ExpressionStatement>().Expression, null);
            }
        }

        private CodeNode ProcessVariableDefinition(
            Hierachy<CodeNode> nodes,
            VariableDeclaration variableDeclaration)
        {
            foreach (var variable in variableDeclaration.Declarations)
            {
                if (variable.Init != null)
                {
                    if (variable.Init is FunctionExpression ||
                        variable.Init is ObjectExpression)
                    {/*
                        var codeNode = new CodeNode
                        {
                            Alias = variable.Id.Name,
                            AliasType = NodeAliasType.Variable,
                            Opcode = "Variable",
                            StartLine = variableDeclaration.Location.Start.Line,
                            StartColumn = variableDeclaration.Location.Start.Column,
                            EndLine = variableDeclaration.Location.End.Line,
                            EndColumn = variableDeclaration.Location.End.Column,
                            //Comment = _comments.GetComment(variableDeclaration.Location.StartLine, variableDeclaration.Location.EndLine)
                        };

                        Hierachy<CodeNode> hi = nodes.Add(codeNode);
                        */
                        ProcessExpression(nodes, variable.Init, new NodeAlias(variable.Id.Name));
                    }
                }
            }

            return nodes.Item;
        }

        private NodeAlias ProcessFunctionDefinition(
            Hierachy<CodeNode> nodes,
            IFunctionDeclaration function,
            NodeAlias functionName)
        {
            if (functionName == null)
            {
                var name = function.Id != null
                    ? function.Id.Name
                    : string.Empty;
                functionName = new NodeAlias(name);
            }

            var pars = string.Join(
                ",",
                (function.Parameters)
                .Select(p => p.Name)
                .ToArray());
            //pars = pars.Shortenize(_settings.MaxParametersLength);

            var syntaxNode = (SyntaxNode) function;

            var codeNode = new CodeNode
            {
                Alias = functionName.GetFullText() + "(" + pars + ")",
                AliasType = functionName.Type,
                Opcode = "Function",
                StartLine = syntaxNode.Location.Start.Line,
                StartColumn = syntaxNode.Location.Start.Column,
                EndLine = syntaxNode.Location.End.Line,
                EndColumn = syntaxNode.Location.End.Column,
                //Comment = _comments.GetComment(function.Location.StartLine, function.Location.EndLine)
            };
            
            if (function.Body is BlockStatement)
            {
                //Go to recursion
                Hierachy<CodeNode> hi = nodes.Add(codeNode);
                ProcessStatements(function.Body.As<BlockStatement>().Body, hi);
            }
            return new NodeAlias(codeNode.Alias);
        }

        private NodeAlias ProcessExpression(Hierachy<CodeNode> nodes, Expression exp, NodeAlias expressionAlias)
        {
            if (exp == null)
            {
                return null;
            }

            if (exp is BinaryExpression)
            {
                var bexp = exp.As<BinaryExpression>();

                var alias = ProcessExpression(nodes, bexp.Left, expressionAlias);

                var falias = bexp.Operator == BinaryOperator.Equal ? expressionAlias.Concat(alias) : expressionAlias;

                ProcessExpression(nodes, bexp.Right, falias);
                return alias;
            }

            if (exp is CallExpression)
            {
                var invexp = exp.As<CallExpression>();
                var alias = ProcessExpression(nodes, invexp.Callee, expressionAlias);

                if (invexp.Arguments.Any())
                {
                    var args = StringifyArguments(invexp.Arguments);
                    if (!string.IsNullOrEmpty(args))
                    {
                        //args = args.Shortenize(_settings.MaxParametersLengthInFunctionChain);
                        alias.Text += "(" + args + ")";
                    }
                }

                foreach (var arg in invexp.Arguments)
                {
                    var argAlias = GetAliasForExpr(arg);
                    var itemAlias = expressionAlias.Concat(alias.Concat(argAlias));

                    ProcessExpression(nodes, arg, itemAlias);
                }

                return alias;
            }

            if (exp is UnaryExpression)
            {
                var uexp = (UnaryExpression)exp;
                var opAlias = GetAliasForExpr(uexp.Argument);
                var alias = ProcessExpression(nodes, uexp.Argument, expressionAlias.Concat(opAlias));
                return alias;
            }

            if (exp is IFunctionDeclaration)
            {
                var alias = ProcessFunctionDefinition(nodes, (IFunctionDeclaration)exp, expressionAlias);
                return alias;
            }

            if (exp is ObjectExpression)
            {
                var ojexp = (ObjectExpression)exp;

                if (expressionAlias == null)
                {
                    expressionAlias = new NodeAlias(string.Empty);
                }

                var codeNode = new CodeNode
                {
                    Alias = expressionAlias.GetFullText(),
                    AliasType = expressionAlias.Type,
                    Opcode = "Object",
                    StartLine = exp.Location.Start.Line,
                    StartColumn = exp.Location.Start.Column,
                    EndLine = exp.Location.End.Line,
                    EndColumn = exp.Location.End.Column,
                    //Comment = _comments.GetComment(exp.Location.StartLine, exp.Location.EndLine)
                };
                Hierachy<CodeNode> hi = nodes.Add(codeNode);

                foreach (var element in ojexp.Properties)
                {
                    var alias = ProcessExpression(hi, (Expression)element.Key, expressionAlias);
                    ProcessExpression(hi, element.Value, alias);
                }

                return expressionAlias;
            }

            if (exp is AssignmentExpression)
            {
                var qexp = (AssignmentExpression)exp;
                var alias = ProcessExpression(nodes, qexp.Left, expressionAlias);
                var basealias = ProcessExpression(nodes, qexp.Right, expressionAlias);
                return basealias.Concat(alias);
            }

            if (exp is Identifier)
            {
                // Just return the alias.
                var iexp = (Identifier)exp;
                return new NodeAlias(iexp.Name);
            }

            if (exp is Literal)
            {
                var slexp = (Literal)exp;
                return new NodeAlias(slexp.Raw);
            }

            return new NodeAlias("$");
        }

        private string StringifyArguments(IEnumerable<Expression> list)
        {
            return string.Join(",", list.Select(a => StringifyArgument(a)).Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }

        private string StringifyArgument(Expression exp)
        {
            if (exp is Identifier)
            {
                var iexp = (Identifier)exp;
                return iexp.Name;
            }

            if (exp is Literal)
            {
                var sexp = (Literal)exp;
                return sexp.Raw;
            }
            /*
            if (exp is NumericLiteralExpression)
            {
                var nexp = (NumericLiteralExpression)exp;
                return nexp.Spelling;
            }
            */
            return null;
        }

        private NodeAlias GetAliasForExpr(Expression expr)
        {
            var aliasStr = "?";
            // Try to find name of the function passed as parameter - name of anonimous function
            if (/*expr.Opcode == Expression.Operation.Function && */expr is FunctionExpression)
            {
                var fexp = expr.As<FunctionExpression>();
                if (fexp.Id != null)
                {
                    var fn = fexp.Id.Name;
                    if (!string.IsNullOrEmpty(fn))
                    {
                        aliasStr = fn;
                    }
                }
            }

            return new NodeAlias(aliasStr, aliasStr == "?" ? NodeAliasType.Anonymous : NodeAliasType.Unknown);
        }
    }
}
