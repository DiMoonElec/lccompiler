using System;
using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler
{
  /// <summary>
  /// Корень семантического дерева
  /// </summary>
  class ModuleRootNode : Node
  {
    public ModuleInitNode ModuleInit { get; private set; }

    public UseDirectives Uses;

    public ModuleRootNode()
    {
      ModuleRoot = this;

      ModuleInit = new ModuleInitNode();
      AddChild(ModuleInit);
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода содержит в себе инициализаторы глобальных переменных данного модуля.
  /// </summary>
  class ModuleInitNode : Node
  {
    public override bool SemanticCheck()
    {
      return true;
    }
  }

  class InitializerNode : HelperNode, ILocateElement
  {
    public LocateElement Locate { get; set; }
    public LocateElement ExpressionLocate { get; private set; }

    public override bool SemanticCheck()
    {
      return true;
    }

    public InitializerNode(LocateElement locate, LocateElement expressionLocate)
    {
      Locate = locate;
      ExpressionLocate = expressionLocate;
    }
  }

  /// <summary>
  /// Значения массива. Каждый потомок является элементом массива типа expression
  /// </summary>
  class ArrayValueNode : HelperNode
  {
    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода содержит локальную таблицу символов. Для работы с таблицей символов служит специальный класс.
  /// </summary>
  class BlockCodeNode : Node, IStatementMarker
  {
    /// <summary>
    /// Локальная таблица символов
    /// </summary>
    public BlockCodeNode()
    {
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  class ManagedFunctionDeclaratorNode : FunctionDeclaratorNode
  {

    /// <summary>
    /// Тело функции
    /// </summary>
    public BlockCodeNode Body { get; private set; }

    /// <summary>
    /// Расположение в исходном коде возвращаемого типа данных
    /// </summary>
    public LocateElement ReturnTypeLocate { get; private set; }

    /// <summary>
    /// Объявление функции
    /// </summary>
    /// <param name="name">Имя функции</param>
    /// <param name="moduleName">Имя текущего модуля</param>
    /// <param name="functionParams">Параметры функции, объектам автоматически присваивается класс ClassFunctionParam</param>
    /// <param name="returnType">Возвращаемый тип, объекту автоматически присваивается ro модификатор доступа</param>
    /// <param name="body">Тело функции</param>
    /// <param name="locateName">Расположение имени функции в исходном коде</param>
    /// <param name="returnTypeLocate">Расположение возвращаемого типа в исходном коде</param>
    public ManagedFunctionDeclaratorNode(string name, string moduleName,
      VariableDeclaratorNode[] functionParams, //Параметры функции
      LCObjectType returnType, //Возвращаемый тип
      BlockCodeNode body, //Тело функции
      LocateElement locateName, //Размещенеи имени функции в исходном коде
      LocateElement returnTypeLocate //Размещение возвращаемого типа в исходном коде
      )
      : base(name, moduleName, locateName, functionParams, returnType)
    {
      Body = body;

      for (int i = FunctionParams.Length - 1; i >= 0; i--)
      {
        //Класс переменной - параметр функции
        FunctionParams[i].ClassValue = ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam;

        //Добавляем декларатор параметра функции в тело
        Body.InsertChild(FunctionParams[i]);
      }

      AddChild(Body); //Добавляем тело функии в общее дерево
      ReturnTypeLocate = returnTypeLocate;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}()", ReturnType.ToString(), Name);
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  class NativeFunctionDeclaratorNode : FunctionDeclaratorNode
  {
    public ushort ID { get; private set; }

    /// <summary>
    /// Конструктор ноды нативной функции
    /// </summary>
    /// <param name="name">Имя функции</param>
    /// <param name="moduleName">Имя модуля, где размещена функция</param>
    /// <param name="functionParams">Параметры функции</param>
    /// <param name="returnType">Возвращаемый тип</param>
    /// <param name="id">ID нативной функции</param>
    public NativeFunctionDeclaratorNode(string name, string moduleName,
      VariableDeclaratorNode[] functionParams,
      LCObjectType returnType,
      ushort id)
        : base(name, moduleName, null, functionParams, returnType)
    {
      ID = id;
    }

    public override string ToString()
    {
      return string.Format("native {0} {1}@{2}", ReturnType.ToString(), Name, ID.ToString());
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  partial class VariableDeclaratorNode : ObjectDeclaratorNode
  {
    public string Attribute { get; set; }

    public VariableDeclaratorNode(LCObjectType objectType, string name, string moduleName,
      LCTypeLocate locateObjectType, LocateElement locateName) : base(objectType, name, moduleName, locateObjectType, locateName)
    {
      Attribute = null;
    }

  }

  class StructDeclaratorNode : UserTypeDeclaratorNode
  {
    /// <summary>
    /// Тип данных структура
    /// </summary>
    public LCStructDeclarator StructType { get; private set; }

    /// <summary>
    /// Расположение структуры в области кода
    /// </summary>
    public LCStructTypeLocate StructTypeLocate { get; private set; }

    public StructDeclaratorNode(string moduleName, LCStructDeclarator structType, LCStructTypeLocate structTypeLocate)
      : base(structType.TypeName, moduleName, structTypeLocate.Locate)
    {
      StructType = structType;
      StructTypeLocate = structTypeLocate;
    }

    public override bool SemanticCheck()
    {
      return true;
    }

    public override string ToString()
    {
      return "struct";
    }
  }

  /// <summary>
  /// Константное значение. Наследованное поле DType и поле ConstantValue.ValueType ссылаются на один и тот же объект.
  /// </summary>
  class ConstantValueNode : TypedNode, IExpressionMarker
  {
    public ConstantValue Constant { get; private set; }

    public ConstantValueNode(ConstantValue constant, LocateElement locate) : base(locate, locate)
    {
      Constant = constant;
      ObjectType = constant.ObjectType;
    }

    public override string ToString()
    {
      return string.Format("{0} ({1})", Constant.ToString(), Constant.ObjectType.ToString());
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода содержит идентификатор исходного кода на начальных этапах компиляции. 
  /// Если данная нода описывает вызов функции, то в качестве потомков будут выступать аргументы данной функции.
  /// </summary>
  class TerminalIdentifierNode : SourceLocatedNode
  {
    public string TerminalID { get; private set; }

    public TerminalIdentifierNode(string terminalID, LocateElement locate) : base(locate, locate)
    {
      TerminalID = terminalID;
    }

    public override string ToString()
    {
      return TerminalID;
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Типизированный объект
  /// </summary>
  class ObjectNode : TypedNode, IExpressionMarker
  {
    public VariableDeclaratorNode ObjDeclaratorNode { get; private set; }

    /// <param name="declarator">Ссылка на декларатор, который объявляет объект</param>
    /// <param name="locate">Размещение терминала объекта в исходном коде</param>
    public ObjectNode(VariableDeclaratorNode declarator, LocateElement locate) : base(locate, locate)
    {
      ObjDeclaratorNode = declarator;
      ObjectType = ObjDeclaratorNode.ObjectType;
    }

    public override string ToString()
    {
      return string.Format("Obj: {0} {1}", ObjDeclaratorNode.ObjectType.ToString(), ObjDeclaratorNode.Name);
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// При добавлении данной ноды в качестве потомка в другую ноду будут добавлены потомки данной ноды. 
  /// Это полезно в случае, если при построении дерева AST нужно вернуть несколько нод.
  /// </summary>
  class CollectionNode : HelperNode
  {
    public override bool SemanticCheck()
    {
      return true;
    }
  }

  /// <summary>
  /// Данная нода представляет поле оператора и используется как контейнер.
  /// </summary>
  class FieldNode : ContainerNode
  {
    string FieldNodeName = null;
    public FieldNode()
    {

    }

    public FieldNode(string fieldNodeName)
    {
      FieldNodeName = fieldNodeName;
    }

    public override string ToString()
    {
      if (FieldNodeName == null)
        return base.ToString();
      else
        return "Field: " + FieldNodeName;
    }

    public override bool SemanticCheck()
    {
      return true;
    }
  }
}
