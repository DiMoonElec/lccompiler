using System.Collections.Generic;

namespace LC2.LCCompiler.CodeGenerator
{
  internal class LocalMemoryAllocator
  {
    //Стеки локальных переменных
    List<MemoryPool> memoryPools = new List<MemoryPool>();

    //Текущий стек
    MemoryPool currentMemoryPool;

    //Параметры функций
    List<LocalMemoryObject> functionParamsObjects;


    public void NewLocalPool(string name)
    {
      currentMemoryPool = new MemoryPool(name);
      memoryPools.Add(currentMemoryPool);

      functionParamsObjects = new List<LocalMemoryObject>();
    }

    public LocalMemoryObject AllocLocalObj(string name, int objSize)
    {
      return currentMemoryPool.Alloc(name, objSize);
    }

    public LocalMemoryObject AllocTailLocalObj(string name, int objSize)
    {
      return currentMemoryPool.AllocTail(name, objSize);
    }

    public void FreeLocalObj(LocalMemoryObject obj)
    {
      currentMemoryPool.Free(obj);
    }

    public int LocalBytesUsed()
    {
      return currentMemoryPool.BytesUsed();
    }

    public LocalMemoryObject CreateParamObj(string name, int objSize)
    {
      int len = functionParamsObjects.Count;
      int offset;

      if (len == 0)
      {
        offset = -objSize;
      }
      else
      {
        offset = functionParamsObjects[len - 1].Address - objSize;
      }

      LocalMemoryObject obj = new LocalMemoryObject(name, objSize, offset, false);
      functionParamsObjects.Add(obj);
      return obj;
    }

    public string GetReport()
    {
      string r = "";

      //Добавляем отчет о всех функциях и локальных объектах
      r += "Local objects:\r\n";

      for (int i = 0; i < memoryPools.Count; i++)
      {
        var pool = memoryPools[i];

        r += pool.GetReport();
      }

      r += "\r\n";

      return r;
    }
  }

  internal class MemoryPool
  {
    /// <summary>
    /// Выделенные объекты
    /// </summary>
    List<LocalMemoryObject> memoryObjects = new List<LocalMemoryObject>();


    /// <summary>
    /// Пул, в котором выполняется распределение объектов, 
    /// 1 элемент пула - 1 байт памяти
    /// </summary>
    List<LocalMemoryObject> pool = new List<LocalMemoryObject>();

    public string PoolName { get; private set; }


    public MemoryPool(string poolName)
    {
      PoolName = poolName;
    }

    public LocalMemoryObject AllocTail(string name, int size)
    {
      int tailFree = TailElementsFree();

      int offset = pool.Count - tailFree;
      int missingBytesCount = size - tailFree;

      for (int i = 0; i < missingBytesCount; i++)
        pool.Add(null);

      var obj = new LocalMemoryObject(name, size, offset, false);
      memoryObjects.Add(obj);

      //Занимаем область памяти данной переменной
      for (int i = offset; i < offset + size; i++)
        pool[i] = obj;

      return obj;
    }

    public LocalMemoryObject Alloc(string name, int size)
    {
      int offset;

      offset = FindFragment(size);

      //Если подходящий фрагмент не найден
      if (offset == -1)
      {
        int tailFree = TailElementsFree();
        offset = pool.Count - tailFree;
        int missingBytesCount = size - tailFree;

        for (int i = 0; i < missingBytesCount; i++)
          pool.Add(null);
      }

      //Добавляем переменную, ее смещение равно offset;
      var obj = new LocalMemoryObject(name, size, offset, false);
      memoryObjects.Add(obj);

      //Занимаем область памяти данной переменной
      for (int i = offset; i < offset + size; i++)
        pool[i] = obj;

      return obj;
    }

    public void Free(LocalMemoryObject obj)
    {
      for (int i = obj.Address; i < obj.Address + obj.Size; i++)
      {
        pool[i] = null;
      }
    }

    int FindFragment(int size)
    {
      for (int i = 0; i < pool.Count;)
      {
        if (pool[i] == null)
        {
          //Вычисляем размер свободной области
          int len = FreeFragmentLenght(i);

          //Если свободная область больше либо равна необходимому фрагменту
          //то возвращаем смещение
          if (len >= size)
            return i;

          //иначе
          i += len;
        }
        else
        {
          i++;
        }
      }

      return -1;
    }

    int FreeFragmentLenght(int offset)
    {
      if (offset >= pool.Count)
        return 0;

      int i;

      //Проходим по свободной области
      for (i = 0; i + offset < pool.Count; i++)
      {
        //Если свободная область закончилась
        if (pool[i + offset] != null)
          break;
      }

      return i;
    }

    int TailElementsFree()
    {
      int freeElements = 0;
      for (int i = pool.Count - 1; i >= 0; i--)
      {
        if (pool[i] == null)
          freeElements++;
        else
          break;
      }
      return freeElements;
    }

    /// <summary>
    /// Сколько задействовано байт на данный момент
    /// </summary>
    /// <returns>Количество задействованных на данный момент байт</returns>
    public int BytesUsed()
    {
      int tailFree = TailElementsFree();
      return pool.Count - tailFree;
    }

    public string GetReport()
    {
      string r = "";

      r += string.Format(" function: {0}\r\n", PoolName);

      for (int i = 0; i < memoryObjects.Count; i++)
      {
        var e = memoryObjects[i];
        r += string.Format("  object: {0} size {1} offset {2}", e.Name, e.Size, e.Address);

        r += "\r\n";
      }
      r += "\r\n";
      return r;
    }
  }

  internal class LocalMemoryObject
  {
    public string Name { get; private set; }
    public int Size { get; private set; }

    public bool ObjIsArray { get; private set; }

    //public bool IsAllocated { get { return isAllocated; } }

    public int Address { get; private set; }

    public LocalMemoryObject(string name, int size, int address, bool isArray)
    {
      Name = name;
      Size = size;
      Address = address;
      ObjIsArray = isArray;
    }

    /*
    public MemoryObject(string name, int size, bool isArray)
    {
      Name = name;
      Size = size;
      isAllocated = false;
      ObjIsArray = isArray;
    }
    */
  }
}
