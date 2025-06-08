namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Данная нода реализует оператор for(Init; Condition; Loop) Body. 
  /// Нода ForNode оборачивается в ноду BlockCodeNode, в которой содержится локальная таблица символов, 
  /// в которой могут быть расположены деклараторы переменных из секции Init.
  /// </summary>
  partial class ForNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// инициализатор, может иметь несколько потомков
    /// </summary>
    //public FieldNode Init;

    /// <summary>
    /// условие цикла, ноль либо один потомок
    /// </summary>
    public FieldNode Condition;

    /// <summary>
    /// выполняется после завершения очередного цикла, может быть ноль либо несколько потомков
    /// </summary>
    public FieldNode Loop;

    /// <summary>
    /// тело цикла. Может иметь одного потомка (нода операции либо BlockCodeNode)
    /// </summary>
    public FieldNode Body;


    public ForNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
      //Init = new FieldNode("Init");
      Condition = new FieldNode("Condition");
      Loop = new FieldNode("Loop");
      Body = new FieldNode("Body");

      //AddChild(Init);
      AddChild(Condition);
      AddChild(Loop);
      AddChild(Body);
    }

    public override string Description()
    {
      return "for";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }


  /// <summary>
  /// Данная нода реализует оператор while(Condition) Body
  /// </summary>
  partial class WhileNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// условие цикла, один потомок
    /// </summary>
    public FieldNode Condition;

    /// <summary>
    /// тело цикла. Может иметь одного потомка (нода операции либо BlockCodeNode)
    /// </summary>
    public FieldNode Body;

    public WhileNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
      Condition = new FieldNode("Condition");
      Body = new FieldNode("Body");
      AddChild(Condition);
      AddChild(Body);
    }

    public override string Description()
    {
      return "while";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода реализует оператор do Body while(Condition);
  /// </summary>
  partial class DoNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// условие цикла, один потомок
    /// </summary>
    public FieldNode Condition;

    /// <summary>
    /// тело цикла. Может иметь одного потомка (нода операции либо BlockCodeNode)
    /// </summary>
    public FieldNode Body;

    public string StatementDoWhileTxt = null;

    public DoNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
      Condition = new FieldNode("Condition");
      Body = new FieldNode("Body");

      AddChild(Condition);
      AddChild(Body);
    }

    public override string Description()
    {
      return "do";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода реализует оператор if(Condition) TrueBody else FalseBody. 
  /// Условие else может отсутствовать, и в этом случае FalseBody = null.
  /// </summary>
  class IfNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// условие цикла, один потомок
    /// </summary>
    public FieldNode Condition = new FieldNode("Condition");

    /// <summary>
    /// тело цикла, если Condition == true
    /// </summary>
    public FieldNode TrueBody = new FieldNode("TrueBody");

    /// <summary>
    /// тело цикла, если Condition == false
    /// </summary>
    public FieldNode FalseBody = new FieldNode("FalseBody");

    public IfNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
      AddChild(Condition);
      AddChild(TrueBody);
      AddChild(FalseBody);
    }

    public override string Description()
    {
      return "if";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода реализует оператор switch(Expression) Body
  /// </summary>
  partial class SwitchNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// условие оператора, один потомок
    /// </summary>
    public FieldNode Expression;

    public LabelCaseNode[] LabelsCase;

    public LabelDefaultNode LabelDefault = null;

    /// <summary>
    /// Тип выражения
    /// </summary>
    public LCPrimitiveType ExpressionType;

    /// <summary>
    /// тело оператора
    /// </summary>
    public FieldNode Body;

    public SwitchNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
      Expression = new FieldNode("Expression");
      Body = new FieldNode("Body");

      AddChild(Expression);
      AddChild(Body);
    }

    public override string Description()
    {
      return "switch";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Оператор Break. Должен быть вложен в тело одного из соответствующих операторов. 
  /// Данный факт проверяется на этапе семантической проверки.
  /// </summary>
  class BreakNode : OperatorNode, IStatementMarker
  {
    public SemanticCheckStatus CheckParentOperator = SemanticCheckStatus.NOT_CHECKED;
    public BreakNode(LocateElement locate) : base(locate, locate)
    {
    }

    /// <summary>
    /// Ближайший оператор, которому соответствует данный оператор break
    /// В качестве такого оператора может выступать
    /// do, while, for, switch
    /// </summary>
    public Node ClosestOperator;

    public DoNode ClosestOperatorDo = null;
    public WhileNode ClosestOperatorWhile = null;
    public ForNode ClosestOperatorFor = null;
    public SwitchNode ClosestOperatorSwitch = null;


    public override string Description()
    {
      return "break";
    }

    public override bool SemanticCheck()
    {
      if (CheckParentOperator != SemanticCheckStatus.OK_CHECK)
        return false;

      return true;
    }
  }

  /// <summary>
  /// Оператор Continue. Должен быть вложен в тело одного из соответствующих операторов. 
  /// Данный факт проверяется на этапе семантической проверки.
  /// </summary>
  class ContinueNode : OperatorNode, IStatementMarker
  {
    public SemanticCheckStatus CheckParentOperator = SemanticCheckStatus.NOT_CHECKED;

    public ContinueNode(LocateElement locate) : base(locate, locate)
    {
    }

    /// <summary>
    /// Ближайший оператор, которому соответствует данный оператор continue
    /// В качестве такого оператора может выступать
    /// do, while, for
    /// </summary>
    public Node ClosestOperator = null;

    public DoNode ClosestOperatorDo = null;
    public WhileNode ClosestOperatorWhile = null;
    public ForNode ClosestOperatorFor = null;

    public override string Description()
    {
      return "continue";
    }

    public override bool SemanticCheck()
    {
      if (CheckParentOperator != SemanticCheckStatus.OK_CHECK)
        return false;

      return true;
    }
  }

  /// <summary>
  /// Оператор case. Должен быть вложен в тело оператора SwitchNode. 
  /// Данный факт проверяется на этапе семантической проверки.
  /// </summary>
  partial class LabelCaseNode : LabelNode
  {
    public LabelCaseNode(LocateElement locate, LocateElement expressionLocate) 
      : base(locate, expressionLocate)
    {
    }

    /// <summary>
    /// Ссылка на ноду оператора switch,
    /// в который вложена данная нода
    /// </summary>
    public SwitchNode ClosestOperatorSwitch = null;

    /// <summary>
    /// Константа, которая выступает в качестве метки
    /// </summary>
    public ConstantValue Constant = null;

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Оператор default. Должен быть вложен в тело оператора SwitchNode. 
  /// Данный факт проверяется на этапе семантической проверки.
  /// </summary>
  partial class LabelDefaultNode : LabelNode
  {
    /// <summary>
    /// default должен быть сложен в блок кода switch
    /// </summary>
    public SemanticCheckStatus CheckParentOperator = SemanticCheckStatus.NOT_CHECKED;

    public LabelDefaultNode(LocateElement locate, LocateElement expressionLocate) : base(locate, expressionLocate)
    {
    }

    /// <summary>
    /// Ссылка на ноду оператора switch,
    /// в который вложена данная нода
    /// </summary>
    public SwitchNode ClosestOperatorSwitch = null;

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Оператор return
  /// </summary>
  class ReturnNode : OperatorNode, IStatementMarker
  {
    /// <summary>
    /// Декларатор функции, в которую вложен оператор return
    /// </summary>
    public ManagedFunctionDeclaratorNode Function { get; set; }


    public ReturnNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    {
    }

    public override string ToString()
    {
      if (Function == null)
        return base.ToString();
      return "return (" + Function.ToString() + ")";
    }

    public override string Description()
    {
      return "return";
    }

    public override bool SemanticCheck()
    {
      return true;
    }

    public TypedNode GetOperand()
    {
      if (CountChildrens == 1)
        if (GetChild(0) is TypedNode typedNode)
          return typedNode;

      return null;
    }
  }

  /// <summary>
  /// Присвоение (a = b)
  /// </summary>
  class AssignNode : AssignOperatorNode, IStatementMarker
  {
    public override string Description()
    {
      return "=";
    }

    public override bool SemanticCheck()
    {
      return true;
    }

    public AssignNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate) { }
  }
}
