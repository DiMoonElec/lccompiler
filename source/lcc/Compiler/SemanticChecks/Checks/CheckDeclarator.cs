using static LC2.LCCompiler.Compiler.LCPrimitiveType;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static partial class CheckDeclarator
  {
    public static bool CheckFunction(ManagedFunctionDeclaratorNode functionDeclarator, CompilerLogger logger)
    {
      bool isOk = true;

      var returnType = functionDeclarator.ReturnType.Type;

      if (!(returnType is LCPrimitiveType))
        throw new InternalCompilerException("Функция не может возвращать данный тип");

      //Проверка количества параметров функции
      if (functionDeclarator.FunctionParams.Length > 256)
      {
        logger.Error(functionDeclarator.LocateName, "Функция не может принимать больше 256 параметров");
        isOk = false;
      }

      return isOk;
    }

    public static bool CheckVariable(VariableDeclaratorNode n, CompilerLogger logger)
    {
      bool isOK = true;
      var oType = n.ObjectType;
      var type = oType.Type;

      if(n.ClassValue != ObjectDeclaratorNode.DeclaratorClass.ClassGlobal && n.Attribute != null)
      {
        logger.Error(n.LocateName, "Атрибут можно присвоить только глобальной переменной");
        isOK = false;
      }

      if (type is LCPrimitiveType primitiveType)
      {
        //Примитивный тип данных, такой как:
        //int a;
        //byte b;

        //Данный тип можно объявлять в любой области 
        if (CheckVariablePrimitiveType(n, primitiveType, logger) == false)
          isOK = false;

        return isOK;
      }
      else if (type is LCArrayType arrayType)
      {
        //Объявление массива

        //Проверка типа массива
        if (arrayType.TypeElement.Type == PrimitiveTypes.LCTypeVoid)
        {
          logger.Error(n.LocateObjectType.Locate, "Неверный тип элемента массива");
          return false;
        }

        //Массив можно объявлять только в глобальной области
        if (n.ClassValue != ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
        {
          logger.Error(n.LocateObjectType.Locate, "Данный тип нельзя объявлять в этой области видимости");
          return false;
        }

        return isOK;
      }
      else if (type is LCPointerArrayType pointerType)
      {
        //Проверка типа массива
        if (pointerType.TypeElement.Type == PrimitiveTypes.LCTypeVoid)
        {
          logger.Error(n.LocateObjectType.Locate, "Неверный тип элемента массива");
          return false;
        }

        //Данный тип можно объявлять только как параметр функции
        if (n.ClassValue != ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam)
        {
          logger.Error(n.LocateObjectType.Locate, "Данный тип нельзя объявлять в этой области видимости");
          return false;
        }

        return isOK;
      }
      else if (type is LCStructType structType)
      {
        //Выполняем поиск объявления пользовательского типа по его имени LCUserType
        if(CheckVariableUserType(n, structType, logger) == false)
          isOK = false;

        //Данный тип можно объявлять в глобальной области
        if (n.ClassValue != ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
        {
          logger.Error(n.LocateObjectType.Locate, "Данный тип нельзя объявлять в этой области видимости");
          return false;
        }

        return isOK;
      }
      else if (type is LCPointerStructType pointerStructType)
      {
        //Выполняем поиск объявления пользовательского типа по его имени LCUserType
        if(CheckVariableUserType(n, pointerStructType, logger) ==false)
          isOK = false;

        //Данный тип можно объявлять только как параметр функции
        if (n.ClassValue != ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam)
        {
          logger.Error(n.LocateObjectType.Locate, "Данный тип нельзя объявлять в этой области видимости");
          return false;
        }

        return isOK;
      }
      else
      {
        logger.Error(n.LocateObjectType.Locate, "Неверный тип переменной");
        return false;
      }
    }

    public static bool CheckStruct(StructDeclaratorNode n, CompilerLogger logger)
    {
      var elements = n.StructType.Elements;

      bool isOK = true;

      for (int i = 0; i < elements.Length; i++)
      {
        var e = elements[i];
        var ename = e.Name;

        var etypeLocate = n.StructTypeLocate.TypesLocate[i];
        var enameLocate = n.StructTypeLocate.NamesLocate[i];

        if (e is LCStructElementPrimitiveType primitiveType)
        {
          if (primitiveType.Type.Type == PrimitiveTypes.LCTypeVoid)
          {
            isOK = false;
            logger.Error(etypeLocate.Locate, "Неверный тип элемента структуры");
          }
        }
        else if (e is LCStructElementArrayType arrayType)
        {
          if (arrayType.Type.TypeElement.Type == PrimitiveTypes.LCTypeVoid)
          {
            isOK = false;
            logger.Error(etypeLocate.Locate, "Неверный тип элемента массива");
          }
        }

        //Проверка имен элементов
        for (int j = 0; j < elements.Length; j++)
        {
          //Не проверяем элемент самим с собой
          if (j == i)
            continue;

          var name = elements[j].Name;
          if (ename == name)
          {
            logger.Error(enameLocate, "Элемент с таким именем уже содержится в структуре");
            isOK = false;
          }
        }
      }

      return isOK;
    }

    #region Вспомогательный методы

    private static bool CheckVariablePrimitiveType(VariableDeclaratorNode n, LCPrimitiveType primitiveType, CompilerLogger logger)
    {
      return CheckPrimitiveDataType(primitiveType, n.LocateObjectType.Locate, logger);
    }

    private static bool CheckPrimitiveDataType(LCPrimitiveType type, LocateElement locate, CompilerLogger logger)
    {
      if (type.Type == PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(locate, "Данный объект не может иметь тип \'void\'");
        return false;
      }

      return true;
    }

    private static LCStructDeclarator FindUserType(Node n, LСStructTypeGroup userType)
    {
      UserTypeDeclaratorNode t;
      t = TreeMISCWorkers.FindUserType(n, userType.TypeName);

      if (t == null)
        return null;

      if (t is StructDeclaratorNode structDeclaratorNode)
      {
        var s = structDeclaratorNode.StructType;
        return s;
      }
      else
        throw new InternalCompilerException("Неизвестный пользовательский тип");
    }

    private static bool CheckVariableUserType(VariableDeclaratorNode n, LСStructTypeGroup userType, CompilerLogger logger)
    {
      if (FindStructDeclarator(n, userType) == false)
      {
        logger.Error(n.LocateObjectType.Locate, "Данный пользовательский тип не объявлен в текущей области видимости");
        return false;
      }

      return true;
    }

    private static bool FindStructDeclarator(VariableDeclaratorNode n, LСStructTypeGroup userType)
    {
      var t = FindUserType(n, userType);
      if (t == null)
        return false;

      userType.StructDeclarator = t;
      return true;
    }

    #endregion
  }
}
