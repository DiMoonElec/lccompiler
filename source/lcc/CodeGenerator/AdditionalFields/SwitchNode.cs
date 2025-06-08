using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  partial class SwitchNode : OperatorNode, IStatementMarker
  {
    public string LabelSwitchEnd = null;
  }
}
