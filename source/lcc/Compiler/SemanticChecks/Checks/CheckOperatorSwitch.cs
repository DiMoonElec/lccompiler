using System.Collections.Generic;
using LC2.LCCompiler.Compiler.LCTypes;
using static LC2.LCCompiler.Compiler.LCTypesUtils;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  internal static class CheckOperatorSwitch
  {
    private class CheckDuplicateCaseValue
    {
      public byte[] Dump;
      public LocateElement Locate;
      public string Txt;

      public bool Compare(CheckDuplicateCaseValue e)
      {
        if (e.Dump.Length != Dump.Length)
          throw new InternalCompilerException("Неверный тип данных");

        for (int i = 0; i < Dump.Length; i++)
          if (Dump[i] != e.Dump[i])
            return false;

        return true;
      }
    }

    internal static bool CheckSwitch(SwitchNode n, CompilerLogger logger)
    {
      bool isOK = true;

      var ex = n.Expression.GetChild(0);

      LCPrimitiveType exprType = null;

      LCPrimitiveType.PrimitiveTypes[] validTypes
        = LCTypesUtils.GetGroupList(
          GroupPrimitiveTypes.ClassInt
        | GroupPrimitiveTypes.ClassUInt);

      //Тип expression должно быть целое число
      if (ex.SemanticallyCorrect == false)
      {
        //Если expression содержит семантические ошибки
        //то далее его не проверяем
        isOK = false;
      }
      else if ((ex is TypedNode expression)
        && (expression.ObjectType.Type is LCPrimitiveType primitiveType)
        && LCTypesUtils.ItIsFromList(primitiveType, validTypes))
      {
        exprType = primitiveType;
        n.ExpressionType = exprType;
      }
      else
      {
        string msg = "В качестве выражения оператора \"switch\" могут использоваться типы: {0}";

        logger.Error(n.Locate, string.Format(msg,
          LCTypesUtils.PrimitiveTypeGetName(validTypes)));

        isOK = false;
      }

      //Проверяем корректность тела оператора switch

      if (CheckSwitchBody(n, exprType, logger) == false)
        isOK = false;


      if (CheckDuplicateCaseValues(n, logger) == false)
        isOK = false;
      /*
      //Проверка на одинаковые константы case-ов
      for (int i = 0; i < n.LabelsCase.Length; i++)
      {
        if (n.LabelsCase[i].Constant == null)
          continue;

        long c = n.LabelsCase[i].Constant.Value;

        for (int j = i + 1; j < n.LabelsCase.Length; j++)
        {
          var label = n.LabelsCase[j];

          if (label.Constant == null)
            continue;

          if (label.Constant.Value == c)
          {
            logger.Error(label.Locate, string.Format("Метка \"case {0}\" уже сущестует в данном операторе \"switch\"", c.ToString()));
            isOK = false;
          }
        }
      }
      */

      if (isOK == false)
        n.SemanticallyCorrect = false;

      return isOK;
    }

    private static bool CheckDuplicateCaseValues(SwitchNode n, CompilerLogger logger)
    {
      bool isOK = true;
      CheckDuplicateCaseValue[] elements
        = new CheckDuplicateCaseValue[n.LabelsCase.Length];

      //Добавляем все элементы
      for (int i = 0; i < n.LabelsCase.Length; i++)
      {
        CheckDuplicateCaseValue e = new CheckDuplicateCaseValue();
        e.Dump = n.LabelsCase[i].Constant.Dump;
        e.Txt = n.LabelsCase[i].Constant.ToString();
        e.Locate = n.LabelsCase[i].Locate;

        elements[i] = e;
      }

      //Проверка на одинаковые константы case-ов
      for (int i = 0; i < elements.Length; i++)
      {
        var currentElement = elements[i];

        for (int j = i + 1; j < elements.Length; j++)
        {
          var compareElement = elements[j];

          if (currentElement.Compare(compareElement))
          {
            logger.Error(compareElement.Locate,
              string.Format("Метка \"case {0}\" уже сущестует в данном операторе \"switch\"", compareElement.Txt));
            isOK = false;
          }
        }
      }

      return isOK;
    }

    private static bool CheckSwitchBody(SwitchNode n, LCPrimitiveType switchExpressionType, CompilerLogger logger)
    {
      int state = 0;
      List<LabelCaseNode> labelsCase = new List<LabelCaseNode>();

      bool isOk = true;

      var body = n.Body.GetChild(0);
      int i = 0;
      int numChilds = body.CountChildrens;

      if (numChilds == 0)
      {
        logger.Error(n.Locate, "Пустой блок \"switch\"");
        return false;
      }

      Node c;
      bool run = true;

      while (run)
      {
        switch (state)
        {
          case 0:
            {
              c = body.GetChild(i++);

              if (c is LabelCaseNode caseNode)
              {
                labelsCase.Add(caseNode);
                if (CheckSwitchCase(caseNode, switchExpressionType, logger) == false)
                  isOk = false;
                state = 1;
              }
              else if (c is LabelDefaultNode defaultNode)
              {
                n.LabelDefault = defaultNode;
                state = 2;
              }
              else
              {
                logger.Error(n.Locate, "Первым элементом тела \"switch\" должен быть \"case\" или \"default\"");
                isOk = false;
                state = 1;
              }
            }
            break;
          ///////////////////////////////////////////
          case 1:
            {
              //Поиск очередного case или default

              //Если ноды закончились, то выходим
              if (i >= numChilds)
              {
                run = false;
                break;
              }

              c = body.GetChild(i++);
              if (c is LabelCaseNode caseNode1)
              {
                labelsCase.Add(caseNode1);
                if (CheckSwitchCase(caseNode1, switchExpressionType, logger) == false)
                  isOk = false;
              }
              else if (c is LabelDefaultNode defaultNode)
              {
                n.LabelDefault = defaultNode;
                state = 2;
              }
            }
            break;
          ///////////////////////////////////////////
          case 2:
            {
              //Ранее встретили ноду default, поэтому проверяем,
              //нет ли нод case после default

              //Если ноды закончились, то выходим
              if (i >= numChilds)
              {
                run = false;
                break;
              }

              c = body.GetChild(i++);
              if (c is LabelCaseNode caseNode)
              {
                labelsCase.Add(caseNode);
                logger.Error(caseNode.Locate, "Оператор \"case\" следует после \"default\"");
                isOk = false;
              }
              else if (c is LabelDefaultNode defaultNode)
              {
                logger.Error(defaultNode.Locate, "\"switch\" содержит несколько операторов \"default\"");
                isOk = false;
              }
            }
            break;
            ///////////////////////////////////////////
            ///////////////////////////////////////////
        }
      }
      n.LabelsCase = labelsCase.ToArray();
      return isOk;
    }

    /// <summary>
    /// Проверка аргумента оператора switch
    /// </summary>
    /// <param name="n">Ссылка на оператор switch</param>
    /// <param name="exprType">Тип данных оператора case</param>
    /// <param name="logger">Ссылка на логгер</param>
    /// <returns>true - ошибок не найдено, false - обнаружены семантические ошибки</returns>
    private static bool CheckSwitchCase(LabelCaseNode n, LCPrimitiveType exprType, CompilerLogger logger)
    {
      if (exprType == null)
        return false;

      //Берем ноду
      TypedNode expressionNode = (TypedNode)n.GetChild(0);

      if (expressionNode is ConstantValueNode constantValueNode)
      {
        ConstantValue castConstant;
        var overrange = ConstantTypeCast.Cast(constantValueNode.Constant, exprType, out castConstant);

        if (castConstant != constantValueNode.Constant)
        {
          ConstantValueNode c = new ConstantValueNode(castConstant, constantValueNode.Locate);
          expressionNode.Replace(c);

          //Обновляем ссылку
          expressionNode = (TypedNode)n.GetChild(0);
        }

        n.Constant = castConstant;

        if (overrange)
        {
          logger.Error(expressionNode.Locate, string.Format("Невозможно неявно данное число привести к типу '{0}'", exprType.ToString()));
          return false;
        }

        return true;
      }
      else
      {
        logger.Error(n.Locate, "Аргументом оператора 'case' должно быть число");
        return false;
      }


      /*
      if ((expressionNode is ConstantValueNode constantValueNode)
        && (constantValueNode.Constant is IntConstantValue integerConstantValue))
      {
        var c = integerConstantValue;
        if (c != null)
        {
          expressionNode.Replace(new ConstantValueNode(c, constantValueNode.Locate));
          n.Constant = c;
          return true;
        }
        else
        {
          logger.Error(constantValueNode.Locate,
            string.Format("Невозможно число \"{0}\" привести к типу \"{1}\"",
            constantValueNode.Constant.ToString(),
            exprType.ToString()));
          return false;
        }
      }
      else
      {
        logger.Error(n.Locate, "В качестве значения метки \"case\" должно выступать целое число");
        return false;
      }
      */
    }
  }
}
