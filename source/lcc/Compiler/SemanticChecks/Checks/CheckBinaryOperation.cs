using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using LC2.LCCompiler.Compiler.LCTypes;
using static LC2.LCCompiler.Compiler.LCTypesUtils;

namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  internal class CheckBinaryOperationRule
  {
    public Type[] OperatorType { get; set; }  // Тип ноды

    public bool DifferendOperandTypes = false;

    public LCPrimitiveType.PrimitiveTypes[] LeftOperandValidTypes { get; set; }
    public LCPrimitiveType.PrimitiveTypes[] RightOperandValidTypes { get; set; }

    public Func<LCType, LCType, LCType> ResultTypeResolver { get; set; } // Определение типа результата
    public string Description { get; set; } // Описание для отладки или документации
  }

  internal class CheckBinaryOperation
  {
    CheckBinaryOperationRule[] Rules;
    CompilerLogger Logger;
    public CheckBinaryOperation(CheckBinaryOperationRule[] rules, CompilerLogger logger)
    {
      Rules = rules;
      Logger = logger;
    }


    public bool Check(BinaryOperationNode op)
    {
      bool IsOK = true;

      CheckBinaryOperationRule rule = FindRule(op);

      //Тип результата операции пока присваиваем в null
      op.ObjectType = null;

      // Если в данной ноде были обнаружены семантические ошибки
      // на предыдущих этапах, то просто выходим
      if (op.SemanticallyCorrect == false)
        return false;

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

      if (leftObjectType.Readable == false)
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


      if ((leftType is LCPrimitiveType leftPrimitiveType
        && LCTypesUtils.ItIsFromList(leftPrimitiveType, rule.LeftOperandValidTypes)) == false)
      {
        IsOK = false;
        string msg = "Неверный тип '{0}' левого операнда, ожидаемые типы: {1}";

        Logger.Error(op.Locate, string.Format(msg, leftType.ToString(),
          LCTypesUtils.PrimitiveTypeGetName(rule.LeftOperandValidTypes)));
      }

      if ((rightType is LCPrimitiveType rightPrimitiveType
        && LCTypesUtils.ItIsFromList(rightPrimitiveType, rule.RightOperandValidTypes)) == false)
      {
        if (rule.DifferendOperandTypes == true
          && right is ConstantValueNode constantValueNode)
        {
          if (checkConstant(op, constantValueNode, new LCPrimitiveType(rule.RightOperandValidTypes[0]), Logger) == false)
            IsOK = false;
        }
        else
        {
          IsOK = false;
          string msg = "Неверный тип '{0}' правого операнда, ожидаемые типы: {1}";

          Logger.Error(op.Locate, string.Format(msg, rightType.ToString(),
            LCTypesUtils.PrimitiveTypeGetName(rule.RightOperandValidTypes)));
        }
      }

      //Проверяем результат предыдущих проверок
      if (IsOK == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      if (rule.DifferendOperandTypes == false)
      {
        if (CheckLeftAndRightTypes(op, left, right, Logger) == false)
        {
          op.SemanticallyCorrect = false;
          return false;
        }
      }

      var operatorType = rule.ResultTypeResolver(leftType, rightType);
      if (operatorType == null)
        throw new InternalCompilerException("Семантическое правило вернуло неверный тип");

      op.ObjectType = new LCObjectType(operatorType, true, false);

      op.OperandsCType = leftType.Clone();

      return true;
    }

    private bool CheckLeftAndRightTypes(BinaryOperationNode op,
      TypedNode left, TypedNode right,
      CompilerLogger logger)
    {
      var leftConstantValueNode = left as ConstantValueNode;
      var rightConstantValueNode = right as ConstantValueNode;

      if (leftConstantValueNode != null && rightConstantValueNode == null)
      {
        // Только левый операнд константное значение
        return checkConstant(op, leftConstantValueNode, (LCPrimitiveType)right.ObjectType.Type, logger);
      }
      else if (leftConstantValueNode == null && rightConstantValueNode != null)
      {
        // Только правый операнд константное значение
        return checkConstant(op, rightConstantValueNode, (LCPrimitiveType)left.ObjectType.Type, logger);
      }
      else if (leftConstantValueNode != null && rightConstantValueNode != null)
      {
        // Оба операнда являются константными значениями
        return checkLeftRightConstants(op, leftConstantValueNode, rightConstantValueNode, logger);
      }
      else
      {
        // Оба операнда не являются константными значениями
        if (LCTypesUtils.IsEqual(left.ObjectType.Type, right.ObjectType.Type) == false)
        {
          Logger.Error(op.Locate, string.Format("Левый и правый операнды оператора \"{0}\" должны иметь одинаковый тип", op.Description()));
          return false;
        }

        return true;
      }
    }

    private bool checkConstant(BinaryOperationNode op,
      ConstantValueNode constantValueNode,
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
        logger.Error(op.Locate,
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

    private bool checkLeftRightConstants(BinaryOperationNode op,
      ConstantValueNode leftConstantValueNode,
      ConstantValueNode rightConstantValueNode,
      CompilerLogger logger)
    {
      //Если оба типа совпадают, то выходим
      if (LCTypesUtils.IsEqual(leftConstantValueNode.Constant.PrimitiveType, rightConstantValueNode.Constant.PrimitiveType))
        return true;

      bool IsOK = true;

      var commonType = AutoTypeCast.Cast(leftConstantValueNode.Constant.PrimitiveType,
            rightConstantValueNode.Constant.PrimitiveType);

      if (commonType == null)
      {
        // Невозможно произвести автоматическое приведение типа

        logger.Error(op.Locate,
          string.Format("Невозможно выполнить автоматическое приведение типов операндов '{0}' и '{1}'",
          leftConstantValueNode.Constant.PrimitiveType.ToString(),
          rightConstantValueNode.Constant.PrimitiveType.ToString()));

        return false;
      }

      if(checkConstant(op, leftConstantValueNode, commonType, logger) == false)
        IsOK = false;

      if (checkConstant(op, rightConstantValueNode, commonType, logger) == false)
        IsOK = false;

      /*
      ConstantValue leftCastConstant;
      ConstantValue rightCastConstant;

      /// Выполняем конвертацию типов констант ///
      var leftOverrange = ConstantTypeCast.Cast(leftConstantValueNode.Constant, commonType,
        out leftCastConstant);

      var rightOverrange = ConstantTypeCast.Cast(leftConstantValueNode.Constant, commonType,
        out rightCastConstant);

      /// Создаем ноды констант ///
      var leftCastConstantValue = new ConstantValueNode(leftCastConstant, leftConstantValueNode.Locate);
      var rightCastConstantValue = new ConstantValueNode(rightCastConstant, rightConstantValueNode.Locate);

      /// Выполняем замену нод ///
      leftConstantValueNode.Replace(leftCastConstantValue);
      rightConstantValueNode.Replace(rightCastConstantValue);

      //Если возникло переполнение для левой константы
      if (leftOverrange)
      {
        IsOK = false;

        logger.Error(op.Locate,
          string.Format("Переполнение левого аргумента при приведении типа из '{0}' в '{1}'",
          leftConstantValueNode.Constant.PrimitiveType.ToString(),
          commonType.ToString()));
      }

      //Если возникло переполнение для правой константы
      if (rightOverrange)
      {
        IsOK = false;

        logger.Error(op.Locate,
          string.Format("Переполнение правого аргумента при приведении типа из '{0}' в '{1}'",
          rightConstantValueNode.Constant.PrimitiveType.ToString(),
          commonType.ToString()));
      }
      */
      return IsOK;
    }

    private CheckBinaryOperationRule FindRule(BinaryOperationNode op)
    {
      Type type = op.GetType();
      CheckBinaryOperationRule rule = null;

      foreach (var r in Rules)
      {
        if (ScanTypes(r, type))
        {
          rule = r;
          break;
        }
      }

      if (rule == null)
        throw new InternalCompilerException("Отсутствует семантическое правило для оператора " + type.Name);

      return rule;
    }

    private bool ScanTypes(CheckBinaryOperationRule rule, Type type)
    {
      foreach (var t in rule.OperatorType)
        if (t == type)
          return true;

      return false;
    }

    private bool CheckTypedNode(TypedNode node)
    {
      if (node == null)
        return false;
      else if (node.SemanticallyCorrect == false)
        return false;
      else if (node.ObjectType == null)
        throw new InternalCompilerException("Семантически корректная нода должна иметь значение поля ObjectType");

      return true;
    }
  }
}
