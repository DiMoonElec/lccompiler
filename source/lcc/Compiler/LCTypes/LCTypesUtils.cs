using System;
using System.Collections.Generic;
using System.Linq;

namespace LC2.LCCompiler.Compiler
{
  internal static class LCTypesUtils
  {
    public enum GroupPrimitiveTypes
    {
      ClassInt = 1 << 0,
      ClassUInt = 1 << 1,
      ClassFloat = 1 << 2,
      ClassBool = 1 << 3,
      ClassAll = ClassInt | ClassUInt | ClassFloat | ClassBool,
    };

    /// <summary>
    /// Проверяет, являются ли типы идентичными
    /// </summary>
    /// <param name="t1">1й тип</param>
    /// <param name="t2">2й тип</param>
    public static bool IsEqual(LCType t1, LCType t2)
    {
      if ((t1 is LCPrimitiveType primitiveType1) && (t2 is LCPrimitiveType primitiveType2))
        return IsEqualPrimitiveType(primitiveType1, primitiveType2);
      else if ((t1 is LCArrayTypeGroup arrayType1) && (t2 is LCArrayTypeGroup arrayType2))
        return IsEqualArrayType(arrayType1, arrayType2);
      else if ((t1 is LСStructTypeGroup structType1) && (t2 is LСStructTypeGroup structType2))
        return IsEqualStructType(structType1, structType2);

      return false;
    }

    /// <summary>
    /// Проверяет, можно ли левому типу присвоить правый
    /// </summary>
    /*public static bool AssignmentValidate(LCType leftType, LCType rightType)
    {
      if (rightType is LCPrimitiveType rightPrimitiveType)
        return ValidateRightPrimitiveType(rightPrimitiveType, leftType);
      else if (rightType is LCArrayType rightArrayType)
        return ValidateRightArrayType(rightArrayType, leftType);
      else if (rightType is LCPointerArrayType rightPointerType)
        return ValidateRightPointerType(rightPointerType, leftType);
      else if (rightType is LCStructType rightStructType)
        return ValidateRightStructType(rightStructType, leftType);
      else
        return false;
    }
    */


    public static string PrimitiveTypeGetName(LCPrimitiveType.PrimitiveTypes[] t)
    {
      string ret = "";
      for (int i = 0; i < t.Length; i++)
      {
        ret += PrimitiveTypeGetName(t[i]);
        if (i != t.Length - 1)
          ret += ", ";
      }

      return ret;
    }

    /// <summary>
    /// Преобразует примитивный тип в его эквивалентное имя
    /// </summary>
    public static string PrimitiveTypeGetName(LCPrimitiveType.PrimitiveTypes t)
    {
      switch (t)
      {
        case LCPrimitiveType.PrimitiveTypes.LCTypeSByte:
          return "sbyte";

        case LCPrimitiveType.PrimitiveTypes.LCTypeShort:
          return "short";

        case LCPrimitiveType.PrimitiveTypes.LCTypeInt:
          return "int";

        case LCPrimitiveType.PrimitiveTypes.LCTypeLong:
          return "long";

        case LCPrimitiveType.PrimitiveTypes.LCTypeByte:
          return "byte";

        case LCPrimitiveType.PrimitiveTypes.LCTypeUShort:
          return "ushort";

        case LCPrimitiveType.PrimitiveTypes.LCTypeUInt:
          return "uint";

        case LCPrimitiveType.PrimitiveTypes.LCTypeULong:
          return "ulong";

        case LCPrimitiveType.PrimitiveTypes.LCTypeFloat:
          return "float";

        case LCPrimitiveType.PrimitiveTypes.LCTypeDouble:
          return "double";

        case LCPrimitiveType.PrimitiveTypes.LCTypeBool:
          return "bool";

        case LCPrimitiveType.PrimitiveTypes.LCTypeVoid:
          return "void";

        default:
          throw new InternalCompilerException("Неизвестный тип данных");
      }
    }

