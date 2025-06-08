using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  static class TreeMISCWorkers
  {
    /// <summary>
    /// Проверяет цепочку предков ноды node по шаблону comparePattern.
    /// Шаблон comparePattern - двумерный массив, который обладает следующей структурой:
    /// Type[][] pattern2 = new Type[][] 
    /// { 
    ///   new Type[] { typeof(Node), typeof(Node), typeof(Node), .... },  //Возможные варианты 1-го предка
    ///   new Type[] { typeof(Node), typeof(Node), ... }, //Возможные варианты 2-го предка
    ///   new Type[] { typeof(Node), .... } //Возможные варианты 3-го предка
    ///   .... //и так далее
    /// };
    /// 
    /// Если дерево предков соответствует шаблону, то возвращает 0, иначе, возвращает число, 
    /// значение которого является номером варианта, на котором произошла ошибка, т.е.
    /// если не совпал шаблон возможных вариантов 2-го предка, то вернет 2.
    /// Если во время анализа дерева предков упермлись в ноду-ограничитель Limiter, то вернет -1.
    /// </summary>
    /// <param name="comparePattern">Шаблон для сравнения</param>
    /// <param name="Limiter">Ограничитель поиска</param>
    /// <param name="node">Нода, относительно которой выполняется поиск</param>
    /// <returns></returns>
    public static int UpCompare(Type[][] comparePattern, Type Limiter, Node node)
    {
      int numParants = comparePattern.Length;

      Node currNode = node;

      for (int i = 0; i < numParants; i++)
      {
        currNode = currNode.Parent;
        Type currNodeType = currNode.GetType();

        //Проверка ограничителей
        if (currNode == null)
          return -1;
        if (currNodeType == Limiter)
          return -1;

        Type[] allowedTypes = comparePattern[i];

        //Если тип текущей ноды не совпадает
        //ни с одним из типов из списка,
        //то это ошибка
        if (CheckNodeType(allowedTypes, currNodeType) == false)
          return i + 1;
      }

      //Если попали сюда, то все ОК
      return 0;
    }


    /// <summary>
    /// То же самое, что и UpCompare, только в случае успеха возвращает найденную ноду
    /// </summary>
    public static Node GetNodeUpPach(Type[][] comparePattern, Type Limiter, Node node)
    {
      int numParants = comparePattern.Length;

      Node currNode = node;

      for (int i = 0; i < numParants; i++)
      {
        currNode = currNode.Parent;
        Type currNodeType = currNode.GetType();

        //Проверка ограничителей
        if (currNode == null)
          return null;
        if (currNodeType == Limiter)
          return null;

        Type[] allowedTypes = comparePattern[i];

        //Если тип текущей ноды не совпадает
        //ни с одним из типов из списка,
        //то это ошибка
        if (CheckNodeType(allowedTypes, currNodeType) == false)
          return null;
      }

      //Если попали сюда, то все ОК
      return currNode;
    }

    /// <summary>
    /// Принадлежит ли тип type списку типов allowedTypesList
    /// </summary>
    /// <param name="allowedTypesList">Список допустимых типов</param>
    /// <param name="type">проверяемый тип</param>
    /// <returns>true - type входит в список allowedTypesList, иначе false</returns>
    static public bool CheckNodeType(Type[] allowedTypesList, Type type)
    {
      foreach (Type t in allowedTypesList)
        if (type == t)
          return true;

      return false;
    }

    /// <summary>
    /// Поиск вверх по дереву ноды определенного типа.
    /// Возвращает первую попавшуюся ноду, 
    /// удовлетворяющую условиям поиска
    /// </summary>
    /// <param name="findNode">Типы нод для поиска</param>
    /// <param name="Limiter">Тип, выше которого поиск не производится</param>
    /// <param name="node">Ссылка на узел AST, относительно которого производится поиск</param>
    /// <returns>Найденная нода в случае успеха, null в случе если нода не найдена</returns>
    public static Node UpFind(Type[] findNode, Type Limiter, Node node)
    {
      Node currNode = node;

      while (true)
      {
        if (currNode == null)
          return null;

        foreach (Type f in findNode)
          if (f == currNode.GetType())
            return currNode;

        if (Limiter == currNode.GetType())
          return null;

        currNode = currNode.Parent;
      }
    }

    /// <summary>
    /// Поиск вверх по дереву ноды определенного типа.
    /// Возвращает первую попавшуюся ноду, 
    /// удовлетворяющую условию поиска
    /// </summary>
    /// <param name="findNode">Тип ноды для поиска</param>
    /// <param name="Limiter">Тип, выше которого поиск не производится</param>
    /// <param name="node">Ссылка на узел AST, относительно которого производится поиск</param>
    /// <returns>Найденная нода в случае успеха, null в случе если нода не найдена</returns>
    public static Node UpFind(Type findNode, Type Limiter, Node node)
    {
      return UpFind(new Type[] { findNode }, Limiter, node);
    }

    /// <summary>
    /// Выполняет поиск объекта symbolName в области видимости, относительно node,
    /// и возвращает его декларатор
    /// </summary>
    /// <param name="symbolName">Имя искомого символа</param>
    /// <param name="node">Нода, относительно которой выполняется поиск</param>
    /// <returns>DragonDeclarator - в случае успеха, null - если символ не найден</returns>
    public static VariableDeclaratorNode FindVariableDeclarator(string symbolName, Node node)
    {
      Node currNode = node;

      while (true)
      {
        var declarator = FindVariableDeclaratorInCurrentScope(symbolName, currNode);

        //Если что-то нашли, то возвращает это
        if (declarator != null)
          return declarator;

        //Если ничего не нашли, то переходим к предку
        currNode = currNode.Parent;

        //Если дошли до корня
        //то выходим
        if (currNode == null)
          return null;
      }
    }

    /// <summary>
    /// Поиск символа symbolName среди подключенных модулей
    /// </summary>
    /// <param name="symbolName">Имя искомого символа</param>
    /// <param name="uses">Подключенные модули</param>
    /// <returns>Декларатор, null - если символ не найден</returns>
    public static VariableDeclaratorNode FindSymbolInUses(string symbolName, UseDirectives uses)
    {
      foreach (var u in uses.UseModule)
      {
        if (u.Table == null)
          continue;

        foreach (var d in u.Table.Declarators)
          if (d is VariableDeclaratorNode objectDeclarator 
            && objectDeclarator.Name == symbolName)
            return objectDeclarator;
      }

      return null;
    }

    /// <summary>
    /// Вроверяет, объявлен ли данный символ в текущей области видимости
    /// </summary>
    /// <param name="symbolName"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    static public bool DeclaredInCurrentScope(DeclaratorNode node)
    {
      var parent = node.Parent;

      if (parent is ModuleRootNode)
      {
        for (int i = 0; i < parent.CountChildrens; i++)
        {
          //Текущую ноду не сравиниваем саму с собой
          if (i == node.ChildrenIndex)
            continue;

          if (parent.GetChild(i) is DeclaratorNode declaratorNode)
          {
            //Если имя декларатора совпадает с искомым символом
            if (declaratorNode.Name == node.Name)
              return true;
          }
        }
      }
      else
      {
        //Пробегаемся по всем сестрам данной ноды, справа налево
        for (int i = node.ChildrenIndex - 1; i >= 0; i--)
        {
          //Если нашли какой-то декларатор
          if (parent.GetChild(i) is DeclaratorNode declaratorNode)
          {
            //Если имя декларатора совпадает с искомым символом
            if (declaratorNode.Name == node.Name)
              return true;
          }
        }
      }

      return false;
    }

    static public bool FunctionIsDeclared(FunctionDeclaratorNode node)
    {
      ModuleRootNode root = (ModuleRootNode)node.Parent;
      for (int i = 0; i < root.CountChildrens; i++)
      {
        //Текущую ноду не сравиниваем саму с собой
        if (i == node.ChildrenIndex)
          continue;

        if (root.GetChild(i) is DeclaratorNode declaratorNode)
        {
          //Если имя декларатора совпадает с искомым символом
          if (declaratorNode.Name == node.Name)
            return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Поиск симпола в текущей области видимости
    /// </summary>
    /// <param name="symbolName">Искомое имя символа</param>
    /// <param name="node">Нода, выше которой выполнять поиск</param>
    /// <returns></returns>
    static VariableDeclaratorNode FindVariableDeclaratorInCurrentScope(string symbolName, Node node)
    {
      int nodeID = node.ChildrenIndex;
      var parent = node.Parent;

      //Если node это уже корень, то выходим
      if (parent == null)
        return null;

      //В корне ищем по всем элементам
      if (parent is ModuleRootNode)
      {
        for (int i = 0; i < parent.CountChildrens; i++)
        {
          if (parent.GetChild(i) is DeclaratorNode declaratorNode)
          {
            //Если имя декларатора совпадает с искомым символом
            if (declaratorNode is VariableDeclaratorNode objectDeclarator
              && objectDeclarator.Name == symbolName)
              return objectDeclarator;
          }
        }
      }
      else
      {
        //Пробегаемся по всем сестрам данной ноды, справа налево
        for (int i = nodeID - 1; i >= 0; i--)
        {
          //Если нашли какой-то декларатор
          if (parent.GetChild(i) is DeclaratorNode declaratorNode)
          {
            //Если имя декларатора совпадает с искомым символом
            if (declaratorNode is VariableDeclaratorNode objectDeclarator
              && objectDeclarator.Name == symbolName)
              return objectDeclarator;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Вставка приведения типа 
    /// </summary>
    /// <param name="t">Тип, к которому выполняется приведение</param>
    /// <param name="n">Нода, перед которой вставляется TypeCast</param>
    public static void InsertTypeCast(LCPrimitiveType t, TypedNode n, CompilerLogger logger)
    {
      if (LCTypesUtils.IsEqual(t, n.ObjectType.Type))
        return;

      TypeCastNode c = new TypeCastNode((LCPrimitiveType)t.Clone());
      c.OperandsCType = n.ObjectType.Type.Clone();
      c.ObjectType = new LCObjectType((LCPrimitiveType)t.Clone());
      c.ObjectType.SetAccessAttributes(true, false);

      n.Parent.ReplaceChild(n, c);
      c.AddChild(n);

      //После того, как выполнили вставку TypeCast-а,
      //пытаемся вычислить значение ветки дерева
      //Actions.ActionExpressionEvaluator.TypeCast(c, logger);
    }

    public static void InsertTypeCast(LCPrimitiveType t, TypedNode n)
    {
      InsertTypeCast(t, n, null);
    }

    /// <summary>
    /// Выполняет поиск таблицы символов модуля по его пути
    /// </summary>
    /// <param name="symbols">Таблицы символов</param>
    /// <param name="modulePath">Путь к искомому модулю</param>
    /// <returns>Найденная таблица, null если таблица не найдена</returns>
    public static SymbolTable FindSymbolTable(SymbolTable[] symbols, string modulePath)
    {
      foreach (var t in symbols)
      {
        if (t.ModuleName == modulePath)
          return t;
      }

      return null;
    }

    internal static FunctionDeclaratorNode FindFunctionDeclarator(Node node, string functionName)
    {
      var root = node.ModuleRoot;

      //Ищем декларатор у себя в модуле
      for (int i = 0; i < root.CountChildrens; i++)
      {
        if (root.GetChild(i) is FunctionDeclaratorNode n)
        {
          if (n.Name == functionName)
            return n;
        }
      }

      return null;
    }

    /// <summary>
    /// Найти декларатор пользовательского типа
    /// </summary>
    internal static UserTypeDeclaratorNode FindUserType(Node node, string typeName)
    {
      var root = node.ModuleRoot;

      //Ищем декларатор у себя в модуле
      for (int i = 0; i < root.CountChildrens; i++)
      {
        if (root.GetChild(i) is UserTypeDeclaratorNode n)
        {
          if (n.Name == typeName)
            return n;
        }
      }

      return null;
    }
  }
}
