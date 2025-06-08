using System;

namespace LC2.DebugInfo
{
  /// <summary>
  /// Отладочная информация программы
  /// </summary>
  [Serializable]
  public class DebugInformation
  {
    public DebugInformation() { }

    /// <summary>
    /// Имя проекта
    /// </summary>
    public string AssemblyName { get; internal set; }

    /// <summary>
    /// Название используемого компилятора
    /// </summary>
    public string CompilerName { get; internal set; }

    /// <summary>
    /// Версия компилятора
    /// </summary>
    public string CompilerVersion { get; internal set; }

    /// <summary>
    /// Строка, которая является точкой входа в main()
    /// </summary>
    //public DebugInformationLine ApplicationEntryLine { get; internal set; }

    /// <summary>
    /// Строки, на которые можно поставить точку останова
    /// </summary>
    public DebugInformationLine[] Lines { get; internal set; }

    public DebugInformationStatement[] Statements { get; internal set; }
  }

  /// <summary>
  /// Строка, на которую можно поставить Breakpoint
  /// </summary>
  [Serializable]
  public class DebugInformationLine
  {
    /// <summary>
    /// Номер строки, на которую можно поставить точку останова
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Адрес точки останова
    /// </summary>
    public UInt32 PC { get; set; }

    /// <summary>
    /// Имя модуля
    /// </summary>
    public string ModuleName { get; set; }
  }

  /// <summary>
  /// Выражение
  /// </summary>
  [Serializable]
  public class DebugInformationStatement
  {
    public DebugInformationStatement() { }

    /// <summary>
    /// Начальный адрес выражения
    /// </summary>
    public UInt32 LowPC { get; set; }

    /// <summary>
    /// Конечный адрес выражения
    /// </summary>
    public UInt32 HighPC { get; set; }

    public int StartLine { get; set; }
    public int StartColumn { get; set; }

    public int EndLine { get; set; }
    public int EndColumn { get; set; }

    /// <summary>
    /// Имя модуля
    /// </summary>
    public string ModuleName { get; set; }
  }

  /// <summary>
  /// Базовый класс переменной
  /// </summary>
  [Serializable]
  public abstract class VariableDbgInformation
  {
    public enum VariableType
    {
      TypeByte,
      TypeShort,
      TypeInt,
      TypeLong,
      TypeFloat,
      TypeDouble,
      TypeBool,
    };

    /// <summary>
    /// Имя переменной
    /// </summary>
    public string Name;

    /// <summary>
    /// Тип переменной
    /// </summary>
    public VariableType Type;

    /// <summary>
    /// Адрес/смещение переменной
    /// </summary>
    public UInt16 Address;
  }

  /// <summary>
  /// Публичные переменные, расположенные в глобальной памяти
  /// </summary>
  [Serializable]
  public class PublicVariableDbgInformation : VariableDbgInformation
  {

  }

  /// <summary>
  /// Приватные переменные, расположенные в глобальной памяти
  /// </summary>
  [Serializable]
  public class PrivateVariableDbgInformation : VariableDbgInformation
  {

  }

  /// <summary>
  /// Локальная переменная
  /// </summary>
  [Serializable]
  public class LocalVariableDbgInformation : VariableDbgInformation
  {
    /// <summary>
    /// Является ли данная переменная статической
    /// </summary>
    public bool IsStaticVariable { get; set; }

    /// <summary>
    /// Начальный адрес области видимости переменной
    /// </summary>
    public UInt32 LowPC { get; set; }

    /// <summary>
    /// Конечный адрес области видимости переменной
    /// </summary>
    public UInt32 HighPC { get; set; }
  }
}
