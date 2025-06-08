using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using LC2.DebugInfo;
using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;
using LC2.LCCompiler.Compiler.SemanticTree;
using LC2.LCCompiler.GUI;
using static LCLangParser;

namespace LC2.LCCompiler
{
  internal class CompilerProcessor
  {

    private const string CompilerVersion = "0.1.0";

    /// <summary>
    /// Папка, в которой расположены файлы компилятора
    /// </summary>
    string CompilerDirectory = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Путь к файлу проекта
    /// </summary>
    public string ProjectFile { get; internal set; }
    public bool GenerateSemanticTreeView = false;

    public CompilerLogger Logger { get; private set; }

    LCProject project;
    PLCConfig plcconf;
    LLOptimizerConfiguration LLOptimizerConfig = new LLOptimizerConfiguration();

    public void Compile()
    {
      Logger = new CompilerLogger();

      //Загружаем файл проекта
      try
      {
        project = new LCProject(ProjectFile);
      }
      catch
      {
        throw new CompilationException(string.Format("Ошибка загрузки файла проекта {0}\r\n", ProjectFile));
      }

      //Загружаем конфигурацию ПЛК
      try
      {
        var platformConfigFile = Path.Combine(CompilerDirectory, "platform", project.Platform + ".xml");
        plcconf = new PLCConfig(platformConfigFile);
      }
      catch
      {
        throw new CompilationException(string.Format("Ошибка загрузки конфигурации ПЛК \"{0}\"", project.Platform));
      }

      //Для всех исходных файлов проектов генерируем
      //семантические деревья и таблицы символов
      var compiledUnits = GenerateSemanticTree(Logger);

      //Объединяем все таблицы символов в один список
      var symbolTables = MergeSymbolTables(compiledUnits);


      //Семантическая проверка модулей
      if (SemanticCheck(compiledUnits, symbolTables) == false)
        throw new CompilationException("Во время компиляции обнаружены ошибки");

      //Поиск функций init и loop
      FunctionDeclaratorNode functionInit = FindEntry(compiledUnits, "init");
      FunctionDeclaratorNode functionLoop = FindRequiredEntry(compiledUnits, "loop");

      //ToDo: выполнить проверку прототипов функций


      //Генерация ассемблерных листингов модулей
      foreach (var module in compiledUnits)
      {
        module.assemblyUnit = LCCodeGenerator.AssemblyUnitGenerate(module.semanticTree,
          module.ModuleName, module.ModuleName);
      }

      //Линкуем программу в один объект
      var asmb = LCCodeGenerator.Link(compiledUnits, functionInit, Logger);

      //Поиск объявлений I/O переменных
      //var IOVariableDeclarations = FindIOVariables(asmb, plcconf.Variables.ToArray());
      var IOVariableDeclarations = ResourceBinder.Binding(asmb.GlobalAllocator.MemoryObjects.ToArray(), 
        plcconf.Variables.ToArray());

      if (IOVariableDeclarations.Length != 0)
      {
        //Выводим в логгер найденные внутренние переменные
        Logger.Message("Созданы ссылки на следующие I/O переменные");
        foreach (var variable in IOVariableDeclarations)
          Logger.Message(string.Format("{0}", variable.Name));
      }

      var FunctionDeclaration = FindFunctionLoop(asmb, functionLoop);

      //ToDo: вызов оптимизатора


      //Трансляция дампа исполняемого кода.
      //Генерирует непосредственно исполняемый код
      var code = LCCodeGenerator.Translate(asmb);

      //Генерация дампа исполняемого файла.
      //Формирует дамп, содердащий исполняемый код
      //и дополнительную информацию о конфигурации ПЛК
      var exe = LCExecutableFileGenerator.Generate(code, new[] { FunctionDeclaration }, IOVariableDeclarations);

      //Генерация отладочной информации
      var debugInfo = LCCodeGenerator.GenerateDebugInformation(asmb, CompilerVersion);

      /// Сохранение результатов ///

      //Создаем директорию выходных файлов,
      //если она не была создана ранее
      if (Directory.Exists(project.OutputDirectoryPath) == false)
        Directory.CreateDirectory(project.OutputDirectoryPath);

      if (GenerateSemanticTreeView)
        GenerateTreeView(compiledUnits);

      File.WriteAllBytes(project.OutputBinaryFilePath, exe); //Сохраняем исполняемый файл
      DebugInformationSerializer.Serialize(debugInfo, project.OutputDebugInformationFilePath); //Сохраняем отладочную информацию
      File.WriteAllText(project.OutputAsmFilePath, asmb.GetAsm()); //Сохраняем ассемблерный листинг
      MemoryAllocationInformationSerializer.Serialize(asmb.GlobalAllocator.MemoryObjects,
        project.OutputMemoryAllocationReportFilePath);

      //File.WriteAllText(project.OutputMemoryAllocationReportFilePath, asmb.GetReport()); //Сохраняем отчет

      Logger.Message("\r\nКомпиляция выполнена успешно");
      Logger.Message(string.Format("Размер исполняемого кода: {0} байт", code.Length.ToString()));
      Logger.Message(string.Format("Размер исполняемого файла: {0} байт", exe.Length.ToString()));
      Logger.Message(string.Format("Выделено ОЗУ в статической области: {0} байт", asmb.GlobalAllocator.MemoryUsage.ToString()));
    }

