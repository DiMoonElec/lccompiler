using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;

namespace LC2.LCCompiler.CodeGenerator
{
  class Translator : TranslatorVisitor
  {
    List<byte> buffer = new List<byte>();

    public Translator()
    {
    }

    public byte[] GetBuffer()
    {
      return buffer.ToArray();
    }

    public override void Generate(INSTR_LABEL instruction)
    {
      //INSTR_LABEL не генерирует код
    }

    public override void Generate(INSTR_ALOAD_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ALOAD_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ALOAD_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ALOAD_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ASTORE_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ASTORE_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ASTORE_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_ASTORE_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_RET instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_HALT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_STORE_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_STORE_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_STORE_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_STORE_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_GSTORE_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GSTORE_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GSTORE_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GSTORE_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_LOAD_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_LOAD_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_LOAD_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_LOAD_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.Offset);
    }

    public override void Generate(INSTR_GLOAD_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GLOAD_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GLOAD_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_GLOAD_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)(instruction.Reference.Address));
    }

    public override void Generate(INSTR_PUSH_1 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddArray(instruction.Value, 1);
    }

    public override void Generate(INSTR_PUSH_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddArray(instruction.Value, 2);
    }

    public override void Generate(INSTR_PUSH_4 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddArray(instruction.Value, 4);
    }

    public override void Generate(INSTR_PUSH_4_MEM_OBJ instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUInt((uint)instruction.Reference.Address);
    }

    public override void Generate(INSTR_PUSH_8 instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddArray(instruction.Value, 8);
    }

    public override void Generate(INSTR_BOOL_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_BOOL_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT8_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT8_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT16_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT16_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT32_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT32_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INT64_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_UINT64_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_FLOAT_TO_DOUBLE instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_BOOL instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_INT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_UINT8 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_INT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_UINT16 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_INT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_UINT32 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_INT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_UINT64 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DOUBLE_TO_FLOAT instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_INCR instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_DECR instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_INV instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_ADD instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_SUB instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_MUL instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_DIV instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_DUP instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DUP_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DROP instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_DROP_2 instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    public override void Generate(INSTR_REM instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_LESS instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_LESSEQ instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }
    public override void Generate(INSTR_MORE instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }
    public override void Generate(INSTR_MOREEQ instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_EQ instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_NEQ instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_RSHIFT instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_LSHIFT instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_AND instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_OR instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_XOR instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_NOT instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.DataType);
    }

    public override void Generate(INSTR_IFFALSE instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.LabelReference.Address);
    }

    public override void Generate(INSTR_IFFALSE_S instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > short.MaxValue) || (offset < short.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddShort((short)offset);
    }

    public override void Generate(INSTR_IFFALSE_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > sbyte.MaxValue) || (offset < sbyte.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddSByte((sbyte)offset);
    }

    public override void Generate(INSTR_IFTRUE instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.LabelReference.Address);
    }

    public override void Generate(INSTR_IFTRUE_S instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > short.MaxValue) || (offset < short.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddShort((short)offset);
    }

    public override void Generate(INSTR_IFTRUE_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > sbyte.MaxValue) || (offset < sbyte.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddSByte((sbyte)offset);
    }

    public override void Generate(INSTR_JMP instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.LabelReference.Address);
    }

    public override void Generate(INSTR_JMP_S instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > short.MaxValue) || (offset < short.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddShort((short)offset);
    }

    public override void Generate(INSTR_JMP_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > sbyte.MaxValue) || (offset < sbyte.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddSByte((sbyte)offset);
    }

    public override void Generate(INSTR_INCR_FP instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUShort(instruction.Value);
    }

    public override void Generate(INSTR_INCR_FP_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte(instruction.Value);
    }

    public override void Generate(INSTR_DECR_FP instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUShort(instruction.Value);
    }

    public override void Generate(INSTR_DECR_FP_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte(instruction.Value);
    }

    public override void Generate(INSTR_CALL instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddInt(instruction.LabelReference.Address);
    }

    public override void Generate(INSTR_CALL_S instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > short.MaxValue) || (offset < short.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddShort((short)offset);
    }

    public override void Generate(INSTR_CALL_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      int offset = instruction.LabelReference.Address - instruction.CurrentPosition;

      if ((offset > sbyte.MaxValue) || (offset < sbyte.MinValue))
        throw new InternalCompilerException(string.Format("Значение смещения для команы {0} выходит за допустимый диапазон", instruction.Bytecode.ToString()));

      AddSByte((sbyte)offset);
    }

    public override void Generate(INSTR_SYSCALL instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddUShort(instruction.ID);
    }

    public override void Generate(INSTR_SYSCALL_T instruction)
    {
      AddBytecode(instruction.Bytecode);
      AddByte((byte)instruction.ID);
    }

    public override void Generate(INSTR_LOAD_FP instruction)
    {
      AddBytecode(instruction.Bytecode);
    }

    private void AddBytecode(InstructionBytecodes data)
    {
      buffer.Add((byte)data);
    }


    #region Вспомогательные методы

    private void AddUInt(uint data)
    {
      buffer.AddRange(BitConverter.GetBytes(data));
    }

    private void AddUShort(ushort data)
    {
      buffer.AddRange(BitConverter.GetBytes(data));
    }

    private void AddShort(short data)
    {
      buffer.AddRange(BitConverter.GetBytes(data));
    }

    private void AddSByte(sbyte data)
    {
      buffer.Add((byte)data);
    }

    private void AddByte(byte data)
    {
      buffer.Add(data);
    }

    private void AddInt(int data)
    {
      buffer.AddRange(BitConverter.GetBytes(data));
    }

    private void AddArray(byte[] data, int lenght)
    {
      if (data.Length != lenght)
        throw new InternalCompilerException("Неверная длина массива");
      buffer.AddRange(data);
    }

    #endregion
  }
}