    public static bool ItIsFromList(LCPrimitiveType primitiveType, LCPrimitiveType.PrimitiveTypes[] types)
    {
      for (int i = 0; i < types.Length; i++)
      {
        if (types[i] == primitiveType.Type)
          return true;
      }

      return false;
    }

    public static LCPrimitiveType.PrimitiveTypes[] GetGroupList(GroupPrimitiveTypes classes)
    {
      List<LCPrimitiveType.PrimitiveTypes> types = new List<LCPrimitiveType.PrimitiveTypes>();

      if ((classes & GroupPrimitiveTypes.ClassInt) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeSByte);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeShort);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeInt);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeLong);
      }

      if ((classes & GroupPrimitiveTypes.ClassUInt) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeByte);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeUShort);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeUInt);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeULong);
      }

      if ((classes & GroupPrimitiveTypes.ClassFloat) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeFloat);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeDouble);
      }

      if ((classes & GroupPrimitiveTypes.ClassBool) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeBool);
      }

      return types.ToArray();
    }

    public static bool ItIsFromGroup(LCPrimitiveType primitiveType, GroupPrimitiveTypes classes)
    {
      List<LCPrimitiveType.PrimitiveTypes> types = new List<LCPrimitiveType.PrimitiveTypes>();

      if ((classes & GroupPrimitiveTypes.ClassInt) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeSByte);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeShort);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeInt);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeLong);
      }

      if ((classes & GroupPrimitiveTypes.ClassUInt) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeByte);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeUShort);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeUInt);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeULong);
      }

      if ((classes & GroupPrimitiveTypes.ClassFloat) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeFloat);
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeDouble);
      }

      if ((classes & GroupPrimitiveTypes.ClassBool) != 0)
      {
        types.Add(LCPrimitiveType.PrimitiveTypes.LCTypeBool);
      }

      for (int i = 0; i < types.Count; i++)
      {
        if (types[i] == primitiveType.Type)
          return true;
      }

      return false;
    }

    #region Вспомогательные методы

    private static bool IsEqualStructType(LСStructTypeGroup userType1, LСStructTypeGroup userType2)
    {
      if (userType1.StructDeclarator == null || userType2.StructDeclarator == null)
        return false;

      var elements1 = userType1.StructDeclarator.Elements;
      var elements2 = userType2.StructDeclarator.Elements;

      if (elements1.Length != elements2.Length)
        return false;

      for (int i = 0; i < elements1.Length; i++)
      {
        var e1 = elements1[i];
        var e2 = elements2[i];

        if (IsEqualStructElements(e1, e2) == false)
          return false;
      }

      return true;
    }

    private static bool IsEqualStructElements(LCStructTypeElement e1, LCStructTypeElement e2)
    {
      if (e1.Name != e2.Name)
        return false;

      if (e1 is LCStructElementPrimitiveType primitiveType1 && e2 is LCStructElementPrimitiveType primitiveType2)
      {
        //Для примитивного типа элементов

        if (primitiveType1.Type.Type == primitiveType2.Type.Type)
          return true;
      }
      else if (e1 is LCStructElementArrayType arrayType1 && e2 is LCStructElementArrayType arrayType2)
      {
        //Для массивов

        if ((arrayType1.Type.TypeElement.Type == arrayType2.Type.TypeElement.Type)
          && (arrayType1.Type.ArrayDepth == arrayType2.Type.ArrayDepth))
          return true;
      }

      return false;
    }

    private static bool IsEqualArrayType(LCArrayTypeGroup arrayType1, LCArrayTypeGroup arrayType2)
    {
      return arrayType1.TypeElement.Type == arrayType2.TypeElement.Type;
    }

    private static bool IsEqualPrimitiveType(LCPrimitiveType primitiveType1, LCPrimitiveType primitiveType2)
    {
      return primitiveType1.Type == primitiveType2.Type;
    }

    #endregion
  }
}
