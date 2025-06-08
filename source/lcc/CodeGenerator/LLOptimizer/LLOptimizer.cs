using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.CodeGenerator
{
  /// <summary>
  /// Низкоуровневый оптимизатор
  /// </summary>
  static internal class LLOptimizer
  {
    public static void Run(LLOptimizerConfiguration config, AssemblyProgram assemblyProgram)
    {
      if (config.Opt001)
        LLOptimizer_001.Run(assemblyProgram);

      if (config.Opt002)
        LLOptimizer_002.Run(assemblyProgram);

      if(config.Opt003)
        LLOptimizer_003.Run(assemblyProgram);

      if (config.Opt004)
        LLOptimizer_004.Run(assemblyProgram);
    }
  }
}
