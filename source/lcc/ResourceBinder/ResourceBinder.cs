using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator;
using LC2.LCCompiler.Compiler;
using static LC2.LCCompiler.Compiler.LCPrimitiveType;

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
      int resourceIndex = -1;

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

      for (int i = 0; i < resourceClass.Resources.Length; i++)
      {
        var r = resourceClass.Resources[i];

        if (r.Name == id)
        {
          resource = r;
          resourceIndex = i;
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
        return bindingResourceClassModbusSlave(resourceClassModbusSlave, resourceIndex,
          variableType, variableAddress, variableName,
          variableAttribute,
          attributeLocate,
          logger);
      }

      throw new InternalCompilerException("Unknown resource class");
    }

    private static bool bindingResourceClassModbusSlave(ResourceClassModbusSlave resourceClass,
      int resourceIndex,
      LCType variableType,
      int variableAddress,
      string variableName,
      ResourceAttribute variableAttribute,
      LocateElement attributeLocate,
      CompilerLogger logger)
    {
      // Проверка типа переменной — должен быть примитив
      if (!(variableType is LCPrimitiveType primitiveType))
      {
        logger.Error(attributeLocate, $"Переменная '{variableName}' должна иметь примитивный тип данных");
        return false;
      }

      int size = primitiveType.Sizeof();

      // Поддерживаем только 2, 4 и 8 байт
      if (size != 2 && size != 4 && size != 8)
      {
        logger.Error(attributeLocate, "В качестве Modbus-переменной данный тип не поддерживается");
        return false;
      }

      // Количество modbus-регистров, которые будут использованы (каждый регистр = 2 байта)
      int registersCount = size / 2;

      // Проверка индекса и выхода за пределы массива регистров
      if (resourceIndex < 0)
      {
        logger.Error(attributeLocate, $"Неверный индекс регистра: {resourceIndex}");
        return false;
      }

      if (resourceIndex + registersCount - 1 >= resourceClass.Resources.Length)
      {
        logger.Error(attributeLocate,
            $"Недостаточно регистров в классе '{resourceClass.Alias}' начиная с индекса {resourceIndex} для переменной размера {size} байт");
        return false;
      }

      // Сначала проверяем, что все нужные регистры свободны (не привязаны)
      for (int i = 0; i < registersCount; i++)
      {
        var chk = resourceClass.Resources[resourceIndex + i];
        if (chk.IsBinded)
        {
          logger.Error(attributeLocate,
              $"К ресурсу '{resourceClass.Alias}.{chk.Name}' уже привязана переменная '{chk.Description}'");
          return false;
        }
      }

      // Все регистры свободны — выполняем привязку.
      // Порядок: от старшего к младшему — старшая часть идет в resourceIndex, младшая в resourceIndex + (registersCount-1)
      for (int i = 0; i < registersCount; i++)
      {
        var targetResource = resourceClass.Resources[resourceIndex + i];
        // вычисляем смещение в байтах от начала переменной: старшая часть имеет больший смещение
        int offsetBytes = (registersCount - 1 - i) * 2;
        int bindAddress = variableAddress + offsetBytes;

        // registersCount — количество регистров (size / 2)
        // i — текущий индекс 0..registersCount-1, где i==0 — старший регистр
        string bindName = variableName;
        if (registersCount > 1)
        {
          int postfixIndex = registersCount - 1 - i; // e.g. 4-byte: i=0 -> 1, i=1 -> 0
          bindName = $"{variableName}_W{postfixIndex}";
        }

        targetResource.Bind(bindAddress, bindName);
      }

      return true;
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
