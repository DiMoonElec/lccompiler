namespace LC2.LCCompiler.Compiler
{
  internal class LCStructType : LСStructTypeGroup
  {
    public LCStructType(string name) : base(name) { }

    public override LCType Clone()
    {
      return new LCStructType(TypeName);
    }

    public override int Sizeof()
    {
      return StructDeclarator.Sizeof();
    }
  }
}
