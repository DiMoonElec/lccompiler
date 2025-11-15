using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler
{
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
          ParsedAttribute parsed = null;

          try
          {
            parsed = AttributesParser.Parse(attribute);
          }
          catch
          {
            logger.Error(obj.AttributeLocate, "Неверный формат атрибута");
            isOK = false;
            continue;
          }

          if (parsed.IsArray == false)
          {
            var variableType = obj.ObjectType.Type;
            var variableAddress = obj.Address;
            var variableName = obj.ObjectName;

            bool r = SingleBinding(variableType, variableAddress, variableName,
              parsed.Single.Resources,
              resourceClasses,
              obj.AttributeLocate,
              logger);

            if (r == false)
              isOK = false;
          }
          else
          {
            if (obj.ObjectType.Type is LCArrayType arrayObject)
            {
              var arrayType = obj.ObjectType.Type;
              var arrayAddress = obj.Address;
              var arrayName = obj.ObjectName;
              var arrayDepth = arrayObject.ArrayDepth;
              var elementType = arrayObject.TypeElement;
              var elementSizeof = elementType.Sizeof();

              if (arrayDepth < parsed.Array.Length)
              {
                logger.Error(obj.AttributeLocate, "Количество атрибутов больше глубины массива");
                isOK = false;
                continue;
              }

              for (int i = 0; i < parsed.Array.Elements.Count; i++)
              {
                var e = parsed.Array.Elements[i];

                var variableType = elementType;
                var variableAddress = arrayAddress + i * elementSizeof;
                var variableName = $"{arrayName}[{i}]";

                bool r = SingleBinding(variableType, variableAddress, variableName,
                  e.Resources,
                  resourceClasses,
                  obj.AttributeLocate,
                  logger);

                if (r == false)
                  isOK = false;
              }

            }
            else
            {
              logger.Error(obj.AttributeLocate, "Неверный формат атрибута для данной переменной");
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

    private static bool SingleBinding(LCType variableType, int variableAddress, string variableName,
      IReadOnlyList<ResourceAttribute> resources,
      IOResourceClass[] resourceClasses,
      LocateElement attributeLocate, 
      CompilerLogger logger)
    {
      bool isOK = true;
      foreach (var attr in resources)
      {
        var r = BindResourceToVariable(attr, resourceClasses,
          variableType,
          variableAddress,
          variableName,
          attributeLocate,
          logger);

        if (r == false)
          isOK = false;
      }

      return isOK;
    }

    private static bool BindResourceToVariable(ResourceAttribute variableAttribute,
      IOResourceClass[] supportedResources,
      LCType variableType,
      int variableAddress,
      string variableName,
      LocateElement attributeLocate,
      CompilerLogger logger)
    {
      string alias = variableAttribute.ResourceClass;
      string id = variableAttribute.Element;

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
        logger.Error(attributeLocate, $"Неизвестный класс ресурса '{variableAttribute.ResourceClass}'");
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
        logger.Error(attributeLocate, $"Неизвестный элемент ресурса '{variableAttribute.ResourceClass}.{variableAttribute.Element}'");
        return false;
      }

      if (resource.IsBinded)
      {
        logger.Error(attributeLocate, $"К ресурсу '{resourceClass.Alias}.{resource.Name}' уже привязана переменная '{resource.Description}'");
        return false;
      }

      if (resourceClass is ResourceClassInputs resourceClassInputs)
      {
        return bindingResourceClassInputOutput(resource,
          variableType, variableAddress, variableName,
          variableAttribute,
          attributeLocate,
          logger);
      }
      else if (resourceClass is ResourceClassOutputs resourceClassOutputs)
      {
        return bindingResourceClassInputOutput(resource,
          variableType, variableAddress, variableName,
          variableAttribute,
          attributeLocate,
          logger);
      }
      else if (resourceClass is ResourceClassModbusSlave resourceClassModbusSlave)
      {
        return bindingResourceClassModbusSlave(resource,
          variableType, variableAddress, variableName,
          variableAttribute,
          attributeLocate,
          logger);
      }

      throw new InternalCompilerException("Unknown resource class");
    }

    private static bool bindingResourceClassModbusSlave(IOResource resource,
      LCType variableType,
      int variableAddress,
      string variableName,
      ResourceAttribute variableAttribute,
      LocateElement attributeLocate,
      CompilerLogger logger)
    {
      return false;
    }

    private static bool bindingResourceClassInputOutput(IOResource resource,
      LCType variableType,
      int variableAddress,
      string variableName,
      ResourceAttribute variableAttribute,
      LocateElement attributeLocate,
      CompilerLogger logger)
    {
      if (LCTypesUtils.IsEqual(variableType, resource.Type))
      {
        resource.Bind(variableAddress, variableName);
        return true;
      }
      else
      {
        logger.Error(attributeLocate, $"Переменная '{variableName}' должна иметь тип '{LCTypesUtils.PrimitiveTypeGetName(resource.Type)}'");
        return false;
      }
    }
  }
}
