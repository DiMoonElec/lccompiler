using System;

namespace LC2.LCCompiler.Compiler.LCTypes
{
  internal static class ConstantTypeCast
  {
    /// <summary>
    /// Выполнить преобразование типа константы
    /// </summary>
    /// <param name="constant">Константа, тип которой необходимо преобразовать</param>
    /// <param name="primitiveType">Тип, к которому выполняется преобразование</param>
    /// <param name="result">Преобразованная константа</param>
    /// <returns>false если не возникло переполнение во время преобразования,
    /// true если возникло переполнение</returns>
    /// <exception cref="InternalCompilerException">Внутренняя ошибка компилятора</exception>
    public static bool Cast(ConstantValue constant,
                            LCPrimitiveType primitiveType,
                            out ConstantValue result)
    {
      if (constant is SByteConstantValue SByteConstant)
        return SByteConstantTypeCast(SByteConstant, primitiveType, out result);
      else if (constant is ShortConstantValue ShortConstant)
        return ShortConstantTypeCast(ShortConstant, primitiveType, out result);
      else if (constant is IntConstantValue IntConstant)
        return IntConstantTypeCast(IntConstant, primitiveType, out result);
      else if (constant is LongConstantValue LongConstant)
        return LongConstantTypeCast(LongConstant, primitiveType, out result);
      else if (constant is ByteConstantValue ByteConstant)
        return ByteConstantTypeCast(ByteConstant, primitiveType, out result);
      else if (constant is UShortConstantValue UShortConstant)
        return UShortConstantTypeCast(UShortConstant, primitiveType, out result);
      else if (constant is UIntConstantValue UIntConstant)
        return UIntConstantTypeCast(UIntConstant, primitiveType, out result);
      else if (constant is ULongConstantValue ULongConstant)
        return ULongConstantTypeCast(ULongConstant, primitiveType, out result);
      else if (constant is FloatConstantValue FloatConstant)
        return FloatConstantTypeCast(FloatConstant, primitiveType, out result);
      else if (constant is DoubleConstantValue DoubleConstant)
        return DoubleConstantTypeCast(DoubleConstant, primitiveType, out result);
      else if (constant is BooleanConstantValue BooleanConstant)
        return BoolConstantTypeCast(BooleanConstant, primitiveType, out result);
      else
        throw new InternalCompilerException("Неизвестный тип константы");
    }

