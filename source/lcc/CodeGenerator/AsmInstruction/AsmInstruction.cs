using System;

namespace LC2.LCCompiler.CodeGenerator.AsmInstruction
{
  enum LCVM_DataTypes
  {
    Type_SByte = 0x0,
    Type_Short = 0x1,
    Type_Int = 0x2,
    Type_Long = 0x3,
    Type_Byte = 0x4,
    Type_UShort = 0x5,
    Type_UInt = 0x6,
    Type_ULong = 0x7,
    Type_Float = 0x8,
    Type_Double = 0x9,
    Type_Bool = 0x0A,
  };

  static class LCVM_DataTypesExtensions
  {
    public static string ToString(LCVM_DataTypes type)
    {
      switch (type)
      {
        case LCVM_DataTypes.Type_SByte:
          return "sbyte";
        case LCVM_DataTypes.Type_Short:
          return "short";
        case LCVM_DataTypes.Type_Int:
          return "int";
        case LCVM_DataTypes.Type_Long:
          return "long";
        case LCVM_DataTypes.Type_Byte:
          return "byte";
        case LCVM_DataTypes.Type_UShort:
          return "ushort";
        case LCVM_DataTypes.Type_UInt:
          return "uint";
        case LCVM_DataTypes.Type_ULong:
          return "ulong";
        case LCVM_DataTypes.Type_Float:
          return "float";
        case LCVM_DataTypes.Type_Double:
          return "double";
        case LCVM_DataTypes.Type_Bool:
          return "bool";
        default:
          throw new ArgumentOutOfRangeException(nameof(type), $"Unknown data type: {type}");
      }
    }
  }


  class INSTR_LABEL : LCVMInstruction, ILCVMInstructionCodeObject
  {
    public CodeLabel Label { get; private set; }

    string Description = "";
    public int Address
    {
      get
      {
        if (_adr_is_define == false)
          throw new InternalCompilerException("Адрес метки не установлен");
        return _address;
      }
      set
      {
        _adr_is_define = true;
        _address = value;
      }
    }

    int _address;
    bool _adr_is_define = false;

    public INSTR_LABEL(CodeLabel label) : base(0)
    {
      Label = label;
    }

    public INSTR_LABEL(CodeLabel label, string description) : base(0)
    {
      Label = label;
      Description = description;
    }

    public override void SetCurrentPosition(int value)
    {
      base.SetCurrentPosition(value);
      Label.Address = value; //Обновляем адрес метки
    }

    public override string ToString()
    {
      if (Description != "")
        return ":" + Label.LabelName + " ;" + Description;
      return ":" + Label.LabelName;
    }
  }

  class INSTR_RET : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.ret;

    public INSTR_RET() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_HALT : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.halt;

    public INSTR_HALT() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_STORE_1 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.store_1;

    public INSTR_STORE_1(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_STORE_2 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.store_2;

    public INSTR_STORE_2(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_STORE_4 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.store_4;

    public INSTR_STORE_4(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }

  }

  class INSTR_STORE_8 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.store_8;

    public INSTR_STORE_8(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }

  }

  class INSTR_GSTORE_1 : LCVMInstructionGLoadGStore, ILCVMInstructionMemoryObjectReference
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gstore_1;

    public INSTR_GSTORE_1(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_GSTORE_2 : LCVMInstructionGLoadGStore, ILCVMInstructionMemoryObjectReference
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gstore_2;


    public INSTR_GSTORE_2(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }

  }

  class INSTR_GSTORE_4 : LCVMInstructionGLoadGStore, ILCVMInstructionMemoryObjectReference
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gstore_4;

    public INSTR_GSTORE_4(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_GSTORE_8 : LCVMInstructionGLoadGStore, ILCVMInstructionMemoryObjectReference
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gstore_8;

    public INSTR_GSTORE_8(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_ASTORE_1 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.astore_1;

    public INSTR_ASTORE_1() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_ASTORE_2 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.astore_2;

    public INSTR_ASTORE_2() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_ASTORE_4 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.astore_4;
    public INSTR_ASTORE_4() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }


  }

  class INSTR_ASTORE_8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.astore_8;

