using LC2.DebugInfo;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;
using System.Collections.Generic;

namespace LC2.LCCompiler.CodeGenerator
{
  internal class AssemblyUnit
  {
    public List<LCVMAsmItem> SectionCode = new List<LCVMAsmItem>();

    public LocalMemoryAllocator Allocator = new LocalMemoryAllocator();
    public GlobalMemoryAllocator GlobalAllocator = new GlobalMemoryAllocator();
    public LabelManager LabelManager = new LabelManager();

    /// <summary>
    /// Секция кода, которая является инициализатором модуля
    /// </summary>
    public List<LCVMAsmItem> SectionInit = new List<LCVMAsmItem>();

    /*
    /// <summary>
    /// Секция глобальной области данных
    /// </summary>
    public List<LCVMAsmItem> SectionData = new List<LCVMAsmItem>();
    */

    public List<LCVMAsmItem> currentSection = null;

    public string ModuleName { get; private set; }
    public AssemblyUnit(string moduleName)
    {
      ModuleName = moduleName;
    }

    public bool CurrentSectionIsCode()
    {
      return currentSection == SectionCode;
    }

    public void SetCurrentSectionInit()
    {
      currentSection = SectionInit;
    }

    public void SetCurrentSectionCode()
    {
      currentSection = SectionCode;
    }

    public void ModuleBegin(string name)
    {
      SectionCode.Add(new LCVMModuleBegin(name));
    }

    public void ModuleEnd()
    {
      SectionCode.Add(new LCVMModuleEnd());
    }

    public void FunctionBegin(string name)
    {
      currentSection.Add(new LCVMFunctionBegin(name));
    }

    public void FunctionEnd()
    {
      currentSection.Add(new LCVMFunctionEnd());
    }

    public void StatementBegin(LocateElement locate)
    {
      currentSection.Add(new LCVMStatementBegin(locate.StartLine, locate.StartColumn, locate.EndLine, locate.EndColumn));
    }

    public void StatementEnd()
    {
      currentSection.Add(new LCVMStatementEnd());
    }

    public void AddInstruction(LCVMAsmItem instruction)
    {
      currentSection.Add(instruction);
    }

    public LCVMAsmItem GetLastInstruction()
    {
      for(int i= currentSection.Count-1; i>=0; i--)
      {
        var instr = currentSection[i];
        if (!((instr is LCVMComment) || (instr is LCVMMicromarking)))
          return instr;

      }

      return null;
    }

    /*
    /// <summary>
    /// Получить отчет о модуле в текстовом виде
    /// </summary>
    public string GetReport()
    {
      string r = "";

      //Вывод информации о переменных
      r += Allocator.GetReport();
      r += "\r\n";

      //Вывод ассемблерного листинга
      //секция инициализации
      r += ".init\r\n";
      for (int i = 0; i < SectionInit.Count; i++)
        r += SectionInit[i].ToString() + "\r\n";
      r += "\r\n";

      //секция кода
      r += ".code\r\n";
      for (int i = 0; i < SectionCode.Count; i++)
        r += SectionCode[i].ToString() + "\r\n";

      r += "\r\n";
      return r;
    }
    */

  }

  internal class AssemblyProgram
  {
    //Глобальные переменные
    public GlobalMemoryAllocator GlobalAllocator = new GlobalMemoryAllocator();
    public LabelManager LabelManager = new LabelManager();

    public List<LCVMAsmItem> Code = new List<LCVMAsmItem>();

    //public DebugInformationLine EntryPoint;

    public int VariablesMemoryUsage { get { return GlobalAllocator.MemoryUsage; } }
    public int ProgramMemoryUsage { get; private set; }

    public string MemoryReportTXT = "";

    public void ProgramAllocate()
    {
      int offset = 0;

      for (int i = 0; i < Code.Count; i++)
      {
        var instr = Code[i];
        instr.SetCurrentPosition(offset);
        offset += instr.InstrSize;
      }

      ProgramMemoryUsage = offset;
    }

    public void AddCodeRange(List<LCVMAsmItem> items)
    {
      for (int i = 0; i < items.Count; i++)
      {
        var instr = items[i];
        /*
        if (instr is LCVMInstructionGLoadGStore gStore)
        {
          GlobalAllocator.AddReference(gStore.Reference);
        }
        else if(instr is INSTR_PUSH_4 push4)
        {
          if(push4.PushType == INSTR_PUSH_4.PushElementType.PushArrayReference)
            GlobalAllocator.AddReference(push4.Reference);
        }
        else if (instr is LCVMJmpInstruction jmpInstruction)
        {
          var labelReference = LabelManager.AddReference(
            jmpInstruction.LabelReference.LabelName);

          jmpInstruction.UpdateReference(labelReference);
        }
        else if(instr is INSTR_LABEL instrLabel)
        {
          var label = LabelManager.Declaration(instrLabel.Label.LabelName);
          instr = new INSTR_LABEL(label);
        }
        */

        if (instr is ILCVMInstructionMemoryObjectReference memoryObjectReference)
        {
          GlobalAllocator.AddReference(memoryObjectReference.Reference);
        }
        else if (instr is ILCVMInstructionCodeObjectReference codeObjectReference)
        {
          var labelReference = LabelManager.AddReference(
            codeObjectReference.LabelReference.LabelName);
          codeObjectReference.UpdateReference(labelReference);
        }
        else if(instr is ILCVMInstructionCodeObject codeObject)
        {
          var label = LabelManager.Declaration(codeObject.Label.LabelName);
          instr = new INSTR_LABEL(label);
        }


        Code.Add(instr);
      }
    }

    public string GetReport()
    {
      string r = "";

      r += GlobalAllocator.GetReport();
      r += "\r\n";

      r += MemoryReportTXT;
      r += "\r\n";

      return r;
    }

    public string GetAsm()
    {
      string r = "";
      for (int i = 0; i < Code.Count; i++)
      {
        var e = Code[i];
        if(e is LCVMMicromarking)
        {
          continue;
        }
        else if ((e is LCVMComment) || (e is LCVMMicromarking))
        {
          r += string.Format("          \t{0}\r\n", e.ToString());
        }
        else if (e is INSTR_LABEL)
        {
          r += string.Format("\r\n{0}\r\n", e.ToString());
        }
        else
        {
          r += string.Format("0x{0}\t{1}\r\n", e.CurrentPosition.ToString("X8"), e.ToString());
        }
      }
      r += "\r\n";

      return r;
    }
  }
}
