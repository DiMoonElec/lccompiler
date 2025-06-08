namespace LC2.LCCompiler.Compiler.SemanticChecks.Checks
{
  static internal class CheckElementAccess
  {
    static internal bool Check(ElementAccessNode n, CompilerLogger logger)
    {
      //Получаем объект структуры
      Node obj = n.GetChild(0);

      if (obj.SemanticallyCorrect == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      if (obj is TypedNode accessingTypedNode)
      {
        var accessingObjectType = accessingTypedNode.ObjectType;
        var accessingType = accessingObjectType.Type;

        if (accessingType is LСStructTypeGroup userType)
        {
          //Если объектом является экземпляр структуры
          return CheckField(n, userType.StructDeclarator, n.Field, n.ExpressionLocate, logger);
        }
        else
        {
          //Если объектом является что-то иное
          n.SemanticallyCorrect = false;
          logger.Error(accessingTypedNode.Locate, "Неверный тип объектa");
          return false;
        }
      }
      else
      {
        n.SemanticallyCorrect = false;
        logger.Error(n.Locate, "Неверный тип объектa");
        return false;
      }
    }

    private static bool CheckField(ElementAccessNode n, LCStructDeclarator structType, string field,
      LocateElement expressionLocate, CompilerLogger logger)
    {
      var elements = structType.Elements;

      int elementOffset = 0;

      for (int i = 0; i < elements.Length; i++)
      {
        var e = elements[i];


        if (e.Name == field)
        {
          //Если нашли элемент с нужным именем
          if (e is LCStructElementPrimitiveType primitiveType)
            n.ObjectType = new LCObjectType(primitiveType.Type.Clone(), true, true);
          else if (e is LCStructElementArrayType arrayType)
            n.ObjectType = new LCObjectType(new LCArrayType(arrayType.Type.TypeElement.ClonePrimitiveType(), arrayType.Type.ArrayDepth), true, true);
            //n.ObjectType = new LCObjectType(new LCPointerArrayType(arrayType.Type.TypeElement.ClonePrimitiveType()), true, true);
          else
                throw new InternalCompilerException("Неверный тип элемента структуры");


          n.ElementOffset = elementOffset;
          return true;
        }
        else
          elementOffset += e.Sizeof();

      }
      n.SemanticallyCorrect = false;
      logger.Error(expressionLocate, string.Format("Поле '{0}' отсутствует в структуре '{1}'", field, structType.TypeName));
      return false;
    }
  }
}
