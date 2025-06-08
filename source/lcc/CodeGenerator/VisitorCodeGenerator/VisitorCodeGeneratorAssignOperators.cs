using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(AssignNode n)
    {
      //Если Statement содержит вызов функций,
      //то отладочную информацию целиком для данной ветки не генерируем
      CheckContainStatementNode(n);

      InsertComment(n);

      FullNodeStatementBegin(n);

      //PreVisit
      var left = n.GetOperandLeft();
      var right = n.GetOperandRight();

      if (left is ObjectNode leftObjectNode)
      {
        AssignObject(n, leftObjectNode, right);
      }
      else
      {
        //throw new NotImplementedException();
        AssignReference(n, left, right);
      }
    }
    
    #region Вспомогательные методы
    private void AssignObject(AssignNode n, ObjectNode leftObjectNode, TypedNode right)
    {
      right.AccessMethod = ResultAccessMethod.MethodGet;

      PartiallyNodeStatementBegin(n);

      //На стек будет положено значение
      right.Visit(this);

      var objDeclarator = leftObjectNode.ObjDeclaratorNode;
      var classValue = objDeclarator.ClassValue;

      //Размер объекта, в который производится сохранение результата
      var size = leftObjectNode.ObjectType.Type.Sizeof();


      //Если сохраняем базовый тип, то выбираем инструкцию, соответствующую размеру данных
      if (objDeclarator.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
      {
        string objName = GetGlobalLabelName(objDeclarator);
        GlobalMemoryObjectReference memoryObjectReference = new GlobalMemoryObjectReference(objName);
        assemblyUnit.GlobalAllocator.AddReference(memoryObjectReference);

        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_GSTORE_1(memoryObjectReference));
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_GSTORE_2(memoryObjectReference));
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_GSTORE_4(memoryObjectReference));
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_GSTORE_8(memoryObjectReference));
        else
          throw new InternalCompilerException("Неверный размер загружаемого объекта");
      }
      else
      {
        //Это локальный объект, используем инструкции STORE и созраняем по смещению

        var offset = objDeclarator.LocalMemoryObject.Address;

        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_STORE_1(offset));
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_STORE_2(offset));
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_STORE_4(offset));
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_STORE_8(offset));
        else
          throw new InternalCompilerException("Неверный размер загружаемого объекта");
      }

      StatementEnd(n);
    }

    private void AssignReference(AssignNode n, TypedNode left, TypedNode right)
    {
      left.AccessMethod = ResultAccessMethod.MethodGetReference;
      right.AccessMethod = ResultAccessMethod.MethodGet;

      //На стек будет положена ссылка на приемник данных
      left.Visit(this);

      PartiallyNodeStatementBegin(n);

      //На стек будет положено значение
      right.Visit(this);

      //Если сохраняем базовый тип, то выбираем инструкцию, соответствующую размеру данных
      switch (n.OperandsCType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_1());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_2());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_4());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_8());
          break;

        default:
          throw new InternalCompilerException("Неверный тип");
      }

      StatementEnd(n);
    }

    #endregion
  }
}
