using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  partial class ForNode : OperatorNode, IStatementMarker
  {
    public string LabelForBegin = null;
    public string LabelForEnd = null;
  }
}
