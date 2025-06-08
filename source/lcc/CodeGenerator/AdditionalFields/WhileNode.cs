namespace LC2.LCCompiler.Compiler
{
  partial class WhileNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// Начало цикла
    /// </summary>
    public string LabelWhileBegin = null;

    /// <summary>
    /// Выход из цикла
    /// </summary>
    public string LabelWhileEnd = null;
  }
}
