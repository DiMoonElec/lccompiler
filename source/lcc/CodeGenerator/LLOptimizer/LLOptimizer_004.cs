using LC2.LCCompiler.CodeGenerator.AsmInstruction;

namespace LC2.LCCompiler.CodeGenerator
{
  static class LLOptimizer_004
  {
    public static void Run(AssemblyProgram assemblyProgram)
    {
      var code = assemblyProgram.Code;
      int count = code.Count;

      for (int i = 0; i < count; i++)
      {
        if (code[i] is INSTR_JMP instr_jmp)
        {
          long offset = instr_jmp.LabelReference.Address - instr_jmp.CurrentPosition;

          if (offset <= sbyte.MaxValue && offset >= sbyte.MinValue)
          {
            code[i] = new INSTR_JMP_T(instr_jmp.LabelReference);
            assemblyProgram.ProgramAllocate();
          }
          else if (offset <= short.MaxValue && offset >= short.MinValue)
          {
            code[i] = new INSTR_JMP_S(instr_jmp.LabelReference);
            assemblyProgram.ProgramAllocate();
          }
        }
      }
    }
  }
}
