namespace LC2.LCCompiler.CodeGenerator.AsmInstruction
{
  class INSTR_BOOL_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_sbyte;

    public INSTR_BOOL_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_byte;

    public INSTR_BOOL_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_short;

    public INSTR_BOOL_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_ushort;

    public INSTR_BOOL_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_int;

    public INSTR_BOOL_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_uint;

    public INSTR_BOOL_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_long;

    public INSTR_BOOL_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_ulong;

    public INSTR_BOOL_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_float;

    public INSTR_BOOL_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_BOOL_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.bool_to_double;

    public INSTR_BOOL_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_bool;

    public INSTR_INT8_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_byte;

    public INSTR_INT8_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_short;

    public INSTR_INT8_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_ushort;

    public INSTR_INT8_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_int;

    public INSTR_INT8_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_uint;

    public INSTR_INT8_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_long;

    public INSTR_INT8_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_ulong;

    public INSTR_INT8_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_float;

    public INSTR_INT8_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT8_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.sbyte_to_double;

    public INSTR_INT8_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_bool;

    public INSTR_UINT8_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_sbyte;

    public INSTR_UINT8_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_short;

    public INSTR_UINT8_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_ushort;

    public INSTR_UINT8_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_int;

    public INSTR_UINT8_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_uint;

    public INSTR_UINT8_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_long;

    public INSTR_UINT8_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_ulong;

    public INSTR_UINT8_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_float;

    public INSTR_UINT8_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT8_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.byte_to_double;

    public INSTR_UINT8_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_bool;

    public INSTR_INT16_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_sbyte;

    public INSTR_INT16_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_byte;

    public INSTR_INT16_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_ushort;

    public INSTR_INT16_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_int;

    public INSTR_INT16_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_uint;

    public INSTR_INT16_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_long;

    public INSTR_INT16_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_ulong;

    public INSTR_INT16_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_float;

    public INSTR_INT16_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT16_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.short_to_double;

    public INSTR_INT16_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_bool;

    public INSTR_UINT16_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_sbyte;

    public INSTR_UINT16_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_byte;

    public INSTR_UINT16_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_short;

    public INSTR_UINT16_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_int;

    public INSTR_UINT16_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_uint;

    public INSTR_UINT16_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_long;

    public INSTR_UINT16_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_ulong;

    public INSTR_UINT16_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_float;

    public INSTR_UINT16_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT16_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ushort_to_double;

    public INSTR_UINT16_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_bool;

    public INSTR_INT32_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_sbyte;

    public INSTR_INT32_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_byte;

    public INSTR_INT32_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_short;

    public INSTR_INT32_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_ushort;

    public INSTR_INT32_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_uint;

    public INSTR_INT32_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_long;

    public INSTR_INT32_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_ulong;

    public INSTR_INT32_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_float;

    public INSTR_INT32_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT32_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.int_to_double;

    public INSTR_INT32_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_bool;

    public INSTR_UINT32_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_sbyte;

    public INSTR_UINT32_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_byte;

    public INSTR_UINT32_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_short;

    public INSTR_UINT32_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_ushort;

    public INSTR_UINT32_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_int;

    public INSTR_UINT32_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_long;

    public INSTR_UINT32_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_ulong;

    public INSTR_UINT32_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_float;

    public INSTR_UINT32_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT32_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.uint_to_double;

    public INSTR_UINT32_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_bool;

    public INSTR_INT64_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_sbyte;

    public INSTR_INT64_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_byte;

    public INSTR_INT64_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_short;

    public INSTR_INT64_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_ushort;

    public INSTR_INT64_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_int;

    public INSTR_INT64_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_uint;

    public INSTR_INT64_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_uong;

    public INSTR_INT64_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_float;

    public INSTR_INT64_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_INT64_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.long_to_double;

    public INSTR_INT64_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_bool;

    public INSTR_UINT64_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_sbyte;

    public INSTR_UINT64_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_byte;

    public INSTR_UINT64_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_short;

    public INSTR_UINT64_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_ushort;

    public INSTR_UINT64_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_int;

    public INSTR_UINT64_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_uint;

    public INSTR_UINT64_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_long;

    public INSTR_UINT64_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_float;

    public INSTR_UINT64_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_UINT64_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ulong_to_double;

    public INSTR_UINT64_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_bool;

    public INSTR_FLOAT_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_sbyte;

    public INSTR_FLOAT_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_byte;

    public INSTR_FLOAT_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_short;

    public INSTR_FLOAT_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_ushort;

    public INSTR_FLOAT_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_int;

    public INSTR_FLOAT_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_uint;

    public INSTR_FLOAT_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_long;

    public INSTR_FLOAT_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_ulong;

    public INSTR_FLOAT_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_FLOAT_TO_DOUBLE : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.float_to_double;

    public INSTR_FLOAT_TO_DOUBLE() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_BOOL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_bool;

    public INSTR_DOUBLE_TO_BOOL() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_INT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_sbyte;

    public INSTR_DOUBLE_TO_INT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_UINT8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_byte;

    public INSTR_DOUBLE_TO_UINT8() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_INT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_short;

    public INSTR_DOUBLE_TO_INT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_UINT16 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_ushort;

    public INSTR_DOUBLE_TO_UINT16() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_INT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_int;

    public INSTR_DOUBLE_TO_INT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_UINT32 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_uint;

    public INSTR_DOUBLE_TO_UINT32() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_INT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_long;

    public INSTR_DOUBLE_TO_INT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_UINT64 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_ulong;

    public INSTR_DOUBLE_TO_UINT64() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_DOUBLE_TO_FLOAT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.double_to_float;

    public INSTR_DOUBLE_TO_FLOAT() : base(1)
    { }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

}
