using System;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  /// <summary>
  /// Данный класс выполняет уникальность имени декларатора
  /// в пределах данного программного блока
  /// </summary>
  static class CheckDeclaratorName
  {
    static public bool CheckFunction(FunctionDeclaratorNode n, CompilerLogger logger)
    {
      string objName = n.Name;
      if (TreeMISCWorkers.FunctionIsDeclared(n))
      {
        logger.Error(n.LocateName, string.Format("Функция с именем \"{0}\" уже объявлена в данном модуле", objName));
        return false;
      }
      return true;
    }
    /*
    static public bool CheckVariable(VariableDeclaratorNode n, CompilerLogger logger)
    {

    }

    internal static bool CheckUserType(StructDeclaratorNode n, CompilerLogger logger)
    {
      string objName = n.Name;
      if (TreeMISCWorkers.UserTypeIsDeclared(n))
      {
        logger.Error(n.LocateName, string.Format("Функция с именем \"{0}\" уже объявлена в данном модуле", objName));
        return false;
      }
      return true;
    }
    */
  }
}
