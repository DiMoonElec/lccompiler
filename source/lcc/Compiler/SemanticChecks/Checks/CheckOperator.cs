namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckOperator
  {
    internal static bool CheckIf(IfNode n, CompilerLogger logger)
    {
      if (CheckCondition(n.Condition, n.ExpressionLocate, "if", logger) == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      return true;
    }

    internal static bool CheckWhile(WhileNode n, CompilerLogger logger)
    {
      if (CheckCondition(n.Condition, n.ExpressionLocate, "while", logger) == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      return true;
    }

    internal static bool CheckDo(DoNode n, CompilerLogger logger)
    {
      if (CheckCondition(n.Condition, n.ExpressionLocate, "do", logger) == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      return true;
    }

    internal static bool CheckFor(ForNode n, CompilerLogger logger)
    {
      bool isOK = true;

      if (n.Condition.CountChildrens != 0) //Если есть условие
      {
        if (CheckCondition(n.Condition, n.ExpressionLocate, "for", logger) == false)
        {
          n.SemanticallyCorrect = false;
          isOK = false;
        }
      }

      return isOK;
    }



    private static bool CheckCondition(FieldNode Condition, LocateElement ConditionLocate, string op, CompilerLogger logger)
    {
      var ex = Condition.GetChild(0);

      if (!(ex is TypedNode))
      {
        logger.Error(ConditionLocate, string.Format("Условие оператора '{0}' должно иметь тип 'bool'", op));
        return false;
      }

      TypedNode cond = (TypedNode)ex;

      if (cond.SemanticallyCorrect == false)
        return false;

      var o = cond.ObjectType.Type;

      if (o == null)
        return false;

      if ((o is LCPrimitiveType primitiveObjectType
        && primitiveObjectType.Type == LCPrimitiveType.PrimitiveTypes.LCTypeBool) == false)
      {
        logger.Error(cond.Locate, string.Format("Условие оператора '{0}' должно иметь тип 'bool'", op));
        return false;
      }

      return true;
    }
  }
}
