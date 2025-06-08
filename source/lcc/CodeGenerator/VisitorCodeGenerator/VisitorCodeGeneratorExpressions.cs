using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(SummNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_ADD(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(SubNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_SUB(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(MulNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_MUL(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(DivNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_DIV(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(RemNode n)
    {
      BinaryArithmeticIntegerOperationGenerate(n,
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_REM(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(LessNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_LESS(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(LessEqualNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_LESSEQ(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(MoreNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_MORE(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(MoreEqualNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_MOREEQ(LCVM_DataTypes.Type_Double); },
        null);
    }

    public override void Visit(EqualNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Double); },
        delegate () { return new INSTR_EQ(LCVM_DataTypes.Type_Byte); });
    }

    public override void Visit(NotEqualNode n)
    {
      BinaryArithmeticOperationGenerate(n,
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_ULong); },

        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Float); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Double); },
        delegate () { return new INSTR_NEQ(LCVM_DataTypes.Type_Byte); });
    }

    public override void Visit(LeftShiftNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_LSHIFT(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(RightShiftNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_RSHIFT(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(AndNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(XorNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_XOR(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(OrNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(LogicAndNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_AND(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(LogicOrNode n)
    {
      BinaryBitOperationGenerate(n,
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_OR(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(NotNode n)
    {
      UnaryBitOperationGenerate(n,
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_SByte); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_Short); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_Int); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_Long); },

        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_Byte); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_UShort); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_UInt); },
        delegate () { return new INSTR_NOT(LCVM_DataTypes.Type_ULong); });
    }

    public override void Visit(NegativeNode n)
    {
      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //PreVisit
      var operand = n.GetOperand();

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        operand.AccessMethod = ResultAccessMethod.MethodUnuse;
        operand.Visit(this);
        return;
      }

      //Кладем на стек значение операнда
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      //выполняем операцию
      var operandType = ((LCPrimitiveType)n.OperandsCType).Type;

      switch (operandType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_SByte));
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_Short));
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_Int));
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_Long));
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_Float));
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          assemblyUnit.AddInstruction(new INSTR_INV(LCVM_DataTypes.Type_Double));
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
      }
    }

    public override void Visit(LogicNotNode n)
    {
      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //PreVisit
      var operand = n.GetOperand();

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        operand.AccessMethod = ResultAccessMethod.MethodUnuse;
        operand.Visit(this);
        return;
      }

      //Кладем на стек значение операнда
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      //выполняем операцию
      var operandType = ((LCPrimitiveType)n.OperandsCType).Type;
      switch (operandType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          assemblyUnit.AddInstruction(new INSTR_NOT(LCVM_DataTypes.Type_Bool));
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
      }
    }

    #region Вспомогательные методы

    delegate LCVMAsmItem DelegateGetInstruction();

    class InstructionDelegates
    {
      public DelegateGetInstruction opSByte;
      public DelegateGetInstruction opShort;
      public DelegateGetInstruction opInt;
      public DelegateGetInstruction opLong;
      public DelegateGetInstruction opByte;
      public DelegateGetInstruction opUShort;
      public DelegateGetInstruction opUInt;
      public DelegateGetInstruction opULong;
      public DelegateGetInstruction opFloat;
      public DelegateGetInstruction opDouble;
      public DelegateGetInstruction opBool;
    }

    void UnaryBitOperationGenerate(UnaryOperationNode n,
      DelegateGetInstruction opInt8,
      DelegateGetInstruction opInt16,
      DelegateGetInstruction opInt32,
      DelegateGetInstruction opInt64,
      
      DelegateGetInstruction opUInt8,
      DelegateGetInstruction opUInt16,
      DelegateGetInstruction opUInt32,
      DelegateGetInstruction opUInt64)
    {
      if (n.GenerateDebugInfo == GenerateDebugValue.Yes)
        InsertComment(n);


      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //PreVisit
      var operand = n.GetOperand();

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        operand.AccessMethod = ResultAccessMethod.MethodUnuse;
        operand.Visit(this);
        return;
      }

      //Кладем на стек значение операнда
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      //выполняем операцию

      //PartiallyNodeStatementBegin(n);

      var operandType = ((LCPrimitiveType)n.OperandsCType).Type;
      switch (operandType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(opInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(opInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(opInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(opInt64());
          break;


        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(opUInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(opUInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(opUInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(opUInt64());
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
      }
    }

    void BinaryBitOperationGenerate(BinaryOperationNode n,
      DelegateGetInstruction opInt8,
      DelegateGetInstruction opInt16,
      DelegateGetInstruction opInt32,
      DelegateGetInstruction opInt64,

      DelegateGetInstruction opUInt8,
      DelegateGetInstruction opUInt16,
      DelegateGetInstruction opUInt32,
      DelegateGetInstruction opUInt64)
    {
      InsertComment(n);

      //PreVisit
      var left = n.GetOperandLeft();
      var right = n.GetOperandRight();

      //Если результат операции не используется,
      //то ставим атрибуты операндов, которые также указывают, что
      //они не используются, и обходим их
      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        left.AccessMethod = ResultAccessMethod.MethodUnuse;
        right.AccessMethod = ResultAccessMethod.MethodUnuse;
        //Visit
        base.Visit(n);
        return;
      }
      else if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
      {
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");
      }

      left.AccessMethod = ResultAccessMethod.MethodGet;
      right.AccessMethod = ResultAccessMethod.MethodGet;

      //Visit
      base.Visit(n);

      //PostVisit

      var operantType = ((LCPrimitiveType)n.OperandsCType).Type;

      switch (operantType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(opInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(opInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(opInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(opInt64());
          break;


        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(opUInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(opUInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(opUInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(opUInt64());
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operantType.ToString(), n.Description()));
      }
    }

    void BinaryArithmeticIntegerOperationGenerate(BinaryOperationNode n,
      DelegateGetInstruction opInt8,
      DelegateGetInstruction opInt16,
      DelegateGetInstruction opInt32,
      DelegateGetInstruction opInt64,

      DelegateGetInstruction opUInt8,
      DelegateGetInstruction opUInt16,
      DelegateGetInstruction opUInt32,
      DelegateGetInstruction opUInt64)
    {
      InsertComment(n);

      //PreVisit
      var left = n.GetOperandLeft();
      var right = n.GetOperandRight();

      left.AccessMethod = ResultAccessMethod.MethodGet;
      right.AccessMethod = ResultAccessMethod.MethodGet;


      //Если результат операции не используется,
      //то ставим атрибуты операндов, которые также указывают, что
      //они не используются, и обходим их
      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        left.AccessMethod = ResultAccessMethod.MethodUnuse;
        right.AccessMethod = ResultAccessMethod.MethodUnuse;
        //Visit
        base.Visit(n);
        return;
      }
      else if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
      {
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");
      }

      //Visit
      base.Visit(n);


      //PostVisit

      var operandType = ((LCPrimitiveType)n.OperandsCType).Type;

      switch (operandType)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          assemblyUnit.AddInstruction(opInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          assemblyUnit.AddInstruction(opInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          assemblyUnit.AddInstruction(opInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          assemblyUnit.AddInstruction(opInt64());
          break;


        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          assemblyUnit.AddInstruction(opUInt8());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          assemblyUnit.AddInstruction(opUInt16());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          assemblyUnit.AddInstruction(opUInt32());
          break;

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          assemblyUnit.AddInstruction(opUInt64());
          break;

        default:
          throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
      }
    }

    void BinaryArithmeticOperationGenerate(BinaryOperationNode n,
      DelegateGetInstruction opInt8,
      DelegateGetInstruction opInt16,
      DelegateGetInstruction opInt32,
      DelegateGetInstruction opInt64,

      DelegateGetInstruction opUInt8,
      DelegateGetInstruction opUInt16,
      DelegateGetInstruction opUInt32,
      DelegateGetInstruction opUInt64,

      DelegateGetInstruction opFloat,
      DelegateGetInstruction opDouble,
      DelegateGetInstruction opBool)
    {
      InsertComment(n);

      //PreVisit
      var left = n.GetOperandLeft();
      var right = n.GetOperandRight();

      left.AccessMethod = ResultAccessMethod.MethodGet;
      right.AccessMethod = ResultAccessMethod.MethodGet;


      //Если результат операции не используется,
      //то ставим атрибуты операндов, которые также указывают, что
      //они не используются, и обходим их
      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        left.AccessMethod = ResultAccessMethod.MethodUnuse;
        right.AccessMethod = ResultAccessMethod.MethodUnuse;
        //Visit
        base.Visit(n);
        return;
      }
      else if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
      {
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");
      }

      //Visit
      base.Visit(n);

      //PostVisit  


      var operandType = n.OperandsCType;

      if (operandType is LCPrimitiveType operandPrimitiveType)
      {
        switch (operandPrimitiveType.Type)
        {
          case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
            assemblyUnit.AddInstruction(opInt8());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
            assemblyUnit.AddInstruction(opInt16());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
            assemblyUnit.AddInstruction(opInt32());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
            assemblyUnit.AddInstruction(opInt64());
            break;


          case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
            assemblyUnit.AddInstruction(opUInt8());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
            assemblyUnit.AddInstruction(opUInt16());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
            assemblyUnit.AddInstruction(opUInt32());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
            assemblyUnit.AddInstruction(opUInt64());
            break;


          case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
            assemblyUnit.AddInstruction(opFloat());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
            assemblyUnit.AddInstruction(opDouble());
            break;

          case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
            assemblyUnit.AddInstruction(opBool());
            break;

          default:
            throw new InternalCompilerException(string.Format("Для типов \"{0}\" отсвутсвует интсрукция \"{1}\"", operandType.ToString(), n.Description()));
        }
      }
      else
        throw new InternalCompilerException("Неверный тип");
    }

    #endregion
  }
}
