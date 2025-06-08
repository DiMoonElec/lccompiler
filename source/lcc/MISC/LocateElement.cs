using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler
{
  /// <summary>
  /// Данный класс содержит информацию о расположении элемента программы в исходном коде.
  /// </summary>
  public class LocateElement
  {
    public int Line { get; private set; }
    public int Start { get; private set; }
    public int Stop { get; private set; }

    public int StartLine { get; private set; }
    public int EndLine { get; private set; }
    public int StartColumn { get; private set; }
    public int EndColumn { get; private set; }

    public LocateElement(LCLangParser.ExpressionContext context)
    {
      Line = context.Start.Line;
      Start = context.Start.StartIndex;
      Stop = context.Stop.StopIndex;

      StartLine = context.Start.Line;
      StartColumn = context.Start.Column + 1;

      EndLine = context.Stop.Line;
      EndColumn = context.Stop.Column + (context.Stop.StopIndex - context.Stop.StartIndex + 1) + 1;
    }

    public LocateElement(Antlr4.Runtime.Tree.ITerminalNode start, Antlr4.Runtime.Tree.ITerminalNode stop)
    {
      Line = start.Symbol.Line;
      Start = start.Symbol.StartIndex;
      Stop = stop.Symbol.StopIndex;

      StartLine = start.Symbol.Line;
      StartColumn = start.Symbol.Column + 1;

      EndLine = stop.Symbol.Line;
      EndColumn = stop.Symbol.Column + (stop.Symbol.StopIndex - stop.Symbol.StartIndex + 1) + 1;
    }

    public LocateElement(Antlr4.Runtime.Tree.ITerminalNode t)
    {
      Line = t.Symbol.Line;
      Start = t.Symbol.StartIndex;
      Stop = t.Symbol.StopIndex;

      StartLine = t.Symbol.Line;
      EndLine = t.Symbol.Line;

      StartColumn = t.Symbol.Column + 1;
      EndColumn = t.Symbol.Column + (t.Symbol.StopIndex - t.Symbol.StartIndex + 1) + 1;
    }


    public LocateElement(Antlr4.Runtime.IToken t)
    {
      Line = t.Line;
      Start = t.StartIndex;
      Stop = t.StopIndex;

      StartLine = t.Line;
      EndLine = t.Line;

      StartColumn = t.Column + 1;
      EndColumn = t.Column + (t.StopIndex - t.StartIndex + 1) + 1;
    }


    public LocateElement(LocateElement e)
    {
      Line = e.Line;
      Start = e.Start;
      Stop = e.Stop;

      StartLine = e.StartLine;
      EndLine = e.EndLine;

      StartColumn = e.StartColumn;
      EndColumn = e.EndColumn;
    }

    public LocateElement(params LocateElement[] elements)
    {
      int line = -1;
      int start = -1;
      int stop = -1;

      int startLine = -1;
      int endLine = -1;
      int startColumn = -1;
      int endColumn = -1;

      for (int i = 0; i < elements.Length; i++)
      {
        //Начальная инициализация
        //Находим первый ненулевой элемент
        //и присваиваем начальные значения
        if (elements[i] != null)
        {
          line = elements[i].Line;
          start = elements[i].Start;
          stop = elements[i].Stop;
          startLine = elements[i].StartLine;
          endLine = elements[i].EndLine;
          startColumn = elements[i].StartColumn;
          endColumn = elements[i].EndColumn;
          break;
        }
      }

      for (int i = 0; i < elements.Length; i++)
      {
        if (elements[i] != null)
        {
          var e = elements[i];

          if (e.Line < line)
            line = e.Line;

          if (e.Start < start)
            start = e.Start;

          if (e.Stop > stop)
            stop = e.Stop;

          if (e.StartLine < startLine)
            startLine = e.StartLine;

          if (e.StartColumn < startColumn)
            startColumn = e.StartColumn;

          if (e.EndLine > endLine)
            endLine = e.EndLine;

          if (e.EndColumn > endColumn)
            endColumn = e.EndColumn;
        }
      }

      Line = line;
      Start = start;
      Stop = stop;

      StartLine = startLine;
      EndLine = endLine;

      StartColumn = startColumn;
      EndColumn = endColumn;
    }


    public void Expand(Antlr4.Runtime.IToken t)
    {
      int line = t.Line;
      int start = t.StartIndex;
      int stop = t.StopIndex;

      int startLine = t.Line;
      int endLine = t.Line;

      int startColumn = t.Column + 1;
      int endColumn = t.Column + (t.StopIndex - t.StartIndex + 1) + 1;

      Expand(line, start, stop, startLine, endLine, startColumn, endColumn);
    }

    public void Expand(Antlr4.Runtime.Tree.ITerminalNode t)
    {
      int line = t.Symbol.Line;
      int start = t.Symbol.StartIndex;
      int stop = t.Symbol.StopIndex;

      int startLine = t.Symbol.Line;
      int endLine = t.Symbol.Line;

      int startColumn = t.Symbol.Column + 1;
      int endColumn = t.Symbol.Column + (t.Symbol.StopIndex - t.Symbol.StartIndex + 1) + 1;

      Expand(line, start, stop, startLine, endLine, startColumn, endColumn);
    }


    public LocateElement Clone()
    {
      return new LocateElement(this);
    }

    public new string ToString()
    {
      return String.Format("[Line: {0}, Start: {1}, Stop: {2}]", Line, Start, Stop);
    }

    void Expand(int line, int start, int stop, int startLine, int endLine, int startColumn, int endColumn)
    {
      if (line < Line)
        Line = line;

      if (start < Start)
        Start = start;

      if (stop > Stop)
        Stop = stop;

      if (startLine < StartLine)
        StartLine = startLine;

      if (startColumn < StartColumn)
        StartColumn = startColumn;

      if (endLine > EndLine)
        EndLine = endLine;

      if (endColumn > EndColumn)
        EndColumn = endColumn;
    }
  }
}
