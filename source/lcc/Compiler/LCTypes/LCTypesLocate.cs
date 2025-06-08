using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC2.LCCompiler.Compiler
{
  internal class LCTypeLocate
  {
    public LocateElement Locate { get; private set; }
    public LCTypeLocate(LocateElement locate)
    {
      Locate = locate;
    }
  }

  internal class LCArrayTypeLocate : LCTypeLocate
  {
    /// <summary>
    /// Расположение имени типа массива
    /// </summary>
    public LocateElement TypeLocate { get; private set; }

    /// <summary>
    /// Расположение глубины массива
    /// </summary>
    public LocateElement ArrayDepthLocate { get; private set; }

    public LCArrayTypeLocate(LocateElement locate, LocateElement typeLocate, LocateElement arrayDepthLocate) : base(locate)
    {
      TypeLocate = typeLocate;
      ArrayDepthLocate = arrayDepthLocate;
    }

  }

  internal class LCTupleTypeLocate : LCTypeLocate
  {
    public LocateElement[] TupleElementsLocate { get; private set; }

    /// <summary>
    /// Расположение кортежа в исходном коде
    /// </summary>
    /// <param name="locate">Расположение всего кортежа</param>
    /// <param name="tupleElementsLocate">Расположение элементов кортежа</param>
    public LCTupleTypeLocate(LocateElement locate, LocateElement[] tupleElementsLocate) : base(locate)
    {
      TupleElementsLocate = tupleElementsLocate;
    }
  }

  internal class LCStructTypeLocate : LCTypeLocate
  {
    /// <summary>
    /// Расположение типов
    /// </summary>
    public LCTypeLocate[] TypesLocate { get; private set; }

    /// <summary>
    /// Расположение имен элементов
    /// </summary>
    public LocateElement[] NamesLocate { get; private set; }

    /// <summary>
    /// Расположение декларатора структуры в исходном коде
    /// </summary>
    /// <param name="typesLocate">Расположение типов</param>
    /// <param name="namesLocate">Расположение имен элементов</param>
    /// <param name="structNameLocate">Расположение имени структуры</param>
    public LCStructTypeLocate(LCTypeLocate[] typesLocate, LocateElement[] namesLocate, LocateElement structNameLocate) : base(structNameLocate)
    {
      TypesLocate = typesLocate;
      NamesLocate = namesLocate;
    }
  }
}
