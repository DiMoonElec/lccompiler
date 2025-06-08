using System;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(TypeCastNode n)
    {
      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");


      //Если пезультат выполнения операции не используется 
      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        var operand = n.GetOperand();
        operand.AccessMethod = ResultAccessMethod.MethodUnuse;
        operand.Visit(this);
        return;
      }

      //Если результат данной операции используется
      var Operand = n.GetOperand();
      Operand.AccessMethod = ResultAccessMethod.MethodGet;
      Operand.Visit(this);

      if (n.CastType == null)
        throw new InternalCompilerException("Полю CastType не присвоено значение");

      if (n.OperandsCType == null)
        throw new InternalCompilerException("Полю OperandsCType не присвоено значение");

      //PostVisit
      //Если оба типа - базовые

      var castType = n.CastType;
      var operandsCType = n.OperandsCType;

      if (operandsCType is LCPrimitiveType operandBaseType)
        TypeCastPrimitiveTypes(castType, operandBaseType);
      else
        throw new InternalCompilerException("Ошибка генерации typecast: если кастуемый тип LCBaseObjectType, то и преобразуемый тип должен быть LCBaseObjectType");
    }

    #region Вспомогательные методы

    private void TypeCastSByteTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_INT8_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastShortTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_INT16_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastIntTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_INT32_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastLongTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_INT64_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastByteTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_UINT8_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastUShortTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_UINT16_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastUIntTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_UINT32_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastULongTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_UINT64_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastFloatTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_FLOAT_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastDoubleTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_DOUBLE_TO_BOOL());
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }

    private void TypeCastBoolTo(LCPrimitiveType.PrimitiveTypes cast)
    {
      switch (cast)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_INT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_INT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_INT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_INT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_UINT8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_UINT16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_UINT32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_UINT64());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_FLOAT());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_BOOL_TO_DOUBLE());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          break;

        default:
          throw new InternalCompilerException($"Cannot cast to type {cast}");
      }
    }



    private void TypeCastPrimitiveTypes(LCPrimitiveType castPrimitiveType, LCPrimitiveType operandPrimitiveType)
    {
      var operandType = operandPrimitiveType.Type;
      var castType = castPrimitiveType.Type;

      switch (operandType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          TypeCastSByteTo(castType);
          break;
        
        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          TypeCastShortTo(castType);
          break;
        
        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          TypeCastIntTo(castType);
          break;
        
        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          TypeCastLongTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          TypeCastByteTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          TypeCastUShortTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          TypeCastUIntTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          TypeCastULongTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          TypeCastFloatTo(castType);
          break;
        
        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          TypeCastDoubleTo(castType);
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          TypeCastBoolTo(castType);
          break;

        default:
          throw new InternalCompilerException($"Cannot cast from type {operandType}");
      }
    }

    #endregion
  }
}
