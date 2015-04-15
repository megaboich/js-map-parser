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

        private List<string> _nameStack = new List<string>();

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
                program = jsp.Parse(code);
                //serialized = new JavaScriptSerializer().Serialize(program);
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
                .Select(c => (ICommentWrapper)new JintCommentWrapper(c)).ToList();

            var codeLines = code.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None);
            _comments = new CommentsAgregator(comments, codeLines);

            ProcessStatements(program.Body, returnedResult.Nodes);

            return returnedResult;
        }

        private void ProcessStatements(IEnumerable<Statement> statements, Hierachy<CodeNode> nodes)
        {
            foreach (var statement in statements)
            {
                ProcessStatement(statement, nodes);
            }
        }

        private void ProcessStatement(Statement statement, Hierachy<CodeNode> nodes)
        {
            _nameStack.Clear();

            if (statement is FunctionDeclaration)
            {
                ProcessFunctionDeclaration(statement.As<FunctionDeclaration>(), nodes);
                return;
            }

            if (statement is VariableDeclaration)
            {
                ProcessVariableDeclaration(statement.As<VariableDeclaration>(), nodes);
                return;
            }

            if (statement is ExpressionStatement)
            {
                ProcessExpression(statement.As<ExpressionStatement>().Expression, nodes);
                return;
            }
        }

        private void ProcessVariableDeclaration(
            VariableDeclaration variableDeclaration,
            Hierachy<CodeNode> nodes)
        {
            foreach (var variable in variableDeclaration.Declarations)
            {
                _nameStack.Clear();
                if (variable.Init != null)
                {
                    ProcessExpression(variable.Id, nodes);

                    ProcessExpression(variable.Init, nodes);
                }
            }
        }

        private void ProcessFunctionDeclaration(
            IFunctionDeclaration function,
            Hierachy<CodeNode> nodes)
        {
            var name = function.Id != null
                ? function.Id.Name
                : _nameStack.Count > 0
                    ? string.Join(".", _nameStack.ToArray().Reverse().ToArray())
                    : "?";
                
            var pars = string.Join(
                ",",
                (function.Parameters)
                .Select(p => p.Name)
                .ToArray());
            pars = pars.Shortenize(_settings.MaxParametersLength);

            var syntaxNode = (SyntaxNode) function;

            var codeNode = new CodeNode
            {
                Alias = name + "("+pars+")",
                AliasType = NodeAliasType.FunctionDefinition,
                Opcode = "Function",
                StartLine = syntaxNode.Location.Start.Line,
                StartColumn = syntaxNode.Location.Start.Column,
                EndLine = syntaxNode.Location.End.Line,
                EndColumn = syntaxNode.Location.End.Column,
                Comment = _comments.GetComment(syntaxNode.Location.Start.Line, syntaxNode.Location.End.Line)
            };
            
            if (function.Body is BlockStatement)
            {
                //Go to recursion
                Hierachy<CodeNode> hi = nodes.Add(codeNode);
                ProcessStatements(function.Body.As<BlockStatement>().Body, hi);
            }
        }

        private void ProcessExpression(Expression exp, Hierachy<CodeNode> nodes)
        {
            if (exp == null)
            {
                return;
            }

            if (exp is BinaryExpression)
            {
                var bexp = exp.As<BinaryExpression>();

                ProcessExpression(bexp.Left, nodes);

                ProcessExpression(bexp.Right, nodes);
                return;
            }

            if (exp is CallExpression)
            {
                var invexp = exp.As<CallExpression>();
                ProcessExpression(invexp.Callee, nodes);

                foreach (var arg in invexp.Arguments)
                {
                    ProcessExpression(arg, nodes);
                }
                return;
            }

            if (exp is UnaryExpression)
            {
                var uexp = (UnaryExpression)exp;
                ProcessExpression(uexp.Argument, nodes);
                return;
            }

            if (exp is IFunctionDeclaration)
            {
                ProcessFunctionDeclaration((IFunctionDeclaration)exp, nodes);
                return;
            }

            if (exp is ObjectExpression)
            {
                var ojexp = (ObjectExpression)exp;

                var codeNode = new CodeNode
                {
                    Alias = _nameStack.Count > 0
                        ? string.Join(".", _nameStack.ToArray().Reverse().ToArray())
                        : "?",
                    AliasType = NodeAliasType.Unknown,
                    Opcode = "Object",
                    StartLine = exp.Location.Start.Line,
                    StartColumn = exp.Location.Start.Column,
                    EndLine = exp.Location.End.Line,
                    EndColumn = exp.Location.End.Column,
                    Comment = _comments.GetComment(exp.Location.Start.Line, exp.Location.End.Line)
                };
                Hierachy<CodeNode> hi = nodes.Add(codeNode);

                foreach (var element in ojexp.Properties)
                {
                    _nameStack.Clear();
                    ProcessExpression((Expression)element.Key, hi);
                    ProcessExpression(element.Value, hi);
                }
                return;
            }

            if (exp is AssignmentExpression)
            {
                var qexp = (AssignmentExpression)exp;
                ProcessExpression(qexp.Left, nodes);
                ProcessExpression(qexp.Right, nodes);
                return;
            }

            if (exp is Identifier)
            {
                var iexp = (Identifier)exp;
                _nameStack.Push(iexp.Name);
                return;
            }

            if (exp is Literal)
            {
                var slexp = (Literal)exp;
                _nameStack.Push(slexp.Raw);
                return;
            }

            if (exp is MemberExpression)
            {
                var mexp = exp.As<MemberExpression>();
                ProcessExpression(mexp.Property, nodes);
                ProcessExpression(mexp.Object, nodes);
                return;
            }

            if (exp is ArrayExpression)
            {
                var arexp = exp.As<ArrayExpression>();
                foreach (var arelt in arexp.Elements)
                {
                    ProcessExpression(arelt, nodes);
                }
            }

            return;
        }
    }
}
