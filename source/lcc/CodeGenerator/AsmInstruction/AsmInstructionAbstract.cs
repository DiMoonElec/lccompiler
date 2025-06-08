using System;

namespace LC2.LCCompiler.CodeGenerator.AsmInstruction
{
  internal abstract class LCVMAsmItem
  {
    /// <summary>
    /// Данная переменная используется оптимизатором LLOptimizer_002
    /// </summary>
    public bool LLOptimizer_002_trace = false;

    public abstract new string ToString();

    public int InstrSize { get; private set; }

    public int CurrentPosition { get; private set; }

    public LCVMAsmItem(int instrSize)
    {
      InstrSize = instrSize;
      CurrentPosition = 0;
    }

    public virtual void SetCurrentPosition(int value)
    {
      CurrentPosition = value;
    }

    public void Translate(TranslatorVisitor translator)
    {
      if (this is LCVMInstruction)
        translator.Generate((dynamic)this);
    }
  }

  /// <summary>
  /// Базовый класс для инструкций микроразметки ассемблерного листинга
  /// </summary>
  internal abstract class LCVMMicromarking : LCVMAsmItem
  {
    public LCVMMicromarking(int instrSize) : base(instrSize)
    { }
  }

  /// <summary>
  /// Базовый класс для инструкций
  /// </summary>
  internal abstract class LCVMInstruction : LCVMAsmItem
  {
    public LCVMInstruction(int instrSize) : base(instrSize)
    {

    }
  }

  internal abstract class INSTR_PUSH : LCVMInstruction
  {
    public byte[] Value { get; private set; }

    public INSTR_PUSH(byte[] value, int instrSize) : base(instrSize)
    {
      Value = value;
    }

    protected string ValueToString()
    {
      if (Value.Length == 1)
        return string.Format(" 0x{0:X2}", Value[0]);

      string r = " {";
      for (int i = 0; i < Value.Length; i++)
      {
        r += string.Format("0x{0:X2}", Value[i]);
        if (i != Value.Length - 1)
          r += ", ";
      }
      r += "}";
      return r;
    }
  }

  internal abstract class LCVMInstructionGLoadGStore : LCVMInstruction, ILCVMInstructionMemoryObjectReference
  {
    public GlobalMemoryObjectReference Reference { get; private set; }

    public LCVMInstructionGLoadGStore(GlobalMemoryObjectReference objectReference, int instrSize) : base(instrSize)
    {
      Reference = objectReference;
    }
  }

  internal abstract class LCVMInstructionLoadStore : LCVMInstruction
  {
    public int Offset { get; private set; }

    public LCVMInstructionLoadStore(int offset, int instrSize) : base(instrSize)
    {
      Offset = offset;
    }
  }


  internal abstract class LCVMArithmeticInstruction : LCVMInstruction
  {
    public LCVM_DataTypes DataType { get; internal set; }

    public InstructionBytecodes Bytecode { get; internal set; }

    public LCVMArithmeticInstruction(InstructionBytecodes bytecode, LCVM_DataTypes dataType) : base(2)
    {
      Bytecode = bytecode;
      DataType = dataType;
    }

    public override string ToString()
    {
      return string.Format("{0} [{1}]", Bytecode.ToString(), LCVM_DataTypesExtensions.ToString(DataType));
    }
  }


  /// <summary>
  /// Базовый класс для инструкций перехода
  /// </summary>
  internal abstract class LCVMJmpInstruction : LCVMInstruction, ILCVMInstructionCodeObjectReference
  {
    public CodeLabelReference LabelReference { get; private set; }

    public LCVMJmpInstruction(CodeLabelReference labelReference, int instrSize) : base(instrSize)
    {
      LabelReference = labelReference;
    }

    public void UpdateReference(CodeLabelReference reference)
    {
      LabelReference = reference;
    }

    public int GetLabelOffset()
    {
      int offset = LabelReference.Address - CurrentPosition;
      return offset;
    }
  }

  internal class LCVMSectionData : LCVMAsmItem
  {
    public LCVMSectionData() : base(0)
    {
    }

