using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LC2.LCCompiler.Compiler
{
  internal static class AutoTypeCast
  {
    public static LCPrimitiveType Cast(LCPrimitiveType type1, LCPrimitiveType type2)
    {
      switch (type1.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          return SByte_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          return Short_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          return Int_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          return Long_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          return Byte_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          return UShort_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          return UInt_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          return ULong_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          return Float_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return Double_(type2);

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return Boolean_(type2);

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Boolean_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool);

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Double_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeDouble);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Float_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeFloat);

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType ULong_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeULong);

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType UInt_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUInt);

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType UShort_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUShort);

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Byte_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeByte);

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Long_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeLong);

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Int_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeInt);

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType Short_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeShort);

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }

    private static LCPrimitiveType SByte_(LCPrimitiveType type)
    {
      switch (type.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          return new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeSByte);

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return new LCPrimitiveType(type.Type);

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return null;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }
    }
  }
}
