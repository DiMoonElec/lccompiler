namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Базовый тип
  /// </summary>
  internal class LCPrimitiveType : LCType
  {
    public enum PrimitiveTypes
    {
      LCTypeSByte,
      LCTypeShort,
      LCTypeInt,
      LCTypeLong,
      
      LCTypeByte,
      LCTypeUShort,
      LCTypeUInt,
      LCTypeULong,
      
      LCTypeFloat,
      LCTypeDouble,
      LCTypeBool,
      LCTypeVoid
    }

    /// <summary>
    /// Примитивный тип данных
    /// </summary>
    public PrimitiveTypes Type { get; private set; }

    public LCPrimitiveType(PrimitiveTypes type)
    {
      Type = type;
    }

    public override LCType Clone()
    {
      return new LCPrimitiveType(Type);
    }

    public LCPrimitiveType ClonePrimitiveType()
    {
      return new LCPrimitiveType(Type);
    }

    public override string ToString()
    {
      return LCTypesUtils.PrimitiveTypeGetName(Type);
    }

    public override int Sizeof()
    {
      switch (Type)
      {
        case PrimitiveTypes.LCTypeSByte:
        case PrimitiveTypes.LCTypeByte:
        case PrimitiveTypes.LCTypeBool:
          return 1;

        case PrimitiveTypes.LCTypeShort:
        case PrimitiveTypes.LCTypeUShort:
          return 2;

        case PrimitiveTypes.LCTypeInt:
        case PrimitiveTypes.LCTypeUInt:
        case PrimitiveTypes.LCTypeFloat:
          return 4;

        case PrimitiveTypes.LCTypeLong:
        case PrimitiveTypes.LCTypeULong:
        case PrimitiveTypes.LCTypeDouble:
          return 8;

        case PrimitiveTypes.LCTypeVoid:
          return 0;

        default:
          throw new InternalCompilerException("Неизвестный тип данных");
      }
    }
  }

}
