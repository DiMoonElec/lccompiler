using System;

namespace LC2.LCCompiler
{
  internal class InternalCompilerException : Exception
  {
    public InternalCompilerException(string msg) : base(msg) { }
  }

  internal class CompilationException : Exception
  { public CompilationException(string msg) : base(msg) { } }
}
