namespace LC2.LCCompiler.Compiler
{
  partial class DoNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// Начало цикла
    /// </summary>
    public string LabelDoBegin = null;

    /// <summary>
    /// Точка проверки условия, в эту точку выполняется переход по оператору continue
    /// </summary>
    public string LabelDoCondition = null;

    /// <summary>
    /// Выход из цикла
    /// </summary>
    public string LabelDoEnd = null;
  }
}
