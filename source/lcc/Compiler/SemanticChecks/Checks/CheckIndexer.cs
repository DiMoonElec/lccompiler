namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class CheckIndexer
  {
    static internal bool Check(IndexerNode n, CompilerLogger logger)
    {
      bool isOK = true;

      if (CheckIndex(n, logger) == false)
        isOK = false;

      if (CheckIndexingObject(n, logger) == false)
        isOK = false;

      n.SemanticallyCorrect = isOK;
      return isOK;
    }

    private static bool CheckIndex(IndexerNode n, CompilerLogger logger)
    {
      Node indexNode = n.Index.GetChild(0);

      if (indexNode.SemanticallyCorrect == false)
        return false;

      if (indexNode is TerminalIdentifierNode)
        return false;
      else if (indexNode is TypedNode index)
      {
        var indexObject = index.ObjectType;
        var indexType = indexObject.Type;

        if (indexObject.Readable == false)
        {
          logger.Error(index.Locate, "Неверный тип индекса");
          return false;
        }

        if (indexType is LCPrimitiveType primitiveObjectType)
        {
          switch (primitiveObjectType.Type)
          {
            case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
            case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
              var t = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeInt);
              TreeMISCWorkers.InsertTypeCast(t, index, logger);
              return true;

            case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
              return true;

            default:
              logger.Error(index.Locate, "В качестве индекса массива может выступать объект, типа \"int\", \"short\" или \"byte\"");
              return false;
          }

        }
        else
        {
          logger.Error(index.Locate, "В качестве индекса массива может выступать объект, типа \"int\", \"short\" или \"byte\"");
          return false;
        }
      }
      else
      {
        logger.Error(n.Locate, "В качестве индекса массива может выступать объект, типа \"int\", \"short\" или \"byte\"");
        return false;
      }
    }

    private static bool CheckIndexingObject(IndexerNode n, CompilerLogger logger)
    {
      //Индексируемый объект должен быть массивом либо ссылкой на массив

      Node indexingObj = n.IndexingObj.GetChild(0);

      if (indexingObj is TerminalIdentifierNode terminal) //Объект с таким именем не был найден
      {
        n.SemanticallyCorrect = false;
        return false;
      }
      else if (indexingObj is TypedNode typedIndexingObject)
      {
        if (typedIndexingObject.SemanticallyCorrect == false)
        {
          n.SemanticallyCorrect = false;
          return false;
        }

        var objectType = typedIndexingObject.ObjectType;
        var type = objectType.Type;

        if (type is LCArrayType arrayType)
        {
          n.ObjectType = new LCObjectType(arrayType.TypeElement, true, true);
        }
        else if (type is LCPointerArrayType pointerType)
        {
          n.ObjectType = new LCObjectType(pointerType.TypeElement, true, true);
        }
        else
        {
          n.SemanticallyCorrect = false;
          logger.Error(typedIndexingObject.Locate, "Данный объект нельзя индексировать");
          return false;
        }
      }
      else
      {
        n.SemanticallyCorrect = false;
        logger.Error(n.Locate, "Данный объект нельзя индексировать");
        return false;
      }

      return true;
    }
  }
}
