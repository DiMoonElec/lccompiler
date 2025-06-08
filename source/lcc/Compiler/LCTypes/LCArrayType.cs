namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Массив
  /// </summary>
  internal class LCArrayType : LCArrayTypeGroup
  {
    /// <summary>
    /// Глубина массива
    /// </summary>
    public int ArrayDepth { get; private set; }

    public LCArrayType(LCPrimitiveType type, int arrayDepth)
    {
      TypeElement = type;
      ArrayDepth = arrayDepth;
    }

    public override LCType Clone()
    {
      return new LCArrayType(new LCPrimitiveType(TypeElement.Type), ArrayDepth);
    }

    public override string ToString()
    {
      return string.Format("{0}[{1}]", TypeElement.ToString(), ArrayDepth.ToString());
    }

    public override int Sizeof()
    {
      int elementSize = TypeElement.Sizeof();
      return ArrayDepth * elementSize;
    }
  }

}
