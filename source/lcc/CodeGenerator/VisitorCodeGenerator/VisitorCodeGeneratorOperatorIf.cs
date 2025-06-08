using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(IfNode n)
    {
      InsertComment(n);

      TypedNode condition;

      if (n.Condition.CountChildrens == 0)
        throw new InternalCompilerException("Поле Condition ноды IfNode должно содержать одного потомка типа TypedNode");
      if (n.Condition.GetChild(0) is TypedNode typedNode)
        condition = typedNode;
      else
        throw new InternalCompilerException("Поле Condition ноды IfNode должно содержать одного потомка типа TypedNode");

      bool TrueBodyIsExists = (n.TrueBody.CountChildrens != 0);
      bool FalseBodyIsExists = (n.FalseBody.CountChildrens != 0);

      //Если нет обоих веток оператора if,
      //то просто выполняем обход условия на случай, если там есть
      //то-то полезное
      if ((TrueBodyIsExists == false) && (FalseBodyIsExists == false))
      {
        condition.AccessMethod = ResultAccessMethod.MethodUnuse;
        condition.Visit(this);
        return;
      }

      //Если попали сюда, то значение условия оператора if нам понадобится
      //VisitorCodeGenerate.codeBuilder.AddInstruction(new DA_COMMENT("if condition"));
      condition.AccessMethod = ResultAccessMethod.MethodGet;

      //Генерация отладочной информации
      CheckContainExpressionNode(n, condition);

      //Если условие оператора if() не содержит потомков, типа Condition,
      //то начало Statement будет тут
      FullNodeStatementBegin(n);

      //Посещаем условие оператора
      condition.Visit(this);

      //Если есть только ветка истинного значения
      if ((TrueBodyIsExists == true) && (FalseBodyIsExists == false))
      {
        OperatorIfTrueBranchOnly(condition, n);
        return;
      }

      //Если есть только ветка ложного значения
      if ((TrueBodyIsExists == false) && (FalseBodyIsExists == true))
      {
        OperatorIfFalseBranchOnly(condition, n);
        return;
      }

      //Если есть обе ветки
      if ((TrueBodyIsExists == true) && (FalseBodyIsExists == true))
      {
        OperatorIfBothBranches(condition, n);
        return;
      }
    }

    private void OperatorIfTrueBranchOnly(TypedNode condition, IfNode n)
    {
      string labelIFExit = GetLabelName();

      //Если условие оператора if() содержит потомков, типа Condition,
      //то начало Statement будет тут
      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_IFFALSE(assemblyUnit.LabelManager.AddReference(labelIFExit)));

      StatementEnd(n);

      //Посещаем ветку True
      n.TrueBody.Visit(this);

      //Выходная метка
      assemblyUnit.AddInstruction(new LCVMComment(""));
      var label = assemblyUnit.LabelManager.Declaration(labelIFExit);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "if-exit"));

    }

    private void OperatorIfFalseBranchOnly(TypedNode condition, IfNode n)
    {
      string labelIFExit = GetLabelName();

      //Если условие оператора if() содержит потомков, типа Condition,
      //то начало Statement будет тут
      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_IFTRUE(assemblyUnit.LabelManager.AddReference(labelIFExit)));

      StatementEnd(n);

      //Посещаем ветку False
      n.FalseBody.Visit(this);

      //Выходная метка
      assemblyUnit.AddInstruction(new LCVMComment(""));
      var label = assemblyUnit.LabelManager.Declaration(labelIFExit);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "if-exit"));

    }

    private void OperatorIfBothBranches(TypedNode condition, IfNode n)
    {
      string labelIFFalse = GetLabelName();
      string labelIFExit = GetLabelName();

      //Если условие оператора if() содержит потомков, типа Condition,
      //то начало Statement будет тут
      PartiallyNodeStatementBegin(n);

      //Если ложно, то переходим к ветке ложного значения
      assemblyUnit.AddInstruction(new INSTR_IFFALSE(assemblyUnit.LabelManager.AddReference(labelIFFalse)));

      StatementEnd(n);

      //Посещаем ветку True
      n.TrueBody.Visit(this);
      //После выполнения ветки True перепрыгиваем ветку False
      assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(labelIFExit)));

      //Метка ветки False
      assemblyUnit.AddInstruction(new LCVMComment(""));
      var label = assemblyUnit.LabelManager.Declaration(labelIFFalse);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "if-false"));
      //Посещаем ветку False
      n.FalseBody.Visit(this);

      //Выходная метка
      assemblyUnit.AddInstruction(new LCVMComment(""));
      label = assemblyUnit.LabelManager.Declaration(labelIFExit);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "if-exit"));

    }
  }
}
