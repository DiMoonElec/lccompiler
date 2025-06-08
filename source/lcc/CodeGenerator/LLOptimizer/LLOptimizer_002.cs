using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using System;

namespace LC2.LCCompiler.CodeGenerator
{
  /// <summary>
  /// Данный оптимизатор удаляет мертвый код
  /// </summary>
  static internal class LLOptimizer_002
  {
    public static void Run(AssemblyProgram assemblyProgram)
    {
      Trace(assemblyProgram, 0);
      CutDeadCode(assemblyProgram);
    }


    static void Trace(AssemblyProgram assemblyProgram, int index)
    {
      var prgm = assemblyProgram.Code;
      for (int i = index; i < prgm.Count; i++)
      {
        var instr = prgm[i];

        //if (e is LCVMInstruction instr)
        //{
        //Исключаем повторного обхода кода
        if (instr.LLOptimizer_002_trace == true)
          return;

        instr.LLOptimizer_002_trace = true;

        if (instr is INSTR_HALT)
        {
          return;
        }
        else if (instr is INSTR_RET)
        {
          return;
        }
        else if (instr is LCVMJmpInstruction jmpInstr)
        {
          if ((jmpInstr is INSTR_IFFALSE) || (jmpInstr is INSTR_IFTRUE))
          {
            var adr = jmpInstr.LabelReference.Address;
            var ndx = FindInstrIndexByAddress(assemblyProgram, adr);
            Trace(assemblyProgram, ndx);
          }
          else if (jmpInstr is INSTR_JMP)
          {
            var adr = jmpInstr.LabelReference.Address;
            var ndx = FindInstrIndexByAddress(assemblyProgram, adr);
            Trace(assemblyProgram, ndx);
            return;
          }
          else if (jmpInstr is INSTR_CALL)
          {
            var adr = jmpInstr.LabelReference.Address;
            var ndx = FindInstrIndexByAddress(assemblyProgram, adr);
            Trace(assemblyProgram, ndx);
          }
        }
        //}
      }
    }

    static int FindInstrIndexByAddress(AssemblyProgram assemblyProgram, int adr)
    {
      var prgm = assemblyProgram.Code;
      for (int i = 0; i < prgm.Count; i++)
      {
        if (prgm[i].CurrentPosition == adr)
          return i;
      }

      throw new Exception(string.Format("Инструкция по адресу {0} не найдена", adr));
    }

    static void CutDeadCode(AssemblyProgram assemblyProgram)
    {
      var code = assemblyProgram.Code;
      int count = code.Count;
      bool changed = false;

      for (int i = 0; i < count; i++)
      {
        var e = assemblyProgram.Code[i];

        //Микроразметку всегда оставляем
        if (e is LCVMMicromarking)
          continue;

        if (e.LLOptimizer_002_trace == false) //если инструкция мертва
        {
          code.RemoveAt(i);
          count--;
          i--;
          changed = true;
        }
      }

      if (changed)
        assemblyProgram.ProgramAllocate();
    }
  }
}
