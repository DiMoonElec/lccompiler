using LC2.LCCompiler.Compiler.SemanticChecks;
using System;
using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler
{
  static class RunSemanticChecks
  {
    static public SymbolTable GetSymbolTable(Node tree, CompilerLogger logger,
      string moduleName)
    {
      bool isOK = true;

      List<ManagedFunctionDeclaratorNode> functions = new List<ManagedFunctionDeclaratorNode>();
      List<VariableDeclaratorNode> variables = new List<VariableDeclaratorNode>();
      List<StructDeclaratorNode> structs = new List<StructDeclaratorNode>();

      for (int i = 0; i < tree.CountChildrens; i++)
      {
        var child = tree.GetChild(i);
        if (child is ManagedFunctionDeclaratorNode function)
          functions.Add(function);
        else if (child is VariableDeclaratorNode variable)
          variables.Add(variable);
        else if (child is StructDeclaratorNode structDeclarator)
          structs.Add(structDeclarator);
      }

      var functionsArray = functions.ToArray();
      var variablesArray = variables.ToArray();
      var structsArray = structs.ToArray();

      //Проверка деклараторов
      if (checkFunctionsDeclarator(functionsArray, logger) == false)
        isOK = false;

      if (checkVariablesDeclarator(variablesArray, logger) == false)
        isOK = false;

      if (checkUsertypesDeclarator(structsArray, logger) == false)
        isOK = false;

      List<DeclaratorNode> PublicDeclarators = new List<DeclaratorNode>();

      for (int i = 0; i < tree.CountChildrens; i++)
      {
        var child = tree.GetChild(i);

        if ((child is DeclaratorNode declaratorNode)
          && !(declaratorNode is NativeFunctionDeclaratorNode))
          PublicDeclarators.Add(declaratorNode);
      }

      SymbolTable symbolTable = new SymbolTable(moduleName, PublicDeclarators.ToArray());
      return symbolTable;
    }

    /// <summary>
    /// Запуск семантических проверок
    /// </summary>
    /// <param name="tree">Семантическое дерево</param>
    /// <param name="symbols">Таблицы символов</param>
    /// <param name="logger">Логгер</param>
    /// <returns>True - семантических ошибок не обнаружено</returns>
    static public bool Run(Node tree, SymbolTable[] symbols, string moduleName, CompilerLogger logger)
    {
      var semanticCheck = new SemanticCheck(logger, moduleName, symbols);
      tree.Visit(semanticCheck);
      if (semanticCheck.PassOk == false)
        return false;

      return true;
    }



    private static bool checkUsertypesDeclarator(StructDeclaratorNode[] declarators, CompilerLogger logger)
    {
      bool isOK = true;

      for (int i = 0; i < declarators.Length; i++)
      {
        //Проверка уникальности имени
        if (checkDeclaratorName(declarators, i))
        {
          logger.Error(declarators[i].LocateName, "Пользовательский тип с таким именем уже объявлен в данном модуле");
          isOK = false;
        }

        //Семантическая проверка декларатора
        if (CheckDeclarator.CheckStruct(declarators[i], logger) == false)
          isOK = false;
      }

      return isOK;
    }

    private static bool checkVariablesDeclarator(VariableDeclaratorNode[] declarators, CompilerLogger logger)
    {
      bool isOK = true;

      for (int i = 0; i < declarators.Length; i++)
      {
        //Проверка уникальности имени
        if (checkDeclaratorName(declarators, i))
        {
          logger.Error(declarators[i].LocateName, "Переменная с таким именем уже объявлена в данном модуле");
          isOK = false;
        }

        //Семантическая проверка декларатора
        if (CheckDeclarator.CheckVariable(declarators[i], logger) == false)
          return false;
      }

      return isOK;
    }

    private static bool checkFunctionsDeclarator(ManagedFunctionDeclaratorNode[] declarators, CompilerLogger logger)
    {
      bool isOK = true;

      for (int i = 0; i < declarators.Length; i++)
      {
        //Проверка уникальности имени
        if (checkDeclaratorName(declarators, i))
        {
          logger.Error(declarators[i].LocateName, "Функция с таким именем уже объявлена в данном модуле");
          isOK = false;
        }

        //Семантическая проверка декларатора
        if (CheckDeclarator.CheckFunction(declarators[i], logger) == false)
          isOK = false;
      }

      return isOK;
    }

    /// <summary>
    /// Выполняет проверку уникальности имени декларатора с указанным индеком среди остальных деклараторов
    /// </summary>
    /// <param name="declarators">Все деклараторы</param>
    /// <param name="index">Индекс проверяемого декларатора</param>
    /// <returns>true - совпадение найдено, false - не найдено</returns>
    private static bool checkDeclaratorName(DeclaratorNode[] declarators, int index)
    {

      var di = declarators[index];
      for (int j = 1; j < declarators.Length; j++)
      {
        if (index == j)
          continue;

        var dj = declarators[j];

        if (dj.Name == di.Name)
          return true;
      }

      return false;
    }
  }
}
