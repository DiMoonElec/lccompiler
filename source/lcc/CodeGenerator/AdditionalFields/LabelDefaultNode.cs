using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  partial class LabelDefaultNode : LabelNode
  {
    /// <summary>
    /// Имя метки текущего default-а
    /// </summary>
    public string LabelDefaultEntry = null;
  }
}
