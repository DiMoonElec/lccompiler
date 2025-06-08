using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

namespace LC2.DebugInfo
{
  public static class DebugInformationSerializer
  {
    /*public static DebugInformation Deserialize(string file)
    {
      
      BinaryFormatter formatter = new BinaryFormatter();

      using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
      {
        DebugInformation information = (DebugInformation)formatter.Deserialize(fs);
        return information;
      }
      
    }*/

    public static void Serialize(DebugInformation dbg, string file)
    {
      XmlDocument xDoc = new XmlDocument();
      XmlNode rootNode = xDoc.CreateElement("DebugInformation");
      xDoc.AppendChild(rootNode);

      XmlNode xAssemblyName = xDoc.CreateElement("AssemblyName");
      xAssemblyName.InnerText = dbg.AssemblyName;
      rootNode.AppendChild(xAssemblyName);

      XmlNode xCompilerName = xDoc.CreateElement("CompilerName");
      xCompilerName.InnerText = dbg.CompilerName;
      rootNode.AppendChild(xCompilerName);


      XmlNode xCompilerVersion = xDoc.CreateElement("CompilerVersion");
      xCompilerVersion.InnerText = dbg.CompilerVersion;
      rootNode.AppendChild(xCompilerVersion);

      XmlNode xStatements = xDoc.CreateElement("Statements");
      for (int i = 0; i < dbg.Statements.Length; i++)
      {
        var Statement = dbg.Statements[i];

        xStatements.AppendChild(SerializeStatement(xDoc, Statement));
      }
      rootNode.AppendChild(xStatements);

      //XmlNode xApplicationEntry = xDoc.CreateElement("ApplicationEntry");

      //xApplicationEntry.AppendChild(SerializeDebugInformationLine(xDoc, dbg.ApplicationEntryLine));
      //rootNode.AppendChild(xApplicationEntry);
      //rootNode.AppendChild(SerializeApplicationEntr(xDoc, dbg.ApplicationEntryLine));


      XmlWriterSettings settings = new XmlWriterSettings
      {
        Encoding = Encoding.UTF8,
        ConformanceLevel = ConformanceLevel.Document,
        OmitXmlDeclaration = false,
        CloseOutput = true,
        Indent = true,
        IndentChars = "  ",
        NewLineHandling = NewLineHandling.Replace
      };

      using (StreamWriter sw = File.CreateText(file))
      using (XmlWriter writer = XmlWriter.Create(sw, settings))
      {
        xDoc.WriteContentTo(writer);
        writer.Close();
      }
    }

    private static XmlNode SerializeApplicationEntr(XmlDocument xDoc, DebugInformationLine applicationEntry)
    {
      XmlNode xApplicationEntry = xDoc.CreateElement("ApplicationEntry");

      XmlAttribute xModuleName = xDoc.CreateAttribute("ModuleName");
      xModuleName.Value = applicationEntry.ModuleName;

      XmlAttribute xLine = xDoc.CreateAttribute("Line");
      xLine.Value = applicationEntry.Line.ToString();

      XmlAttribute xPC = xDoc.CreateAttribute("PC");
      xPC.Value = UInt32ToHex(applicationEntry.PC);

      xApplicationEntry.Attributes.Append(xModuleName);
      xApplicationEntry.Attributes.Append(xLine);
      xApplicationEntry.Attributes.Append(xPC);

      return xApplicationEntry;
    }

    private static XmlNode SerializeStatement(XmlDocument xDoc, DebugInformationStatement statement)
    {
      XmlNode xStatement = xDoc.CreateElement("Statement");

      ///////////////////////////////////////////////////////////////

      XmlAttribute xModuleName = xDoc.CreateAttribute("ModuleName");
      xModuleName.Value = statement.ModuleName;

      XmlAttribute xLowPC = xDoc.CreateAttribute("LowPC");
      xLowPC.Value = UInt32ToHex(statement.LowPC);

      XmlAttribute xHighPC = xDoc.CreateAttribute("HighPC");
      xHighPC.Value = UInt32ToHex(statement.HighPC);

      XmlAttribute xStartLine = xDoc.CreateAttribute("StartLine");
      xStartLine.Value = statement.StartLine.ToString();

      XmlAttribute xStartColumn = xDoc.CreateAttribute("StartColumn");
      xStartColumn.Value = statement.StartColumn.ToString();

      XmlAttribute xEndLine = xDoc.CreateAttribute("EndLine");
      xEndLine.Value = statement.EndLine.ToString();

      XmlAttribute xEndColumn = xDoc.CreateAttribute("EndColumn");
      xEndColumn.Value = statement.EndColumn.ToString();

      ///////////////////////////////////////////////////////////////

      xStatement.Attributes.Append(xModuleName);
      xStatement.Attributes.Append(xLowPC);
      xStatement.Attributes.Append(xHighPC);
      xStatement.Attributes.Append(xStartLine);
      xStatement.Attributes.Append(xStartColumn);
      xStatement.Attributes.Append(xEndLine);
      xStatement.Attributes.Append(xEndColumn);

      return xStatement;
    }

    static XmlNode SerializeDebugInformationLine(XmlDocument xDoc, DebugInformationLine dbgLine)
    {
      XmlNode xBreakpoint = xDoc.CreateElement("Breakpoint");

      XmlAttribute xModuleName = xDoc.CreateAttribute("ModuleName");
      xModuleName.Value = dbgLine.ModuleName;

      XmlAttribute xLine = xDoc.CreateAttribute("Line");
      xLine.Value = dbgLine.Line.ToString();

      XmlAttribute xPC = xDoc.CreateAttribute("PC");
      xPC.Value = UInt32ToHex(dbgLine.PC);

      xBreakpoint.Attributes.Append(xModuleName);
      xBreakpoint.Attributes.Append(xLine);
      xBreakpoint.Attributes.Append(xPC);

      return xBreakpoint;
    }


    static string UInt32ToHex(UInt32 value)
    {
      return string.Format("0x{0:X8}", value);
    }
  }
}
