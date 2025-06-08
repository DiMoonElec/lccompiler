namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Указатель
  /// </summary>
  internal class LCPointerArrayType : LCArrayTypeGroup
  {
    public LCPointerArrayType(LCPrimitiveType type)
    {
      TypeElement = type;
    }

    public override LCType Clone()
    {
      return new LCPointerArrayType(new LCPrimitiveType(TypeElement.Type));
    }

    public override string ToString()
    {
      return string.Format("{0}[]", TypeElement.ToString());
    }

    public override int Sizeof()
    {
      return 4;
    }
  }

}
