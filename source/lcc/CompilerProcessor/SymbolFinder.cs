using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler
{
  static internal class SymbolFinder
  {
    /*
    public class FoundFunction
    {
      public string FunctionName { get; set; }
      public FunctionDeclaratorNode[] functionDeclarators { get; set; }
    }
    */

    static public FunctionDeclaratorNode[] FindFunction(CompiledUnit[] compiledUnits, string functionName)
    {
      List<FunctionDeclaratorNode> foundDeclarators = new List<FunctionDeclaratorNode>();

      //Поиск модуля с точкой входа в приложение
      foreach (var module in compiledUnits)
        foundDeclarators.AddRange(FindFunctionInSymbols(module.symbolTable.Declarators, functionName));

      return foundDeclarators.ToArray();
    }

    static FunctionDeclaratorNode[] FindFunctionInSymbols(DeclaratorNode[] declarators, string functionName)
    {
      List<FunctionDeclaratorNode> foundDeclarators = new List<FunctionDeclaratorNode>();
      foreach (var d in declarators)
        if (d is FunctionDeclaratorNode functionDeclarator) //Если это декларатор функции
          if (functionDeclarator.Name == functionName) //Если имя функции совпало
            foundDeclarators.Add(functionDeclarator);

      return foundDeclarators.ToArray();
    }

    static public VariableDeclaratorNode[] FindVariable(CompiledUnit[] compiledUnits, string variableName)
    {
      List<VariableDeclaratorNode> foundDeclarators = new List<VariableDeclaratorNode>();

      //Поиск модуля с точкой входа в приложение
      foreach (var module in compiledUnits)
        foundDeclarators.AddRange(FindVariableInSymbols(module.symbolTable.Declarators, variableName));

      return foundDeclarators.ToArray();
    }

    private static VariableDeclaratorNode[] FindVariableInSymbols(DeclaratorNode[] declarators, string variableName)
    {
      List<VariableDeclaratorNode> foundDeclarators = new List<VariableDeclaratorNode>();
      foreach (var d in declarators)
        if (d is VariableDeclaratorNode variableDeclarator) //Если это декларатор переменной
          if (variableDeclarator.Name == variableName) //Если имя совпало
            foundDeclarators.Add(variableDeclarator);

      return foundDeclarators.ToArray();
    }
  }
}
