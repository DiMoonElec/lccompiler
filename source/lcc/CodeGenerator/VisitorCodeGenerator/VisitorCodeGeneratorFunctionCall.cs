using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    /*
     Блок переменных для вызова функции
     Например, параметры функции retVal f(a1, a2, a3) в озу будут лежать в следующем порядке:
     | a3 | a2 | a1 | ret |
     Если функция не возвращает никакого значения, то формат расположения элементов следующий:
      void f(a1, a2, a3) -> | a3 | a2 | a1 |
    */
    public override void Visit(FunctionCallNode n)
    {
      //Возможные варианты:
      //Вызов обычной функции (по метке)
      //Системный вызов (нативная функция)
      var functionDeclarator = n.ManagedFunctionDeclarator;
      var nativeFunctionDeclarator = n.NativeFunctionDeclarator;

      if (functionDeclarator != null) //обычная функця
        GenerateFunctionCallManaged(functionDeclarator, n);
      else if (nativeFunctionDeclarator != null)
        GenerateFunctionCallNative(nativeFunctionDeclarator, n);
      else
        throw new InternalCompilerException("FunctionCallNode должна иметь одно из полей FunctionDeclarator или NativeFunctionDeclarator не равное null");
    }

    private void GenerateFunctionCallNative(NativeFunctionDeclaratorNode functionDeclarator, FunctionCallNode n)
    {
      CheckContainStatementNode(n);

      InsertComment(n);

      FullNodeStatementBegin(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Оператор не поддерживает данный способ доступа");

      //Передаваемые аргументы
      var functionArgiments = n.Params.GetAllChilds();

      //Память под передаваемые параметры
      LocalMemoryObject[] paramsMemoryObjects = VisitFunctionArguments(functionArgiments);

      PartiallyNodeStatementBegin(n);

      //Резервируем память для возвращаемого значения
      LocalMemoryObject returnMemoryObject = null;


      returnMemoryObject = FunctionCallAllocReturnMemory(functionDeclarator.ReturnType,
        n.AccessMethod == ResultAccessMethod.MethodGet);

      n.ReturnMemoryObject = returnMemoryObject;

      /// Вызов ///

      //Текущий размер фрейма
      int frameOffset = assemblyUnit.Allocator.LocalBytesUsed();
      ushort functionID = (ushort)functionDeclarator.ID;

      //Двигаем указатель на фрейм
      AddInstructionIncrFPWithReduce(frameOffset);

      //Вызов функции
      if (functionID < 256)
        assemblyUnit.AddInstruction(new INSTR_SYSCALL_T(functionID));
      else
        assemblyUnit.AddInstruction(new INSTR_SYSCALL(functionID));

      //Возвращаем размер фрейма в исходное значение
      AddInstructionDecrFPWithReduce(frameOffset);

      /// Эпилог вызова функции ///

      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        if (returnMemoryObject == null)
          throw new InternalCompilerException("Функция не возвращает значения, но результат ее работы пытаются использовать");

        //Кладем возвращаемое значение на стек
        int size = functionDeclarator.ReturnType.Type.Sizeof();
        AddInstructionLocalMemoryLoad((short)returnMemoryObject.Address, size);

        assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);
      }
      else
      {
        if (returnMemoryObject != null)
          assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);
      }

      for (int i = 0; i < paramsMemoryObjects.Length; i++)
        assemblyUnit.Allocator.FreeLocalObj(paramsMemoryObjects[i]);

      StatementEnd(n);
    }

    private void GenerateFunctionCallManaged(FunctionDeclaratorNode functionDeclarator, FunctionCallNode n)
    {
      CheckContainStatementNode(n);

      InsertComment(n);

      FullNodeStatementBegin(n);

      if (n.AccessMethod == ResultAccessMethod.MethodGetReference)
        throw new InternalCompilerException("Конструкцию вызова функции нельзя использовать в качестве приемника данных");

      //Передаваемые аргументы
      var functionArgiments = n.Params.GetAllChilds();

      LocalMemoryObject[] paramsMemoryObjects = VisitFunctionArguments(functionArgiments);

      PartiallyNodeStatementBegin(n);

      //Резервируем память для возвращаемого значения
      LocalMemoryObject returnMemoryObject;

      returnMemoryObject = FunctionCallAllocReturnMemory(functionDeclarator.ReturnType,
        n.AccessMethod == ResultAccessMethod.MethodGet);

      n.ReturnMemoryObject = returnMemoryObject;

      /// Вызов ///
      //Генерация метки вызываемой функции
      string functionLabel = GetGlobalLabelName(functionDeclarator);
      int frameOffset = assemblyUnit.Allocator.LocalBytesUsed(); //Текущий размер фрейма

      //Двигаем указатель на фрейм
      AddInstructionIncrFPWithReduce(frameOffset);

      //Вызываем функцию
      assemblyUnit.AddInstruction(new INSTR_CALL(assemblyUnit.LabelManager.AddReference(functionLabel)));

      //Возвращаем размер фрейма в исходное значение
      AddInstructionDecrFPWithReduce(frameOffset);

      /// Эпилог вызова функции ///

      if (n.AccessMethod == ResultAccessMethod.MethodGet)
      {
        if (returnMemoryObject == null)
          throw new InternalCompilerException("Функция не возвращает значения, но результат ее работы пытаются использовать");

        //Кладем возвращаемое значение на стек
        int size = functionDeclarator.ReturnType.Type.Sizeof();
        AddInstructionLocalMemoryLoad((short)returnMemoryObject.Address, size);

        assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);
      }
      else
      {
        if (returnMemoryObject != null)
          assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);
      }

      for (int i = 0; i < paramsMemoryObjects.Length; i++)
        assemblyUnit.Allocator.FreeLocalObj(paramsMemoryObjects[i]);

      StatementEnd(n);
    }

    private LocalMemoryObject[] VisitFunctionArguments(Node[] functionArgiments)
    {
      //Память под передаваемые параметры
      List<LocalMemoryObject> paramsMemoryObjects = new List<LocalMemoryObject>();

      //Выполняем посещение аргументов функции
      for (int i = 0; i < functionArgiments.Length; i++)
      {
        int size;
        var functionArgument = (TypedNode)functionArgiments[i];

        if ((functionArgument is ObjectNode objectNode)
          && (objectNode.ObjectType.Type is LCStructType || objectNode.ObjectType.Type is LCArrayType))
        {
          //Если в качестве передаваемого объекта выступает структура или массив
          //то на стек необходимо положить адрес данного объекта, а не значение объекта
          functionArgument.AccessMethod = ResultAccessMethod.MethodGetReference;
          
          //Размер ссылки равен 4 байта
          size = 4;
        }
        else
        {
          //Если это переменная, то на стек необходимо положить значение,
          //которое хранит данный объект
          functionArgument.AccessMethod = ResultAccessMethod.MethodGet;

          //Получаем размер объекта
          size = functionArgument.ObjectType.Type.Sizeof();
        }

        //Посещаем аргумент
        functionArgument.Visit(this);

        //Выделяем место под передаваемый объект
        var memoryObject = assemblyUnit.Allocator.AllocTailLocalObj("param" + i.ToString(), size);
        paramsMemoryObjects.Add(memoryObject);

        //Переносим значение со стека в память
        AddInstructionLocalMemoryStore((short)memoryObject.Address, size);

      }

      return paramsMemoryObjects.ToArray();
    }



    #region Вспомогательные методы
    /*
    private void FunctionCallNative(NativeFunctionDeclaratorNode functionDeclarator, ResultAccessMethod accessMethod, string moduleName)
    {
      /// Пролог вызова функции ///

      //Резервируем память для аргументов функции
      List<LocalMemoryObject> paramsMemoryObjects = FunctionCallAllocArgumentsMemory(functionDeclarator.Params.GetAllChilds());

      //Резервируем память для возвращаемого значения
      LocalMemoryObject returnMemoryObject = FunctionCallAllocReturnMemory(functionDeclarator.ReturnObjectType, accessMethod);

      /// Вызов ///

      //Текущий размер фрейма
      int frameOffset = assemblyUnit.Allocator.LocalBytesUsed();
      ushort functionID = (ushort)functionDeclarator.ID;

      //Двигаем указатель на фрейм
      AddInstructionIncrFPWithReduce(frameOffset);

      //Вызов функции
      if (functionID < 256)
        assemblyUnit.AddInstruction(new INSTR_SYSCALL_T(functionID));
      else
        assemblyUnit.AddInstruction(new INSTR_SYSCALL(functionID));

      //Возвращаем размер фрейма в исходное значение
      AddInstructionDecrFPWithReduce(frameOffset);

      /// Эпилог вызова функции ///

      //Если результат выполнения функции используется программой
      //То кладем его на стек
      if (accessMethod == ResultAccessMethod.MethodGet)
      {
        if (returnMemoryObject == null)
          throw new CompilerException("Нативная функция не возвращает значения, но результат ее работы пытаются использовать");

        int size = functionDeclarator.ReturnObjectType.Sizeof();

        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_LOAD_1((short)returnMemoryObject.Address));
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_LOAD_2((short)returnMemoryObject.Address));
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_LOAD_4((short)returnMemoryObject.Address));
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_LOAD_8((short)returnMemoryObject.Address));
        else
          throw new CompilerException("Неверный размер загружаемого объекта");
      }


      //Помечяем всю используемую для вызова функции память как свободную
      if (returnMemoryObject != null)
        assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);

      for (int i = 0; i < paramsMemoryObjects.Count; i++)
        assemblyUnit.Allocator.FreeLocalObj(paramsMemoryObjects[i]);

    }

    private void FunctionCallManaged(FunctionDeclaratorNode functionDeclarator, ResultAccessMethod accessMethod, string moduleName)
    {
      /// Пролог вызова функции ///

      //Резервируем память для аргументов функции
      List<LocalMemoryObject> paramsMemoryObjects = FunctionCallAllocArgumentsMemory(functionDeclarator.Params.GetAllChilds());

      //Резервируем память для возвращаемого значения
      LocalMemoryObject returnMemoryObject = FunctionCallAllocReturnMemory(functionDeclarator.ReturnObjectType, accessMethod);

      /// Вызов ///
      //Генерация метки вызываемой функции
      string functionLabel = GetGlobalLabelName(functionDeclarator);
      int frameOffset = assemblyUnit.Allocator.LocalBytesUsed(); //Текущий размер фрейма

      //Двигаем указатель на фрейм
      AddInstructionIncrFPWithReduce(frameOffset);

      //Вызываем функцию
      assemblyUnit.AddInstruction(new INSTR_CALL(assemblyUnit.LabelManager.AddReference(functionLabel)));

      //Возвращаем размер фрейма в исходное значение
      AddInstructionDecrFPWithReduce(frameOffset);

      /// Эпилог вызова функции ///

      //Если результат выполнения функции используется программой
      //То кладем его на стек
      if (accessMethod == ResultAccessMethod.MethodGet)
      {
        if (returnMemoryObject == null)
          throw new CompilerException("Функция не возвращает значения, но результат ее работы пытаются использовать");

        int size = functionDeclarator.ReturnObjectType.Sizeof();

        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_LOAD_1((short)returnMemoryObject.Address));
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_LOAD_2((short)returnMemoryObject.Address));
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_LOAD_4((short)returnMemoryObject.Address));
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_LOAD_8((short)returnMemoryObject.Address));
        else
          throw new CompilerException("Неверный размер загружаемого объекта");
      }

      //Помечяем всю используемую для вызова функции память как свободную
      if (returnMemoryObject != null)
        assemblyUnit.Allocator.FreeLocalObj(returnMemoryObject);

      for (int i = 0; i < paramsMemoryObjects.Count; i++)
        assemblyUnit.Allocator.FreeLocalObj(paramsMemoryObjects[i]);
    }

    private List<LocalMemoryObject> FunctionCallAllocArgumentsMemory(Node[] funcParams)
    {
      List<LocalMemoryObject> paramsMemoryObjects = new List<LocalMemoryObject>();

      for (int i = funcParams.Length - 1; i >= 0; i--)
      {
        if (funcParams[i] is ObjectDeclaratorNode node)
        {
          int size = node.ObjectType.Sizeof();
          var memoryObject = assemblyUnit.Allocator.AllocTailLocalObj("param" + i.ToString(), size);
          paramsMemoryObjects.Add(memoryObject);

          //Переносим значение со стека в память
          if (size == 1)
            assemblyUnit.AddInstruction(new INSTR_STORE_1((short)memoryObject.Address));
          else if (size == 2)
            assemblyUnit.AddInstruction(new INSTR_STORE_2((short)memoryObject.Address));
          else if (size == 4)
            assemblyUnit.AddInstruction(new INSTR_STORE_4((short)memoryObject.Address));
          else if (size == 8)
            assemblyUnit.AddInstruction(new INSTR_STORE_8((short)memoryObject.Address));
          else
            throw new CompilerException("Неверный размер загружаемого объекта");
        }
        else
          throw new CompilerException("Поле Params ноды FunctionDeclaratorNode должно содержать ноль либо несколько потомков типа TypedNode");
      }

      return paramsMemoryObjects;
    }
    */


    private LocalMemoryObject FunctionCallAllocReturnMemory(LCObjectType objectType, bool resetAllocatedMemory)
    {
      LocalMemoryObject ret = null;
      var size = objectType.Type.Sizeof();

      if (size != 0)
      {
        //Резервируем память для возвращаемого значения
        ret = assemblyUnit.Allocator.AllocTailLocalObj("retobj", size);

        if (resetAllocatedMemory)
        {
          AddInstructionLocalMemorySetZero((short)ret.Address, size);
        }
      }
      else
      {
        //Попытка занулить память для возвращаемого значения,
        //в ситуации, когда функция типа void.
        //Генерируем исключение, чтоб обратить внимание на это логическое противоречение
        if (resetAllocatedMemory)
          throw new InternalCompilerException("Возвращаемый функцией объект имеет размер ноль байт");
      }

      return ret;
    }

    private void AddInstructionLocalMemoryStore(short address, int size)
    {
      if (size == 1)
        assemblyUnit.AddInstruction(new INSTR_STORE_1(address));
      else if (size == 2)
        assemblyUnit.AddInstruction(new INSTR_STORE_2(address));
      else if (size == 4)
        assemblyUnit.AddInstruction(new INSTR_STORE_4(address));
      else if (size == 8)
        assemblyUnit.AddInstruction(new INSTR_STORE_8(address));
      else
        throw new InternalCompilerException("Неверный размер загружаемого объекта");
    }

    private void AddInstructionLocalMemoryLoad(short address, int size)
    {
      if (size == 1)
        assemblyUnit.AddInstruction(new INSTR_LOAD_1(address));
      else if (size == 2)
        assemblyUnit.AddInstruction(new INSTR_LOAD_2(address));
      else if (size == 4)
        assemblyUnit.AddInstruction(new INSTR_LOAD_4(address));
      else if (size == 8)
        assemblyUnit.AddInstruction(new INSTR_LOAD_8(address));
      else
        throw new InternalCompilerException("Неверный размер загружаемого объекта");
    }

    private void AddInstructionLocalMemorySetZero(short adr, int size)
    {
      switch (size)
      {
        case 1:
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { 0x00 }));
          assemblyUnit.AddInstruction(new INSTR_STORE_1(adr));
          break;

        case 2:
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { 0x00 }));
          assemblyUnit.AddInstruction(new INSTR_STORE_2(adr));
          break;

        case 4:
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { 0x00 }));
          assemblyUnit.AddInstruction(new INSTR_STORE_4(adr));
          break;

        case 8:
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { 0x00 }));
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new byte[] { 0x00 }));
          assemblyUnit.AddInstruction(new INSTR_STORE_8(adr));
          break;

        default:
          throw new InternalCompilerException("Неверный размер загружаемого объекта");
      }
    }

    private void AddInstructionIncrFPWithReduce(int frameOffset)
    {
      if (frameOffset < 256)
        assemblyUnit.AddInstruction(new INSTR_INCR_FP_T((byte)frameOffset));
      else
        assemblyUnit.AddInstruction(new INSTR_INCR_FP((ushort)frameOffset));
    }

    private void AddInstructionDecrFPWithReduce(int frameOffset)
    {
      if (frameOffset < 256)
        assemblyUnit.AddInstruction(new INSTR_DECR_FP_T((byte)frameOffset));
      else
        assemblyUnit.AddInstruction(new INSTR_DECR_FP((ushort)frameOffset));
    }

    #endregion
  }
}
