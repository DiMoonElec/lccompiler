using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  internal class LCObjectType
  {
    /// <summary>
    /// Тип объекта
    /// </summary>
    public LCType Type { get; private set; }

    /// <summary>
    /// Возможно ли чтение значения объекта
    /// </summary>
    public bool Readable
    {
      get
      {
        if (_attributesInitialized == false)
          throw new InternalCompilerException("Атрибуты доступа объекта не были инициализированы ранее");

        return _readable;
      }
      private set
      {
        _readable = value;
      }
    }

    /// <summary>
    /// Возможна ли запись значения объекта
    /// </summary>
    public bool Writeable
    {
      get
      {
        if (_attributesInitialized == false)
          throw new InternalCompilerException("Атрибуты доступа объекта не были инициализированы ранее");

        return _writeable;
      }
      private set
      {
        _writeable = value;
      }
    }

    bool _readable = false;
    bool _writeable = false;
    bool _attributesInitialized = false;

    public LCObjectType(LCType type)
    {
      Type = type;
    }

    public LCObjectType(LCType type, bool readable, bool writeable)
    {
      Type = type;
      SetAccessAttributes(readable, writeable);
    }

    public void SetAccessAttributes(bool readable, bool writeable)
    {
      if (_attributesInitialized)
        throw new InternalCompilerException("Атрибуты доступа объекта уже были инициализированы ранее");

      Readable = readable;
      Writeable = writeable;
      _attributesInitialized = true;
    }

    public LCObjectType Clone()
    {
      LCObjectType r = new LCObjectType(Type.Clone(), Readable, Writeable);
      return r;
    }
    /*
    static public bool ObjectAssignmentValidate(LCObjectType leftObject, LCObjectType rightObject)
    {
      if (leftObject.Writeable == false)
        return false;

      if (rightObject.Readable == false)
        return false;

      var leftType = leftObject.Type;
      var rightType = rightObject.Type;

      return LCTypesUtils.AssignmentValidate(leftType, rightType);
    }
    */

    public new string ToString()
    {
      return Type.ToString();
    }
  }
}
