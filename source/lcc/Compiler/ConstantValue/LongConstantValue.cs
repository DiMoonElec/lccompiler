using System;

namespace LC2.LCCompiler.Compiler
{
  internal class LongConstantValue : IntegerConstantValue
  {
    public long Value { get; private set; }

    public LongConstantValue(long value) : base(new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeLong))
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }


    public override ConstantValue Summ(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value + rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Sub(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value - rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Mul(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value * rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Div(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value / rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Rem(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value % rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue RightShift(ConstantValue rightValue)
    {
      if (rightValue is ByteConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value >> (int)rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue LeftShift(ConstantValue rightValue)
    {
      if (rightValue is ByteConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value << (int)rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Less(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value < rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue LessEqual(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value <= rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue More(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value > rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue MoreEqual(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value >= rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue And(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value & rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Xor(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value ^ rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue Or(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new LongConstantValue((long)(Value | rightIntegerConstant.Value));

      return null;
    }

    public override ConstantValue LogicalAnd(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue LogicalOr(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Not()
    {
      return new LongConstantValue((long)(~Value));
    }

    public override ConstantValue LogicalNot()
    {
      return null;
    }

    public override ConstantValue Inv()
    {
      return new LongConstantValue((long)(-Value));
    }

    public override ConstantValue Incr()
    {
      return new LongConstantValue((long)(Value + 1));
    }

    public override ConstantValue Decr()
    {
      return new LongConstantValue((long)(Value - 1));
    }

    public override ConstantValue Eq(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value == rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Neq(ConstantValue rightValue)
    {
      if (rightValue is LongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value != rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue TypeConvert(LCType toType)
    {
      if (toType is LCPrimitiveType primitiveType)
      {
        switch (primitiveType.Type)
        {
          case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
            return new SByteConstantValue((sbyte)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
            return new ShortConstantValue((short)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
            return new IntConstantValue((int)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
            return new LongConstantValue(Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
            return new ByteConstantValue((byte)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
            return new UShortConstantValue((ushort)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
            return new UIntConstantValue((uint)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
            return new ULongConstantValue((ulong)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
            return new FloatConstantValue(Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
            return new DoubleConstantValue(Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
            return new BooleanConstantValue(Value != 0);
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
