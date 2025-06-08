using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  internal static class CheckArgumentTypeValidationHelper
  {

    /*
    public static bool AssignmentValidate(TypedNode leftNode, TypedNode rightNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType; //Левый объект
      var rightObject = rightNode.ObjectType; //Правый объект

      var leftObjectLocate = leftNode.Locate;
      var rightObjectLocate = rightNode.Locate;

      bool isOK = true;

      if (leftNode.SemanticallyCorrect == false)
        return false;

      if (rightNode.SemanticallyCorrect == false)
        return false;

      //Проверяем права доступа
      if (leftObject.Writeable == false)
      {
        logger.Error(leftObjectLocate, string.Format("Данный объект должен быть доступен на запись"));
        isOK = false;
      }

      if (rightObject.Readable == false)
      {
        logger.Error(rightObjectLocate, string.Format("Данный объект должен быть доступен для чтения"));
        isOK = false;
      }

      if (isOK == false)
        return false;

      //Проверяем типы 
      var rightType = rightObject.Type;

      if (rightType is LCPrimitiveType rightPrimitiveType)
        return ValidateRightPrimitiveType(rightPrimitiveType, rightNode, leftNode, logger);
      else if (rightType is LCArrayType rightArrayType)
        return ValidateRightArrayType(rightArrayType, rightNode, leftNode, logger);
      else if (rightType is LCPointerType rightPointerType)
        return ValidateRightPointerType(rightPointerType, rightNode, leftNode, logger);
      else if (rightType is LCPointerArrayType rightPointerArrayType)
        return ValidateRightPointerArrayType(rightPointerArrayType, rightNode, leftNode, logger);
      else if (rightType is LCStructType rightStructType)
        return ValidateRightStructType(rightStructType, rightNode, leftNode, logger);
      else if (rightType is LCTupleType rightTupleType)
        return ValidateRightTupleType(rightTupleType, rightNode, leftNode, logger);
      else if (rightType is LCNullType rightNullType)
        return ValidateRightNullType(rightNullType, rightNode, leftNode, logger);
      else
      {
        logger.Error(rightObjectLocate, string.Format("Данный объект должен быть доступен для чтения"));
        return false;
      }
    }
    */
    /*
    private static bool ValidateRightTupleType(LCTupleType rightTupleType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {

    }
    */
    /*
    private static bool ValidateRightStructType(LCStructType rightStructType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType;
      var rightObject = rightNode.ObjectType;

      var leftType = leftObject.Type;

      //Правый объект является источником данных
      rightObject.AccessMethod = LCObjectType.ObjectAccessMethod.GetValue;


      if (leftType is LCPointerType leftPointerType)
      {
        //Левый объект является приемником данных
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.SetValue;

        if (LCTypesUtils.IsEqual(rightStructType, leftPointerType.Type))
          return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftType.ToString()));
      return false;

    }
    */
    /*
    private static bool ValidateRightPointerArrayType(LCPointerArrayType rightPointerArrayType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType;
      var rightObject = rightNode.ObjectType;

      var leftType = leftObject.Type;

      //Правый объект является источником данных
      rightObject.AccessMethod = LCObjectType.ObjectAccessMethod.GetValue;

      if (leftType is LCPointerArrayType leftPointerarrayType)
      {
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.SetValue;
        if (LCTypesUtils.IsEqual(rightObject.Type, leftPointerarrayType.Type))
          return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftType.ToString()));
      return false;
    }

    private static bool ValidateRightPointerType(LCPointerType rightPointerType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType;
      var rightObject = rightNode.ObjectType;

      var leftType = leftObject.Type;

      //Правый объект является источником данных
      rightObject.AccessMethod = LCObjectType.ObjectAccessMethod.GetValue;

      if (leftType is LCPointerType leftPointerType)
      {
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.SetValue;
        if (LCTypesUtils.IsEqual(rightObject.Type, leftPointerType.Type))
          return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftType.ToString()));
      return false;
    }

    private static bool ValidateRightArrayType(LCArrayType rightArrayType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType;
      var rightObject = rightNode.ObjectType;

      var leftType = leftObject.Type;

      //Правый объект является источником данных
      rightObject.AccessMethod = LCObjectType.ObjectAccessMethod.GetValue;

      if (leftType is LCPointerArrayType leftPointerArrayType)
      {
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.SetValue;
        if (LCTypesUtils.IsEqual(rightObject.Type, leftPointerArrayType.Type))
          return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftType.ToString()));
      return false;
    }

    public static bool ValidateRightPrimitiveType(LCPrimitiveType rightPrimitiveType, TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var leftObject = leftNode.ObjectType;
      var rightObject = rightNode.ObjectType;

      var leftType = leftObject.Type;

      //Правый объект является источником данных
      rightObject.AccessMethod = LCObjectType.ObjectAccessMethod.GetValue;

      if (leftType is LCPrimitiveType leftPrimitiveType)
      {
        //Левый объект является приемником данных
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.SetValue;

        return ValidateRightLeftPrimitiveType(rightPrimitiveType, leftPrimitiveType, rightNode, leftNode, logger);
      }
      else if (leftType is LCPointerType leftPointerType)
      {
        //Объект, на который указывает левый объект, является приемником данных
        leftObject.AccessMethod = LCObjectType.ObjectAccessMethod.PointerSetValue;
        var leftPointer = leftPointerType.Type;

        if (leftPointer is LCPrimitiveType leftPointerPrimitiveType)
          return ValidateRightLeftPrimitiveType(rightPrimitiveType, leftPointerPrimitiveType, rightNode, leftNode, logger);
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftType.ToString()));
      return false;
    }
    */
    /*
    private static bool ValidateRightLeftPrimitiveType(LCPrimitiveType rightPrimitiveType, LCPrimitiveType leftPrimitiveType,
      TypedNode rightNode, TypedNode leftNode, CompilerLogger logger)
    {
      var rightObjectLocate = rightNode.Locate;
      var leftObjectLocate = leftNode.Locate;

      if (rightPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(rightObjectLocate, "Данный объект не может быть типа \'void\'");
        return false;
      }

      if (leftPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(leftObjectLocate, "Данный объект не может быть типа \'void\'");
        return false;
      }

      //Если типы одинаковы, то все ОК
      if (LCTypesUtils.IsEqual(rightPrimitiveType, leftPrimitiveType))
        return true;

      //Если константа, то принудительно выполняем приведение типа
      if (rightNode is ConstantValueNode constantValueNode)
      {
        TreeMISCWorkers.InsertTypeCast(leftPrimitiveType, rightNode);
        return true;
      }

      //Если попали сюда, то проверяем, можно ли выполнить автоматическое приведение типа
      if (CheckBaseAutoTypeCast(rightPrimitiveType, leftPrimitiveType))
      {
        TreeMISCWorkers.InsertTypeCast(leftPrimitiveType, rightNode);
        return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftPrimitiveType.ToString()));
      return false;
    }
    */
#if false
    public static bool CheckBinaryOpAssignBaseTypes(AssignOperatorNode op,
      LCPrimitiveType leftPrimitiveType, LCPrimitiveType rightPrimitiveType,
      CompilerLogger logger)
    {
      //Если один из типов void
      if (leftPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid
        || rightPrimitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        logger.Error(op.Locate, string.Format("Невозможно типу \"{0}\" выполнить присвоение типа \"{1}\"",
            leftPrimitiveType.ToString(), rightPrimitiveType.ToString()));

        return false;
      }

      op.OperandsCType = leftPrimitiveType.Clone();

      //Если типы одинаковы, то все ОК
      if (LCTypesUtils.IsEqual(rightPrimitiveType, leftPrimitiveType))
      {
        return true;
      }

      
      TypedNode rightNode = op.GetOperandRight();
      TreeMISCWorkers.InsertTypeCast(leftPrimitiveType, rightNode);
      return true;

      /*
      //Если константа, то принудительно выполняем приведение типа
      if (rightNode is ConstantValueNode constantValueNode)
      {
        TreeMISCWorkers.InsertTypeCast(leftPrimitiveType, rightNode);
        op.OperandsCType = leftPrimitiveType.Clone();
        return true;
      }

      
      //Если попали сюда, то проверяем, можно ли выполнить автоматическое приведение типа
      if (CheckBaseAutoTypeCast(rightPrimitiveType, leftPrimitiveType))
      {
        TreeMISCWorkers.InsertTypeCast(leftPrimitiveType, rightNode);
        op.OperandsCType = leftPrimitiveType.Clone();
        return true;
      }

      logger.Error(rightNode.Locate, string.Format("Невозможно выполнить автоматическое приведение типа к \'{0}\'", leftPrimitiveType.ToString()));
      return false;
      */
    }
#endif
    public static bool CheckOperatorTypeCastPrimitiveType(TypeCastNode op, LCPrimitiveType castBaseObjectType,
      TypedNode Operand, CompilerLogger Logger)
    {
      LCObjectType otype = Operand.ObjectType;

      //Кастуемый тип не должен быть типом void, к типу void ничего привести нельзя
      if (castBaseObjectType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
      {
        Logger.Error(op.Locate, "Невозможно выполнить преобразование к типу \"void\"");
        op.SemanticallyCorrect = false;
        return false;
      }

      //Приводимый тип должен быть LCBaseObjectType (любым базовым типом...)
      if (otype.Type is LCPrimitiveType primitiveType)
      {
        //(...кроме void)
        if (primitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
        {
          Logger.Error(op.Locate,
            string.Format("Невозможно привести тип {0} к типу {1}", otype.ToString(), op.CastType.ToString()));
          op.SemanticallyCorrect = false;
          return false;
        }

        //Если попали сюда, то у нас ни один из типов не void, а
        //остальные базовые типы можно привести друг к другу
        op.ObjectType = new LCObjectType(op.CastType, true, false);
        op.OperandsCType = Operand.ObjectType.Type.Clone();

        return true;
      }
      else
      {
        Logger.Error(op.Locate, string.Format("Невозможно тип {0} привести к типу {1}", otype.ToString(), castBaseObjectType.ToString()));

        op.SemanticallyCorrect = false;
        return false;
      }
    }

    /// <summary>
    /// Код, который выполняет общие проверки для определенной группы проверок
    /// </summary>
    /// <param name="op">Ссылка на ноду унарной операции</param>
    /// <param name="Operand">Выходное значение: операнд</param>
    /// <param name="Logger">Ссылка на логгер ошибок</param>
    /// <returns>False - обнаружены ошибки, True - ошибок не обнаружено</returns>
    public static bool UnaryOperationsCommonChecks(UnaryOperationNode op, out TypedNode Operand, CompilerLogger Logger)
    {
      //Тип результата операции пока null
      op.ObjectType = null;

      //Тип операндов - пока не установлен, будет выяснено в процессе
      op.OperandsCType = null;

      //Берем операнд
      Operand = CheckTypedNode(op.GetOperand());

      if (Operand == null)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      return true;
    }

    /// <summary>
    /// Код, который выполняет общие проверки для определенной группы проверок
    /// </summary>
    /// <param name="op">Ссылка на ноду бинарной операции</param>
    /// <param name="Left">Выходное значение: левый операнд</param>
    /// <param name="Right">Выходное значение: правый операнд</param>
    /// <param name="Logger">Ссылка на логгер ошибок</param>
    /// <returns>False - обнаружены ошибки, True - ошибок не обнаружено</returns>
    public static bool BinaryOperationsCommonChecks(BinaryOperationNode op, out TypedNode Left,
      out TypedNode Right, CompilerLogger Logger)
    {
      //Тип результата операции пока null
      op.ObjectType = null;

      //Тип операндов - пока не установлен, будет выяснено в процессе
      op.OperandsCType = null;

      //Берем левый и правый операнды
      Left = CheckTypedNode(op.GetOperandLeft());
      Right = CheckTypedNode(op.GetOperandRight());

      if ((Left == null) || (Right == null))
      {
        op.SemanticallyCorrect = false;
        Left = null;
        Right = null;
        return false;
      }

      return true;
    }

    /// <summary>
    /// Код, который выполняет общие проверки для операций присвоения
    /// </summary>
    /// <param name="op">Ссылка на ноду бинарной операции</param>
    /// <param name="Left">Выходное значение: левый операнд</param>
    /// <param name="Right">Выходное значение: правый операнд</param>
    /// <param name="Logger">Ссылка на логгер ошибок</param>
    /// <returns>False - обнаружены ошибки, True - ошибок не обнаружено</returns>
    public static bool BinaryOperationsCommonChecksAssign(AssignOperatorNode op, out TypedNode Left, out TypedNode Right, CompilerLogger Logger)
    {
      //Тип операндов - пока не установлен, будет выяснено в процессе
      op.OperandsCType = null;

      //Берем левый и правый операнды
      Left = CheckTypedNode(op.GetOperandLeft());
      Right = CheckTypedNode(op.GetOperandRight());

      if ((Left == null) || (Right == null))
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      return true;
    }

    /// <summary>
    /// Проверяет, возможно ли ватоматически привести тип type к типу targetType
    /// </summary>
    /// <param name="type">Тип, который приводят</param>
    /// <param name="targetType">Тип, к которому приводят</param>
    /// <returns>True - приведение типа возможно, False - не возможно</returns>
    public static bool CheckBaseAutoTypeCast(LCPrimitiveType type, LCPrimitiveType targetType)
    {
      if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeSByte) //Тип byte автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeSByte) //byte
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeShort) //short
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeInt) //int
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeLong) //long
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //float
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeShort) //Тип short автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeShort) //short
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeInt) //int
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeLong) //long
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //float
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeInt) //Тип int автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeInt) //int
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeLong) //long
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //float
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeLong) //Тип long автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeLong) //long
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //float
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //Тип float автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeFloat) //float
          return true;
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //Тип double автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeDouble) //double
          return true;
      }
      else if (type.Type == LCPrimitiveType.PrimitiveTypes.LCTypeBool) //Тип bool автоматически можно привести к типам:
      {
        if (targetType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeBool) //bool
          return true;
      }

      return false;
    }

    public static TypedNode CheckTypedNode(TypedNode n)
    {
      if (n == null)
        return null;

      if (n.SemanticallyCorrect == false)
        return null;

      var objectType = n.ObjectType;
      if (objectType == null)
        return null;

      /*
      var type = objectType.Type;

      
      if (type is LCPointerType leftPointerType)
      {
        var leftPointerAccessNode = new PointerAccessNode(n.Locate, n.ExpressionLocate);
        leftPointerAccessNode.ObjectType = new LCObjectType(leftPointerType.Type.Clone());
        leftPointerAccessNode.ObjectType.SetAccessAttributes(objectType.Readable, objectType.Writeable);

        n.Replace(leftPointerAccessNode);
        leftPointerAccessNode.AddChild(n);

        return leftPointerAccessNode;
      }
      */

      return n;
    }
  }
}
