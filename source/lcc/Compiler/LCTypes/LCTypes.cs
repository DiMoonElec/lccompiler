namespace LC2.LCCompiler.Compiler
{
  internal abstract class LCType
  {
    public abstract LCType Clone();

    public abstract new string ToString();

    public abstract int Sizeof();
  }


  /// <summary>
  /// Пользовательский тип, при парсинге будет заменен на объявленный тип, имеющий такое же имя
  /// </summary>
  internal abstract class LСStructTypeGroup : LCType
  {
    public string TypeName { get; private set; }

    /// <summary>
    /// Декларатор структуры
    /// </summary>
    public LCStructDeclarator StructDeclarator { get; set; }

    public LСStructTypeGroup(string name)
    {
      TypeName = name;
      StructDeclarator = null;
    }

    public void SetStructDeclarator(LCStructDeclarator structDeclarator)
    {
      if (StructDeclarator != null)
        throw new InternalCompilerException("Декларатор структуры был установлен ранее");

      StructDeclarator = structDeclarator;
    }

    public override string ToString()
    {
      return TypeName;
    }
  }


  internal abstract class LCArrayTypeGroup : LCType
  {
    /// <summary>
    /// Тип элемента массива
    /// </summary>
    public LCPrimitiveType TypeElement { get; internal set; }
  }
}


