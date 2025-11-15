using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LC2.LCCompiler
{
  // Описание одного ресурса: ResourceClass.Element[,Flag1,Flag2,...]
  public class ResourceAttribute
  {
    public string ResourceClass { get; }
    public string Element { get; }
    public IReadOnlyList<string> Flags { get; }
    public string RawText { get; }
    public int SourceOffset { get; } // позиция в исходной строке, если известна, иначе -1

    public ResourceAttribute(string resourceClass, string element, IEnumerable<string> flags, string rawText, int sourceOffset = -1)
    {
      ResourceClass = resourceClass ?? throw new ArgumentNullException(nameof(resourceClass));
      Element = element ?? throw new ArgumentNullException(nameof(element));
      Flags = (flags ?? Enumerable.Empty<string>()).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
      RawText = rawText ?? $"{resourceClass}.{element}";
      SourceOffset = sourceOffset;
    }

    public bool HasFlag(string flag) => Flags.Any(f => string.Equals(f, flag, StringComparison.OrdinalIgnoreCase));

    public string ToAttributeString()
    {
      var sb = new StringBuilder();
      sb.Append($"{ResourceClass}.{Element}");
      if (Flags?.Count > 0)
      {
        sb.Append(",");
        sb.Append(string.Join(",", Flags));
      }
      return sb.ToString();
    }

    public override string ToString() => ToAttributeString();
  }

  // Набор ресурсов, привязанных к одной логической переменной: "DI.1; DO.1"
  public class VariableAttributeSet
  {
    public IReadOnlyList<ResourceAttribute> Resources { get; }
    public string RawText { get; }

    public VariableAttributeSet(IEnumerable<ResourceAttribute> resources, string rawText)
    {
      Resources = (resources ?? Enumerable.Empty<ResourceAttribute>()).ToArray();
      RawText = rawText ?? string.Empty;
    }

    public bool IsEmpty => Resources.Count == 0;

    public string ToAttributeString()
    {
      return string.Join("; ", Resources.Select(r => r.ToAttributeString()));
    }

    public override string ToString() => ToAttributeString();
  }

  // Набор VariableAttributeSet'ов для массива: "{ ... } { ... }"
  public class ArrayAttributeSet
  {
    public IReadOnlyList<VariableAttributeSet> Elements { get; }
    public string RawText { get; }

    public ArrayAttributeSet(IEnumerable<VariableAttributeSet> elements, string rawText)
    {
      Elements = (elements ?? Enumerable.Empty<VariableAttributeSet>()).ToArray();
      RawText = rawText ?? string.Empty;
    }

    public int Length => Elements.Count;

    public VariableAttributeSet GetElement(int index) => Elements[index];

    public string ToAttributeString()
    {
      return string.Join(" ", Elements.Select(e => "{" + e.ToAttributeString() + "}"));
    }

    public override string ToString() => ToAttributeString();
  }

  // Корневой результат парсинга
  public class ParsedAttribute
  {
    public bool IsArray { get; }
    public VariableAttributeSet Single { get; }
    public ArrayAttributeSet Array { get; }
    public string RawText { get; }

    private ParsedAttribute(VariableAttributeSet single, ArrayAttributeSet array, string rawText)
    {
      Single = single;
      Array = array;
      RawText = rawText;
      IsArray = array != null;
    }

    public static ParsedAttribute FromSingle(VariableAttributeSet single, string raw)
    {
      return new ParsedAttribute(single, null, raw);
    }

    public static ParsedAttribute FromArray(ArrayAttributeSet array, string raw)
    {
      return new ParsedAttribute(null, array, raw);
    }

    public override string ToString()
    {
      return IsArray ? Array.ToAttributeString() : Single.ToAttributeString();
    }
  }

  // Сам парсер
  public static class AttributesParser
  {
    // Внешний API: парсит входную строку 'attribute' (содержимое строкового литерала)
    // Бросает CompilationException при ошибке
    public static ParsedAttribute Parse(string attribute)
    {
      if (attribute == null)
        throw new CompilationException("Attribute string is null.");

      string trimmed = attribute.Trim();

      if (trimmed.Length == 0)
      {
        // пустая строка — трактуем как пустой набор (можно изменить по вкусу)
        return ParsedAttribute.FromSingle(new VariableAttributeSet(new ResourceAttribute[0], trimmed), attribute);
      }

      // Если есть хотя бы одна '{' или '}', считаем, что это синтаксис массива
      if (trimmed.Contains('{') || trimmed.Contains('}'))
      {
        return ParsedAttribute.FromArray(ParseArrayAttributeSet(attribute), attribute);
      }
      else
      {
        var single = ParseVariableAttributeSet(attribute, 0);
        return ParsedAttribute.FromSingle(single, attribute);
      }
    }

    // Parse array: извлекаем top-level { ... } блоки и парсим каждый как VariableAttributeSet
    private static ArrayAttributeSet ParseArrayAttributeSet(string text)
    {
      var elements = new List<VariableAttributeSet>();
      int i = 0;
      int len = text.Length;
      bool foundAny = false;

      while (i < len)
      {
        // пропускаем пробелы/новые строки/табы
        while (i < len && char.IsWhiteSpace(text[i])) i++;

        if (i >= len) break;

        if (text[i] != '{')
        {
          // если встретился мусор до первого '{' — это ошибка формата
          throw new CompilationException($"Expected '{{' at position {i} in array attribute, found '{text[i]}'.");
        }

        int start = i;
        i++; // пропустить '{'
        var sb = new StringBuilder();
        int innerStartIndex = i;

        bool closed = false;
        while (i < len)
        {
          if (text[i] == '}')
          {
            closed = true;
            break;
          }
          if (text[i] == '{')
          {
            // вложенных блоков не ожидаем
            throw new CompilationException($"Nested '{{' is not allowed in array attribute at position {i}.");
          }
          sb.Append(text[i]);
          i++;
        }

        if (!closed)
        {
          throw new CompilationException($"Unterminated '{{' starting at position {start} - missing closing '}}'.");
        }

        string inner = sb.ToString();
        // парсим содержимое блока как VariableAttributeSet
        var element = ParseVariableAttributeSet(inner, innerStartIndex);
        elements.Add(element);
        foundAny = true;

        i++; // пропустить '}'
             // после '}' допустимы пробелы и сразу следующий '{' или конец
      }

      if (!foundAny)
        throw new CompilationException("Array attribute contains no '{...}' elements.");

      return new ArrayAttributeSet(elements, text);
    }

    // Parse single block (либо вся строка, либо содержимое { ... })
    // baseOffset используется для корректных сообщений об ошибке (позиции)
    private static VariableAttributeSet ParseVariableAttributeSet(string text, int baseOffset)
    {
      if (text == null)
        throw new CompilationException("Empty attribute block.");

      // разделяем по ';' (ресурсы для одной переменной)
      // допускаем, что разделители могут иметь пробелы вокруг
      var rawParts = SplitTopLevel(text, ';');

      var resources = new List<ResourceAttribute>();
      int cursor = 0;

      foreach (var raw in rawParts)
      {
        string part = raw.Trim();
        if (part.Length == 0)
        {
          throw new CompilationException($"Empty resource entry in attribute at approximate position {baseOffset + cursor}.");
        }

        // парсим один ресурс — ResourceClass.Element[,Flag1,Flag2...]
        var res = ParseResourceAttribute(part, baseOffset + cursor);
        resources.Add(res);

        cursor += raw.Length + 1; // приблизительное смещение для следующего
      }

      return new VariableAttributeSet(resources, text);
    }

    // Разделение по символу разделителя (top-level). Пока нет вложенных структур,
    // поэтому простая split с учётом строк работает.
    private static List<string> SplitTopLevel(string text, char separator)
    {
      var list = new List<string>();
      var sb = new StringBuilder();
      for (int i = 0; i < text.Length; i++)
      {
        char c = text[i];
        if (c == separator)
        {
          list.Add(sb.ToString());
          sb.Clear();
        }
        else
        {
          sb.Append(c);
        }
      }
      list.Add(sb.ToString());
      return list;
    }

    // Парсинг одного ресурса из строки вида "DI.1,Flag1,Flag2"
    private static ResourceAttribute ParseResourceAttribute(string raw, int sourceOffset)
    {
      if (string.IsNullOrWhiteSpace(raw))
        throw new CompilationException($"Empty resource attribute at position {sourceOffset}.");

      // Разделим по запятым — первый токен содержит ResourceClass.Element
      var commaParts = SplitTopLevel(raw, ',').Select(s => s.Trim()).ToArray();
      if (commaParts.Length == 0)
        throw new CompilationException($"Invalid resource format at position {sourceOffset}: '{raw}'.");

      string first = commaParts[0];
      if (string.IsNullOrEmpty(first))
        throw new CompilationException($"Missing resource class and element at position {sourceOffset}.");

      int dotIndex = first.IndexOf('.');
      if (dotIndex <= 0 || dotIndex == first.Length - 1)
      {
        throw new CompilationException($"Invalid resource identifier '{first}' at position {sourceOffset}. Expected format: Class.Element");
      }

      string resourceClass = first.Substring(0, dotIndex).Trim();
      string element = first.Substring(dotIndex + 1).Trim();

      if (string.IsNullOrEmpty(resourceClass))
        throw new CompilationException($"Empty resource class in '{first}' at position {sourceOffset}.");
      if (string.IsNullOrEmpty(element))
        throw new CompilationException($"Empty element identifier in '{first}' at position {sourceOffset}.");

      var flags = new List<string>();
      if (commaParts.Length > 1)
      {
        for (int i = 1; i < commaParts.Length; i++)
        {
          var f = commaParts[i];
          if (!string.IsNullOrWhiteSpace(f))
            flags.Add(f);
          else
            throw new CompilationException($"Empty flag in resource '{raw}' at position {sourceOffset}.");
        }
      }

      return new ResourceAttribute(resourceClass, element, flags, raw, sourceOffset);
    }
  }
}
