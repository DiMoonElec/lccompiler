using LC2.LCCompiler.CodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  partial class FunctionCallNode : OperationNode, IExpressionMarker, IStatementMarker
  {
    /// <summary>
    /// Вобъект памяти, в котором храниться возвращенное функцией значение
    /// </summary>
    public LocalMemoryObject ReturnMemoryObject = null;
  }
}
