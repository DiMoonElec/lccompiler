using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LC2.LCCompiler.CodeGenerator;

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
    public static PLCVariableDeclaration[] Binding(GlobalMemoryObject[] objects, IOResourceClass[] resourceClasses)
    {
      List<PLCVariableDeclaration> result = new List<PLCVariableDeclaration>();

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
              IOResource resource = FindIOResource(resourceClasses, b);
              if (resource == null)
                throw new Exception("Unknown resource: " + b.ToString());

              result.Add(new PLCVariableDeclaration(resource.ID, obj.Address, b.ToString()));
            }
          }
        }
      }

      if (result.Count == 0)
        return null;
      return result.ToArray();
    }

    private static IOResource FindIOResource(IOResourceClass[] resourceClasses, VariableAttribute a)
    {
      string alias = a.Alias;
      string id = a.ID;

      foreach (var c in resourceClasses)
      {
        if(c.Alias == alias)
          foreach(var r in c.Resources)
            if(r.Name == id)
              return r;
      }

      return null;
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
