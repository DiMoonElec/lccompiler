namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Нода вызова функции. Потомками данной ноды являются аргументы вызываемой функции.
  /// </summary>
  partial class FunctionCallNode : OperationNode, IExpressionMarker, IStatementMarker
  {
    /// <summary>
    /// Имя вызываемой функции
    /// </summary>
    public string FunctionName { get; private set; }

    /// <summary>
    /// Если вызывается функция, то ее декларатор будет находиться здесь
    /// </summary>
    public ManagedFunctionDeclaratorNode ManagedFunctionDeclarator { get; set; }

    /// <summary>
    /// Если вызывается нативная функция, то ее декларатор будет находиться здесь
    /// </summary>
    public NativeFunctionDeclaratorNode NativeFunctionDeclarator { get; set; }

    public override string Description()
    {
      return "()";
    }

    /// <summary>
    /// Каждый потомок поля Params является параметром вызова функции
    /// </summary>
    public FieldNode Params = new FieldNode("Params");

    public FunctionCallNode(string functionName, LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    {
      AddChild(Params);
      FunctionName = functionName;
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Предоставляет доступ к элементу массива
  /// </summary>
  class IndexerNode : OperationNode, IExpressionMarker
  {
    /// <summary>
    /// Индекс массива должен приводиться к типу int
    /// </summary>
    public SemanticCheckStatus CheckIndexIsValid = SemanticCheckStatus.NOT_CHECKED;

    /// <summary>
    /// Индексируемый объект должен быть массивом или ссылкой на массив, и при этом
    /// не быть типа void[]
    /// </summary>
    public SemanticCheckStatus CheckIndexingObjIsValid = SemanticCheckStatus.NOT_CHECKED;

    /// <summary>
    /// Должен иметь одного потомка, который является индексом массива
    /// </summary>
    public FieldNode Index;

    public LCPrimitiveType IndexType = null;

    public override string Description()
    {
      return "[]";
    }

    /// <summary>
    /// Должен иметь одного потомка, который должен являться TerminalIDNode после построения дерева AST,
    /// и в дальнейшем заменен на ObjectNode
    /// </summary>
    public FieldNode IndexingObj;

    public IndexerNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    {
      Index = new FieldNode("Index");
      AddChild(Index);

      IndexingObj = new FieldNode("IndexingObj");
      AddChild(IndexingObj);
    }

    public override bool SemanticCheck()
    {
      if (CheckIndexIsValid != SemanticCheckStatus.OK_CHECK)
        return false;
      if (CheckIndexingObjIsValid != SemanticCheckStatus.OK_CHECK)
        return false;
      return true;
    }
  }

  /// <summary>
  /// Предоставляет доступ к элементу структуры через '->'
  /// </summary>
  class ElementAccessNode : OperationNode, IExpressionMarker
  {
    public string Field { get; private set; }
    public int ElementOffset { get; set; }

    /// <summary>
    /// Конструктор ноды, предоставляющей доступ к элементу структуры
    /// </summary>
    /// <param name="field">Имя поля, к которому выполняется доступ</param>
    /// <param name="structObject">Структура или ссылка на структуру</param>
    /// <param name="objectLocate">Расположение оператора в исходном коде</param>
    /// <param name="objectFieldLocate">Расположение выражения в исходном коде</param>
    public ElementAccessNode(string field, Node structObject,
      LocateElement objectLocate, LocateElement objectFieldLocate)
      : base(objectLocate, objectFieldLocate)
    {
      Field = field;
      AddChild(structObject);
    }

    public override string Description()
    {
      return "." + Field;
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Постфиксный инкремент (i++)
  /// </summary>
  class PostfixIncrementNode : UnaryOperationNode, IExpressionMarker, IStatementMarker
  {
    public override string Description()
    {
      return "++ (postfix increment)";
    }

    public PostfixIncrementNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Постфиксный декремент (i--)
  /// </summary>
  class PostfixDecrementNode : UnaryOperationNode, IExpressionMarker, IStatementMarker
  {
    public override string Description()
    {
      return "-- (postfix decrement)";
    }

    public PostfixDecrementNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Префиксный инкремент (++i)
  /// </summary>
  class PrefixIncrementNode : UnaryOperationNode, IExpressionMarker, IStatementMarker
  {
    public override string Description()
    {
      return "++ (prefix increment)";
    }

    public PrefixIncrementNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Префиксный декремент (--i)
  /// </summary>
  class PrefixDecrementNode : UnaryOperationNode, IExpressionMarker, IStatementMarker
  {
    public override string Description()
    {
      return "-- (prefix decrement)";
    }

    public PrefixDecrementNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Изменение знака (-i)
  /// </summary>
  class NegativeNode : UnaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "- (change sign)";
    }
    public NegativeNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Логическое НЕ (!а)
  /// </summary>
  class LogicNotNode : UnaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "! (logical not)";
    }
    public LogicNotNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }


  /// <summary>
  /// Побитовое НЕ (~)
  /// </summary>
  class NotNode : UnaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "~ (not)";
    }

    public NotNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Приведение типа
  /// </summary>
  class TypeCastNode : UnaryOperationNode, IExpressionMarker
  {

    /// <summary>
    /// Тип, к которому выполняется приведение типа аргумента.
    /// Данное поле создано для упрощения доступа к приводимому типу.
    /// В процессе семантического анализа полю ObjectType будет присвоено
    /// значение, в соответствии с CastType
    /// </summary>
    public bool IsAutoCast { get; private set; }
    public LCPrimitiveType CastType { get; private set; }

    public override string Description()
    {
      return string.Format("({0})", CastType.ToString());
    }

    public TypeCastNode(LCPrimitiveType cType, LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    {
      CastType = cType;
      IsAutoCast = false;
    }

    public TypeCastNode(LCPrimitiveType cType) : base(null, null)
    {
      CastType = cType;
      Locate = null;
      IsAutoCast = true;
    }
    public override string ToString()
    {
      return "(" + CastType.ToString() + ")";
    }
  }

  /// <summary>
  /// Суммирование (a + b)
  /// </summary>
  class SummNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "+";
    }

    public SummNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Вычитание (a - b)
  /// </summary>
  class SubNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "-";
    }

    public SubNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Умножение (a * b)
  /// </summary>
  class MulNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "*";
    }

    public MulNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Деление (a / b)
  /// </summary>
  class DivNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "/";
    }

    public DivNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Остаток от деления (a % b)
  /// </summary>
  class RemNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "%";
    }

    public RemNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Сдвиг вправо (a >> b)
  /// </summary>
  class RightShiftNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return ">>";
    }

    public RightShiftNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Сдвиг влево (a << b)
  /// </summary>
  class LeftShiftNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "<<";
    }

    public LeftShiftNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Меньше (a < b)
  /// </summary>
  class LessNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "<";
    }

    public LessNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Меньше либо равно (a <= b)
  /// </summary>
  class LessEqualNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "<=";
    }

    public LessEqualNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Больше (a > b)
  /// </summary>
  class MoreNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return ">";
    }

    public MoreNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Больлше либо равно (a >= b)
  /// </summary>
  class MoreEqualNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return ">=";
    }

    public MoreEqualNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Равно (a == b)
  /// </summary>
  class EqualNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "==";
    }

    public EqualNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Не равно (a != b)
  /// </summary>
  class NotEqualNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "!=";
    }

    public NotEqualNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Побитовое И (a & b)
  /// </summary>
  class AndNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "&";
    }

    public AndNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Исключающее ИЛИ (a ^ b)
  /// </summary>
  class XorNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "^";
    }

    public XorNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Побитовое ИЛИ (a | b)
  /// </summary>
  class OrNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "|";
    }

    public OrNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Логическое И (a && b)
  /// </summary>
  class LogicAndNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "&&";
    }

    public LogicAndNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }

  /// <summary>
  /// Логическое ИЛИ (a || b)
  /// </summary>
  class LogicOrNode : BinaryOperationNode, IExpressionMarker
  {
    public override string Description()
    {
      return "||";
    }

    public LogicOrNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }
}
