namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  internal static class CheckArgumentTypeValidationTypeCast
  {
    public static bool Check(TypeCastNode op, CompilerLogger Logger)
    {
      if (op.SemanticallyCorrect == false)
        return false;

      op.ObjectType = null;

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


      //Проверка типа операнда
      if (!(operandType is LCPrimitiveType primitiveType
        && primitiveType.Type != LCPrimitiveType.PrimitiveTypes.LCTypeVoid))
      {
        Logger.Error(operand.Locate, string.Format("Неверный тип операнда оператора \"{0}\"", op.Description()));
        op.SemanticallyCorrect = false;
        return false;
      }

      //Проветка приводимого значения
      if (!(op.CastType is LCPrimitiveType primitiveCastType &&
        primitiveCastType.Type != LCPrimitiveType.PrimitiveTypes.LCTypeVoid))
      {
        Logger.Error(op.Locate, string.Format("Неверный приводимый тип \"{0}\"", op.Description()));
        op.SemanticallyCorrect = false;
        return false;
      }

      op.ObjectType = new LCObjectType(op.CastType, true, false);

      op.OperandsCType = operandType.Clone();

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
  }
}
