using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  partial class LabelCaseNode : LabelNode
  {
    /// <summary>
    /// Имя метки текущего case-а
    /// </summary>
    public string LabelCaseEntry = null;
  }
}
