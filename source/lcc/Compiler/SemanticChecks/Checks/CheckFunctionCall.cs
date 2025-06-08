namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckFunctionCall
  {
    public static bool Check(FunctionCallNode n, CompilerLogger logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      var functionDeclarator = TreeMISCWorkers.FindFunctionDeclarator(n, n.FunctionName);

      if (functionDeclarator == null)
      {
        logger.Error(n.Locate, string.Format("Функция с именем \'{0}\' не найдена", n.FunctionName));
        n.SemanticallyCorrect = false;
        return false;
      }

      //Проверяем кол-во параметров вызываемой функции, и 
      //кол-во фактически переданных параметров
      if (functionDeclarator.FunctionParams.Length != n.Params.CountChildrens)
      {
        logger.Error(n.Locate, "Количество аргументов не совпадает с количесвом параметров вызываемой функции");

        n.SemanticallyCorrect = false;
        return false;
      }

      //Проверяем типы параметров функции и переданных значений
      bool isOK = true;

      for (int i = 0; i < functionDeclarator.FunctionParams.Length; i++)
      {
        if (CheckCallTypes(n.Params.GetChild(i), functionDeclarator.FunctionParams[i], logger) == false)
          isOK = false;
      }

      //Если в ходе проверки типов обнаружены ошибки
      if (isOK == false)
        n.SemanticallyCorrect = false;


      //Тип у FunctionCallNode такой же, как и у вызываемой функции
      var o = functionDeclarator.ReturnType;
      n.ObjectType = o.Clone();

      if (functionDeclarator is ManagedFunctionDeclaratorNode managedFunction)
        n.ManagedFunctionDeclarator = managedFunction;
      else if (functionDeclarator is NativeFunctionDeclaratorNode nativeFunction)
        n.NativeFunctionDeclarator = nativeFunction;
      else
        throw new InternalCompilerException("Неверный тип функции");

      return isOK;
    }

    /// <summary>
    /// Проверка типов передаваемых в функцию параметров
    /// </summary>
    /// <param name="callParamNode">Параметр, который передается в функцию</param>
    /// <param name="funcParamNode">Параметр из прототипа функции</param>
    /// <param name="logger">Логгер</param>
    /// <returns>true - семантических ошибок не обнаружено</returns>
    static bool CheckCallTypes(Node callParamNode, Node funcParamNode, CompilerLogger logger)
    {
      if (callParamNode.SemanticallyCorrect == false)
        return false;

      if (callParamNode is TypedNode callParam && funcParamNode is ObjectDeclaratorNode funcParam)
      {
        var callParamType = callParam.ObjectType.Type;
        var funcParamType = funcParam.ObjectType.Type;

        if (LCTypesUtils.IsEqual(callParamType, funcParamType) == false)
        {
          logger.Error(callParam.Locate, string.Format("Тип параметра должен быть '{0}'", funcParamType.ToString()));
          return false;
        }

        return true;
      }
      else
        return false;
    }
  }
}
