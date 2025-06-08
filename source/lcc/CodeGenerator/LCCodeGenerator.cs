using LC2.DebugInfo;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  static internal class LCCodeGenerator
  {
    static public AssemblyUnit AssemblyUnitGenerate(Node node,
      string moduleName, string moduleRelativePath)
    {
      AssemblyUnit assemblyUnit = new AssemblyUnit(moduleName);
      VisitorCodeGenerator generator = new VisitorCodeGenerator(assemblyUnit);

      assemblyUnit.ModuleBegin(moduleRelativePath);
      node.Visit(generator);
      assemblyUnit.ModuleEnd();

      return assemblyUnit;
    }

    /// <summary>
    /// Выполнить линковку проекта
    /// </summary>
    /// <param name="compiledUnits">Все модули проекта</param>
    /// <param name="entryInit">Функция init</param>
    /// <param name="logger">Логгер ошибок</param>
    /// <returns>Собранная программа</returns>
    static public AssemblyProgram Link(CompiledUnit[] compiledUnits, FunctionDeclaratorNode entryInit, CompilerLogger logger)
    {
      AssemblyProgram assemblyProgram = new AssemblyProgram();

      //Добавляем все глобальные объекты из ОЗУ
      foreach (var u in compiledUnits)
      {
        assemblyProgram.GlobalAllocator.Merge(u.assemblyUnit.GlobalAllocator);
        //assemblyProgram.LabelManager.Merge(u.assemblyUnit.LabelManager);
      }

      //Распределение глобальных объектов в памяти
      assemblyProgram.GlobalAllocator.Allocate();

      //Пустой элемент для инструкции инициализации FP
      //assemblyProgram.Code.Add(null);

      //Инициализация FP
      assemblyProgram.Code.Add(new INSTR_INCR_FP((ushort)assemblyProgram.GlobalAllocator.MemoryUsage));

      //Добавляем код инициализации из каждого модуля
      foreach (var u in compiledUnits)
      {
        assemblyProgram.AddCodeRange(u.assemblyUnit.SectionInit);
      }

      //Генерируем Startup-код
      //Вызываем функцию main из модуля main

      //Отладочная информация точки входа в приложение
      /*
      assemblyProgram.Code.Add(new LCVMModuleBegin(moduleEntryPoint.ModuleName));
      assemblyProgram.Code.Add(new LCVMFunctionBegin(functionInit.Name));
      assemblyProgram.Code.Add(new LCVMStatementBegin(functionInit.LocateName.StartLine,
        functionInit.LocateName.StartColumn,
        functionInit.LocateName.EndLine,
        functionInit.LocateName.EndColumn));
      */


      //Если функция инициализации определена, то добавляем ее в вызов
      if (entryInit != null)
      {
        string entryLabel = entryInit.ModuleName + "::" + entryInit.Name;
        var mainCall = new INSTR_CALL(assemblyProgram.LabelManager.AddReference(entryLabel));
        assemblyProgram.Code.Add(mainCall);
      }

      /*
      //Отладочная информация точки входа в приложение
      assemblyProgram.Code.Add(new LCVMStatementEnd());
      assemblyProgram.Code.Add(new LCVMFunctionEnd());
      assemblyProgram.Code.Add(new LCVMModuleEnd());
      */

      //После возврата из init вызываем return
      //для передачи управления супервизору
      assemblyProgram.Code.Add(new INSTR_RET());


      //Добавляем код модулей
      foreach (var u in compiledUnits)
        assemblyProgram.AddCodeRange(u.assemblyUnit.SectionCode);

      if (CheckLabels(assemblyProgram, logger) == false)
        return null;

      //Обновляем адреса меток
      assemblyProgram.ProgramAllocate();

      //Запуск низкоуровнего оптимизатора
      //LLOptimizer.Run(optimizerConfig, assemblyProgram);

      //Создаем отладочную информацию для точки входа
      /*
      assemblyProgram.EntryPoint = new DebugInformationLine()
      {
        ModuleName = moduleEntryPoint.ModuleName,
        Line = entryInit.LocateName.StartLine,
        PC = (uint)mainCall.CurrentPosition
      };
      */

      return assemblyProgram;
    }

    private static bool CheckLabels(AssemblyProgram assemblyProgram, CompilerLogger logger)
    {
      bool isOK = true;
      foreach (var label in assemblyProgram.LabelManager.Labels)
      {
        if (label.Declared == false)
        {
          logger.Error(string.Format(Resources.Messages.LinkerErrorLabelNotDefined, label.LabelName));
          isOK = false;
        }
      }

      return isOK;
    }

    static public byte[] Translate(AssemblyProgram prgm)
    {
      //Создаем транслятор из ассемблерного листинга программы
      //в исполняемый код
      Translator t = new Translator();

      //Транслируем программу
      for (int i = 0; i < prgm.Code.Count; i++)
        prgm.Code[i].Translate(t);

      //Возвращаем исполняемый код
      return t.GetBuffer();
    }

    static public DebugInformation GenerateDebugInformation(AssemblyProgram program, string compilerVersion)
    {
      DebugInformationBuilder builder = new DebugInformationBuilder();

      for (int i = 0; i < program.Code.Count; i++)
      {
        var instr = program.Code[i];

        if (instr is LCVMModuleBegin moduleBegin)
        {
          builder.ModuleBegin(moduleBegin.Name);
        }
        else if (instr is LCVMModuleEnd moduleEnd)
        {
          builder.ModuleEnd();
        }
        else if (instr is LCVMStatementBegin statementBegin)
        {
          builder.StatementBegin((uint)statementBegin.CurrentPosition,
            statementBegin.StartLine, statementBegin.StartColumn,
            statementBegin.EndLine, statementBegin.EndColumn);
        }
        else if (instr is LCVMStatementEnd statementEnd)
        {
          builder.StatementEnd((uint)statementEnd.CurrentPosition - 1);
        }
      }

      return builder.Generate("", "LCCompiler", compilerVersion);
    }

  }
}
