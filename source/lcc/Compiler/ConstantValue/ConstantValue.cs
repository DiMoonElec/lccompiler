using System;

namespace LC2.LCCompiler.Compiler
{
  internal abstract class ConstantValue
  {
    public LCObjectType ObjectType { get; private set; }
    public LCPrimitiveType PrimitiveType { get; private set; }

    public byte[] Dump { get { return GetDump(); } }

    public ConstantValue(LCPrimitiveType type)
    {
      PrimitiveType = type;
      ObjectType = new LCObjectType(type);
      ObjectType.SetAccessAttributes(true, false);
    }

    

    public new abstract string ToString();

    /// <summary>
    /// Получить дамп константного значения
    /// </summary>
    /// <returns>Дамп</returns>
    public abstract byte[] GetDump();

    /// <summary>
    /// Сумма двух чисел
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Summ(ConstantValue rightValue);

    /// <summary>
    /// Резница двух чисел
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Sub(ConstantValue rightValue);

    /// <summary>
    /// Произведение двух чисел
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Mul(ConstantValue rightValue);

    /// <summary>
    /// Частное двух чисел
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Div(ConstantValue rightValue);

    /// <summary>
    /// Остаток от деления двух чисел
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Rem(ConstantValue rightValue);

    /// <summary>
    /// Сдвиг вправо
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue RightShift(ConstantValue rightValue);

    /// <summary>
    /// Сдвиг влево
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue LeftShift(ConstantValue rightValue);

    /// <summary>
    /// Меньше
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Less(ConstantValue rightValue);

    /// <summary>
    /// Меньше либо равно
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue LessEqual(ConstantValue rightValue);

    /// <summary>
    /// Больше
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue More(ConstantValue rightValue);

    /// <summary>
    /// Больше либо равно
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue MoreEqual(ConstantValue rightValue);

    /// <summary>
    /// Побитовое И
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue And(ConstantValue rightValue);

    /// <summary>
    /// Исключающее ИЛИ
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Xor(ConstantValue rightValue);

    /// <summary>
    /// Побитовое ИЛИ
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Or(ConstantValue rightValue);

    /// <summary>
    /// Логическое И
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue LogicalAnd(ConstantValue rightValue);

    /// <summary>
    /// Логическое ИЛИ
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue LogicalOr(ConstantValue rightValue);

    /// <summary>
    /// Побитовое НЕ
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Not();

    /// <summary>
    /// Логическое НЕ
    /// </summary>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue LogicalNot();

    /// <summary>
    /// Инверсия знака
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Inv();

    /// <summary>
    /// Увеличить значение на 1
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Incr();

    /// <summary>
    /// Уменьшить значение на 1
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Decr();

    /// <summary>
    /// a == b
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Eq(ConstantValue rightValue);

    /// <summary>
    /// a != b
    /// </summary>
    /// <param name="rightValue">Правый операнд</param>
    /// <returns>ConstantValue - результат, null - неверные типы операндов</returns>
    public abstract ConstantValue Neq(ConstantValue rightValue);

    /// <summary>
    /// Преобразовать тип константы в указанный тип
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public abstract ConstantValue TypeConvert(LCType toType);
  }

  internal abstract class IntegerConstantValue : ConstantValue
  {
    public IntegerConstantValue(LCPrimitiveType type) : base(type)
    {
    }
    /*
    internal ConstantValue ChangeBitDepth(LCType type)
    {
      
    }
    */
  }

  internal abstract class FloatingConstantValue : ConstantValue
  {
    public FloatingConstantValue(LCPrimitiveType type) : base(type)
    {
    }
  }
}
