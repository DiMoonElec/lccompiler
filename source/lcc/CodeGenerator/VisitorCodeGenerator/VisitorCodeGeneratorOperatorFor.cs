using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {
    public override void Visit(ForNode n)
    {
      InsertComment(n);

      //Генерируем имена меток начала и конца цикла for
      n.LabelForBegin = GetLabelName();
      n.LabelForEnd = GetLabelName();

      //Начало цикла
      var label = assemblyUnit.LabelManager.Declaration(n.LabelForBegin);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label));

      //Генерируем код для условия оператора for
      GenerateForCondition(n);

      //Генерируем код тела оператора
      n.Body.Visit(this);

      //Генерируем код итератора (loop)
      GenerateForIterator(n);

      //Добавляем переход в начало цикла
      //При желании тут можно добавить микроразметку на закрывающуюся скобку оператора for(;;) {}
      assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.LabelForBegin)));

      //Добавляем метку выхода из цикла
      label = assemblyUnit.LabelManager.Declaration(n.LabelForEnd);
      assemblyUnit.AddInstruction(new INSTR_LABEL(label));
    }

    private void GenerateForCondition(ForNode n)
    {
      //Если условие выхода из цикла присутствует
      if (n.Condition.CountChildrens != 0)
      {
        TypedNode condition = (TypedNode)n.Condition.GetChild(0);
        condition.AccessMethod = ResultAccessMethod.MethodGet;

        //Генерация отладочной информации
        if (ContainStatementNode(condition))
        {
          n.GenerateDebugInfo = GenerateDebugValue.PartiallyExpression;
        }
        else
        {
          n.GenerateDebugInfo = GenerateDebugValue.FullExpression;
          StatementBegin(condition);
        }

        //Генерируем код
        condition.Visit(this);

        //Генерация отладочной информации
        if (n.GenerateDebugInfo == GenerateDebugValue.PartiallyExpression)
          StatementBegin(condition);


        //Генерируем код выхода из цикла
        //При желании тут можно добавить микроразметку на оператор for(;;),
        //но я такого решения ни у кого не видел
        assemblyUnit.AddInstruction(new INSTR_IFFALSE(assemblyUnit.LabelManager.AddReference(n.LabelForEnd)));

        //Генерация отладочной информации завершена
        StatementEnd(condition);
      }
    }

    private void GenerateForIterator(ForNode n)
    {
      for (int i = 0; i < n.Loop.CountChildrens; i++)
      {
        //Итератор состоит из Statement-ов,
        //отладочная информация для Statement генерируется в любом случае
        n.Loop.GetChild(i).Visit(this);
      }
    }
  }
}
