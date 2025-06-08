using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckUses
  {
    /// <summary>
    /// Выполнить семантическую проверку списка используемых модулей
    /// </summary>
    /// <param name="uses">Используемые модули в данном модуле</param>
    /// <param name="moduleName">Путь к текущему модулю</param>
    /// /// <param name="symbols">Глобальная таблица символов</param>
    /// <param name="logger">Логгер</param>
    /// <returns>True - если семантических ошибок не обнаружено</returns>
    static public bool Check(UseDirectives uses, string moduleName,
      SymbolTable[] symbols, CompilerLogger logger)
    {
      bool isOK = true;

      if (uses == null)
        return true;

      for (int i = 0; i < uses.UseModule.Count; i++)
      {
        var u = uses.UseModule[i];

        //Проверка на включение в use самого себя
        if (u.UseModule == moduleName)
        {
          logger.Error(u.UseModuleLocate, Resources.Messages.ErrorRecurrentUSE);
          isOK = false;
        }

        //Проверка наличия модуля в проекте
        var currTable = TreeMISCWorkers.FindSymbolTable(symbols, u.UseModule);
        if (currTable == null)
        {
          logger.Error(u.UseModuleLocate,
            string.Format(Resources.Messages.ErrorModuleNotFound, u.UseModule));
          isOK = false;
        }
        else
        {
          u.Table = currTable;
        }
      }

      //Получаем таблицу символов для своего модуля
      var myTable = TreeMISCWorkers.FindSymbolTable(symbols, moduleName);

      for (int i = 0; i < uses.UseModule.Count; i++)
      {
        var currentUse = uses.UseModule[i];

        for (int k = i + 1; k < uses.UseModule.Count; k++)
        {
          var nextUse = uses.UseModule[k];

          if (ContainsIdenticalObjects(currentUse, nextUse, logger) == true)
            isOK = false;
        }

        CheckOverrides(myTable, currentUse, logger);
      }

      return isOK;
    }

    /// <summary>
    /// Проверяет случаи, когда объект с одинаковым именем объявлен в текущем и подключаемом модуле
    /// </summary>
    /// <param name="myTable">Таблица символов текущего модуля</param>
    /// <param name="use">Подключенный модуль</param>
    /// <param name="logger">Логгер</param>
    private static void CheckOverrides(SymbolTable myTable, UseDirective use, 
      CompilerLogger logger)
    {
      if (use.Table == null)
        return;

      var useTable = use.Table;

      for (int i = 0; i < myTable.Declarators.Length; i++)
      {
        var myDeclarator = myTable.Declarators[i];

        for (int k = 0; k < useTable.Declarators.Length; k++)
        {
          var useDeclarator = useTable.Declarators[k];

          if(myDeclarator.Name == useDeclarator.Name)
          {
            logger.Warning(myDeclarator.LocateName,
              string.Format(Resources.Messages.WarningObjectOverridden,
              myDeclarator.Name, use.UseModule));
          }
        }
      }
    }

    /// <summary>
    /// Выполняет поиск объектов с одинаковыми именами в двух подключенных модулях
    /// </summary>
    /// <param name="x">Первый модуль</param>
    /// <param name="y">Второй модуль</param>
    /// <param name="logger">Логгер</param>
    /// <returns>true - в модулях x и y обнаружены объекты с одинаковыми именами</returns>
    private static bool ContainsIdenticalObjects(UseDirective x, UseDirective y,
      CompilerLogger logger)
    {
      bool result = false;

      if (x.Table == null || y.Table == null)
        return true;

      for (int i = 0; i < x.Table.Declarators.Length; i++)
      {
        var declarator1 = x.Table.Declarators[i];

        for (int k = 0; k < y.Table.Declarators.Length; k++)
        {
          var declarator2 = y.Table.Declarators[k];
          if (declarator1.Name == declarator2.Name)
          {
            logger.Error(x.UseModuleLocate,
              string.Format(Resources.Messages.ErrorUseModuleObjectConflict,
              x.UseModule, y.UseModule, declarator1.Name));

            result = true;
          }
        }
      }

      return result;
    }
  }
}
