namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckModuleInit
  {
    static public bool Check(ModuleInitNode moduleInit, CompilerLogger logger)
    {
      bool isOK = true;

      for (int i = 0; i < moduleInit.CountChildrens; i++)
      {
        //Получаем ноду, производящую инициализацию переменной
        AssignNode assign = (AssignNode)moduleInit.GetChild(i);
        //Получаем значение 
        if (CheckElement(assign, logger) == false)
          isOK = false;
      }

      return isOK;
    }

    static bool CheckElement(AssignNode assign, CompilerLogger logger)
    {
      var right = assign.GetOperandRight();

      if (right == null)
        return false;

      if (right is ConstantValueNode) //Если константное значение
      {
        return true;
      }
      else if (right is TypeCastNode typeCastNode) //Если приведение типа
      {
        var typeCastOperand = typeCastNode.GetOperand();

        //Если нет операнда у typecast
        //то ошибка была зафиксирована ранее
        if (typeCastOperand == null)
          return false;

        /*
        //и приводится массив
        if ((typeCastOperand is ObjectNode objectNode)
        && (objectNode.ObjectType is LCBaseArrayObjectType))
        {
          return true;
        }
        */
      }
      /*
      else if ((right is ObjectNode objectNode)
          && (objectNode.ObjectType is LCBaseArrayObjectType))
      {
        return true;
      }
      */

      logger.Error(assign.Locate, "Инициализатором глобального объекта должна быть константа");
      return false;
    }
  }
}
