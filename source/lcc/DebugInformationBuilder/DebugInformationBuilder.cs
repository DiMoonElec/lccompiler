using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LC2.DebugInfo
{
  public class DebugInformationBuilder
  {
    List<DebugInformationStatement> StatementsList = new List<DebugInformationStatement>();
    DebugInformationStatement currentStatement;
    string currentModule = null;

    public DebugInformationBuilder()
    {
    }

    public DebugInformation Generate(string AssemblyName,
      string CompilerName,
      string CompilerVersion)
    {
      StatementsList.Sort(delegate (DebugInformationStatement x, DebugInformationStatement y)
      {
        if (x.LowPC > y.LowPC)
          return 1;

        if (x.LowPC < y.LowPC)
          return -1;

        return 0;
      });

      DebugInformation debugInformation = new DebugInformation();

      debugInformation.AssemblyName = AssemblyName;
      debugInformation.CompilerName = CompilerName;
      debugInformation.CompilerVersion = CompilerVersion;

      debugInformation.Statements = StatementsList.ToArray();

      return debugInformation;
    }

    public void ModuleBegin(string Name)
    {
      currentModule = Name;
    }

    public void ModuleEnd()
    {
      currentModule = null;
    }

    public void StatementBegin(uint LowPC, int StartLine, int StartColumn, int EndLine, int EndColumn)
    {
      currentStatement = new DebugInformationStatement();

      currentStatement.LowPC = LowPC;
      currentStatement.StartLine = StartLine;
      currentStatement.StartColumn = StartColumn;
      currentStatement.EndLine = EndLine;
      currentStatement.EndColumn = EndColumn;
      currentStatement.ModuleName = currentModule;
    }

    public void StatementEnd(uint HighPC)
    {
      currentStatement.HighPC = HighPC;
      StatementsList.Add(currentStatement);
    }

  }
}
