namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  internal static class CheckArgumentTypeValidationReturn
  {
    public static bool CheckOperatorReturn(ReturnNode op, CompilerLogger Logger)
    {
      if (op.SemanticallyCorrect == false)
        return false;


      if (op.Function == null)
      {
        //Если функция, соответвтующая оператору return не была найдена
        //то выходим
        op.SemanticallyCorrect = false;
        return false;
      }

      var functionObjectType = op.Function.ReturnType; //Тип, возвращаемый функцией
      var operand = op.GetOperand(); //Тип операнда

      if (functionObjectType.Type is LCPrimitiveType primitiveType)
      {
        if (primitiveType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeVoid)
        {
          //Если функция возвращает void

          if (operand == null)
          {
            //И если у return нет аргумента (конструкция return;)
            //то все OK
            return true;
          }
          else
          {
            Logger.Error(op.Locate, "Функция не возвращает какое-либо значение");
            op.SemanticallyCorrect = false;
            return false;
          }
        }
        else
        {
          //Функция возвращает значение, отличное от void
          if (operand == null)
          {
            //Если у return нет аргумента
            Logger.Error(op.Locate, string.Format("Оператор 'return' должен возвращать тип '{0}'", primitiveType.ToString()));
            op.SemanticallyCorrect = false;
            return false;
          }
          else
          {
            //У return есть какой-то аргумент
            //Проверяем этот аргумент
            if (CheckTypedNode(operand) == false)
            {
              //Если аргумент семантически некорректен
              //то выходим
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

            //Типы аргумента оператора return и
            //тип возвращаемого значения функции
            //должны быть одинаковыми
            if (LCTypesUtils.IsEqual(operandType, primitiveType) == false)
            {
              Logger.Error(op.Locate, string.Format("Тип возвращаемого значения оператором 'return' должен быть \"{0}\"", primitiveType.ToString()));
              op.SemanticallyCorrect = false;
              return false;
            }

            return true;
          }
        }
      }
      else
        throw new InternalCompilerException("Неверный тип возвращаемый функцией");
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
  }
}
