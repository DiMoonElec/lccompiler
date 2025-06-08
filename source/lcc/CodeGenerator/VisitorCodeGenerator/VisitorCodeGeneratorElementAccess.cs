using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(ElementAccessNode n)
    {
      InsertComment(n);

      TypedNode obj = (TypedNode)n.GetChild(0);

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        obj.AccessMethod = ResultAccessMethod.MethodUnuse;
        obj.Visit(this);
        return;
      }

      var objType = obj.ObjectType.Type;

      if (objType is LCStructType)
        obj.AccessMethod = ResultAccessMethod.MethodGetReference;
      else if(objType is LCPointerStructType)
        obj.AccessMethod = ResultAccessMethod.MethodGet;
      else if(objType is LCArrayType)
        obj.AccessMethod = ResultAccessMethod.MethodGetReference;
      else if (objType is LCPointerArrayType)
        obj.AccessMethod = ResultAccessMethod.MethodGet;
      else
        throw new InternalCompilerException("Индексируемый объект должен быть массивом либо указателем");

      obj.Visit(this); //Кладем на стек ссылку на структуру

      assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes((uint)n.ElementOffset))); //Кладем смещение элемента
      assemblyUnit.AddInstruction(new INSTR_ADD(LCVM_DataTypes.Type_UInt)); //Вычисляем адрес элемента

      //На данном этапе на стеке лежит адрес элемента

      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        //Если необходимо получить значение элемента массива, то выполняем команду ALOAD_x
        var size = n.ObjectType.Type.Sizeof();

        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_ALOAD_1());
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_ALOAD_2());
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_ALOAD_4());
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_ALOAD_8());
        else
          throw new InternalCompilerException("Неверный размер загружаемого объекта");
      }
    }
  }
}
