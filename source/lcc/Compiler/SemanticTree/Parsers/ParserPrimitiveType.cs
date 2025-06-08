namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  static internal class ParserPrimitiveType
  {
    public static void ParsePrimitiveType(LCLangParser.LcPrimitiveTypeContext context,
      out LCPrimitiveType primitiveType, out LocateElement locate)
    {
      locate = new LocateElement(context.TypeName);

      switch (context.TypeName.Type)
      {
        case LCLangLexer.SByte:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeSByte);
          break;

        case LCLangLexer.Short:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeShort);
          break;

        case LCLangLexer.Int:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeInt);
          break;

        case LCLangLexer.Long:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeLong);
          break;

        case LCLangLexer.Byte:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeByte);
          break;

        case LCLangLexer.UShort:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUShort);
          break;

        case LCLangLexer.UInt:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUInt);
          break;

        case LCLangLexer.ULong:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeULong);
          break;

        case LCLangLexer.Float:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeFloat);
          break;

        case LCLangLexer.Double:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeDouble);
          break;

        case LCLangLexer.Bool:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool);
          break;

        case LCLangLexer.Void:
          primitiveType = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeVoid);
          break;

        default:
          throw new InternalCompilerException(string.Format("Неизвестный тип данных \"{0}\"", context.TypeName.Text));
      }
    }
  }
}
