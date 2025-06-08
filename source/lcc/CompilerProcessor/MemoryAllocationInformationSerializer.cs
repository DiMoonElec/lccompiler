using System;
using System.Collections.Generic;
using System.Xml;
using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler
{
  internal static class MemoryAllocationInformationSerializer
  {
    public static void Serialize(IEnumerable<GlobalMemoryObject> globalMemoryObjects, string path)
    {
      using (var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true }))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("memoryObjects");

        foreach (var obj in globalMemoryObjects)
        {
          SerializeGlobalMemoryObject(writer, obj);
        }

        writer.WriteEndElement(); // memoryObjects
        writer.WriteEndDocument();
      }
    }

    private static void SerializeGlobalMemoryObject(XmlWriter writer, GlobalMemoryObject obj)
    {
      var type = obj.ObjectType.Type;

      switch (type)
      {
        case LCPrimitiveType primitiveType:
          SerializePrimitive(writer, obj, primitiveType);
          break;

        case LCArrayType arrayType:
          SerializeArray(writer, obj, arrayType);
          break;

        case LCStructType structType:
          SerializeStruct(writer, obj, structType);
          break;

        default:
          throw new NotSupportedException($"Unsupported type: {type.GetType().Name}");
      }
    }

    private static void SerializePrimitive(XmlWriter writer, GlobalMemoryObject obj, LCPrimitiveType primitiveType)
    {
      writer.WriteStartElement("var");
      writer.WriteAttributeString("type", LCTypesUtils.PrimitiveTypeGetName(primitiveType.Type));
      writer.WriteAttributeString("name", obj.ObjectName);
      writer.WriteAttributeString("address", obj.Address.ToString());
      if (obj.Attribute != null && obj.Attribute != "")
        writer.WriteAttributeString("Attribute", obj.Attribute);
      writer.WriteEndElement();
    }

    private static void SerializeArray(XmlWriter writer, GlobalMemoryObject obj, LCArrayType arrayType)
    {
      writer.WriteStartElement("array");
      writer.WriteAttributeString("type", LCTypesUtils.PrimitiveTypeGetName(arrayType.TypeElement.Type));
      writer.WriteAttributeString("name", obj.ObjectName);
      writer.WriteAttributeString("depth", arrayType.ArrayDepth.ToString());
      writer.WriteAttributeString("address", obj.Address.ToString());
      if (obj.Attribute != null && obj.Attribute != "")
        writer.WriteAttributeString("Attribute", obj.Attribute);
      writer.WriteEndElement();
    }

    private static void SerializeStruct(XmlWriter writer, GlobalMemoryObject obj, LCStructType structType)
    {
      writer.WriteStartElement("struct");
      writer.WriteAttributeString("name", obj.ObjectName);
      writer.WriteAttributeString("size", obj.ObjectSize.ToString());
      writer.WriteAttributeString("address", obj.Address.ToString());
      if (obj.Attribute != null && obj.Attribute != "")
        writer.WriteAttributeString("Attribute", obj.Attribute);

      var adr = obj.Address;

      foreach (var element in structType.StructDeclarator.Elements)
      {
        var size = SerializeStructElement(writer, element, adr);
        adr += size;
      }

      writer.WriteEndElement(); // struct
    }

    private static int SerializeStructElement(XmlWriter writer, LCStructTypeElement element, int baseAddress)
    {
      switch (element)
      {
        case LCStructElementPrimitiveType primitiveElement:
          writer.WriteStartElement("var");
          writer.WriteAttributeString("type", LCTypesUtils.PrimitiveTypeGetName(primitiveElement.Type.Type));
          writer.WriteAttributeString("name", element.Name);
          writer.WriteAttributeString("address", baseAddress.ToString());
          writer.WriteEndElement();
          return primitiveElement.Sizeof();

        case LCStructElementArrayType arrayElement:
          writer.WriteStartElement("array");
          writer.WriteAttributeString("type", LCTypesUtils.PrimitiveTypeGetName(arrayElement.Type.TypeElement.Type));
          writer.WriteAttributeString("name", element.Name);
          writer.WriteAttributeString("depth", arrayElement.Type.ArrayDepth.ToString());
          writer.WriteAttributeString("address", baseAddress.ToString());
          writer.WriteEndElement();
          return arrayElement.Type.ArrayDepth * arrayElement.Type.TypeElement.Sizeof();

        default:
          throw new NotSupportedException($"Unsupported struct element type: {element.GetType().Name}");
      }
    }
  }
}
