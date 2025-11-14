using System;
using System.Collections.Generic;
using System.Net;
using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler
{
  internal class VariableAttribute
  {
    public string Alias { get; private set; }
    public string ID { get; private set; }
    public string[] Flags { get; private set; }

    public VariableAttribute(string alias, string id, string[] flags)
    {
      Alias = alias;
      ID = id;
      Flags = flags;
    }

    public VariableAttribute(string alias, string id)
    {
      Alias = alias;
      ID = id;
      Flags = null;
    }

    public new string ToString()
    {
      return Alias + "." + ID;
    }
  }
  internal class ResourceBinder
  {
    public static PLCVariableDeclaration[] Binding(GlobalMemoryObject[] objects,
      IOResourceClass[] resourceClasses,
      CompilerLogger logger)
    {
      bool isOK = true;

      foreach (var obj in objects)
      {
        var attribute = obj.Attribute;
        if (attribute != null)
        {
          var a = AttributesParser(attribute);
          if (a != null)
          {
            foreach (var b in a)
            {
              var r = BindingIOResource(obj, b, resourceClasses, logger);
              if (r == false)
                isOK = false;
            }
          }
        }
      }

      if (isOK == false)
        throw new CompilationException("PLC variables binding error");

      List<PLCVariableDeclaration> result = new List<PLCVariableDeclaration>();

      foreach (var rc in resourceClasses)
      {
        foreach (var a in rc.Resources)
        {
          if (a.IsBinded)
            result.Add(new PLCVariableDeclaration(a.ID, a.Address, a.ToString()));
        }
      }

      return result.ToArray();
    }

    private static bool BindingIOResource(GlobalMemoryObject memoryObject,
      VariableAttribute variableAttribute,
      IOResourceClass[] supportedResources,
      CompilerLogger logger)
    {
      string alias = variableAttribute.Alias;
      string id = variableAttribute.ID;

      IOResourceClass resourceClass = null;
      IOResource resource = null;

      foreach (var rc in supportedResources)
      {
        if (rc.Alias == alias)
        {
          resourceClass = rc;
          break;
        }
      }

      if (resourceClass == null)
      {
        logger.Error($"Неизвестный тип ресурса '{variableAttribute.ToString()}'");
        return false;
      }

      foreach (var r in resourceClass.Resources)
      {
        if (r.Name == id)
        {
          resource = r;
          break;
        }
      }

      if (resource == null)
      {
        logger.Error($"Неизвестный тип ресурса '{variableAttribute.ToString()}'");
        return false;
      }

      if (resource.IsBinded)
      {
        logger.Error($"К ресурсу '{resourceClass.Alias}.{resource.Name}' уже привязана переменная '{resource.MemoryObject.ObjectName}'");
        return false;
      }

      if (resourceClass is ResourceClassInputs resourceClassInputs)
        return bindingResourceClassInputOutput(resource, memoryObject, variableAttribute, logger);
      else if (resourceClass is ResourceClassOutputs resourceClassOutputs)
        return bindingResourceClassInputOutput(resource, memoryObject, variableAttribute, logger);
      else if (resourceClass is ResourceClassModbusSlave resourceClassModbusSlave)
        return bindingResourceClassModbusSlave(resource, memoryObject, variableAttribute, logger);

      throw new InternalCompilerException("Unknown resource class");
    }

    private static bool bindingResourceClassModbusSlave(IOResource resource,
      GlobalMemoryObject memoryObject,
      VariableAttribute variableAttribute,
      CompilerLogger logger)
    {
      return false;
    }

    private static bool bindingResourceClassInputOutput(IOResource resource,
      GlobalMemoryObject memoryObject,
      VariableAttribute variableAttribute,
      CompilerLogger logger)
    {
      if (LCTypesUtils.IsEqual(memoryObject.ObjectType.Type, resource.Type))
      {
        resource.Bind(memoryObject.Address, memoryObject);
        return true;
      }
      else
      {
        logger.Error($"Переменная '{memoryObject.ObjectName}' должна иметь тип '{LCTypesUtils.PrimitiveTypeGetName(resource.Type)}'");
        return false;
      }
    }

    static VariableAttribute[] AttributesParser(string Attribute)
    {
      List<VariableAttribute> result = new List<VariableAttribute>();

      string[] strings = Attribute.Split(';');

      foreach (string s in strings)
      {
        var astr = s.Trim();
        result.Add(ParseAttribute(astr));
      }

      if (result.Count == 0)
        return null;
      return result.ToArray();
    }

    private static VariableAttribute ParseAttribute(string astr)
    {
      var split = astr.Split('.');
      if (split.Length == 2)
      {
        string alias = split[0].Trim();
        string id = split[1].Trim();
        return new VariableAttribute(alias, id);
      }
      else
      {
        throw new ArgumentException("Invalid Attribute: " + astr);
      }
    }
  }
}
