using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler
{
  internal class CompiledUnit
  {
    /// <summary>
    /// Имя модуля
    /// </summary>
    public string ModuleName;

    /// <summary>
    /// Путь к файлу модуля, относительно каталоге проекта
    /// </summary>
    public string ModuleRelativePath;

    /// <summary>
    /// Абсолютный путь к файлу модуля
    /// </summary>
    public string ModuleFullPath;

    /// <summary>
    /// Лог компиляции данного модуля
    /// </summary>
    //public CompilerLogger logger;

    /// <summary>
    /// Таблица символов данного модуля
    /// </summary>
    public SymbolTable symbolTable;

    /// <summary>
    /// Семантическое дерево данного модуля
    /// </summary>
    public Node semanticTree;

    /// <summary>
    /// Объектный файл данного модуля
    /// </summary>
    public AssemblyUnit assemblyUnit;
  }
}
