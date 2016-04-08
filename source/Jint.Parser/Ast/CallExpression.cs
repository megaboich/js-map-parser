using Jint.Native;
using System.Collections.Generic;

namespace Jint.Parser.Ast
{
    public class CallExpression : Expression
    {
        public Expression Callee;
        public IList<Expression> Arguments;

    }
}