namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Декларация структуры
  /// </summary>
  internal class LCStructDeclarator
  {
    public string TypeName { get; private set; }

    public LCStructTypeElement[] Elements { get; private set; }

    /// <summary>
    /// Используется при объявлении структуры
    /// </summary>
    public LCStructDeclarator(string typeName, LCStructTypeElement[] elements)
    {
      TypeName = typeName;
      Elements = elements;
    }

    public LCStructDeclarator Clone()
    {
      LCStructTypeElement[] e = new LCStructTypeElement[Elements.Length];
      for (int i = 0; i < e.Length; i++)
        e[i] = Elements[i].Clone();

      LCStructDeclarator r = new LCStructDeclarator(TypeName, e);

      return r;
    }

    public override string ToString()
    {
      return "struct";
    }

    public int Sizeof()
    {
      int size = 0;
      for (int i = 0; i < Elements.Length; i++)
        size += Elements[i].Sizeof();
      return size;
    }
  }

  /// <summary>
  /// Абстрактный класс элемента структуры LCStructType
  /// </summary>
  internal abstract class LCStructTypeElement
  {
    /// <summary>
    /// Имя элемента структуры
    /// </summary>
    public string Name { get; protected set; }

    public abstract int Sizeof();

    public abstract LCStructTypeElement Clone();
  }

  /// <summary>
  /// Элемент структуры LCPrimitiveType
  /// </summary>
  internal class LCStructElementPrimitiveType : LCStructTypeElement
  {
    /// <summary>
    /// Тип элемента структуры
    /// </summary>
    public LCPrimitiveType Type { get; private set; }

    public LCStructElementPrimitiveType(LCPrimitiveType type, string name)
    {
      Type = type;
      Name = name;
    }

    public override int Sizeof()
    {
      return Type.Sizeof();
    }

    public override LCStructTypeElement Clone()
    {
      return new LCStructElementPrimitiveType(new LCPrimitiveType(Type.Type), Name);
    }
  }


  /// <summary>
  /// Элемент структуры LCArrayType
  /// </summary>
  internal class LCStructElementArrayType : LCStructTypeElement
  {
    /// <summary>
    /// Тип элемента структуры
    /// </summary>
    public LCArrayType Type { get; private set; }

    public LCStructElementArrayType(LCArrayType type, string name)
    {
      Type = type;
      Name = name;
    }

    public override int Sizeof()
    {
      return Type.Sizeof();
    }

    public override LCStructTypeElement Clone()
    {
      return new LCStructElementArrayType(new LCArrayType(Type.TypeElement, Type.ArrayDepth), Name);
    }
  }

}
