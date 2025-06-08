using LC2.LCCompiler.CodeGenerator.AsmInstruction;

namespace LC2.LCCompiler.CodeGenerator
{
  /// <summary>
  ///Данный оптимизатор удаляет инструкции сдвига fp на ноль эле
  /// </summary>
  static internal class LLOptimizer_001
  {
    public static void Run(AssemblyProgram assemblyProgram)
    {
      var code = assemblyProgram.Code;
      int count = code.Count;
      bool changed = false;

      for (int i = 0; i < count; i++)
      {
        if (code[i] is INSTR_INCR_FP instr_INSTR_INCR_FP)
        {
          if (instr_INSTR_INCR_FP.Value == 0)
          {
            code.RemoveAt(i);
            count--;
            i--;
            changed = true;
            continue;
          }
        }


        if (code[i] is INSTR_DECR_FP instr_INSTR_DECR_FP)
        {
          if (instr_INSTR_DECR_FP.Value == 0)
          {
            code.RemoveAt(i);
            count--;
            i--;
            changed = true;
            continue;
          }
        }
      }

      if (changed)
        assemblyProgram.ProgramAllocate();
    }
  }
}
