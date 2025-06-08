namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckTerminalID
  {
    public static bool Check(TerminalIdentifierNode n, UseDirectives uses, CompilerLogger Logger)
    {
      //Если на предыдущих этапах выявлены семантические ошибки в данной ноде,
      //то дальнейшую проверку не производим
      if (n.SemanticallyCorrect == false)
        return false;

      //Ищем объект в области видимости
      VariableDeclaratorNode declarator = TreeMISCWorkers.FindVariableDeclarator(n.TerminalID, n);

      //Если символ не найден в текущем модуле, то
      //пытаемся его найти среди подключенных модулей
      /*
       if (declarator == null)
        declarator = TreeMISCWorkers.FindSymbolInUses(n.TerminalID, uses);
      */
      //Если символ не найден
      if (declarator == null)
      {
        //Ошибка, символ не найден в текущей области видимости
        Logger.Error(n.Locate, string.Format(Resources.Messages.ErrorSymbolNotFound, n.TerminalID));

        n.SemanticallyCorrect = false;
        return false;
      }

      ObjectNode objectNode = new ObjectNode(declarator, n.Locate);

      //Заменяем ноду терминала на ноду константы
      n.Replace(objectNode);
      return true;
    }
  }
}
