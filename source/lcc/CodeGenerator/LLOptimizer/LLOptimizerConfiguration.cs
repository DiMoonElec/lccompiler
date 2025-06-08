namespace LC2.LCCompiler.CodeGenerator
{
  /// <summary>
  /// Настройки низкоуровнего оптимизатора
  /// </summary>
  public class LLOptimizerConfiguration
  {
    /// <summary>
    /// Данный оптимизатор удаляет конструкции incr_fp 0, decr_fp 0
    /// </summary>
    public bool Opt001 = true;

    /// <summary>
    /// Данный оптимизатор удаляет мертвый код
    /// </summary>
    public bool Opt002 = true;

    /// <summary>
    /// Оптимизатор пытается заменить длинные вызовы функций call на короткие вызовы call_s и call_t
    /// </summary>
    public bool Opt003 = true;

    /// <summary>
    /// Оптимизатор пытается заменить длинные переходы jmp на короткие jmp_s и jmp_t
    /// </summary>
    public bool Opt004 = true;
  }
}
