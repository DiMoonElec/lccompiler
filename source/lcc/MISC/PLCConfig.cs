using LC2.LCCompiler.Compiler;
using System;
using System.Collections.Generic;
using System.Xml;

namespace LC2.LCCompiler
{
  internal class PLCConfig
  {
    /// <summary>
    /// Доступно ОЗУ в данном ПЛК
    /// </summary>
    public int RamMemorySize { get; private set; }

    /// <summary>
    /// Доступно ПЗУ в данном ПЛК
    /// </summary>
    public int FlashMemorySize { get; private set; }

    /// <summary>
    /// Описание ПЛК, к которому выполнена данная конфигурация
    /// </summary>
    public string PLCDescription { get; private set; }

    public List<NativeFunctionDeclaratorNode> NativeFunctions = new List<NativeFunctionDeclaratorNode>();

    public List<IOResourceClass> Variables = new List<IOResourceClass>();

    public PLCConfig(string file)
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.Load(file);

      XmlElement xRoot = xDoc.DocumentElement;

      if (xRoot == null)
        throw new Exception();

      var nodes = xRoot.ChildNodes;

      for (int i = 0; i < nodes.Count; i++)
      {
        var xnode = nodes[i];
        var name = xnode.Name;
        switch (name)
        {
          case "description":
            PLCDescription = xnode.InnerText;
            break;

          case "ramavailable":
            RamMemorySize = Convert.ToInt32(xnode.InnerText);
            break;

          case "flashavailable":
            FlashMemorySize = Convert.ToInt32(xnode.InnerText);
            break;

          case "native":
            ParseNative(xnode);
            break;

          case "resources":
            ParseResources(xnode);
            break;
        }
      }
    }

    void ParseNative(XmlNode xnode)
    {
      string functionName = xnode.Attributes.GetNamedItem("name").Value;
      string functionID = xnode.Attributes.GetNamedItem("id").Value;
      LCObjectType returnType = null;
      LCObjectType[] functionParams = null;
      VariableDeclaratorNode[] functionVariableParams = null;

      foreach (XmlElement e in xnode)
      {
        switch (e.Name)
        {
          case "return":
            returnType = ParseReturn(e);
            break;
          case "params":
            functionParams = ParseParams(e);
            break;
        }
      }

      if (functionParams != null)
      {
        functionVariableParams = new VariableDeclaratorNode[functionParams.Length];

        for (int i = 0; i < functionParams.Length; i++)
        {
          functionVariableParams[i] = new VariableDeclaratorNode(functionParams[i], "param" + i.ToString(), "system", null, null);
          functionVariableParams[i].ClassValue = ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam;
        }
      }

      int id = Convert.ToInt32(functionID);
      if (id < 0 || id > 65535)
        throw new Exception("PLC configuration parsing error: native function id must be in the range 0...65535");

      if (returnType == null)
        returnType = new LCObjectType(new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeVoid));

      NativeFunctionDeclaratorNode declarator = new NativeFunctionDeclaratorNode(functionName, "system", functionVariableParams, returnType, (ushort)id);

      NativeFunctions.Add(declarator);
    }

    private LCObjectType[] ParseParams(XmlElement e)
    {
      List<LCObjectType> functionParams = new List<LCObjectType>();
      var nodes = e.ChildNodes;
      for (int i = 0; i < nodes.Count; i++)
      {
        var p = ToObjectType(nodes[i]);
        functionParams.Add(p);
      }
      return functionParams.ToArray();
    }

    private LCObjectType ParseReturn(XmlElement e)
    {
      var v = e.FirstChild;
      return ToObjectType(v);
    }

    private LCObjectType ToObjectType(XmlNode e)
    {
      var typeName = e.Name;
      LCType type;

      switch (typeName)
      {
        case "void":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeVoid);
          break;
        case "bool":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool);
          break;
        case "byte":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeByte);
          break;
        case "sbyte":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeSByte);
          break;
        case "short":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeShort);
          break;
        case "ushort":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUShort);
          break;
        case "int":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeInt);
          break;
        case "uint":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeUInt);
          break;
        case "long":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeLong);
          break;
        case "ulong":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeULong);
          break;
        case "float":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeFloat);
          break;
        case "double":
          type = new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeDouble);
          break;
        default:
          throw new Exception("PLC configuration parsing error: unknown type " + typeName);
      }

      return new LCObjectType(type);
    }


    private void ParseResources(XmlNode variablesNode)
    {
      foreach (XmlNode variableNode in variablesNode.ChildNodes)
      {
        if (variableNode.NodeType == XmlNodeType.Comment)
        {
          // Игнорируем комментарии
          continue;
        }

        string typeName = variableNode.Name;
        string alias = variableNode.Attributes["alias"].Value;

        IOResourceClass resourceClass = null;

        switch (typeName)
        {
          case "INPUTS":
            {
              IOResource[] resource = ParseIO(variableNode);
              resourceClass = new ResourceClassInputs(alias, resource);
            }
            break;

          case "OUTPUTS":
            {
              IOResource[] resource = ParseIO(variableNode);
              resourceClass = new ResourceClassOutputs(alias, resource);
            }
            break;
        }

        if (resourceClass != null)
          Variables.Add(resourceClass);
      }
    }

    private IOResource[] ParseIO(XmlNode variablesNode)
    {
      List<IOResource> result = new List<IOResource>();

      foreach (XmlNode variableNode in variablesNode.ChildNodes)
      {
        if (variableNode.NodeType == XmlNodeType.Comment)
        {
          // Игнорируем комментарии
          continue;
        }

        string typeName = variableNode.Name;
        ushort id = Convert.ToUInt16(variableNode.Attributes["id"].Value);
        string name = variableNode.Attributes["name"].Value;

        LCPrimitiveType.PrimitiveTypes type;

        switch (typeName)
        {
          case "bool":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeBool;
            break;
          case "sbyte":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeSByte;
            break;
          case "short":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeShort;
            break;
          case "int":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeInt;
            break;
          case "long":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeLong;
            break;
          case "float":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeFloat;
            break;
          case "double":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeDouble;
            break;
          case "byte":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeByte;
            break;
          case "ushort":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeUShort;
            break;
          case "uint":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeUInt;
            break;
          case "ulong":
            type = LCPrimitiveType.PrimitiveTypes.LCTypeULong;
            break;
          default:
            throw new Exception("Unknown variable type: " + typeName);
        }

        result.Add(new IOResource(id, name, type));
      }

      return result.ToArray();
    }
  }
}