    private void GenerateTreeView(CompiledUnit[] compiledUnits)
    {
      string directoryHTML = Path.Combine(project.OutputDirectoryPath, "tree");

      if (Directory.Exists(directoryHTML) == false)
        Directory.CreateDirectory(directoryHTML);

      foreach (CompiledUnit compiledUnit in compiledUnits)
      {
        string outputPath = Path.Combine(directoryHTML, compiledUnit.ModuleName + ".html");
        new HtmlVisualizer(compiledUnit.semanticTree, compiledUnit.ModuleName, outputPath);
      }
    }

    private CompiledUnit[] GenerateSemanticTree(CompilerLogger logger)
    {
      bool isOK = true;
      List<CompiledUnit> compiledUnits = new List<CompiledUnit>();

      foreach (var s in project.SourceFiles)
      {
        CompiledUnit cUnit = new CompiledUnit();

        cUnit.ModuleName = Path.GetFileNameWithoutExtension(s);
        cUnit.ModuleRelativePath = s;
        cUnit.ModuleFullPath = Path.Combine(project.ProjectFileDirectory, s);

        logger.SetCurrentModuleName(cUnit.ModuleRelativePath);

        //Строим семантическое дерево
        cUnit.semanticTree = BuildAST(cUnit.ModuleFullPath, cUnit.ModuleName, logger);

        if (logger.ErrorCount != 0)
          isOK = false;

        //Генерируем таблицу символов
        if (cUnit.semanticTree == null)
        {
          isOK = false;
        }
        else
        {
          cUnit.symbolTable = RunSemanticChecks.GetSymbolTable(cUnit.semanticTree, logger, cUnit.ModuleName);
        }

        compiledUnits.Add(cUnit);
      }

      if (isOK == false)
      {
        //Logger.GenericError("Во время компиляции возникли ошибки");
        throw new CompilationException("Во время компиляции возникли ошибки");
      }

      return compiledUnits.ToArray();
    }

    Node BuildAST(string SourceFile, string moduleName, CompilerLogger logger)
    {
      if (File.Exists(SourceFile) == false)
        throw new CompilationException(string.Format("Файл не найден: '{0}'"));

      //Пользовательский листенер ошибок
      DescriptiveErrorListener errorListener = new DescriptiveErrorListener(logger);

      //Генерация дерева разбора из исходного кода
      ICharStream stream = CharStreams.fromPath(SourceFile);
      ITokenSource lexer = new LCLangLexer(stream);
      ITokenStream tokens = new CommonTokenStream(lexer);
      LCLangParser parser = new LCLangParser(tokens);
      parser.BuildParseTree = true;
      parser.RemoveErrorListeners();
      parser.AddErrorListener(errorListener);
      CompilationUnitContext tree = parser.compilationUnit();

      //Если есть синтаксические ошибки, то выходим
      if (errorListener.ErrorCounter != 0)
        return null;

      //Строим AST из дерева разбора
      try
      {
        BuildSemanticTree builder = new BuildSemanticTree(moduleName, stream, logger);
        Node r = builder.Visit(tree);
        return r;
      }
      catch
      {
        throw new InternalCompilerException(string.Format("Ошибка генерации синтаксического дерева для файла '{0}'", SourceFile));
      }
    }

    SymbolTable[] MergeSymbolTables(CompiledUnit[] compiledUnits)
    {
      List<SymbolTable> symbolTables = new List<SymbolTable>();
      foreach (var u in compiledUnits)
        symbolTables.Add(u.symbolTable);

      return symbolTables.ToArray();
    }

    bool SemanticCheck(CompiledUnit[] compiledUnits, SymbolTable[] symbolTables)
    {
      bool isOK = true;

      foreach (var module in compiledUnits)
      {
        Logger.SetCurrentModuleName(module.ModuleRelativePath);

        //Добавляем в модуль нативные функции
        foreach (var f in plcconf.NativeFunctions)
          module.semanticTree.AddChild(f);

        if (RunSemanticChecks.Run(module.semanticTree, symbolTables,
          module.ModuleName, Logger) == false)
          isOK = false;
      }

      return isOK;
    }

    FunctionDeclaratorNode FindRequiredEntry(CompiledUnit[] compiledUnits, string functionName)
    {
      var entry = FindEntry(compiledUnits, functionName);

      if (entry == null)
        throw new CompilationException(string.Format("Функция '{0}' не определена", functionName));

      return entry;
    }

