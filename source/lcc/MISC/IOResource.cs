using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler
{
  internal abstract class IOResourceClass
  {
    public string Alias { get; private set; }
    public IOResource[] Resources { get; private set; }

    public IOResourceClass(string alias, IOResource[] resources)
    {
      Alias = alias;
      Resources = resources;
    }
  }

  internal class ResourceClassInputs : IOResourceClass
  {
    public ResourceClassInputs(string alias, IOResource[] resources)
      : base(alias, resources)
    {
    }
  }

  internal class ResourceClassOutputs : IOResourceClass
  {
    public ResourceClassOutputs(string alias, IOResource[] resources)
      : base(alias, resources)
    {
    }
  }

  internal class IOResource
  {
    public ushort ID { get; }
    public string Name { get; }
    public LCPrimitiveType.PrimitiveTypes Type { get; }

    public IOResource(ushort id, string name, LCPrimitiveType.PrimitiveTypes type)
    {
      ID = id;
      Name = name;
      Type = type;
    }

    public new string ToString()
    {
      return Name;
    }
  }

}