    public INSTR_ASTORE_8() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }


  class INSTR_ALOAD_1 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.aload_1;

    public INSTR_ALOAD_1() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_ALOAD_2 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.aload_2;

    public INSTR_ALOAD_2() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_ALOAD_4 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.aload_4;

    public INSTR_ALOAD_4() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_ALOAD_8 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.aload_8;

    public INSTR_ALOAD_8() : base(1)
    {
    }

    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_LOAD_1 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.load_1;

    public INSTR_LOAD_1(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_LOAD_2 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.load_2;

    public INSTR_LOAD_2(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_LOAD_4 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.load_4;

    public INSTR_LOAD_4(int adr) : base(adr, 5)
    {
    }
    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_LOAD_8 : LCVMInstructionLoadStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.load_8;

    public INSTR_LOAD_8(int adr) : base(adr, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Offset);
    }
  }

  class INSTR_GLOAD_1 : LCVMInstructionGLoadGStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gload_1;

    public INSTR_GLOAD_1(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_GLOAD_2 : LCVMInstructionGLoadGStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gload_2;

    public INSTR_GLOAD_2(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_GLOAD_4 : LCVMInstructionGLoadGStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gload_4;

    public INSTR_GLOAD_4(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_GLOAD_8 : LCVMInstructionGLoadGStore
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.gload_8;

    public INSTR_GLOAD_8(GlobalMemoryObjectReference objectReference) : base(objectReference, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X4})",
        Bytecode.ToString(), Reference.ObjectName, Reference.Address);
    }
  }

  class INSTR_PUSH_1 : INSTR_PUSH
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.push_1;

    public INSTR_PUSH_1(byte[] value) : base(value, 2)
    {
      if (value.Length != 1)
        throw new InternalCompilerException("INSTR_PUSH_B должен принимать массив длинной 1 байт");

    }

    public override string ToString()
    {
      return Bytecode.ToString() + ValueToString();
    }

  }

  class INSTR_PUSH_2 : INSTR_PUSH
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.push_2;

    /// <summary>
    /// Положить на стек число
    /// </summary>
    /// <param name="value">Значение</param>
    public INSTR_PUSH_2(byte[] value) : base(value, 3)
    {
      if (value.Length != 2)
        throw new InternalCompilerException("INSTR_PUSH_W должен принимать массив длинной 2 байта");
    }

    public override string ToString()
    {
      return Bytecode.ToString() + ValueToString();
    }
  }

  /// <summary>
  /// Инструкция push4 data
  /// </summary>
  class INSTR_PUSH_4 : INSTR_PUSH
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.push_4;

    /// <summary>
    /// Положить на стек число
    /// </summary>
    /// <param name="value">Значение</param>
    public INSTR_PUSH_4(byte[] value) : base(value, 5)
    {
      if (value.Length != 4)
        throw new InternalCompilerException("INSTR_PUSH_D должен принимать массив длинной 4 байта");
    }

    public override string ToString()
    {
      return Bytecode.ToString() + ValueToString();
    }
  }

  /// <summary>
  /// Инструкция push4 mem_ref
  /// </summary>
  class INSTR_PUSH_4_MEM_OBJ : INSTR_PUSH, ILCVMInstructionMemoryObjectReference
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.push_4;

    public GlobalMemoryObjectReference Reference { get; private set; }

    /// <summary>
    /// Используется для загрузки в стек ссылки на массив
    /// </summary>
    /// <param name="reference">Ссылка на объект</param>
    /// <param name="lenght">Длина объекта</param>
    public INSTR_PUSH_4_MEM_OBJ(GlobalMemoryObjectReference reference) : base(null, 5)
    {
      Reference = reference;
    }

    public override string ToString()
    {
      return string.Format("{0} 0x{1:X8} ({2})", Bytecode.ToString(), Reference.Address, Reference.ObjectName);
    }
  }



  /// <summary>
  /// Положить на стек значение (8 байт). В качестве значения выступает число.
  /// Результат будет занимать 2 ячейки стека операндов
  /// </summary>
  class INSTR_PUSH_8 : INSTR_PUSH
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.push_8;
    public INSTR_PUSH_8(byte[] value) : base(value, 9)
    {
      if (value.Length != 8)
        throw new InternalCompilerException("INSTR_PUSH_Q должен принимать массив длинной 8 байт");
    }

    public override string ToString()
    {
      return Bytecode.ToString() + ValueToString();
    }
  }

  class INSTR_DUP : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.dup;
    public INSTR_DUP() : base(1)
    { }
    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_DUP_2 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.dup_2;
    public INSTR_DUP_2() : base(1)
    { }
    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_DROP : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.drop;
    public INSTR_DROP() : base(1)
    { }
    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_DROP_2 : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.drop_2;
    public INSTR_DROP_2() : base(1)
    { }
    public override string ToString()
    {
      return Bytecode.ToString();
    }
  }

  class INSTR_IFFALSE : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iffalse;

    public INSTR_IFFALSE(CodeLabelReference label) : base(label, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_IFFALSE_S : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iffalse_s;


    public INSTR_IFFALSE_S(CodeLabelReference label) : base(label, 3)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_IFFALSE_T : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iffalse_t;

    public INSTR_IFFALSE_T(CodeLabelReference label) : base(label, 2)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_IFTRUE : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iftrue;

    public INSTR_IFTRUE(CodeLabelReference label) : base(label, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_IFTRUE_S : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iftrue_s;

    public INSTR_IFTRUE_S(CodeLabelReference label) : base(label, 3)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_IFTRUE_T : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.iftrue_t;

    public INSTR_IFTRUE_T(CodeLabelReference label) : base(label, 2)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_JMP : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.jmp;

    public INSTR_JMP(CodeLabelReference label) : base(label, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), LabelReference.LabelName);
    }
  }

  class INSTR_JMP_S : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.jmp_s;

    public INSTR_JMP_S(CodeLabelReference label) : base(label, 3)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} ({2})", Bytecode.ToString(), GetLabelOffset(), LabelReference.LabelName);
    }
  }

  class INSTR_JMP_T : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.jmp_t;

    public INSTR_JMP_T(CodeLabelReference label) : base(label, 2)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} ({2})", Bytecode.ToString(), GetLabelOffset(), LabelReference.LabelName);
    }
  }

  class INSTR_INCR_FP : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.incr_fp;

    public ushort Value { get; private set; }

    public INSTR_INCR_FP(ushort v) : base(3)
    {
      Value = v;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Value.ToString());
    }
  }

  class INSTR_INCR_FP_T : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.incr_fp_t;

    public byte Value { get; private set; }

    public INSTR_INCR_FP_T(byte v) : base(2)
    {
      Value = v;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Value.ToString());
    }
  }

  class INSTR_DECR_FP : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.decr_fp;

    public ushort Value { get; private set; }

    public INSTR_DECR_FP(ushort v) : base(3)
    {
      Value = v;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Value.ToString());
    }
  }

  class INSTR_DECR_FP_T : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.decr_fp_t;

    public byte Value { get; private set; }

    public INSTR_DECR_FP_T(byte v) : base(2)
    {
      Value = v;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), Value.ToString());
    }
  }


  class INSTR_LOAD_FP : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.load_fp;

    public INSTR_LOAD_FP() : base(1)
    {
    }

    public override string ToString()
    {
      return string.Format("{0}", Bytecode.ToString());
    }
  }

  class INSTR_CALL : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.call;

    public INSTR_CALL(CodeLabelReference label) : base(label, 5)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} (0x{2:X8})",
        Bytecode.ToString(), LabelReference.LabelName, LabelReference.Address);
    }
  }

  class INSTR_CALL_S : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.call_s;

    public INSTR_CALL_S(CodeLabelReference label) : base(label, 3)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} ({2})", Bytecode.ToString(), GetLabelOffset(), LabelReference.LabelName);
    }
  }

  class INSTR_CALL_T : LCVMJmpInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.call_t;

    public INSTR_CALL_T(CodeLabelReference label) : base(label, 2)
    {
    }

    public override string ToString()
    {
      return string.Format("{0} {1} ({2})", Bytecode.ToString(), GetLabelOffset(), LabelReference.LabelName);
    }
  }

  class INSTR_SYSCALL : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.syscall;

    public ushort ID { get; private set; }

    public INSTR_SYSCALL(ushort id) : base(3)
    {
      ID = id;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), ID.ToString());
    }
  }

  class INSTR_SYSCALL_T : LCVMInstruction
  {
    public readonly InstructionBytecodes Bytecode = InstructionBytecodes.syscall_t;

    public ushort ID { get; private set; }

    public INSTR_SYSCALL_T(ushort id) : base(2)
    {
      ID = id;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", Bytecode.ToString(), ID.ToString());
    }
  }

}
