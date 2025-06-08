using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  internal class CheckUnaryOperationRule
  {
    public Type[] OperatorType { get; set; }  // Тип ноды
    public bool OperandMustBeWritable { get; set; } //Должен ли операнд поддерживать запись
    public LCPrimitiveType.PrimitiveTypes[] OperandValidTypes { get; set; }
    public Func<LCType, LCType> ResultTypeResolver { get; set; } // Определение типа результата
    public string Description { get; set; } // Описание для отладки или документации
  }

  internal class CheckUnaryOperation
  {
    CheckUnaryOperationRule[] Rules;
    CompilerLogger Logger;

    public CheckUnaryOperation(CheckUnaryOperationRule[] rules, CompilerLogger logger)
    {
      Rules = rules;
      Logger = logger;
    }

    public bool Check(UnaryOperationNode op)
    {
      CheckUnaryOperationRule rule = FindRule(op);

      //Тип результата операции пока присваиваем в null
      op.ObjectType = null;

      // Если в данной ноде были обнаружены семантические ошибки
      // на предыдущих этапах, то просто выходим
      if (op.SemanticallyCorrect == false)
        return false;

      //Берем операнд
      TypedNode operand = op.GetOperand();

      if (CheckTypedNode(operand) == false)
      {
        op.SemanticallyCorrect = false;
        return false;
      }

      LCObjectType operandObjectType = operand.ObjectType;
      LCType operandType = operandObjectType.Type;

      if (operandObjectType.Readable == false)
      {
        Logger.Error(operand.Locate, string.Format("Неверный операнд оператора \"{0}\"", op.Description()));
        op.SemanticallyCorrect = false;
        return false;
      }

      if ((operandType is LCPrimitiveType primitiveType
        && LCTypesUtils.ItIsFromList(primitiveType, rule.OperandValidTypes)) == false)
      {
        string msg = "Неверный тип '{0}' операнда, ожидаемые типы: {1}";

        Logger.Error(op.Locate, string.Format(msg, operandType.ToString(),
          LCTypesUtils.PrimitiveTypeGetName(rule.OperandValidTypes)));

        op.SemanticallyCorrect = false;
        return false;
      }

      if (rule.OperandMustBeWritable && !operandObjectType.Writeable)
      {
        Logger.Error(operand.Locate, string.Format("Операнд оператора \"{0}\" должен поддерживать установку значения", op.Description()));
        op.SemanticallyCorrect = false;
        return false;
      }

      var operatorType = rule.ResultTypeResolver(operandType);
      if (operatorType == null)
        throw new InternalCompilerException("Семантическое правило вернуло неверный тип");

      op.ObjectType = new LCObjectType(operatorType, true, false);

      op.OperandsCType = operatorType.Clone();

      return true;
    }



    private CheckUnaryOperationRule FindRule(UnaryOperationNode op)
    {
      Type type = op.GetType();
      CheckUnaryOperationRule rule = null;

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

    private bool ScanTypes(CheckUnaryOperationRule rule, Type type)
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
