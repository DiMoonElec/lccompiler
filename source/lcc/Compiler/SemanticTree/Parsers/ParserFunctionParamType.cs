namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  internal static class ParserFunctionParamType
  {
    public static void Parse(LCLangParser.LcFunctionParamTypeContext context,
      out LCType type, out LocateElement typeLocate)
    {
      var primitiveTypeContext = context.lcPrimitiveType();
      var refArrayPrimitiveTypeContext = context.lcRefArrayPrimitiveType();
      var userTypeContext = context.lcUserType();

      if (primitiveTypeContext != null)
      {
        LCPrimitiveType primitiveType;
        ParserPrimitiveType.ParsePrimitiveType(primitiveTypeContext, out primitiveType, out typeLocate);

        type = primitiveType;
      }
      else if (refArrayPrimitiveTypeContext != null)
      {
        var arrayTypeContext = refArrayPrimitiveTypeContext.lcPrimitiveType();

        LCPrimitiveType primitiveType;
        ParserPrimitiveType.ParsePrimitiveType(arrayTypeContext, out primitiveType, out typeLocate);

        type = new LCPointerArrayType(primitiveType);
      }
      else if (userTypeContext != null)
      {
        var userTypeNameContext = userTypeContext.Identifier();
        var userTypeName = userTypeNameContext.Symbol.Text;

        type = new LCPointerStructType(userTypeName);
        typeLocate = new LocateElement(userTypeNameContext);
      }
      else
        throw new InternalCompilerException("Неизвестная категория типа данных");
    }
  }
}
