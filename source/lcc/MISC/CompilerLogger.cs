using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using static LC2.LCCompiler.CompilerLoggerElement;

namespace LC2.LCCompiler
{
  internal class CompilerLoggerElement
  {
    public enum MsgType
    {
      Error,
      Warning,
      Info,
      Message,
    };

    public bool MsgLocalizedInSource = true;
    public MsgType msgType;
    public string msg;

    public int StartLine;
    public int StartColumn;
    public int EndLine;
    public int EndColumn;

    public string ModuleName;

    /// <summary>
    /// Для сообщений, которые связаны с участном исходного кода
    /// </summary>
    /// <param name="moduleName">Имя модуля</param>
    /// <param name="t">Тип сообщения</param>
    /// <param name="StartLine"></param>
    /// <param name="StartColumn"></param>
    /// <param name="EndLine"></param>
    /// <param name="EndColumn"></param>
    /// <param name="Message"></param>
    public CompilerLoggerElement(string moduleName, MsgType t,
      int StartLine, int StartColumn,
      int EndLine, int EndColumn,
      string Message)
    {
      msgType = t;
      msg = Message;
      ModuleName = moduleName;
      this.StartLine = StartLine;
      this.StartColumn = StartColumn;
      this.EndLine = EndLine;
      this.EndColumn = EndColumn;
      MsgLocalizedInSource = true;
    }

    /// <summary>
    /// Для сообщений, которые не связаны с конкретным участком исходного кода
    /// </summary>
    public CompilerLoggerElement(string moduleName, MsgType t, string Message)
    {
      msgType = t;
      msg = Message;
      ModuleName = moduleName;
      MsgLocalizedInSource = false;
    }

    public CompilerLoggerElement(MsgType t, string Message)
    {
      msgType = t;
      msg = Message;
      ModuleName = "";
      MsgLocalizedInSource = false;
    }

    public new string ToString()
    {
      if (msgType == MsgType.Message)
        return msg;
      else if (ModuleName == "")
        return string.Format("{0}:{1}", msgType.ToString(), msg);
      else if (MsgLocalizedInSource)
      {
        return string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}",
          ModuleName,
          StartLine, StartColumn,
          EndLine, EndColumn,
          msgType.ToString(), msg);
      }
      else
      {
        return string.Format("{0}:{1}:{2}", ModuleName, msgType.ToString(), msg);
      }
    }
  }

  internal class CompilerLogger
  {
    public int ErrorCount { get; private set; }
    public int WarningCount { get; private set; }
    public int InfoCount { get; private set; }

    public int CountMessages { get { return LoggerElements.Count; } }

    string CurrentModule = "";

    public List<CompilerLoggerElement> LoggerElements = new List<CompilerLoggerElement>();

    //List<CompilerLoggerElement> MessageElements = new List<CompilerLoggerElement>();

    public CompilerLogger()
    {
      ErrorCount = 0;
      WarningCount = 0;
      InfoCount = 0;
    }

    public string[] GetMessages()
    {
      List<string> r = new List<string>();
      foreach (CompilerLoggerElement e in LoggerElements)
        r.Add(e.ToString());
      return r.ToArray();
    }

    public void SetCurrentModuleName(string currentModule)
    {
      CurrentModule = currentModule;
    }

    #region Error

    public void GenericError(string Message)
    {
      LoggerElements.Add(new CompilerLoggerElement(MsgType.Error, Message));
    }

    public void Error(string Message)
    {
      OutMessage(MsgType.Error, Message);
      ErrorCount++;
    }

    public void Error(LocateElement e, string Message)
    {
      OutMessage(MsgType.Error, e.StartLine, e.StartColumn, e.EndLine, e.EndColumn, Message);
      ErrorCount++;
    }

    public void Error(ITerminalNode node, string Message)
    {
      int StartLine = node.Symbol.Line;
      int StartColumn = node.Symbol.Column + 1;

      int EndLine = node.Symbol.Line;
      int EndColumn = node.Symbol.Column + (node.Symbol.StopIndex - node.Symbol.StartIndex + 1);

      OutMessage(MsgType.Error, StartLine, StartColumn, EndLine, EndColumn, Message);
      ErrorCount++;
    }

    /*
    public void Error(int Line, int Start, int Stop, string Message)
    {
      ErrorCount++;
      OutMessage(CompilerLoggerElement.MsgType.Error, Line, Start, Stop, Message);
    }
    */
    #endregion

    #region Warning
    public void Warning(LocateElement e, string Message)
    {
      OutMessage(MsgType.Warning, e.StartLine, e.StartColumn, e.EndLine, e.EndColumn, Message);
      WarningCount++;
    }

    public void Warning(TerminalNodeImpl node, string Message)
    {
      int StartLine = node.Symbol.Line;
      int StartColumn = node.Symbol.Column + 1;

      int EndLine = node.Symbol.Line;
      int EndColumn = node.Symbol.Column + (node.Symbol.StopIndex - node.Symbol.StartIndex + 1);

      OutMessage(MsgType.Warning, StartLine, StartColumn, EndLine, EndColumn, Message);
      WarningCount++;
    }

    #endregion

    #region Info
    /*
    public void Info(string Message)
    {
      Info(-1, -1, -1, Message);
    }
    public void Info(LocateElement e, string Message)
    {
      Info(e.Line, e.Start, e.Stop, Message);
    }

    public void Info(TerminalNodeImpl node, string Message)
    {
      Info(node.Symbol.Line, node.Symbol.StartIndex, node.Symbol.StopIndex, Message);
    }
    
    public void Info(int Line, int Start, int Stop, string Message)
    {
      InfoCount++;
      OutMessage(CompilerLoggerElement.MsgType.Info, Line, Start, Stop, Message);
    }
    */

    public void Info(LocateElement e, string Message)
    {
      OutMessage(MsgType.Info, e.StartLine, e.StartColumn, e.EndLine, e.EndColumn, Message);
      InfoCount++;
    }

    public void GenericInfo(string Message)
    {
      LoggerElements.Add(new CompilerLoggerElement(MsgType.Info, Message));
    }

    #endregion

    public void Message(string message)
    {
      OutMessage(MsgType.Message, message);
    }

    void OutMessage(MsgType t,
      int StartLine, int StartColumn,
      int EndLine, int EndColumn,
      string Message)
    {
      LoggerElements.Add(new CompilerLoggerElement(CurrentModule, t, StartLine, StartColumn, EndLine, EndColumn, Message));
    }

    void OutMessage(MsgType t, string Message)
    {
      LoggerElements.Add(new CompilerLoggerElement(CurrentModule, t, Message));
    }
  }
}
