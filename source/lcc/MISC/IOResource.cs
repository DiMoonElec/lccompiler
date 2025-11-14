using System.Runtime.InteropServices;
using LC2.LCCompiler.CodeGenerator;
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

      foreach (var r in Resources)
        r.SetBacklink(this);
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

  internal class ResourceClassModbusSlave : IOResourceClass
  {
    public ResourceClassModbusSlave(string alias, IOResource[] resources)
      : base(alias, resources)
    {
    }
  }

  internal class IOResource
  {
    public ushort ID { get; }
    public string Name { get; }
    public LCPrimitiveType.PrimitiveTypes Type { get; }
    public bool IsBinded { get; private set; }
    public int Address { get; private set; }
    public GlobalMemoryObject MemoryObject { get; private set; }
    private IOResourceClass Backlink { get; set; }

    public IOResource(ushort id, string name, LCPrimitiveType.PrimitiveTypes type)
    {
      ID = id;
      Name = name;
      Type = type;

      IsBinded = false;
      Address = 0;
    }

    public void SetBacklink(IOResourceClass backlink)
    {
      Backlink = backlink;
    }

    public void Bind(int address, GlobalMemoryObject memoryObject)
    {
      if (IsBinded)
        throw new InternalCompilerException("This resource is already binded");

      MemoryObject = memoryObject;
      Address = address;
      IsBinded = true;
    }

    public new string ToString()
    {
      if (Backlink == null)
        return $"{Name} -> {MemoryObject.ObjectName}";

      return $"{Backlink.Alias}.{Name} -> {MemoryObject.ObjectName}";
    }
  }

}
