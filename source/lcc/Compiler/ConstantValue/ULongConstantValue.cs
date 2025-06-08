using System;

namespace LC2.LCCompiler.Compiler
{
  internal class ULongConstantValue : IntegerConstantValue
  {
    public ulong Value { get; private set; }

    public ULongConstantValue(ulong value)
        : base(new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeULong))
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    public override ConstantValue Summ(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value + rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Sub(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value - rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Mul(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value * rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Div(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value / rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Rem(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value % rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue RightShift(ConstantValue rightValue)
    {
      if (rightValue is ByteConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value >> (int)rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue LeftShift(ConstantValue rightValue)
    {
      if (rightValue is ByteConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value << (int)rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Less(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value < rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue LessEqual(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value <= rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue More(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value > rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue MoreEqual(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value >= rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue And(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value & rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Xor(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value ^ rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Or(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new ULongConstantValue(Value | rightIntegerConstant.Value);

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
      return new ULongConstantValue(~Value);
    }

    public override ConstantValue LogicalNot()
    {
      return new BooleanConstantValue(Value == 0);
    }

    public override ConstantValue Inv()
    {
      return null;
    }

    public override ConstantValue Incr()
    {
      return new ULongConstantValue(Value + 1);
    }

    public override ConstantValue Decr()
    {
      return new ULongConstantValue(Value - 1);
    }

    public override ConstantValue Eq(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value == rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue Neq(ConstantValue rightValue)
    {
      if (rightValue is ULongConstantValue rightIntegerConstant)
        return new BooleanConstantValue(Value != rightIntegerConstant.Value);

      return null;
    }

    public override ConstantValue TypeConvert(LCType toType)
    {
      if (toType is LCPrimitiveType primitiveType)
      {
        switch (primitiveType.Type)
        {
          case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
            return new ULongConstantValue(Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
            return new UIntConstantValue((uint)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
            return new UShortConstantValue((ushort)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
            return new ByteConstantValue((byte)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
            return new IntConstantValue((int)Value);
          case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
            return new LongConstantValue((long)Value);
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
