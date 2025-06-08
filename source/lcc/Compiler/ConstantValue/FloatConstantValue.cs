using System;

namespace LC2.LCCompiler.Compiler
{
  internal class FloatConstantValue : FloatingConstantValue
  {
    public float Value { get; private set; }

    public FloatConstantValue(float value) : base(new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeFloat))
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    public override ConstantValue Summ(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new FloatConstantValue(Value + rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue Sub(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new FloatConstantValue(Value - rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue Mul(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new FloatConstantValue(Value * rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue Div(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new FloatConstantValue(Value / rightfloatConstant.Value);

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
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value < rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue LessEqual(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value <= rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue More(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value > rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue MoreEqual(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value >= rightfloatConstant.Value);

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
      return null;
    }

    public override ConstantValue LogicalOr(ConstantValue rightValue)
    {
      return null;
    }

    public override ConstantValue Not()
    {
      return null;
    }

    public override ConstantValue LogicalNot()
    {
      return null;
    }

    public override ConstantValue Inv()
    {
      return new FloatConstantValue(-Value);
    }

    public override ConstantValue Incr()
    {
      return new FloatConstantValue(Value + 1);
    }

    public override ConstantValue Decr()
    {
      return new FloatConstantValue(Value - 1);
    }

    public override ConstantValue Eq(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value == rightfloatConstant.Value);

      return null;
    }

    public override ConstantValue Neq(ConstantValue rightValue)
    {
      if (rightValue is FloatConstantValue rightfloatConstant)
        return new BooleanConstantValue(Value != rightfloatConstant.Value);

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
            return new LongConstantValue((long)Value);
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
