using LC2.LCCompiler.CodeGenerator.AsmInstruction;

namespace LC2.LCCompiler.CodeGenerator
{
  static class LLOptimizer_003
  {
    public static void Run(AssemblyProgram assemblyProgram)
    {
      var code = assemblyProgram.Code;
      int count = code.Count;

      for (int i = 0; i < count; i++)
      {
        if (code[i] is INSTR_CALL instr_call)
        {
          long offset = instr_call.LabelReference.Address - instr_call.CurrentPosition;

          if (offset <= sbyte.MaxValue && offset >= sbyte.MinValue)
          {
            code[i] = new INSTR_CALL_T(instr_call.LabelReference);
            assemblyProgram.ProgramAllocate();
          }
          else if (offset <= short.MaxValue && offset >= short.MinValue)
          {
            code[i] = new INSTR_CALL_S(instr_call.LabelReference);
            assemblyProgram.ProgramAllocate();
          }
        }
      }
    }
  }
}
