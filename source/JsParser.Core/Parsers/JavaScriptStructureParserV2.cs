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
    public class JavascriptStructureParserV2
    {
        private CommentsAgregator _comments;
        private JavascriptParserSettings _settings;

        public JavascriptStructureParserV2(JavascriptParserSettings settings)
        {
            _settings = settings;
        }

        public JSParserResult Parse(string code)
        {
            var jsp = new Jint.Parser.JavaScriptParser();
            var returnedResult = new JSParserResult();
            string serialized;

            Program program = null;
            try
            {
                program = jsp.Parse(code, new ParserOptions()
                {
                    Tokens = true,
                    Comment = true,
                });
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

            var comments = (program.Comments ?? Enumerable.Empty<Comment>())
                .Select(c => new CommentWrapper(c));

            _comments = new CommentsAgregator();
            _comments.ProcessComments(comments);

            ProcessStatements(program.Body, new ParserContext(returnedResult.Nodes));

            returnedResult.TaskList = TaskListAggregator.GetTaskList(_comments.Comments, _settings.ToDoKeyWords).ToList();
            
            return returnedResult;
        }

        private void ProcessStatements(IEnumerable<Statement> statements, ParserContext context)
        {
            foreach (var statement in statements)
            {
                ProcessStatement(statement, context);
            }
        }

        private void ProcessStatement(Statement statement, ParserContext context)
        {
            context = new ParserContext(context.Nodes);

            if (statement is FunctionDeclaration)
            {
                ProcessFunctionDeclaration(statement.As<FunctionDeclaration>(), context);
                return;
            }

            if (statement is VariableDeclaration)
            {
                ProcessVariableDeclaration(statement.As<VariableDeclaration>(), context);
                return;
            }

            if (statement is ExpressionStatement)
            {
                ProcessExpression(statement.As<ExpressionStatement>().Expression, context);
                return;
            }

            if (statement is ReturnStatement)
            {
                ProcessExpression(statement.As<ReturnStatement>().Argument, context);
                return;
            }

            if (statement is BlockStatement)
            {
                ProcessStatements(statement.As<BlockStatement>().Body, context);
            }

            if (statement is IfStatement)
            {
                ProcessExpression(statement.As<IfStatement>().Test, context);
                ProcessStatement(statement.As<IfStatement>().Consequent, context);
                ProcessStatement(statement.As<IfStatement>().Alternate, context);
            }

            if (statement is TryStatement)
            {
                ProcessStatement(statement.As<TryStatement>().Block, context);
                ProcessStatement(statement.As<TryStatement>().Finalizer, context);
                ProcessStatements(statement.As<TryStatement>().Handlers.OfType<Statement>(), context);
                ProcessStatements(statement.As<TryStatement>().GuardedHandlers, context);
            }

            if (statement is CatchClause)
            {
                ProcessStatement(statement.As<CatchClause>().Body, context);
            }

            if (statement is SwitchStatement)
            {
                ProcessExpression(statement.As<SwitchStatement>().Discriminant, context);
                ProcessStatements(statement.As<SwitchStatement>().Cases.SelectMany(c => c.Consequent), context);
            }
        }

        private void ProcessVariableDeclaration(
            VariableDeclaration variableDeclaration,
            ParserContext context)
        {
            foreach (var variable in variableDeclaration.Declarations)
            {
                if (variable.Init != null)
                {
                    var childContext = new ParserContext(context);

                    ProcessExpression(variable.Id, childContext);
                    ProcessExpression(variable.Init, childContext);
                }
            }
        }

        private void ProcessFunctionDeclaration(
            IFunctionDeclaration function,
            ParserContext context)
        {
            string name;
            bool isAnonymous = false;
            if (function.Id != null)
            {
                name = function.Id.Name;
            }
            else
            {
                name = context.GetNameFromStack();
                isAnonymous = name.EndsWith("?");
            }
            
            var pars = string.Join(",",
                function.Parameters.Select(p => p.Name).ToArray());
            pars = pars.Shortenize(_settings.MaxParametersLength);

            var syntaxNode = (SyntaxNode) function;

            var codeNode = new CodeNode
            {
                Alias = name + "(" + pars + ")",
                NodeType = isAnonymous ? CodeNodeType.AnonymousFunction : CodeNodeType.Function,
                StartLine = syntaxNode.Location.Start.Line,
                StartColumn = syntaxNode.Location.Start.Column,
                EndLine = syntaxNode.Location.End.Line,
                EndColumn = syntaxNode.Location.End.Column,
                Comment = _comments.GetComment(syntaxNode.Location.Start.Line, syntaxNode.Location.End.Line)
            };

            Hierarchy<CodeNode> hi = context.Nodes.Add(codeNode);
            ProcessStatement(function.Body, new ParserContext(hi));
        }

        private void ProcessExpression(Expression exp, ParserContext context)
        {
            if (exp == null)
            {
                return;
            }

            if (exp is BinaryExpression)
            {
                var bexp = exp.As<BinaryExpression>();

                ProcessExpression(bexp.Left, context);

                ProcessExpression(bexp.Right, context);
                return;
            }

            if (exp is CallExpression)
            {
                var invexp = exp.As<CallExpression>();
                ProcessExpression(invexp.Callee, context);

                foreach (var arg in invexp.Arguments)
                {
                    var cc = new ParserContext(context, copyNames: true);
                    cc.NameStack.Insert(0, "?");
                    ProcessExpression(arg, cc);
                }
                return;
            }

            if (exp is UnaryExpression)
            {
                var uexp = (UnaryExpression)exp;
                ProcessExpression(uexp.Argument, context);
                return;
            }

            if (exp is IFunctionDeclaration)
            {
                ProcessFunctionDeclaration((IFunctionDeclaration)exp, context);
                return;
            }

            if (exp is ObjectExpression)
            {
                var ojexp = (ObjectExpression)exp;

                if (ojexp.Properties.Any())
                {
                    var codeNode = new CodeNode
                    {
                        Alias = context.GetNameFromStack(),
                        NodeType = CodeNodeType.Object,
                        StartLine = exp.Location.Start.Line,
                        StartColumn = exp.Location.Start.Column,
                        EndLine = exp.Location.End.Line,
                        EndColumn = exp.Location.End.Column,
                        Comment = _comments.GetComment(exp.Location.Start.Line, exp.Location.End.Line)
                    };
                    Hierarchy<CodeNode> hi = context.Nodes.Add(codeNode);

                    foreach (var element in ojexp.Properties)
                    {
                        var childContext = new ParserContext(hi);
                        ProcessExpression((Expression)element.Key, childContext);
                        ProcessExpression(element.Value, childContext);
                    }
                }

                return;
            }

            if (exp is AssignmentExpression)
            {
                var qexp = (AssignmentExpression)exp;
                ProcessExpression(qexp.Left, context);
                ProcessExpression(qexp.Right, context);
                return;
            }

            if (exp is Identifier)
            {
                var iexp = (Identifier)exp;
                context.NameStack.Push(iexp.Name);
                return;
            }

            if (exp is Literal)
            {
                var slexp = (Literal)exp;
                context.NameStack.Push(slexp.Raw);
                return;
            }

            if (exp is MemberExpression)
            {
                var mexp = exp.As<MemberExpression>();
                ProcessExpression(mexp.Property, context);
                ProcessExpression(mexp.Object, new ParserContext(context));
                return;
            }

            if (exp is ArrayExpression)
            {
                var arexp = exp.As<ArrayExpression>();
                foreach (var arelt in arexp.Elements)
                {
                    ProcessExpression(arelt, new ParserContext(context));
                }
            }

            if (exp is NewExpression)
            {
                var nexp = exp.As<NewExpression>();
                foreach (var elt in nexp.Arguments)
                {
                    ProcessExpression(elt, new ParserContext(context.Nodes));
                }
            }

            if (exp is LogicalExpression)
            {
                var lexp = exp.As<LogicalExpression>();
                ProcessExpression(lexp.Left, new ParserContext(context, copyNames: true));
                ProcessExpression(lexp.Right, new ParserContext(context, copyNames: true));
            }

            return;
        }
    }
}
