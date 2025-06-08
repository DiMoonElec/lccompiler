using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler
{
  internal interface ILocateElement
  {
    LocateElement Locate { get; }

    LocateElement ExpressionLocate { get; }
  }
}
