using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(WhileNode n)
    {
      InsertComment(n);

      n.LabelWhileBegin = GetLabelName();
      n.LabelWhileEnd = GetLabelName();

      //Метка начала цикла
      var label = assemblyUnit.LabelManager.Declaration(n.LabelWhileBegin);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "Label WhileBegin (for 'continue' operator)"));

      //Условие оператора While
      GenerateWhileCondition(n);

      //Тело цикла
      n.Body.Visit(this);

      //Переходим в начало цикла
      assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.LabelWhileBegin)));

      //Точка выхода из цикла
      label = assemblyUnit.LabelManager.Declaration(n.LabelWhileEnd);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label, "Label WhileEnd (for 'break' operator)"));

    }

    private void GenerateWhileCondition(WhileNode n)
    {
      TypedNode condition = (TypedNode)n.Condition.GetChild(0);

      //Генерация отладочной информации
      CheckContainExpressionNode(n, condition);
      //Если condition не содежит потомков типа Statement, 
      //то Statement начинается тут
      FullNodeStatementBegin(n);

      condition.AccessMethod = ResultAccessMethod.MethodGet;
      condition.Visit(this);

      //Если condition содежит потомков типа Statement, 
      //то Statement начинается тут
      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_IFFALSE(assemblyUnit.LabelManager.AddReference(n.LabelWhileEnd)));

      //Конец Statement-а
      StatementEnd(condition);
    }
  }
}
