namespace LC2.LCCompiler.CodeGenerator.AsmInstruction
{
  class INSTR_INCR : LCVMArithmeticInstruction
  {
    public INSTR_INCR(LCVM_DataTypes dataType) : base(InstructionBytecodes.incr, dataType) { }
  }

  class INSTR_DECR : LCVMArithmeticInstruction
  {
    public INSTR_DECR(LCVM_DataTypes dataType) : base(InstructionBytecodes.decr, dataType) { }
  }

  class INSTR_INV : LCVMArithmeticInstruction
  {
    public INSTR_INV(LCVM_DataTypes dataType) : base(InstructionBytecodes.inv, dataType) { }
  }

  class INSTR_MUL : LCVMArithmeticInstruction
  {
    public INSTR_MUL(LCVM_DataTypes dataType) : base(InstructionBytecodes.mul, dataType) { }
  }

  class INSTR_DIV : LCVMArithmeticInstruction
  {
    public INSTR_DIV(LCVM_DataTypes dataType) : base(InstructionBytecodes.div, dataType) { }
  }

  class INSTR_ADD : LCVMArithmeticInstruction
  {
    public INSTR_ADD(LCVM_DataTypes dataType) : base(InstructionBytecodes.add, dataType) { }
  }

  class INSTR_SUB : LCVMArithmeticInstruction
  {
    public INSTR_SUB(LCVM_DataTypes dataType) : base(InstructionBytecodes.sub, dataType) { }
  }

  class INSTR_LESS : LCVMArithmeticInstruction
  {
    public INSTR_LESS(LCVM_DataTypes dataType) : base(InstructionBytecodes.less, dataType) { }
  }

  class INSTR_LESSEQ : LCVMArithmeticInstruction
  {
    public INSTR_LESSEQ(LCVM_DataTypes dataType) : base(InstructionBytecodes.lesseq, dataType) { }
  }

  class INSTR_MORE : LCVMArithmeticInstruction
  {
    public INSTR_MORE(LCVM_DataTypes dataType) : base(InstructionBytecodes.more, dataType) { }
  }

  class INSTR_MOREEQ : LCVMArithmeticInstruction
  {
    public INSTR_MOREEQ(LCVM_DataTypes dataType) : base(InstructionBytecodes.moreeq, dataType) { }
  }

  class INSTR_EQ : LCVMArithmeticInstruction
  {
    public INSTR_EQ(LCVM_DataTypes dataType) : base(InstructionBytecodes.eq, dataType) { }
  }

  class INSTR_NEQ : LCVMArithmeticInstruction
  {
    public INSTR_NEQ(LCVM_DataTypes dataType) : base(InstructionBytecodes.neq, dataType) { }
  }

  class INSTR_REM : LCVMArithmeticInstruction
  {
    public INSTR_REM(LCVM_DataTypes dataType) : base(InstructionBytecodes.rem, dataType) { }
  }

  class INSTR_RSHIFT : LCVMArithmeticInstruction
  {
    public INSTR_RSHIFT(LCVM_DataTypes dataType) : base(InstructionBytecodes.rshift, dataType) { }
  }

  class INSTR_LSHIFT : LCVMArithmeticInstruction
  {
    public INSTR_LSHIFT(LCVM_DataTypes dataType) : base(InstructionBytecodes.lshift, dataType) { }
  }

  class INSTR_AND : LCVMArithmeticInstruction
  {
    public INSTR_AND(LCVM_DataTypes dataType) : base(InstructionBytecodes.and, dataType) { }
  }

  class INSTR_XOR : LCVMArithmeticInstruction
  {
    public INSTR_XOR(LCVM_DataTypes dataType) : base(InstructionBytecodes.xor, dataType) { }
  }

  class INSTR_OR : LCVMArithmeticInstruction
  {
    public INSTR_OR(LCVM_DataTypes dataType) : base(InstructionBytecodes.or, dataType) { }
  }

  class INSTR_NOT : LCVMArithmeticInstruction
  {
    public INSTR_NOT(LCVM_DataTypes dataType) : base(InstructionBytecodes.not, dataType) { }
  }
}
