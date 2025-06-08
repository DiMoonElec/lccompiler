using LC2.LCCompiler.CodeGenerator;

namespace LC2.LCCompiler.Compiler
{
  partial class VariableDeclaratorNode : ObjectDeclaratorNode
  {
    /// <summary>
    /// Объект, размещенный в локальном стеке. Доступ к глобальным объектам 
    /// осуществляется через ссылки
    /// </summary>
    public LocalMemoryObject LocalMemoryObject;
  }
}
