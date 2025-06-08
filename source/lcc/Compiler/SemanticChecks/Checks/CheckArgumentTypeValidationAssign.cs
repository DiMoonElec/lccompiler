using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LC2.LCCompiler.Compiler.LCTypes;

namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  internal static class CheckArgumentTypeValidationAssign
  {
    /// <summary>
    /// Проверка операций присвоения. В качестве левого операнда (приемника)
    /// может выступать объект, доступный для записи, например
    /// переменная базового типа (все типы, кроме void),
    /// ссылка на массив.
    /// 
    /// 
    /// В качестве правого операнда (источника) может выступать любой 
    /// объект, доступный на чтение, например, 
    /// переменная или константа базового типа (все типы, кроме void),
    /// функция, возвращающая переменную базового типа,
    /// ссылка на массив базовых типов,
    /// массив базовых типов
    /// 
    /// ---------------------------------------------------------
    /// Операции:
    /// Присвоение (a = b)
    /// ---------------------------------------------------------
    /// Данные операции могут выполняться над следующими типами:
    /// </summary>
    /// <param name="op">Нода бинарной операции</param>
    /// <param name="logger">Логгер для вывода сообщений об ошибках</param>
    /// <returns>True - обнаружены ошибки, False - ошибок не обнаружено</returns>
    public static bool CheckBinaryOpAssign(AssignOperatorNode op, CompilerLogger Logger)
    {
      if (op.SemanticallyCorrect == false)
        return false;

      bool IsOK = true;

      //Берем правый и лервый операнды
      TypedNode left = op.GetOperandLeft();
      TypedNode right = op.GetOperandRight();

      //Проверка левой и правой нод
      if (CheckTypedNode(left) == false)
        IsOK = false;

      if (CheckTypedNode(right) == false)
        IsOK = false;


      //Проверяем результат на данном этапе
      if (IsOK == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      LCObjectType leftObjectType = left.ObjectType;
      LCObjectType rightObjectType = right.ObjectType;

      LCType leftType = leftObjectType.Type;
      LCType rightType = rightObjectType.Type;

      if (leftObjectType.Writeable == false)
      {
        IsOK = false;
        Logger.Error(left.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      if (rightObjectType.Readable == false)
      {
        IsOK = false;
        Logger.Error(right.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      //Проверяем результат предыдущих проверок
      if (IsOK == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      ////////////////////

      var leftPrimitiveType = leftType as LCPrimitiveType;
      var rightPrimitiveType = rightType as LCPrimitiveType;

      if(leftPrimitiveType == null)
      {
        IsOK = false;
        Logger.Error(left.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      if(rightPrimitiveType == null)
      {
        IsOK = false;
        Logger.Error(right.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      if (IsOK == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }
      
      ////////////////////
      
      if(leftPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        IsOK = false;
        Logger.Error(left.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      if(rightPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        IsOK = false;
        Logger.Error(right.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
      }

      if (IsOK == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      ////////////////////

      if(leftPrimitiveType.Type != rightPrimitiveType.Type)
      {
        if (right is ConstantValueNode rightConstantValueNode)
        {
          if(checkConstant(rightConstantValueNode, leftPrimitiveType, Logger) == false)
          {
            op.SemanticallyCorrect = false;
            return false;
          }
        }
        else
        {
          Logger.Error(op.Locate, string.Format("Правый операнд должен иметь тип \"{0}\"", leftPrimitiveType.ToString()));
          op.SemanticallyCorrect = false;
          return false;
        }
      }

      op.OperandsCType = new LCPrimitiveType(leftPrimitiveType.Type);

      return true;
    }

    private static bool checkConstant(ConstantValueNode constantValueNode,
      LCPrimitiveType commonType,
      CompilerLogger logger)
    {
      //Если оба типа совпадают, то выходим
      if (LCTypesUtils.IsEqual(constantValueNode.Constant.PrimitiveType, commonType))
        return true;

      ConstantValue castConstant;

      /// Выполняем конвертацию типа константы ///
      var overrange = ConstantTypeCast.Cast(constantValueNode.Constant, commonType,
        out castConstant);

      //Если возникло переполнение
      if (overrange)
      {
        logger.Error(constantValueNode.Locate,
          string.Format("Переполнение константного значения '{0}' при приведении типа из '{1}' в '{2}'",
          constantValueNode.Constant.ToString(),
          constantValueNode.Constant.PrimitiveType.ToString(),
          commonType.ToString()));

        return false;
      }
      else
      {
        logger.Info(constantValueNode.Locate, string.Format("Константное значение '{0}' преобразовано из типа '{1}' в '{2}'",
           constantValueNode.Constant.ToString(),
          constantValueNode.Constant.PrimitiveType.ToString(),
          commonType.ToString()));
      }

      /// Создаем ноду константы ///
      var castConstantValueNode = new ConstantValueNode(castConstant, constantValueNode.Locate);

      /// Выполняем замену ноды ///
      constantValueNode.Replace(castConstantValueNode);

      return true;
    }


    private static bool CheckTypedNode(TypedNode node)
    {
      if (node == null)
        return false;
      else if (node.SemanticallyCorrect == false)
        return false;
      else if (node.ObjectType == null)
        throw new InternalCompilerException("Семантически корректная нода должна иметь значение поля ObjectType");

      return true;
    }

#if false
    public static bool CheckAssign(TypedNode sourseNode, LCObjectType destObjectType, CompilerLogger logger)
    {
      var sourseType = sourseNode.ObjectType.Type;
      var desType = destObjectType.Type;

      //источник должен быть доступен для чтения
      if (sourseNode.ObjectType.Readable == false)
      {
        logger.Error(sourseNode.Locate, string.Format("Данный объект должен быть доступен для чтения"));
        return false;
      }

      //приемник должен быть доступен для записи
      if (destObjectType.Writeable == false)
      {
        logger.Error(sourseNode.Locate, string.Format("Приемник данных должен быть доступен для записи"));
        return false;
      }

      if ((sourseType is LCPrimitiveType soursePrimitiveType)
        && (desType is LCPrimitiveType destPrimitiveType))
      {
        return CheckAssignPrimitiveTypes(sourseNode, destObjectType, soursePrimitiveType, destPrimitiveType, logger);
      }
      else if (desType is LCPointerArrayType destPointerType)
      {
        var destReferenseType = destPointerType.Type;

        //Указателю присваиваем указатель
        if ((sourseType is LCPointerArrayType sourcePointerType)
          && LCTypesUtils.IsEqual(destReferenseType, sourcePointerType.Type))
          return true;
        /*
        //Указателю присваиваем ссылку на массив
        if ((sourseType is LCArrayType sourceArrayType)
          && LCTypesUtils.IsEqual(destReferenseType, sourceArrayType.Type))
          return true;
        */
        //Указателю присваиваем null
        if (sourseType is LCNullType)
          return true;
      }

      logger.Error(sourseNode.Locate, string.Format("Тип \'{0}\' невозможно присвоить типу \'{1}\'", sourseType.ToString(), desType.ToString()));
      return false;
    }

    private static bool CheckAssignPrimitiveTypes(TypedNode sourseNode, LCObjectType destObjectType,
      LCPrimitiveType soursePrimitiveType, LCPrimitiveType destPrimitiveType,
      CompilerLogger logger)
    {
      //Если один из типов void
      if (soursePrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(sourseNode.Locate, string.Format("Значение типа \'void\' не может быть присвоено объекту типа \'{0}\'", destPrimitiveType.ToString()));
        return false;
      }

      if (destPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(sourseNode.Locate, string.Format("Объекту типа \'void\' не может быть присвоено данное значение"));
        return false;
      }

      //Если типы одинаковы, то все ОК
      if (LCTypesUtils.IsEqual(soursePrimitiveType, destPrimitiveType))
        return true;

      //Если источником является константа
      //то выполняем преобразование типа без дополнительных проверок
      if (sourseNode is ConstantValueNode)
      {
        TreeMISCWorkers.InsertTypeCast(destPrimitiveType, sourseNode);
        return true;
      }

      //ToDo: добавить проверку типов
      TreeMISCWorkers.InsertTypeCast(destPrimitiveType, sourseNode);
      return true;
    }

#endif
  }
}
