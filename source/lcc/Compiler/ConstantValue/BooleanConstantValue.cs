using System;

namespace LC2.LCCompiler.Compiler
{
  internal class BooleanConstantValue : ConstantValue
  {
    public bool Value { get; private set; }

    public BooleanConstantValue(bool value) : base(new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool))
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    public override ConstantValue Summ(ConstantValue value)
    {
      return null;
    }

    public override ConstantValue Sub(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Mul(ConstantValue rightValue)
    {
      return null;

    }

    public override ConstantValue Div(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Rem(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue RightShift(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue LeftShift(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Less(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue LessEqual(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue More(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue MoreEqual(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue And(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Xor(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Or(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue LogicalAnd(ConstantValue rightValue)
    {
      if (rightValue is BooleanConstantValue rightBoolConstant)
      {
        return new BooleanConstantValue(Value && rightBoolConstant.Value);
      }

      return null;
    }

    public override ConstantValue LogicalOr(ConstantValue rightValue)
    {
      if (rightValue is BooleanConstantValue rightBoolConstant)
      {
        return new BooleanConstantValue(Value || rightBoolConstant.Value);
      }

      return null;
    }

    public override ConstantValue Not()
    {
      return null;
    }

    public override ConstantValue LogicalNot()
    {
      return new BooleanConstantValue(!Value);
    }

    public override ConstantValue Inv()
    {
      return null;
    }

    public override ConstantValue Incr()
    {
      return null;
    }

    public override ConstantValue Decr()
    {
      return null;
    }

    public override ConstantValue Eq(ConstantValue rightValue)
    {
      if (rightValue is BooleanConstantValue rightBoolConstant)
      {
        return new BooleanConstantValue(Value == rightBoolConstant.Value);
      }

      return null;
    }

    public override ConstantValue Neq(ConstantValue rightValue)
    {
      if (rightValue is BooleanConstantValue rightBoolConstant)
      {
        return new BooleanConstantValue(Value != rightBoolConstant.Value);
      }

      return null;
    }

    public override ConstantValue TypeConvert(LCType toType)
    {
      if (toType is LCPrimitiveType primitiveType)
      {
        switch (primitiveType.Type)
        {
          case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
            return new SByteConstantValue((sbyte)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
            return new ShortConstantValue((short)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
            return new IntConstantValue(Value ? 1 : 0);
          case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
            return new LongConstantValue(Value ? 1 : 0);
          case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
            return new ByteConstantValue((byte)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
            return new UShortConstantValue((ushort)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
            return new UIntConstantValue((uint)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
            return new ULongConstantValue((ulong)(Value ? 1 : 0));
          case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
            return new FloatConstantValue(Value ? 1 : 0);
          case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
            return new DoubleConstantValue(Value ? 1 : 0);
          case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
            return new BooleanConstantValue(Value);
        }
      }
      return null; // Если тип не поддерживается
    }

    public override byte[] GetDump()
    {
      return BitConverter.GetBytes(Value);
    }
  }

}