    public override string ToString()
    {
      return ".data";
    }
  }

  internal class LCVMData : LCVMAsmItem
  {
    /// <summary>
    /// Имя объекта памяти
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Размер занимаемой памяти
    /// </summary>
    public uint Size { get; private set; }

    /// <summary>
    /// Тип
    /// </summary>
    public string Type { get; private set; }



    public LCVMData(string name, uint size, string type) : base(0)
    {
      Name = name;
      Size = size;
      Type = type;
    }

    public override string ToString()
    {
      throw new NotImplementedException();
    }
  }

  internal class LCVMSectionInit : LCVMAsmItem
  {
    public LCVMSectionInit() : base(0)
    {
    }

    public override string ToString()
    {
      return ".init";
    }
  }

  internal class LCVMSectionCode : LCVMAsmItem
  {
    public LCVMSectionCode() : base(0)
    {
    }

    public override string ToString()
    {
      return ".code";
    }
  }


  internal class LCVMComment : LCVMAsmItem
  {
    public string Comment;

    public LCVMComment() : base(0)
    {

    }

    public LCVMComment(string comment) : base(0)
    {
      Comment = comment;
    }

    public override string ToString()
    {
      if (Comment == "")
        return "";

      return ";" + Comment;
    }
  }

  internal class LCVMModuleBegin : LCVMMicromarking
  {
    public readonly string Name;
    public LCVMModuleBegin(string name) : base(0)
    {
      Name = name;
    }

    public override string ToString()
    {
      return string.Format(";dbginfo: Module: {0}", Name);
    }
  }

  internal class LCVMModuleEnd : LCVMMicromarking
  {
    public LCVMModuleEnd() : base(0) { }

    public override string ToString()
    {
      return ";dbginfo: End module";
    }
  }

  internal class LCVMFunctionBegin : LCVMMicromarking
  {
    public readonly string Name;
    public LCVMFunctionBegin(string name) : base(0)
    {
      Name = name;
    }

    public override string ToString()
    {
      return string.Format(";dbginfo: Function: {0}", Name);
    }
  }

  internal class LCVMFunctionEnd : LCVMMicromarking
  {
    public LCVMFunctionEnd() : base(0) { }

    public override string ToString()
    {
      return ";dbginfo: End function";
    }
  }

  internal class LCVMStatementBegin : LCVMMicromarking
  {
    public readonly int StartLine;
    public readonly int StartColumn;
    public readonly int EndLine;
    public readonly int EndColumn;

    public LCVMStatementBegin(int startLine, int startColumn, int endLine, int endColumn) : base(0)
    {
      StartLine = startLine;
      StartColumn = startColumn;
      EndLine = endLine;
      EndColumn = endColumn;
    }

    public override string ToString()
    {
      return string.Format(";dbginfo: Statement Begin [StartLine={0}, StartColumn={1}, EndLine={2}, EndColumn={3}]", StartLine, StartColumn, EndLine, EndColumn);
    }
  }

  internal class LCVMStatementEnd : LCVMMicromarking
  {
    public LCVMStatementEnd() : base(0) { }

    public override string ToString()
    {
      return ";dbginfo: Statement End";
    }
  }

  /// <summary>
  /// Интерфейс, который реализует инструкции, 
  /// содержащие ссылку на объект, расположенный в ОЗУ
  /// </summary>
  internal interface ILCVMInstructionMemoryObjectReference
  {
    //Ссылка на объект ОЗУ
    GlobalMemoryObjectReference Reference { get; }
  }

  /// <summary>
  /// Интерфейс, который реализует инструкции, 
  /// содержащие ссылку на объект, расположенный в ПЗУ
  /// </summary>
  internal interface ILCVMInstructionCodeObjectReference
  {
    CodeLabelReference LabelReference { get; }
    void UpdateReference(CodeLabelReference reference);
  }

  /// <summary>
  /// Интерфейс, который реализует инструкции, 
  /// являющимися ссылками на объект, расположенный в ПЗУ
  /// </summary>
  internal interface ILCVMInstructionCodeObject
  {
    CodeLabel Label { get; }
  }

}
