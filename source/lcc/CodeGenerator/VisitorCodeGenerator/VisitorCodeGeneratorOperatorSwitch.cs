using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(SwitchNode n)
    {
      InsertComment(n);

      //Генерируем метку выхода из цикла
      n.LabelSwitchEnd = GetLabelName();

      //Вычисляем Expression оператора
      TypedNode expression = (TypedNode)n.Expression.GetChild(0);
      expression.AccessMethod = ResultAccessMethod.MethodGet;

      //Генерация отладочной информации
      CheckContainExpressionNode(n, expression);

      //Если выражение оператора switch() не содержит потомков, типа Condition,
      //то начало Statement будет тут
      FullNodeStatementBegin(n);

      //Генерируем код выражения
      expression.Visit(this);

      //Генерация кода перехода к метке оператора switch()
      GenerateSwitchGotoLabel(n);

      //Генерируем код тела оператора
      n.Body.Visit(this);

      //Точка выхода
      //Тут можно добавить микроразметку на закрывающуюся скобку оператора switch() { }
      var labelSwitchEnd = assemblyUnit.LabelManager.Declaration(n.LabelSwitchEnd);
      assemblyUnit.AddInstruction(new INSTR_LABEL(labelSwitchEnd));

      var argumentSize = n.ExpressionType.Sizeof();
      switch (argumentSize)
      {
        case 1:
        case 2:
        case 4:
          assemblyUnit.AddInstruction(new INSTR_DROP());
          break;

        case 8:
          assemblyUnit.AddInstruction(new INSTR_DROP_2());
          break;

        default:
          throw new InternalCompilerException("Неверный тип данных");
      }

    }

    private void GenerateSwitchGotoLabel(SwitchNode n)
    {
      //Переход к нужной метке пока реализован на if-ах,
      //в дальнейшем можно добавить переход табличным методом

      //Если условие оператора if() содержит потомков, типа Condition,
      //то начало Statement будет тут

      var argumentSize = n.ExpressionType.Sizeof();

      PartiallyNodeStatementBegin(n);

      foreach (var c in n.LabelsCase)
      {
        //Генерируем имя метки для данного case-а
        string label = GetLabelName();

        //В дальнейшем этот label вставит в код эту метку
        c.LabelCaseEntry = label;

        //Дублируем значение на стеке, это сделано для того, 
        //чтобы сохранить это значение для следующих сравнений
        switch(argumentSize)
        {
          case 1:
          case 2:
          case 4:
            assemblyUnit.AddInstruction(new INSTR_DUP());
            break;

          case 8:
            assemblyUnit.AddInstruction(new INSTR_DUP_2());
            break;

          default:
            throw new InternalCompilerException("Неверный тип данных");
        }
        

        //Вставляем значение метки case, с которым выполняется сравнение


        var caseConstant = c.Constant;

        if (caseConstant is SByteConstantValue sByteConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new[] { (byte)sByteConstant.Value }));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_SByte));
        }
        else if (caseConstant is ShortConstantValue shortConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_2(BitConverter.GetBytes(shortConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_Short));
        }
        else if (caseConstant is IntConstantValue intConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(intConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_Int));
        }
        else if (caseConstant is LongConstantValue longConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_8(BitConverter.GetBytes(longConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_Long));
        }
        else if (caseConstant is ByteConstantValue byteConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_1(new[] { byteConstant.Value }));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_Byte));
        }
        else if (caseConstant is UShortConstantValue ushortConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_2(BitConverter.GetBytes(ushortConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_UShort));
        }
        else if (caseConstant is UIntConstantValue uintConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_4(BitConverter.GetBytes(uintConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_UInt));
        }
        else if (caseConstant is ULongConstantValue ulongConstant)
        {
          assemblyUnit.AddInstruction(new INSTR_PUSH_8(BitConverter.GetBytes(ulongConstant.Value)));
          assemblyUnit.AddInstruction(new INSTR_EQ(LCVM_DataTypes.Type_ULong));
        }
        else
          throw new InternalCompilerException("Неверный тип агрумента оператора switch");

        //Добавляем условный переход на метку
        assemblyUnit.AddInstruction(new INSTR_IFTRUE(assemblyUnit.LabelManager.AddReference(label)));
      }

      //Генерируем код для default
      if (n.LabelDefault != null)
      {
        string label = GetLabelName();
        n.LabelDefault.LabelDefaultEntry = label;
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(label)));
      }
      else //Если default значения нет, то выходим, если ничего не подошло
      {
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.LabelSwitchEnd)));
      }

      StatementEnd(n);
    }

  }
}
