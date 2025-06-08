using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler
{
  internal class UseDirective
  {
    /// <summary>
    /// Имя подключаемого модуля
    /// </summary>
    public string UseModule { get; private set; }

    /// <summary>
    /// Размещение названия подключаемого модуля в исходном тексте
    /// </summary>
    public LocateElement UseModuleLocate { get; private set; }

    /// <summary>
    /// Таблица символов данного модуля
    /// </summary>
    public SymbolTable Table = null;

    public UseDirective(string useModule, LocateElement useModuleLocate)
    {
      UseModule = useModule;
      UseModuleLocate = useModuleLocate;
    }
  }
  internal class UseDirectives
  {
    public List<UseDirective> UseModule = new List<UseDirective>();
  }
}
