using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  enum ResultAccessMethod
  {
    /// <summary>
    /// Чтение значения
    /// </summary>
    MethodGet,

    /// <summary>
    /// Положить на стек адрес объекта
    /// </summary>
    MethodGetReference,

    /// <summary>
    /// Не используется вышестоящей нодой
    /// </summary>
    MethodUnuse
  };

  abstract partial class TypedNode : SourceLocatedNode
  {
    public ResultAccessMethod AccessMethod = ResultAccessMethod.MethodUnuse;
  }
}
