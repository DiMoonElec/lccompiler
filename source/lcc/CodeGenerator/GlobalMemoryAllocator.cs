using LC2.LCCompiler.Compiler;
using System;
using System.Collections.Generic;

namespace LC2.LCCompiler.CodeGenerator
{
  internal class GlobalMemoryAllocator
  {
    public List<GlobalMemoryObject> MemoryObjects = new List<GlobalMemoryObject>();

    public int MemoryUsage { get; private set; }
    public int RetainMemoryUsage { get; private set; }

    /// <summary>
    /// Объявление объекта
    /// </summary>
    /// <param name="name">Имя объявляемого объекта</param>
    /// <param name="objSize">Размер объекта в байтах</param>
    /// <param name="isArray">Является ли объект массивом</param>
    public void Declaration(string name, int objSize, string attribute, LCObjectType objectType)
    {
      var globalObject = Find(name);

      if (globalObject == null) //Если объект не был объявлен ранее
      {
        //Создаем объект до его объявления, параметры объекта будут заполнены позже
        globalObject = new GlobalMemoryObject(name, objSize, attribute, objectType);

        //Добавляем объект в список глобальных объектов
        MemoryObjects.Add(globalObject);
      }
      else //Если объект был использован ранее, то заполняем поля объекта
      {
        globalObject.Fill(objSize, attribute, objectType);
      }
    }

    public void Merge(GlobalMemoryAllocator globalAllocator)
    {
      foreach (var m in globalAllocator.MemoryObjects)
      {
        if (m.Filled == true)
          Declaration(m.ObjectName, m.ObjectSize, m.Attribute, m.ObjectType);
      }

      //MemoryObjects.AddRange(globalAllocator.MemoryObjects);
    }

    /// <summary>
    /// Добавить ссылку на объект
    /// </summary>
    /// <param name="reference">Добавляемая ссылка</param>
    public void AddReference(GlobalMemoryObjectReference reference)
    {
      var objName = reference.ObjectName;

      var globalObject = Find(objName);
      if (globalObject == null) //Если объект не был объявлен ранее
      {
        //Создаем объект до его объявления, параметры объекта будут заполнены позже
        globalObject = new GlobalMemoryObject(objName);

        //Добавляем объект в список глобальных объектов
        MemoryObjects.Add(globalObject);
      }

      //Присваиваем reference ссылку на объект, на который она ссылается
      reference.SetBackLink(globalObject);

      globalObject.AddReference(reference);
    }

    /// <summary>
    /// Распределить память для объектов
    /// </summary>
    public void Allocate()
    {
      Sort();

      int offset = 0;

      if (MemoryObjects.Count > 0)
      {
        GlobalMemoryObject obj = MemoryObjects[0];
        if (obj.Filled == false)
          throw new InternalCompilerException(string.Format("Объект {0} не объявлен", obj.ObjectName));

        obj.Address = offset;
        offset += obj.ObjectSize;

        for (int i = 1; i < MemoryObjects.Count; i++)
        {
          obj = MemoryObjects[i];
          if (obj.Filled == false)
            throw new InternalCompilerException(string.Format("Объект {0} не объявлен", obj.ObjectName));

          obj.Address = offset;
          offset += obj.ObjectSize;
        }
      }

      MemoryUsage = offset;
    }

    public string GetReport()
    {
      string r = "";

      //Добавляем информацию о глобальных объектах
      //r += "Global objects:\r\n";
      /*
      for (int i = 0; i < MemoryObjects.Count; i++)
      {
        var e = MemoryObjects[i];
        if (e.Filled == false)
          r += string.Format(" object: {0} (undefined)", e.ObjectName);
        else
        {
          if (e.Allocated == false)
            r += string.Format(" object: {0} size {1}", e.ObjectName, e.ObjectSize.ToString());
          else
            r += string.Format(" object: {0} size {1} adr {2}", e.ObjectName, e.ObjectSize.ToString(), e.Address.ToString());
        }

        r += "\r\n";
      }
      */
      for (int i = 0; i < MemoryObjects.Count; i++)
      {
        var e = MemoryObjects[i];
        r += string.Format("name={0}\tsize={1}\tadr={2}\ttype={3}\r\n", e.ObjectName, e.ObjectSize.ToString(), e.Address.ToString(), e.ObjectType.ToString());
      }

      return r;
    }

