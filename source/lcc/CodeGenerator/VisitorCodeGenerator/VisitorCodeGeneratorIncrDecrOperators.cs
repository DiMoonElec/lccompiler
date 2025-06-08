using System;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(PrefixIncrementNode n)
    {
      InstructionDelegates instructions = new InstructionDelegates();

      instructions.opSByte = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_SByte); };
      instructions.opShort = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Short); };
      instructions.opInt = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Int); };
      instructions.opLong = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Long); };

      instructions.opByte = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Byte); };
      instructions.opUShort = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_UShort); };
      instructions.opUInt = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_UInt); };
      instructions.opULong = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_ULong); };

      instructions.opFloat = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Float); };
      instructions.opDouble = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Double); };

      ArithmeticPrefixIncrDecrOperationGenerate(n, instructions);
    }

    public override void Visit(PrefixDecrementNode n)
    {
      InstructionDelegates instructions = new InstructionDelegates();

      instructions.opSByte = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_SByte); };
      instructions.opShort = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Short); };
      instructions.opInt = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Int); };
      instructions.opLong = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Long); };

      instructions.opByte = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Byte); };
      instructions.opUShort = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_UShort); };
      instructions.opUInt = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_UInt); };
      instructions.opULong = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_ULong); };

      instructions.opFloat = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Float); };
      instructions.opDouble = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Double); };

      ArithmeticPrefixIncrDecrOperationGenerate(n, instructions);
    }

    public override void Visit(PostfixIncrementNode n)
    {
      InstructionDelegates instructions = new InstructionDelegates();

      instructions.opSByte = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_SByte); };
      instructions.opShort = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Short); };
      instructions.opInt = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Int); };
      instructions.opLong = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Long); };

      instructions.opByte = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Byte); };
      instructions.opUShort = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_UShort); };
      instructions.opUInt = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_UInt); };
      instructions.opULong = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_ULong); };

      instructions.opFloat = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Float); };
      instructions.opDouble = delegate () { return new INSTR_INCR(LCVM_DataTypes.Type_Double); };

      ArithmeticPostfixIncrDecrOperationGenerate(n, instructions);
    }

    public override void Visit(PostfixDecrementNode n)
    {
      InstructionDelegates instructions = new InstructionDelegates();

      instructions.opSByte = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_SByte); };
      instructions.opShort = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Short); };
      instructions.opInt = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Int); };
      instructions.opLong = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Long); };

      instructions.opByte = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Byte); };
      instructions.opUShort = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_UShort); };
      instructions.opUInt = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_UInt); };
      instructions.opULong = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_ULong); };

      instructions.opFloat = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Float); };
      instructions.opDouble = delegate () { return new INSTR_DECR(LCVM_DataTypes.Type_Double); };

      ArithmeticPostfixIncrDecrOperationGenerate(n, instructions);
    }

    #region Вспомогательные методы

    private void ArithmeticPostfixIncrDecrOperationGenerate(UnaryOperationNode n, InstructionDelegates op)
    {
      /*
       * Постфиксные и префексные операции мы воспринимаем как Statement,
       * поэтому для такой ноды отладочная информация генерируется полностью,
       * аналогично вызовам функций
       */

      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //PreVisit
      var operand = n.GetOperand();

      //Начало Statement
      StatementBegin(n);

      if (operand is ObjectNode objectNode)
      {
        PostfixIncrDecrObject(n, objectNode, op);
      }
      else
      {
        PostfixIncrDecrReference(n, operand, op);
      }

      StatementEnd(n);
    }
    
    private void PostfixIncrDecrObject(UnaryOperationNode n, ObjectNode objectNode, InstructionDelegates op)
    {
      var objDeclarator = objectNode.ObjDeclaratorNode;
      var classValue = objDeclarator.ClassValue;

      //Кладем на стек значение переменной
      objectNode.AccessMethod = ResultAccessMethod.MethodGet;
      objectNode.Visit(this);

      //Если результат операции используется вышестоящей нодой
      //то перед операцией дублируем результат
      PostfixIncrDecrResultDup(n);

      //Выполняем операцию
      PostfixIncrDecrAddOperation(n, op);

      //Сохраняем результат обратно в переменную
      PostfixIncrDecrSaveResultObject(n, objDeclarator);
    }

    private void PostfixIncrDecrReference(UnaryOperationNode n, TypedNode operandNode, InstructionDelegates op)
    {
      //Кладем на стек адрес переменной
      operandNode.AccessMethod = ResultAccessMethod.MethodGetReference;
      operandNode.Visit(this);

      //Дублируем ссылку
      assemblyUnit.AddInstruction(new INSTR_DUP());

      //Загружаем значение по ссылке
      PostfixIncrDecrLoadValueReference(n);

      //Если результат операции используется вышестоящей нодой
      //то перед операцией сохраняем результат в tmp переменную
      var objReference = PostfixIncrDecrValueToTmp(n);

      //Выполняем операцию
      PostfixIncrDecrAddOperation(n, op);

      //Сохраняем результат обратно в переменную
      PostfixIncrDecrSaveValueReference(n);

      //Если результат операции используется вышестоящей нодой
      //то после операции восстанавливаем результат из tmp переменной
      PostfixIncrDecrTmpToValue(n, objReference);
    }



    private void ArithmeticPrefixIncrDecrOperationGenerate(UnaryOperationNode n, InstructionDelegates op)
    {
      /*
       * Постфиксные и префексные операции мы воспринимаем как Statement,
       * поэтому для такой ноды отладочная информация генерируется полностью,
       * аналогично вызовам функций
       */

      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //PreVisit
      var operand = n.GetOperand();

      //Начало Statement
      StatementBegin(n);

      if (operand is ObjectNode objectNode)
      {
        PrefixIncrDecrObject(n, objectNode, op);
      }
      else
      {
        PrefixIncrDecrReference(n, operand, op);
      }

      StatementEnd(n);
    }

    private void PrefixIncrDecrObject(UnaryOperationNode n, ObjectNode objectNode, InstructionDelegates op)
    {
      var objDeclarator = objectNode.ObjDeclaratorNode;
      var classValue = objDeclarator.ClassValue;

      //Кладем на стек значение переменной
      objectNode.AccessMethod = ResultAccessMethod.MethodGet;
      objectNode.Visit(this);

      //Выполняем операцию
      PostfixIncrDecrAddOperation(n, op);

      //Если результат операции используется вышестоящей нодой
      //то перед операцией дублируем результат
      PostfixIncrDecrResultDup(n);

      //Сохраняем результат обратно в переменную
      PostfixIncrDecrSaveResultObject(n, objDeclarator);
    }

    private void PrefixIncrDecrReference(UnaryOperationNode n, TypedNode operandNode, InstructionDelegates op)
    {
      //Кладем на стек адрес переменной
      operandNode.AccessMethod = ResultAccessMethod.MethodGetReference;
      operandNode.Visit(this);

      //Дублируем ссылку
      assemblyUnit.AddInstruction(new INSTR_DUP());

      //Загружаем значение по ссылке
      PostfixIncrDecrLoadValueReference(n);

      //Выполняем операцию
      PostfixIncrDecrAddOperation(n, op);

      //Если результат операции используется вышестоящей нодой
      //то перед операцией сохраняем результат в tmp переменную
      var objReference = PostfixIncrDecrValueToTmp(n);

      //Сохраняем результат обратно в переменную
      PostfixIncrDecrSaveValueReference(n);

      //Если результат операции используется вышестоящей нодой
      //то после операции восстанавливаем результат из tmp переменной
      PostfixIncrDecrTmpToValue(n, objReference);
    }

    private void PostfixIncrDecrResultDup(UnaryOperationNode n)
    {
      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        //Размер объекта
        var operandSizeof = n.OperandsCType.Sizeof();

        switch (operandSizeof)
        {
          case 1:
          case 2:
          case 4:
            assemblyUnit.AddInstruction(new INSTR_DUP());
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_DUP_2());
            break;

          default:
            throw new InternalCompilerException("Неверный размер объекта");
        }
      }
    }

    private void PostfixIncrDecrAddOperation(UnaryOperationNode n, InstructionDelegates op)
    {
      var operandType = (LCPrimitiveType)n.OperandsCType;

      switch (operandType.Type)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(op.opSByte());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(op.opShort());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(op.opInt());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(op.opLong());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(op.opByte());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(op.opUShort());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(op.opUInt());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(op.opULong());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(op.opFloat());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(op.opDouble());
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
      }
    }

    private void PostfixIncrDecrSaveResultObject(UnaryOperationNode n, VariableDeclaratorNode objDeclarator)
    {
      //Размер объекта
      var operandSizeof = n.OperandsCType.Sizeof();

      if (objDeclarator.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
      {
        //Это глобальный объект, использует операцию GSTORE
        string objName = GetGlobalLabelName(objDeclarator);
        GlobalMemoryObjectReference memoryObjectReference = new GlobalMemoryObjectReference(objName);
        assemblyUnit.GlobalAllocator.AddReference(memoryObjectReference);

        switch (operandSizeof)
        {
          case 1:
            assemblyUnit.AddInstruction(new INSTR_GSTORE_1(memoryObjectReference));
            break;

          case 2:
            assemblyUnit.AddInstruction(new INSTR_GSTORE_2(memoryObjectReference));
            break;

          case 4:
            assemblyUnit.AddInstruction(new INSTR_GSTORE_4(memoryObjectReference));
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_GSTORE_8(memoryObjectReference));
            break;

          default:
            throw new InternalCompilerException("Неверный размер загружаемого объекта");
        }
      }
      else
      {
        //Это локальный объект, используем инструкции STORE и созраняем по смещению

        var offset = objDeclarator.LocalMemoryObject.Address;

        switch (operandSizeof)
        {
          case 1:
            assemblyUnit.AddInstruction(new INSTR_STORE_1(offset));
            break;

          case 2:
            assemblyUnit.AddInstruction(new INSTR_STORE_2(offset));
            break;

          case 4:
            assemblyUnit.AddInstruction(new INSTR_STORE_4(offset));
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_STORE_8(offset));
            break;

          default:
            throw new InternalCompilerException("Неверный размер загружаемого объекта");
        }
      }
    }



    private void PostfixIncrDecrLoadValueReference(UnaryOperationNode n)
    {
      //Размер объекта
      var operandSizeof = n.OperandsCType.Sizeof();

      switch (operandSizeof)
      {
        case 1:
          assemblyUnit.AddInstruction(new INSTR_ALOAD_1());
          break;

        case 2:
          assemblyUnit.AddInstruction(new INSTR_ALOAD_2());
          break;

        case 4:
          assemblyUnit.AddInstruction(new INSTR_ALOAD_4());
          break;

        case 8:
          assemblyUnit.AddInstruction(new INSTR_ALOAD_8());
          break;

        default:
          throw new InternalCompilerException("Неверный размер объекта");
      }

    }

    private void PostfixIncrDecrSaveValueReference(UnaryOperationNode n)
    {
      //Размер объекта
      var operandSizeof = n.OperandsCType.Sizeof();

      switch (operandSizeof)
      {
        case 1:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_1());
          break;

        case 2:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_2());
          break;

        case 4:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_4());
          break;

        case 8:
          assemblyUnit.AddInstruction(new INSTR_ASTORE_8());
          break;

        default:
          throw new InternalCompilerException("Неверный размер объекта");
      }
    }

    private void PostfixIncrDecrTmpToValue(UnaryOperationNode n, LocalMemoryObject objReference)
    {
      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        //Размер объекта
        var operandSizeof = n.OperandsCType.Sizeof();

        switch (operandSizeof)
        {
          case 1:
            assemblyUnit.AddInstruction(new INSTR_LOAD_1(objReference.Address));
            break;

          case 2:
            assemblyUnit.AddInstruction(new INSTR_LOAD_2(objReference.Address));
            break;

          case 4:
            assemblyUnit.AddInstruction(new INSTR_LOAD_4(objReference.Address));
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_LOAD_8(objReference.Address));
            break;

          default:
            throw new InternalCompilerException("Неверный размер объекта");
        }

        assemblyUnit.Allocator.FreeLocalObj(objReference);
      }
    }

    private LocalMemoryObject PostfixIncrDecrValueToTmp(UnaryOperationNode n)
    {
      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        //Размер объекта
        var operandSizeof = n.OperandsCType.Sizeof();

        var objReference = assemblyUnit.Allocator.AllocLocalObj("_tmp", operandSizeof);

        switch (operandSizeof)
        {
          case 1:
            assemblyUnit.AddInstruction(new INSTR_DUP());
            assemblyUnit.AddInstruction(new INSTR_STORE_1(objReference.Address));
            break;

          case 2:
            assemblyUnit.AddInstruction(new INSTR_DUP());
            assemblyUnit.AddInstruction(new INSTR_STORE_2(objReference.Address));
            break;

          case 4:
            assemblyUnit.AddInstruction(new INSTR_DUP());
            assemblyUnit.AddInstruction(new INSTR_STORE_4(objReference.Address));
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_DUP_2());
            assemblyUnit.AddInstruction(new INSTR_STORE_8(objReference.Address));
            break;

          default:
            throw new InternalCompilerException("Неверный размер объекта");
        }

        return objReference;
      }

      return null;
    }



    #endregion
  }
}
