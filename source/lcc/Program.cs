using System;
using System.Text;

namespace LC2.LCCompiler
{
  internal class Program
  {
    static CompilerProcessor compilerProcessor = new CompilerProcessor();

    static int Main(string[] args)
    {
      //Парсинг аргументов командной строки
      if (ParceArgs(args) == false)
      {
        Console.WriteLine("Формат командной строки: lcc PROJ_FILE.lcprj");
        return -1;
      }

      try
      {
        compilerProcessor.Compile();
      }
      catch (CompilationException ex)
      {
        Console.WriteLine("Ошибка компиляции");
        Console.WriteLine(ex.Message);
        PrintLogger(compilerProcessor.Logger);
        return -1;
      }
      catch (InternalCompilerException ex)
      {
        Console.WriteLine(string.Format("Внутренняя ошибка компилятора\r\n{0}\r\nИнформация о ошибке\r\n{1}\r\n", ex.Message, ex.ToString()));
        //PrintLogger(compilerProcessor.Logger);
        return -1;
      }
      catch (Exception ex)
      {
        Console.WriteLine(string.Format("Внутренняя ошибка компилятора\r\n{0}\r\nИнформация о ошибке\r\n{1}\r\n", ex.Message, ex.ToString()));
        return -1;
      }

      PrintLogger(compilerProcessor.Logger);
      return 0;
    }


    static void PrintLogger(CompilerLogger logger)
    {
      foreach (var s in logger.LoggerElements)
        Console.WriteLine(s.ToString());
    }


    static bool ParceArgs(string[] args)
    {
      if (args.Length < 1)
        return false;

      compilerProcessor.ProjectFile = args[0];

      for (int i = 1; i < args.Length; i++)
      {
        switch (args[i].ToLower())
        {
          case "-utf8":
            Console.OutputEncoding = Encoding.UTF8;
            break;

          case "-ast":
            compilerProcessor.GenerateSemanticTreeView = true;
            break;
        }
      }

      return true;
    }
  }
}
