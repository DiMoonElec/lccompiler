using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    AssemblyUnit assemblyUnit;

    string currentFunctionName;

    public ulong LabelCounter { get; private set; }

    public VisitorCodeGenerator(AssemblyUnit u)
    {
      assemblyUnit = u;
      LabelCounter = 0;
    }

    public override void Visit(ModuleRootNode n)
    {
      base.Visit(n);
    }

    public override void Visit(ModuleInitNode n)
    {
      assemblyUnit.SetCurrentSectionInit();
      base.Visit(n);
    }

    public override void Visit(VariableDeclaratorNode n)
    {
      int size = n.ObjectType.Type.Sizeof();

      if (n.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
      {
        assemblyUnit.GlobalAllocator.Declaration(GetGlobalLabelName(n), size, n.Attribute, n.ObjectType);
      }
      else if (n.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassLocal)
      {
        n.LocalMemoryObject = assemblyUnit.Allocator.AllocLocalObj(n.Name, size);
      }
      else if (n.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam)
      {
        n.LocalMemoryObject = assemblyUnit.Allocator.CreateParamObj(n.Name, size);
      }
    }

    public override void Visit(ManagedFunctionDeclaratorNode n)
    {
      currentFunctionName = n.Name;
      string labelName = GetGlobalLabelName(n);

      assemblyUnit.SetCurrentSectionCode();

      //Начало определения функции
      assemblyUnit.FunctionBegin(currentFunctionName);

      //Создаем новый локальный пул для хранения локальных переменных
      //данной функции
      assemblyUnit.Allocator.NewLocalPool(labelName);

      //создаем метку на данную функцию
      InsertComment(n);
      var label = assemblyUnit.LabelManager.Declaration(labelName);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label));

      //создаем ссылку на возвращаемое значение
      var size = n.ReturnType.Type.Sizeof();
      if (size != 0)
        assemblyUnit.Allocator.CreateParamObj(n.Name, size);

      //Посещаем всех потомков
      base.Visit(n);

      //Проверяем, если последняя инструкция не равна ret, то добавляем ее
      if (assemblyUnit.GetLastInstruction().GetType() != typeof(INSTR_RET))
      {
        StatementBegin(n);
        assemblyUnit.AddInstruction(new INSTR_RET());
        StatementEnd(n);
      }

      //Конец определения функции
      assemblyUnit.FunctionEnd();
    }

    public override void Visit(BlockCodeNode n)
    {
      //Посещаем всех потомков
      base.Visit(n);

      //В конце блока
      for (int i = 0; i < n.CountChildrens; i++)
      {
        if (n.GetChild(i) is VariableDeclaratorNode variableDeclaratorNode)
        {
          //Помечаем выделенную память для локлаьных переменных
          //как свободную
          if (variableDeclaratorNode.ClassValue == ObjectDeclaratorNode.DeclaratorClass.ClassLocal)
            assemblyUnit.Allocator.FreeLocalObj(variableDeclaratorNode.LocalMemoryObject);
        }
      }
    }

    public override void Visit(ObjectNode n)
    {
      InsertComment(n);

      var obj = n.ObjectType;
      var type = obj.Type;
      var objDeclarator = n.ObjDeclaratorNode;
      var classValue = objDeclarator.ClassValue;

      //Если Set или Unuse, то выходим
      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
        return;
      else if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        var size = type.Sizeof();

        if (classValue == ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
        {
          string objName = GetGlobalLabelName(objDeclarator);
          GlobalMemoryObjectReference memoryObjectReference = new GlobalMemoryObjectReference(objName);
          assemblyUnit.GlobalAllocator.AddReference(memoryObjectReference);

          if (size == 1)
            assemblyUnit.AddInstruction(new INSTR_GLOAD_1(memoryObjectReference));
          else if (size == 2)
            assemblyUnit.AddInstruction(new INSTR_GLOAD_2(memoryObjectReference));
          else if (size == 4)
            assemblyUnit.AddInstruction(new INSTR_GLOAD_4(memoryObjectReference));
          else if (size == 8)
            assemblyUnit.AddInstruction(new INSTR_GLOAD_8(memoryObjectReference));
          else
            throw new InternalCompilerException("Неверный размер загружаемого объекта");
        }
        else
        {
          //Это локальный объект, используем инструкции LOAD и загружаем по смещению

          var offset = objDeclarator.LocalMemoryObject.Address;

          if (size == 1)
            assemblyUnit.AddInstruction(new INSTR_LOAD_1(offset));
          else if (size == 2)
            assemblyUnit.AddInstruction(new INSTR_LOAD_2(offset));
          else if (size == 4)
            assemblyUnit.AddInstruction(new INSTR_LOAD_4(offset));
          else if (size == 8)
            assemblyUnit.AddInstruction(new INSTR_LOAD_8(offset));
          else
            throw new InternalCompilerException("Неверный размер загружаемого объекта");
        }
      }
      else if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
      {
        if (classValue == ObjectDeclaratorNode.DeclaratorClass.ClassGlobal)
        {
          string objName = GetGlobalLabelName(objDeclarator);
          GlobalMemoryObjectReference memoryObjectReference = new GlobalMemoryObjectReference(objName);
          assemblyUnit.GlobalAllocator.AddReference(memoryObjectReference);

          assemblyUnit.AddInstruction(new INSTR_PUSH_4_MEM_OBJ(memoryObjectReference));
        }
        else
        {
          throw new InternalCompilerException("Невозможно получить ссылку на локальный ссылочный объект");
          /*
          var offset = objDeclarator.LocalMemoryObject.Address;
          assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(offset)));
          assemblyUnit.AddInstruction(new INSTR_ADD_FP());
          */
        }
      }

    }

    public override void Visit(ConstantValueNode n)
    {
      InsertComment(n);

      if (n.AccessMethod == ResultAccessMethod.MethodUnuse)
        return;
      else if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Неврзможно получить ссылку на константное значение");

      //Остался только Get
      var constant = n.Constant;

      if (constant is SByteConstantValue sbyteConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { (byte)sbyteConstant.Value }));
      else if (constant is ShortConstantValue shortConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_2(BitConverter.GetBytes(shortConstant.Value)));
      else if (constant is IntConstantValue intConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(intConstant.Value)));
      else if (constant is LongConstantValue longConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_8(BitConverter.GetBytes(longConstant.Value)));

      else if (constant is ByteConstantValue byteConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { (byte)byteConstant.Value }));
      else if (constant is UShortConstantValue ushortConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_2(BitConverter.GetBytes(ushortConstant.Value)));
      else if (constant is UIntConstantValue uintConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(uintConstant.Value)));
      else if (constant is ULongConstantValue ulongConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_8(BitConverter.GetBytes(ulongConstant.Value)));

      else if (constant is FloatConstantValue floatConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(floatConstant.Value)));
      else if (constant is DoubleConstantValue doubleConstant)
        assemblyUnit.AddInstruction(new INSTR_PUSH_8(BitConverter.GetBytes(doubleConstant.Value)));
      else if (constant is BooleanConstantValue constantValue)
        assemblyUnit.AddInstruction(new INSTR_PUSH_1(BitConverter.GetBytes(constantValue.Value)));
      else
        throw new InternalCompilerException("Неизвестный тип константы");
    }

    public override void Visit(LabelCaseNode n)
    {
      InsertComment(n);
      var label = assemblyUnit.LabelManager.Declaration(n.LabelCaseEntry);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label));
    }

    public override void Visit(LabelDefaultNode n)
    {
      InsertComment(n);
      var label = assemblyUnit.LabelManager.Declaration(n.LabelDefaultEntry);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label));
    }

  }
}
