namespace LC2.LCCompiler.Compiler
{
  internal class SymbolTable
  {
    public string ModuleName { get; private set; }

    public DeclaratorNode[] Declarators { get; private set; }

    public SymbolTable(string moduleName, DeclaratorNode[] declarators)
    {
      ModuleName = moduleName;
      Declarators = declarators;
    }
  }
}
