using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  internal static class BuilderConstantValueNode
  {
    public static bool BuildIntConstantNode(ulong value, LocateElement locate, 
      CompilerLogger logger, out ConstantValueNode node)
    {
      bool isOK = true;
      ConstantValue v;

      if (value <= (ulong)sbyte.MaxValue)
        v = new SByteConstantValue((sbyte)value);
      else if (value <= (ulong)short.MaxValue)
        v = new ShortConstantValue((short)value);
      else if (value <= (ulong)int.MaxValue)
        v = new IntConstantValue((int)value);
      else if (value <= (ulong)long.MaxValue)
        v = new LongConstantValue((long)value);
      else
      {
        v = new LongConstantValue((long)value);
        logger.Error(locate, "Значение константы слишком велико");
        isOK = false;
      }

      node = new ConstantValueNode(v, locate);

      return isOK;
    }

    public static void BuildFloatConstantNode(double value, LocateElement locate,
      CompilerLogger logger, out ConstantValueNode node)
    {
      ConstantValue c = new DoubleConstantValue(value);
      node = new ConstantValueNode(c, locate);
    }
  }
}
