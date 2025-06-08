namespace LC2.LCCompiler.Compiler.Actions
{
  static internal class ActionExpressionEvaluator
  {
    /// <summary>
    /// Вычисляет выражение a + b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду PlusNode</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Summ(SummNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Summ(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a - b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду MinusNode</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Sub(SubNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Sub(right);
      });
    }



    /// <summary>
    /// Вычисляет выражение a * b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду MulNode</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Mul(MulNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Mul(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a / b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду DivNode</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Div(DivNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Div(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a % b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Rem(RemNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Rem(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a >> b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool RightShift(RightShiftNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.RightShift(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a << b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool LeftShift(LeftShiftNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.LeftShift(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a < b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Less(LessNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Less(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a <= b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool LessEqual(LessEqualNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.LessEqual(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a > b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool More(MoreNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.More(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a <= b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool MoreEqual(MoreEqualNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.MoreEqual(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a & b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool And(AndNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.And(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a ^ b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Xor(XorNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Xor(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a | b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Or(OrNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Or(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a && b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool LogicalAnd(LogicAndNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.LogicalAnd(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение a || b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool LogicalOr(LogicOrNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.LogicalOr(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение ~a если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Not(NotNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Not();
      });
    }

    /// <summary>
    /// Вычисляет выражение !a если a это ConstantValueNode, и имеет типы:
    /// LogicalType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool LogicalNot(LogicNotNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.LogicalNot();
      });
    }

    /// <summary>
    /// Вычисляет выражение -a если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Inv(NegativeNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Inv();
      });
    }


    /// <summary>
    /// Вычисляет выражение ++a если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool PrefixIncr(PrefixIncrementNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Incr();
      });
    }

    /// <summary>
    /// Вычисляет выражение --a если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool PrefixDecr(PrefixDecrementNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Decr();
      });
    }


    /// <summary>
    /// Вычисляет выражение a++ если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool PostfixIncr(PostfixIncrementNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Incr();
      });
    }

    /// <summary>
    /// Вычисляет выражение a-- если a это ConstantValueNode, и имеет типы:
    /// IntegerType
    /// FloatType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool PostfixDecr(PostfixDecrementNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return operand.Decr();
      });
    }

    /// <summary>
    /// Вычисляет выражение a == b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// CharType
    /// LogicalType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Eq(EqualNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Eq(right);
      });
    }

    /// <summary>
    /// Вычисляет выражение a != b если a, b это ConstantValueNode, и имеют типы:
    /// IntegerType
    /// FloatType
    /// CharType
    /// LogicalType
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool Neq(NotEqualNode n, CompilerLogger logger)
    {
      return BinaryOperationAnalysis(n, delegate (ConstantValue left, ConstantValue right, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        return left.Neq(right);
      });
    }


    /// <summary>
    /// Вычисляет выражение (type)a если a это ConstantValueNode, и имеет типы:
    /// 
    /// 
    /// </summary>
    /// <param name="n">Ссылка на ноду операции<</param>
    /// <param name="logger">Ссылка на логгер ошибок компиляции</param>
    /// <returns>true - вычисление выполнено, false - вычисление не выполнено</returns>
    static public bool TypeCast(TypeCastNode n, CompilerLogger logger)
    {
      return UnaryOperationAnalysis(n, delegate (ConstantValue operand, out string name)
      {
        name = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //Если попали сюда, то operand является константным значением,
        //и все сематнические проверки прошли успешно.
        var type = n.ObjectType.Type;

        if (type is LCPrimitiveType primitiveType)
          return operand.TypeConvert(primitiveType);
        else
          return null;
      });
    }


    #region Вспомогательные методы

    delegate ConstantValue BinaryOperatorSpecificAnalysisDelegate(ConstantValue left, ConstantValue right, out string name);
    static bool BinaryOperationAnalysis(BinaryOperationNode n, BinaryOperatorSpecificAnalysisDelegate specificAnalysisCallBack)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      //Берем правый и левый операнды
      TypedNode Left = n.GetOperandLeft();
      TypedNode Right = n.GetOperandRight();

      if ((Left == null) || (Right == null))
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      if (Left.SemanticallyCorrect == false || Right.SemanticallyCorrect == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }
      
      //Если оба операнда константные значения
      if ((Left is ConstantValueNode leftConstantNode) && (Right is ConstantValueNode rightConstantNode))
      {
        //Получаем значения левого и правого операндов
        ConstantValue leftValue = leftConstantNode.Constant;
        ConstantValue rightValue = rightConstantNode.Constant;

        string name;

        //Выполняем вычисление
        ConstantValue r = specificAnalysisCallBack(leftValue, rightValue, out name);

        //Если вычисление выполнилось с ошибкой
        if (r == null) throw new InternalCompilerException(
            string.Format("Ошибка при выполнении вычисления выражения в методе \"{0}\". Операнд \"{1}\", расположение {2}",
              name, n.Description(), n.Locate.ToString()));

        ConstantValueNode valueNode = new ConstantValueNode(r, 
          new LocateElement(leftConstantNode.Locate, n.Locate, rightConstantNode.Locate));
        n.Replace(valueNode);
      }

      return true;
    }


    delegate ConstantValue UnaryOperatorSpecificAnalysisDelegate(ConstantValue operand, out string name);
    static bool UnaryOperationAnalysis(UnaryOperationNode n, UnaryOperatorSpecificAnalysisDelegate specificAnalysisCallBack)
    {
      if (n.SemanticallyCorrect == false)
        return false;

      TypedNode Operand = n.GetOperand();

      if (Operand == null)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      if(Operand.SemanticallyCorrect == false)
      {
        n.SemanticallyCorrect = false;
        return false;
      }

      //Если оба операнда константные значения
      if (Operand is ConstantValueNode operandConstantValue)
      {
        ConstantValue valueBase = operandConstantValue.Constant;

        string name;

        ConstantValue r = specificAnalysisCallBack(valueBase, out name);

        if (r == null) throw new InternalCompilerException(
            string.Format("Ошибка при выполнении вычисления выражения в методе \"{0}\". Операнд \"{1}\", расположение {2}",
              name, n.Description(), n.Locate.ToString()));

        ConstantValueNode valueNode = new ConstantValueNode(r, 
          new LocateElement(n.Locate, Operand.Locate));
        n.Replace(valueNode);

      }

      return true;
    }

    #endregion

  }
}
