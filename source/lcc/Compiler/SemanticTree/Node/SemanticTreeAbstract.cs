using System;
using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler
{
  public enum GenerateDebugValue
  {
    Undefined,
    Yes,
    FullExpression,
    PartiallyExpression,
  }
  enum SemanticCheckStatus
  {
    /// <summary>
    /// Данная проверка еще не выполнена
    /// </summary>
    NOT_CHECKED = 0,

    /// <summary>
    /// Невозможно выполнить проверку из-за ошибок в зависимых нодах
    /// </summary>
    NOT_POSSIBLE = 1,

    /// <summary>
    /// Во время проверки обнаружена ошибка
    /// </summary>
    ERROR = 2,

    /// <summary>
    /// Проверка прошла успешно
    /// </summary>
    OK_CHECK = 3
  };

  /// <summary>
  /// Базовый класс дерева AST
  /// </summary>
  abstract class Node
  {
    /// <summary>
    /// Ссылка на родителя
    /// </summary>
    public Node Parent = null;

    public ModuleRootNode ModuleRoot
    {
      get
      {
        return _ModuleRoot;
      }

      internal set
      {
        _ModuleRoot = value;
        foreach (var c in Childrens)
          c.ModuleRoot = _ModuleRoot;
      }
    }

    ModuleRootNode _ModuleRoot;

    /// <summary>
    /// Количество потомков
    /// </summary>
    public int CountChildrens { get { return Childrens.Count; } }

    /// <summary>
    /// Список потомков
    /// </summary>
    List<Node> Childrens = new List<Node>();

    /// <summary>
    /// Для генератора отладочной инфомарции. Является ли данная нода Statement-ом
    /// </summary>
    public GenerateDebugValue GenerateDebugInfo = GenerateDebugValue.Undefined;

    /// <summary>
    /// Номер потомка в списке потомков предка
    /// </summary>
    public int ChildrenIndex { get; private set; } = -1;

    public virtual new string ToString()
    {
      return this.GetType().Name;
    }

    public virtual void AddChild(Node node)
    {
      if (node == null)
        return;

      if (node is CollectionNode)
      {
        Node[] nodes = node.GetAllChilds();
        foreach (var n in nodes)
          AddChild(n);
        return;
      }

      node.Parent = this;
      node.ChildrenIndex = Childrens.Count;
      node.ModuleRoot = ModuleRoot;
      Childrens.Add(node);
    }

    public virtual void AddChilds(Node[] nodes)
    {
      foreach (Node n in nodes)
        AddChild(n);
    }

    public Node GetChild(int childID)
    {
      return Childrens[childID];
    }

    public Node[] GetAllChilds()
    {
      return Childrens.ToArray();
    }

    /// <summary>
    /// Вставить ноду в начало 
    /// </summary>
    public virtual void InsertChild(Node node)
    {
      InsertChild(node, 0);
    }

    /// <summary>
    /// Вставить ноду по индексу
    /// </summary>
    public virtual void InsertChild(Node node, int index)
    {
      node.Parent = this;
      Childrens.Insert(index, node);

      //Пересчет индексов потомков
      for (int k = 0; k < Childrens.Count; k++)
        Childrens[k].ChildrenIndex = k;
    }

    public virtual Node ExtractChild(int i)
    {
      Node r = Childrens[i];
      Childrens.RemoveAt(i);

      //Пересчет индексов потомков
      for (int k = 0; k < Childrens.Count; k++)
        Childrens[k].ChildrenIndex = k;

      return r;
    }

    public virtual void ReplaceChild(Node OldChild, Node NewChild)
    {
      if (OldChild.ChildrenIndex >= Childrens.Count)
        throw new Exception("This child does not belong to this parent");

      if (!Object.ReferenceEquals(Childrens[OldChild.ChildrenIndex], OldChild))
        throw new Exception("This child does not belong to this parent");

      Childrens[OldChild.ChildrenIndex] = NewChild;
      NewChild.ChildrenIndex = OldChild.ChildrenIndex;
      NewChild.Parent = this;
    }

    public virtual void Replace(Node NewChild)
    {
      if (Parent == null)
        throw new Exception("This is the root node");

      Parent.ReplaceChild(this, NewChild);
    }

    /// <summary>
    /// Удалить данную ноду из дерева
    /// </summary>
    public virtual void Delete()
    {
      Parent.ExtractChild(ChildrenIndex);
    }

    public virtual void DeleteAllChild()
    {
      Childrens.Clear();
    }

    /// <summary>
    /// Успешно ли данная нода прошла все семантические проверки
    /// </summary>
    /// <returns>True - ошибок нет, False - в результате проверок обнаружены ошибки</returns>
    public abstract bool SemanticCheck();

    /// <summary>
    /// Является ли данная нода семантически корректной
    /// </summary>
    public bool SemanticallyCorrect = true;

    public string StatementTxt = null;

    public virtual void Visit(SemanticVisitor v)
    {
      v.Visit((dynamic)this);
    }

  }

  /// <summary>
  /// Данная нода является элементом таблицы символов. 
  /// В качестве потомков могут выступать инициализаторы констант.
  /// </summary>
  abstract class DeclaratorNode : Node
  {
    public string Name { get; private set; }

    public string ModuleName { get; private set; }

    public LocateElement LocateName { get; private set; }


    public DeclaratorNode(string name, string moduleName, LocateElement locateName)
    {
      Name = name;
      ModuleName = moduleName;

      LocateName = locateName;
    }

    public override abstract string ToString();
  }

  abstract class UserTypeDeclaratorNode : DeclaratorNode
  {
    public UserTypeDeclaratorNode(string name, string moduleName, LocateElement locateName) 
      : base(name, moduleName, locateName)
    {

    }
  }

  abstract class FunctionDeclaratorNode : DeclaratorNode
  {
    /// <summary>
    /// Параметры функции, внутри VariableDeclaratorNode так же хранится 
    /// расположение декларатора в исходном коде
    /// </summary>
    public VariableDeclaratorNode[] FunctionParams { get; private set; }

    /// <summary>
    /// Возвращаемый функцией объект
    /// </summary>
    public LCObjectType ReturnType { get; private set; }

    public FunctionDeclaratorNode(string name, string moduleName, LocateElement locateName, VariableDeclaratorNode[] functionParams, LCObjectType returnType)
      : base(name, moduleName, locateName)
    {
      FunctionParams = functionParams;

      if(FunctionParams != null)
      {
        foreach (var param in FunctionParams)
          param.ObjectType.SetAccessAttributes(true, true);
      }

      ReturnType = returnType;
      ReturnType.SetAccessAttributes(true, false); //Возвращаемый объект доступен только на чтение
    }
  }

  /// <summary>
  /// Декларатор пользовательского типа - структуры
  /// </summary>
  /*
  internal class StructDeclaratorNode : DeclaratorNode
  {
    /// <summary>
    /// Структура
    /// </summary>
    public LCStructObjectType StructType { get; private set; }

    /// <summary>
    /// Положение в исходниках Типов элементов
    /// </summary>
    public LocateElement[] ElementsTypeLocate { get; private set; }

    /// <summary>
    /// Положние в исходниках Имен элементов
    /// </summary>
    public LocateElement[] ElementsNameLocate { get; private set; }

    /// <summary>
    /// Конструктор ноды декларатора структуры
    /// </summary>
    /// <param name="structType">Тип структуры</param>
    /// <param name="name">Имя типа структуры</param>
    /// <param name="moduleName">Имя модуля, в котором объявлена структура</param>
    /// <param name="locateName">Расположение имени структуры</param>
    /// <param name="elementsTypeLocate">Расположение типов элементов структуры</param>
    /// <param name="elementsNameLocate">Расположение имен элементов структуры</param>
    public StructDeclaratorNode(LCStructObjectType structType, string name, string moduleName,
      LocateElement locateName, LocateElement[] elementsTypeLocate, LocateElement[] elementsNameLocate) : base(name, moduleName, locateName)
    {
      StructType = structType;
      ElementsTypeLocate = elementsTypeLocate;
      ElementsNameLocate = ElementsNameLocate;
    }

    public override string ToString()
    {
      return "struct";
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }
  */
  abstract class ObjectDeclaratorNode : DeclaratorNode
  {
    public enum DeclaratorClass
    {
      ClassGlobal,
      ClassLocal,
      ClassFunctionParam,
      //ClassTupleReturnElement
    };

    public DeclaratorClass ClassValue { get; set; }

    public LCObjectType ObjectType { get; private set; }

    public LCTypeLocate LocateObjectType { get; private set; }

    public ObjectDeclaratorNode(LCObjectType objectType, string name, string moduleName,
      LCTypeLocate locateObjectType, LocateElement locateName) : base(name, moduleName, locateName)
    {
      ObjectType = objectType;
      LocateObjectType = locateObjectType;
      ClassValue = DeclaratorClass.ClassLocal;
    }

    public override bool SemanticCheck()
    {
      return true;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", ObjectType.ToString(), Name);
    }
  }

  /// <summary>
  /// От данной ноды наследуются вспомогательные ноды-контейнеры
  /// </summary>
  abstract class ContainerNode : Node
  {

  }

  /// <summary>
  /// Базовая нода операторов. Наследованное поле NodeLocate содержит позицию ключевого слова оператора в исходном тексте программы.
  /// </summary>
  abstract class OperatorNode : SourceLocatedNode
  {
    public abstract string Description();

    public OperatorNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  /// <summary>
  /// Базовая нода операторов присвоенния.
  /// Нулевая нода - Receiver, первая - Source
  /// </summary>
  abstract class AssignOperatorNode : OperatorNode
  {
    public LCPrimitiveType OperandsCType;
    public TypedNode GetOperandLeft()
    {
      if (CountChildrens == 2)
        if(GetChild(0) is TypedNode typedNode)
          return typedNode;

      return null;
    }

    public TypedNode GetOperandRight()
    {
      if (CountChildrens == 2)
        if(GetChild(1) is TypedNode typedNode)
        return typedNode;
      return null;
    }

    public AssignOperatorNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  abstract class SourceLocatedNode : Node, ILocateElement
  {
    /// <summary>
    /// Размещение оператора в исходном коде
    /// </summary>
    public virtual LocateElement Locate { get; set; }

    /// <summary>
    /// Размещение выражения в исходном коде
    /// </summary>
    public virtual LocateElement ExpressionLocate { get; set; }

    public SourceLocatedNode(LocateElement locate, LocateElement expressionLocate)
    {
      Locate = locate;
      ExpressionLocate = expressionLocate;
    }
  }

  /// <summary>
  /// Базовая нода для типизированных нод, таких как операции, константные значения, переменные и константы, вызовы функций.
  /// </summary>
  abstract partial class TypedNode : SourceLocatedNode
  {
    /// <summary>
    /// тип объекта
    /// </summary>
    public LCObjectType ObjectType
    {
      get
      {
        return _ObjectType;
      }
      set
      {
        if (_ObjectType != null)
          throw new Exception("Тип данной ноды был присвоен ранее");
        _ObjectType = value;
      }
    }

    LCObjectType _ObjectType = null;

    public TypedNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  /// <summary>
  /// Базовая нода для операций (+, -, >, ==, и т.д.). 
  /// Наследованное поле DType содержит тип возвращаемого значения, 
  /// OperandsDType — типы операндов. Поле NodeLocate содержат 
  /// позицию ключевого слова операции в исходном тексте программы.
  /// </summary>
  abstract class OperationNode : TypedNode
  {
    /// <summary>
    /// тип операнда(ов)
    /// </summary>
    public LCType OperandsCType;

    public abstract string Description();

    public OperationNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  /// <summary>
  /// Базовая нода для операций, имеющих два операнда. Нулевая нода - левый операнд, первая - правый.
  /// </summary>
  abstract class BinaryOperationNode : OperationNode
  {
    public TypedNode GetOperandLeft()
    {
      if (CountChildrens == 2)
      {
        var r = GetChild(0);
        if (r is TypedNode typedNode)
          return typedNode;
      }
      return null;
    }

    public TypedNode GetOperandRight()
    {
      if (CountChildrens == 2)
      {
        var r = GetChild(1);
        if (r is TypedNode typedNode)
          return typedNode;
      }
      return null;
    }

    public override bool SemanticCheck()
    {
      return true;
    }

    public BinaryOperationNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  /// <summary>
  /// Базовая нода для операций, имеющих один операнд.
  /// </summary>
  abstract class UnaryOperationNode : OperationNode
  {
    public TypedNode GetOperand()
    {
      if (CountChildrens == 1)
      {
        var r = GetChild(0);
        if (r is TypedNode typedNode)
          return typedNode;
      }
      return null;
    }

    public override bool SemanticCheck()
    {
      return false;
    }

    public UnaryOperationNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }

  /// <summary>
  /// Базовая нода для различных вспомогательных нод.
  /// </summary>
  abstract class HelperNode : Node
  {

  }

  /// <summary>
  /// Базовая нода для нод, которые представляют собой метки.
  /// </summary>
  abstract class LabelNode : SourceLocatedNode
  {
    public LabelNode(LocateElement locate, LocateElement expressionLocate)
      : base(locate, expressionLocate)
    { }
  }
}