    FunctionDeclaratorNode FindEntry(CompiledUnit[] compiledUnits, string functionName)
    {
      var entry = SymbolFinder.FindFunction(compiledUnits, functionName);

      if (entry.Length == 0)
        return null;

      if (entry.Length > 1)
      {
        foreach (var u in entry)
          Logger.Error(u.LocateName, string.Format("Точка входа '{0}' определена в нескольких файлах", functionName));

        throw new CompilationException("Многократная декларация точки входа");
      }

      return entry[0];
    }

    PLCFunctionDeclaration FindFunctionLoop(AssemblyProgram program, FunctionDeclaratorNode functionDeclarator)
    {
      string labelName = string.Format("{0}::{1}", functionDeclarator.ModuleName, functionDeclarator.Name);
      var foundLabel = FindLabelByName(program, labelName);

      PLCFunctionDeclaration r = new PLCFunctionDeclaration(0, foundLabel.Label.Address, labelName);
      return r;
    }

    INSTR_LABEL FindLabelByName(AssemblyProgram program, string targetName)
    {
      foreach (var i in program.Code)
      {
        if (i is INSTR_LABEL label)
        {
          var labelName = label.Label.LabelName;

          if (labelName == targetName)
            return label;
        }
      }

      throw new InternalCompilerException(string.Format("Метка {0} не найдена", targetName));
    }
    /*
    PLCVariableDeclaration[] FindIOVariables(AssemblyProgram program, IOResource[] variables)
    {
      bool isOK = true;

      List<PLCVariableDeclaration> variableDeclarations = new List<PLCVariableDeclaration>();

      var memoryObjects = program.GlobalAllocator.MemoryObjects.ToArray();

      foreach (var v in variables)
      {
        var variableName = v.Name;
        var variableType = v.Type;

        var foundMemoryObjects = FindObjectsByName(memoryObjects, variableName);

        if (foundMemoryObjects.Length == 1) //Найдена одна объявленная переменная 
        {
          var foundObject = foundMemoryObjects[0];
          if (foundObject.ObjectType.Type is LCPrimitiveType primitiveType && primitiveType.Type == variableType)
          {
            variableDeclarations.Add(new PLCVariableDeclaration(v.ID, foundObject.Address, variableName));
          }
          else
          {
            Logger.GenericError(string.Format("I/O переменная '{0}' должна иметь тип '{1}'",
              variableName, LCTypesUtils.PrimitiveTypeGetName(variableType)));
            isOK = false;
          }
        }
        else if (foundMemoryObjects.Length > 1)
        {
          Logger.GenericError(string.Format("Множественное объявление I/O переменной '{0}'", variableName));
          isOK = false;
        }
      }

      if (isOK == false)
        throw new CompilationException("Ошибка объявления I/O переменных");

      return variableDeclarations.ToArray();

    }
    */
    public static GlobalMemoryObject[] FindObjectsByName(GlobalMemoryObject[] objects, string targetName)
    {
      if (objects == null || string.IsNullOrEmpty(targetName))
        return new GlobalMemoryObject[0];

      var result = new List<GlobalMemoryObject>();

      foreach (var obj in objects)
      {
        if (obj != null && obj.ObjectName != null)
        {
          var parts = obj.ObjectName.Split(new[] { "::" }, StringSplitOptions.None);
          if (parts.Length == 2 && parts[1] == targetName)
          {
            result.Add(obj);
          }
        }
      }

      return result.ToArray();
    }

    /*
    VariableDeclaratorNode FindVariable(CompiledUnit[] compiledUnits, string variableName)
    {
      var variable = SymbolFinder.FindVariable(compiledUnits, variableName);

      if (variable.Length == 0)
        return null;

      if (variable.Length > 1)
      {
        foreach (var u in variable)
          Logger.Error(u.LocateName, string.Format("Встроенная переменная '{0}' объявлена в нескольких файлах", variableName));

        throw new CompilationException("Многократная декларация встроенной переменной");
      }

      return variable[0];
    }
    */

    class DescriptiveErrorListener : BaseErrorListener
    {
      public int ErrorCounter = 0;
      CompilerLogger Logger;
      public DescriptiveErrorListener(CompilerLogger logger)
      {
        Logger = logger;
      }

      public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
      {
        //Logger.Error(offendingSymbol.Line, offendingSymbol.StartIndex, offendingSymbol.StopIndex, msg);
        Logger.Error(new LocateElement(offendingSymbol), msg);
        /*
        Console.WriteLine("Line={0} StartIndex={1} StopIndex={2}: {3}",
          offendingSymbol.Line.ToString(), offendingSymbol.StartIndex.ToString(), offendingSymbol.StopIndex.ToString(), msg);
        */
        ErrorCounter++;
      }
    }
  }
}
