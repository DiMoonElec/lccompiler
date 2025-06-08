namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  internal static class ParserVariableType
  {
    internal static bool Parse(LCLangParser.LcVariableTypeContext context, CompilerLogger logger,
      out LCType type, out LCTypeLocate typeLocate)
    {
      var primitiveTypeContext = context.lcPrimitiveType();
      var arrayPrimitiveTypeContext = context.lcArrayPrimitiveType();
      var userTypeContext = context.lcUserType();

      if (primitiveTypeContext != null)
      {
        LCPrimitiveType primitiveType;
        LocateElement primitiveTypeLocate;
        ParserPrimitiveType.ParsePrimitiveType(primitiveTypeContext, out primitiveType, out primitiveTypeLocate);

        type = primitiveType;
        typeLocate = new LCTypeLocate(primitiveTypeLocate);
        return true;
      }
      else if (arrayPrimitiveTypeContext != null)
      {
        LCArrayType arrayType;

        var r = ParseArrayPrimitiveType(arrayPrimitiveTypeContext, logger, out arrayType, out typeLocate);
        type = arrayType;
        return r;
      }
      else if (userTypeContext != null)
      {
        var userTypeNameContext = userTypeContext.Identifier();
        var userTypeName = userTypeNameContext.Symbol.Text;

        type = new LCStructType(userTypeName);
        typeLocate = new LCTypeLocate(new LocateElement(userTypeNameContext));
        return true;
      }

      throw new InternalCompilerException("Неизвестный тип переменной");
    }

    internal static bool ParseArrayPrimitiveType(LCLangParser.LcArrayPrimitiveTypeContext context,
      CompilerLogger logger,
      out LCArrayType type, out LCTypeLocate typeLocate)
    {
      //Берем элементы дерева разбора
      var primitiveTypeContext = context.lcPrimitiveType();
      var intConstant = context.intConstant();
      var RightBracket = context.RightBracket();

      //Парсим тип массива
      LCPrimitiveType primitiveType;
      LocateElement primitiveTypeLocate;
      ParserPrimitiveType.ParsePrimitiveType(primitiveTypeContext, out primitiveType, out primitiveTypeLocate);

      //Парсим глубину массива
      ulong arrayDepth;
      LocateElement arrayDepthLocate;
      if (ParserConstants.ParseIntConstant(intConstant, logger, out arrayDepth, out arrayDepthLocate) == false)
      {
        type = null;
        typeLocate = null;
        return false;
      }

      //проверяем значение глубины массива
      if (arrayDepth > int.MaxValue)
      {
        logger.Error(arrayDepthLocate, "Глубина массива слишком большая");
        type = null;
        typeLocate = null;
        return false;
      }

      //Создаем тип массива
      type = new LCArrayType(primitiveType, (int)arrayDepth);

      //Создаем расположение
      typeLocate = new LCArrayTypeLocate(
        new LocateElement(primitiveTypeLocate, new LocateElement(RightBracket)),
        primitiveTypeLocate,
        arrayDepthLocate);

      return true;
    }
  }
}