    /// <summary>
    /// Отсортировать глобальные объекты в соответствии с требованиями размещения.
    /// Правила сортировки следующие:
    /// 1. переменные
    /// 2. массивы
    /// 3. retain-переменные
    /// 4. retain-массивы
    /// 5. const-переменные
    /// 6. const-массивы
    /// </summary>
    void Sort()
    {
      //Сортировка пока не выполняется, так как в этом пока нет острой необходимости
    }

    GlobalMemoryObject Find(string name)
    {
      foreach (var e in MemoryObjects)
      {
        if (e.ObjectName == name)
          return e;
      }

      return null;
    }
  }

  internal class GlobalMemoryObject
  {
    /// <summary>
    /// Имя объекта
    /// </summary>
    public string ObjectName { get; private set; }

    /// <summary>
    /// Размер объекта
    /// </summary>
    public int ObjectSize { get; private set; }

    /// <summary>
    /// Тип объекта
    /// </summary>
    public LCObjectType ObjectType { get; private set; }

    /// <summary>
    /// Определен ли этот объект в памяти на данный момент
    /// </summary>
    public bool Filled { get; private set; }

    /// <summary>
    /// Распределена ли память для данного объекта
    /// </summary>
    public bool Allocated { get; private set; }

    /// <summary>
    /// Атрибуты объекта
    /// </summary>
    public string Attribute { get; private set; }

    /// <summary>
    /// Адрес объекта. Присваивается в процессе распределения памяти для глобальных объектов
    /// </summary>
    public int Address
    {
      get
      {
        if (Allocated == false)
          throw new InternalCompilerException("Объект не распределен в памяти");
        return _Address;
      }
      set
      {
        _Address = value;
        Allocated = true;
      }
    }

    int _Address;

    /// <summary>
    /// Ссылки в исходном коде на данный объект
    /// </summary>
    List<GlobalMemoryObjectReference> References = new List<GlobalMemoryObjectReference>();

    /// <summary>
    /// Этот констуктор используется в случае, если объект был объявлен до его использования,
    /// и в этом случае мы знаем все параметры данного объекта
    /// </summary>
    /// <param name="name">Имя объекта</param>
    /// <param name="size">Размер объекта в байтах</param>
    public GlobalMemoryObject(string name, int size, string attribute, LCObjectType objectType)
    {
      Attribute = attribute;
      ObjectName = name;
      ObjectSize = size;
      ObjectType = objectType;
      Filled = true;
      Allocated = false;
    }

    /// <summary>
    /// Этот конструктор используется в случае, если объект был использован до его объявления,
    /// и в этом случае мы знаем только имя объекта
    /// </summary>
    /// <param name="name"></param>
    public GlobalMemoryObject(string name)
    {
      ObjectName = name;
      Attribute = null;
      Filled = false;
      Allocated = false;
    }

    public void Fill(int size, string attribute, LCObjectType objectType)
    {
      ObjectSize = size;
      ObjectType = objectType;
      Attribute = attribute;
      Filled = true;
    }


    /// <summary>
    /// Добавить ссылку на данный объект
    /// </summary>
    /// <param name="reference">Ссылка</param>
    public void AddReference(GlobalMemoryObjectReference reference)
    {
      References.Add(reference);
    }

    public new string ToString()
    {
      return ObjectName;
    }
  }

  internal class GlobalMemoryObjectReference
  {
    /// <summary>
    /// Адрес в программе, где упоминается ссылка на метку с именем LabelName
    /// </summary>
    public int Address
    {
      get
      {
        if (MemoryObject == null)
          return 0;

        return MemoryObject.Address;
      }
    }

    /// <summary>
    /// Имя объекта, на который указывает ссылка
    /// </summary>
    public string ObjectName { get; private set; }

    /// <summary>
    /// Метка, на которую указывает данная ссылка
    /// </summary>
    GlobalMemoryObject MemoryObject = null;

    public GlobalMemoryObjectReference(string labelName)
    {
      ObjectName = labelName;
    }

    public void SetBackLink(GlobalMemoryObject memoryObject)
    {
      MemoryObject = memoryObject;
    }
  }

}