    private static bool BoolConstantTypeCast(BooleanConstantValue booleanConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          result = new SByteConstantValue(booleanConstant.Value ? (sbyte)1 : (sbyte)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          result = new ShortConstantValue(booleanConstant.Value ? (short)1 : (short)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = new IntConstantValue(booleanConstant.Value ? (int)1 : (int)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(booleanConstant.Value ? (long)1 : (long)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          result = new ByteConstantValue(booleanConstant.Value ? (byte)1 : (byte)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          result = new UShortConstantValue(booleanConstant.Value ? (ushort)1 : (ushort)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          result = new UIntConstantValue(booleanConstant.Value ? (uint)1 : (uint)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          result = new ULongConstantValue(booleanConstant.Value ? (ulong)1 : (ulong)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(booleanConstant.Value ? (float)1 : (float)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(booleanConstant.Value ? (double)1 : (double)0);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          result = booleanConstant;
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool DoubleConstantTypeCast(DoubleConstantValue doubleConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (doubleConstant.Value < sbyte.MinValue || doubleConstant.Value > sbyte.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new SByteConstantValue((sbyte)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (doubleConstant.Value < short.MinValue || doubleConstant.Value > short.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new ShortConstantValue((short)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          if (doubleConstant.Value < int.MinValue || doubleConstant.Value > int.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new IntConstantValue((int)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          if (doubleConstant.Value < long.MinValue || doubleConstant.Value > long.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new LongConstantValue((long)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (doubleConstant.Value < byte.MinValue || doubleConstant.Value > byte.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new ByteConstantValue((byte)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (doubleConstant.Value < ushort.MinValue || doubleConstant.Value > ushort.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new UShortConstantValue((ushort)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (doubleConstant.Value < uint.MinValue || doubleConstant.Value > uint.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new UIntConstantValue((uint)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (doubleConstant.Value < ulong.MinValue || doubleConstant.Value > ulong.MaxValue || doubleConstant.Value % 1 != 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          Overrange = true;
          result = new FloatConstantValue((float)doubleConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = doubleConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (doubleConstant.Value != 0 && doubleConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(doubleConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool FloatConstantTypeCast(FloatConstantValue floatConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (floatConstant.Value < sbyte.MinValue || floatConstant.Value > sbyte.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new SByteConstantValue((sbyte)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (floatConstant.Value < short.MinValue || floatConstant.Value > short.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new ShortConstantValue((short)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          if (floatConstant.Value < int.MinValue || floatConstant.Value > int.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new IntConstantValue((int)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          if (floatConstant.Value < long.MinValue || floatConstant.Value > long.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new LongConstantValue((long)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (floatConstant.Value < byte.MinValue || floatConstant.Value > byte.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new ByteConstantValue((byte)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (floatConstant.Value < ushort.MinValue || floatConstant.Value > ushort.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new UShortConstantValue((ushort)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (floatConstant.Value < uint.MinValue || floatConstant.Value > uint.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new UIntConstantValue((uint)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (floatConstant.Value < ulong.MinValue || floatConstant.Value > ulong.MaxValue || floatConstant.Value % 1 != 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = floatConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(floatConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (floatConstant.Value != 0 && floatConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(floatConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool ULongConstantTypeCast(ULongConstantValue ulongConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (ulongConstant.Value > (ulong)sbyte.MaxValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (ulongConstant.Value > (ulong)short.MaxValue)
            Overrange = true;
          result = new ShortConstantValue((short)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          if (ulongConstant.Value > (ulong)int.MaxValue)
            Overrange = true;
          result = new IntConstantValue((int)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          if (ulongConstant.Value > long.MaxValue)
            Overrange = true;
          result = new LongConstantValue((long)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (ulongConstant.Value > byte.MaxValue)
            Overrange = true;
          result = new ByteConstantValue((byte)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (ulongConstant.Value > ushort.MaxValue)
            Overrange = true;
          result = new UShortConstantValue((ushort)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (ulongConstant.Value > uint.MaxValue)
            Overrange = true;
          result = new UIntConstantValue((uint)ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          result = ulongConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(ulongConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (ulongConstant.Value != 0 && ulongConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(ulongConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool UIntConstantTypeCast(UIntConstantValue uintConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (uintConstant.Value > sbyte.MaxValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (uintConstant.Value > short.MaxValue)
            Overrange = true;
          result = new ShortConstantValue((short)uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          if (uintConstant.Value > int.MaxValue)
            Overrange = true;
          result = new IntConstantValue((int)uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (uintConstant.Value > byte.MaxValue)
            Overrange = true;
          result = new ByteConstantValue((byte)uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (uintConstant.Value > ushort.MaxValue)
            Overrange = true;
          result = new UShortConstantValue((ushort)uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          result = uintConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          result = new ULongConstantValue(uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(uintConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (uintConstant.Value != 0 && uintConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(uintConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool UShortConstantTypeCast(UShortConstantValue ushortConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (ushortConstant.Value > sbyte.MaxValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (ushortConstant.Value > short.MaxValue)
            Overrange = true;
          result = new ShortConstantValue((short)ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = new IntConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (ushortConstant.Value > byte.MaxValue)
            Overrange = true;
          result = new ByteConstantValue((byte)ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          result = ushortConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          result = new UIntConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          result = new ULongConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(ushortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (ushortConstant.Value != 0 && ushortConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(ushortConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool ByteConstantTypeCast(ByteConstantValue byteConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (byteConstant.Value > sbyte.MaxValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          result = new ShortConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = new IntConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          result = byteConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          result = new UShortConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          result = new UIntConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          result = new ULongConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(byteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (byteConstant.Value != 0 && byteConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(byteConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool LongConstantTypeCast(LongConstantValue longConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (longConstant.Value > sbyte.MaxValue || longConstant.Value < sbyte.MinValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (longConstant.Value > short.MaxValue || longConstant.Value < short.MinValue)
            Overrange = true;
          result = new ShortConstantValue((short)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          if (longConstant.Value > int.MaxValue || longConstant.Value < int.MinValue)
            Overrange = true;
          result = new IntConstantValue((int)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = longConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (longConstant.Value > byte.MaxValue || longConstant.Value < byte.MinValue)
            Overrange = true;
          result = new ByteConstantValue((byte)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (longConstant.Value < 0)
            Overrange = true;
          result = new UShortConstantValue((ushort)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (longConstant.Value < 0)
            Overrange = true;
          result = new UIntConstantValue((uint)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (longConstant.Value < 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(longConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (longConstant.Value != 0 && longConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(longConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool IntConstantTypeCast(IntConstantValue intConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (intConstant.Value > sbyte.MaxValue || intConstant.Value < sbyte.MinValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          if (intConstant.Value > short.MaxValue || intConstant.Value < short.MinValue)
            Overrange = true;
          result = new ShortConstantValue((short)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = intConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (intConstant.Value > byte.MaxValue || intConstant.Value < byte.MinValue)
            Overrange = true;
          result = new ByteConstantValue((byte)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (intConstant.Value < 0)
            Overrange = true;
          result = new UShortConstantValue((ushort)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (intConstant.Value < 0)
            Overrange = true;
          result = new UIntConstantValue((uint)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (intConstant.Value < 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(intConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (intConstant.Value != 0 && intConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(intConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool ShortConstantTypeCast(ShortConstantValue shortConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          if (shortConstant.Value > sbyte.MaxValue || shortConstant.Value < sbyte.MinValue)
            Overrange = true;
          result = new SByteConstantValue((sbyte)shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          result = shortConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = new IntConstantValue(shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (shortConstant.Value > byte.MaxValue || shortConstant.Value < byte.MinValue)
            Overrange = true;
          result = new ByteConstantValue((byte)shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (shortConstant.Value < ushort.MinValue)
            Overrange = true;
          result = new UShortConstantValue((ushort)shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (shortConstant.Value < 0)
            Overrange = true;
          result = new UIntConstantValue((uint)shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (shortConstant.Value < 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(shortConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (shortConstant.Value != 0 && shortConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(shortConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }

    private static bool SByteConstantTypeCast(SByteConstantValue SByteConstant, LCPrimitiveType primitiveType, out ConstantValue result)
    {
      bool Overrange = false;

      switch (primitiveType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          result = SByteConstant;
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          result = new ShortConstantValue(SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          result = new IntConstantValue(SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          result = new LongConstantValue(SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          if (SByteConstant.Value < 0)
            Overrange = true;
          result = new ByteConstantValue((byte)SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          if (SByteConstant.Value < 0)
            Overrange = true;
          result = new UShortConstantValue((ushort)SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          if (SByteConstant.Value < 0)
            Overrange = true;
          result = new UIntConstantValue((uint)SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          if (SByteConstant.Value < 0)
            Overrange = true;
          result = new ULongConstantValue((ulong)SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          result = new FloatConstantValue(SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          result = new DoubleConstantValue(SByteConstant.Value);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          if (SByteConstant.Value != 0 && SByteConstant.Value != 1)
            Overrange = true;
          result = new BooleanConstantValue(SByteConstant.Value != 0);
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      return Overrange;
    }
  }
}
