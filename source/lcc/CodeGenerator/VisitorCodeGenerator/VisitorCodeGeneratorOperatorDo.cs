using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(DoNode n)
    {
      InsertComment(n);

      n.LabelDoBegin = GetLabelName();
      n.LabelDoCondition = GetLabelName();
      n.LabelDoEnd = GetLabelName();

      //Метка начала цикла
      var label = assemblyUnit.LabelManager.Declaration(n.LabelDoBegin);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "Label DoBegin"));

      //Тело цикла
      n.Body.Visit(this);

      //Условие
      InsertComment(n.StatementTxt + "-" + n.StatementDoWhileTxt);
      label = assemblyUnit.LabelManager.Declaration(n.LabelDoCondition);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "Label DoCondition"));

      GenerateDoCondition(n);

      //Точка выхода из цикла
      label = assemblyUnit.LabelManager.Declaration(n.LabelDoEnd);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "Label DoEnd"));
    }

    private void GenerateDoCondition(DoNode n)
    {
      TypedNode condition = (TypedNode)n.Condition.GetChild(0);

      //Генерация отладочной информации
      CheckContainExpressionNode(n, condition);
      //Если condition не содежит потомков типа Statement, 
      //то Statement начинается тут
      FullNodeStatementBegin(n);

      //Генерируем код для условия цикла
      condition.AccessMethod = ResultAccessMethod.MethodGet;
      condition.Visit(this);

      
      //Если condition содежит потомков типа Statement, 
      //то Statement начинается тут
      PartiallyNodeStatementBegin(n);

      //Генерируем код условия выхода из цикла
      assemblyUnit.AddInstruction(new INSTR_IFTRUE(assemblyUnit.LabelManager.AddReference(n.LabelDoBegin)));

      //Конец Statement-а
      StatementEnd(n);
    }
  }
}
