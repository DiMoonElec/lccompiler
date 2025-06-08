using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  static class ParserStructDeclaration
  {
    static public bool Parse(LCLangParser.StructDeclarationContext context, CompilerLogger logger,
      out LCStructDeclarator structDeclarator, out LCStructTypeLocate structTypeLocate)
    {
      bool isOK = true;

      var userTypeNameContext = context.Identifier(); //Имя структуры
      string userTypeName = userTypeNameContext.Symbol.Text;

      LocateElement userTypeNameLocate = new LocateElement(userTypeNameContext);

      //Элементы структуры
      var structDeclarationElementsContext = context.structDeclarationElements().structDeclarationElement();

      List<LCStructTypeElement> structElements = new List<LCStructTypeElement>();

      List<LCTypeLocate> elementTypesLocate = new List<LCTypeLocate>();
      List<LocateElement> elementNamesLocate = new List<LocateElement>();

      for (int i = 0; i < structDeclarationElementsContext.Length; i++)
      {
        var e = structDeclarationElementsContext[i];

        var structElementNameContext = e.Identifier();
        var structElementTypeContext = e.lcStructElementType();

        //Имя элемента структуры
        string structElementName = structElementNameContext.Symbol.Text;
        LocateElement structElementNameLocate = new LocateElement(structElementNameContext);
        elementNamesLocate.Add(structElementNameLocate);

        //Парсим тип структуры
        var structElementPrimitiveTypeContext = structElementTypeContext.lcPrimitiveType();
        var structElementArrayPrimitiveTypeContext = structElementTypeContext.lcArrayPrimitiveType();

        if (structElementPrimitiveTypeContext != null)
        {
          LCPrimitiveType structElementPrimitiveType;
          LocateElement structElementPrimitiveTypeLocate;

          ParserPrimitiveType.ParsePrimitiveType(structElementPrimitiveTypeContext,
            out structElementPrimitiveType, out structElementPrimitiveTypeLocate);

          LCStructElementPrimitiveType structElementType =
            new LCStructElementPrimitiveType(structElementPrimitiveType, structElementName);

          structElements.Add(structElementType);
          elementTypesLocate.Add(new LCTypeLocate(structElementPrimitiveTypeLocate));
        }
        else if (structElementArrayPrimitiveTypeContext != null)
        {
          LCArrayType structElementArrayPrimitiveType;
          LCTypeLocate structElementArrayPrimitiveTypeLocate;


          var r = ParserVariableType.ParseArrayPrimitiveType(structElementArrayPrimitiveTypeContext, logger,
            out structElementArrayPrimitiveType, out structElementArrayPrimitiveTypeLocate);
          if (r == false)
          {
            isOK = false;
          }
          else
          {
            LCStructElementArrayType structElementType =
              new LCStructElementArrayType(structElementArrayPrimitiveType, structElementName);

            structElements.Add(structElementType);
            elementTypesLocate.Add(structElementArrayPrimitiveTypeLocate);
          }
        }
        else
          throw new InternalCompilerException("Неизвестный тип элемента структуры");
      }

      structDeclarator = new LCStructDeclarator(userTypeName, structElements.ToArray());
      structTypeLocate = new LCStructTypeLocate(elementTypesLocate.ToArray(), elementNamesLocate.ToArray(), userTypeNameLocate);

      return isOK;
    }
  }
}
