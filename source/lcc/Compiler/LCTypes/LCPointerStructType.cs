namespace LC2.LCCompiler.Compiler
{
  internal class LCPointerStructType : LСStructTypeGroup
  {
    public LCPointerStructType(string name) : base(name) { }

    public override LCType Clone()
    {
      return new LCPointerStructType(TypeName);
    }

    public override int Sizeof()
    {
      return 4;
    }
  }
}
