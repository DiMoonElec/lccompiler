using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(IndexerNode n)
    {
      InsertComment(n);

      //PreVisit
      TypedNode obj = (TypedNode)n.IndexingObj.GetChild(0);
      TypedNode index = (TypedNode)n.Index.GetChild(0);

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
      {
        obj.AccessMethod = ResultAccessMethod.MethodUnuse;
        index.AccessMethod = ResultAccessMethod.MethodUnuse;

        obj.Visit(this);
        index.Visit(this);

        return;
      }


      var objType = obj.ObjectType.Type;

      if (objType is LCArrayType)
        obj.AccessMethod = ResultAccessMethod.MethodGetReference;
      else if (objType is LCPointerArrayType)
        obj.AccessMethod = ResultAccessMethod.MethodGet;
      else
        throw new InternalCompilerException("Индексируемый объект должен быть массивом либо указателем");

      index.AccessMethod = ResultAccessMethod.MethodGet;

      obj.Visit(this);
      index.Visit(this);

      var size = n.ObjectType.Type.Sizeof();

      //На вершине стека лежит индекс массива

      assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes((uint)size))); //Кладем на стек размер одного элемента
      assemblyUnit.AddInstruction(new INSTR_MUL(LCVM_DataTypes.Type_Int)); //умножаем индекс на размер элемента
      assemblyUnit.AddInstruction(new INSTR_ADD(LCVM_DataTypes.Type_Int)); //вычисляем адрес элемента

      //На данном этапе на стеке лежит адрес элемента

      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        //Если необходимо получить значение элемента массива, то выполняем команду ALOAD_x
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
