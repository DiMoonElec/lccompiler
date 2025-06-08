using System;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{

  static class ComplementaryOperatorSearch
  {
    internal static bool OperatorCase(LabelCaseNode n, CompilerLogger logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      //Case должен быть включен в ноду switch
      Type[][] comparePattern = new Type[][]
      {
        new Type[] {typeof(BlockCodeNode) },
        new Type[] {typeof(FieldNode) },
        new Type[] {typeof(SwitchNode) }
      };

      var node = TreeMISCWorkers.GetNodeUpPach(comparePattern, typeof(ModuleRootNode), n);

      if (node == null)
      {
        //Ошибка
        logger.Error(n.Locate, "Оператор 'case' должен быть вложен в тело оператора switch");

        n.SemanticallyCorrect = false;
        return false;
      }

      n.ClosestOperatorSwitch = (SwitchNode)node;

      return true;
    }

    internal static bool OperatorDefault(LabelDefaultNode n, CompilerLogger logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      //Default должен быть включен в ноду switch
      Type[][] comparePattern = new Type[][]
      {
        new Type[] {typeof(BlockCodeNode) },
        new Type[] {typeof(FieldNode) },
        new Type[] {typeof(SwitchNode) }
      };

      var node = TreeMISCWorkers.GetNodeUpPach(comparePattern, typeof(ModuleRootNode), n);

      if (node == null)
      {
        //Ошибка
        logger.Error(n.Locate, "Оператор 'default' должен быть вложен в тело оператора switch");

        n.SemanticallyCorrect = false;
        return false;
      }

      n.ClosestOperatorSwitch = (SwitchNode)node;

      return true;
    }

    internal static bool OperatorReturn(ReturnNode n, CompilerLogger logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      //Ищем функцию
      Node f = TreeMISCWorkers.UpFind(typeof(ManagedFunctionDeclaratorNode), typeof(ModuleRootNode), n);

      if (f == null)
      {
        logger.Error(n.Locate, "Оператор 'return' может использоваться только в теле функции");
        n.Function = null;
        n.SemanticallyCorrect = false;
        return false;
      }


      ManagedFunctionDeclaratorNode functionDeclaratorNode = (ManagedFunctionDeclaratorNode)f;
      n.Function = functionDeclaratorNode;
      return true;
    }

    internal static bool OperatorBreak(BreakNode n, CompilerLogger Logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      Type[] validOperators = {
        typeof(DoNode),
        typeof(WhileNode),
        typeof(ForNode),
        typeof(SwitchNode)};

      Node op = TreeMISCWorkers.UpFind(validOperators, typeof(ModuleRootNode), n);

      if (op == null)
      {
        Logger.Error(n.Locate, "Оператор 'break' должен быть сложен в тело оператора 'do', 'while', 'for' или 'switch'");

        n.SemanticallyCorrect = false;
        return false;
      }

      if (op is DoNode doNode)
        n.ClosestOperatorDo = doNode;
      else if (op is WhileNode whileNode)
        n.ClosestOperatorWhile = whileNode;
      else if (op is ForNode forNode)
        n.ClosestOperatorFor = forNode;
      else if (op is SwitchNode switchNode)
        n.ClosestOperatorSwitch = switchNode;

      n.ClosestOperator = op;
      return true;
    }

    internal static bool OperatorContinue(ContinueNode n, CompilerLogger Logger)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      Type[] validOperators = {
        typeof(DoNode),
        typeof(WhileNode),
        typeof(ForNode)};

      Node op = TreeMISCWorkers.UpFind(validOperators, typeof(ModuleRootNode), n);

      if (op == null)
      {
        Logger.Error(n.Locate, "Оператор 'continue' должен быть сложен в тело оператора 'do', 'while' или 'for'");

        n.SemanticallyCorrect = false;
        return false;
      }

      if (op is DoNode doNode)
        n.ClosestOperatorDo = doNode;
      else if (op is WhileNode whileNode)
        n.ClosestOperatorWhile = whileNode;
      else if (op is ForNode forNode)
        n.ClosestOperatorFor = forNode;

      n.ClosestOperator = op;
      return true;
    }

  }
}
