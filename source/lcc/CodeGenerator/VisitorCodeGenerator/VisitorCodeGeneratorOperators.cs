using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {

    public override void Visit(BreakNode n)
    {
      InsertComment(n);

      StatementBegin(n);

      if (n.ClosestOperatorFor != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorFor.LabelForEnd)));
      else if (n.ClosestOperatorDo != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorDo.LabelDoEnd)));
      else if (n.ClosestOperatorWhile != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorWhile.LabelWhileEnd)));
      else if (n.ClosestOperatorSwitch != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorSwitch.LabelSwitchEnd)));

      StatementEnd(n);
    }

    public override void Visit(ContinueNode n)
    {
      InsertComment(n);

      StatementBegin(n);

      if (n.ClosestOperatorFor != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorFor.LabelForBegin)));
      else if (n.ClosestOperatorDo != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorDo.LabelDoCondition)));
      else if (n.ClosestOperatorWhile != null)
        assemblyUnit.AddInstruction(new INSTR_JMP(assemblyUnit.LabelManager.AddReference(n.ClosestOperatorWhile.LabelWhileBegin)));

      StatementEnd(n);
    }

    public override void Visit(ReturnNode n)
    {
      CheckContainStatementNode(n);

      InsertComment(n);

      FullNodeStatementBegin(n);

      if (n.CountChildrens != 0)
      {
        TypedNode operand = (TypedNode)n.GetChild(0);

        operand.AccessMethod = ResultAccessMethod.MethodGet;
        operand.Visit(this);

        PartiallyNodeStatementBegin(n);

        var size = n.Function.ReturnType.Type.Sizeof();
        if (size == 1)
          assemblyUnit.AddInstruction(new INSTR_STORE_1(-1));
        else if (size == 2)
          assemblyUnit.AddInstruction(new INSTR_STORE_2(-2));
        else if (size == 4)
          assemblyUnit.AddInstruction(new INSTR_STORE_4(-4));
        else if (size == 8)
          assemblyUnit.AddInstruction(new INSTR_STORE_8(-8));
        else
          throw new InternalCompilerException("Неверный размер загружаемого объекта");
      }
      else
      {
        PartiallyNodeStatementBegin(n);
      }
      assemblyUnit.AddInstruction(new INSTR_RET());

      StatementEnd(n);
    }

    /*
    public override void Visit(SourceTupleNode n)
    {
      for (int i = 0; i < n.CountChildrens; i++)
      {
        TypedNode operand = (TypedNode)n.GetChild(i);
        operand.AccessMethod = n.AccessMethod;
        operand.Visit(this);

        if (n.AccessMethod == ResultAccessMethod.MethodGet)
        {
          int size = operand.ObjectType.Sizeof();
          if (size == 1)
            assemblyUnit.AddInstruction(new INSTR_STORE_1((short)(-((i + 1) * 1))));
          else if (size == 2)
            assemblyUnit.AddInstruction(new INSTR_STORE_2((short)(-((i + 1) * 2))));
          else if (size == 4)
            assemblyUnit.AddInstruction(new INSTR_STORE_4((short)(-((i + 1) * 4))));
          else if (size == 8)
            assemblyUnit.AddInstruction(new INSTR_STORE_8((short)(-((i + 1) * 8))));
          else
            throw new CompilerException("Неверный размер загружаемого объекта");
        }
      }
    }


    private void GenerateReturnForAuxiliaryNullType(ReturnNode n, TypedNode operand)
    {
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_STORE_4(-4));
    }

    private void GenerateReturnForBaseArrayType(ReturnNode n, TypedNode operand)
    {
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_STORE_4(-4));
    }

    private void GenerateReturnForBaseReferenceArrayType(ReturnNode n, TypedNode operand)
    {
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      PartiallyNodeStatementBegin(n);

      assemblyUnit.AddInstruction(new INSTR_STORE_4(-4));
    }


    private void GenerateReturnForTupleType(ReturnNode n, TypedNode operand)
    {
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      PartiallyNodeStatementBegin(n);
    }

    private void GenerateReturnForBaseType(ReturnNode n, TypedNode operand)
    {
      operand.AccessMethod = ResultAccessMethod.MethodGet;
      operand.Visit(this);

      PartiallyNodeStatementBegin(n);

      var size = n.Function.ReturnType.Type.Sizeof();
      if (size == 1)
        assemblyUnit.AddInstruction(new INSTR_STORE_1(-1));
      else if (size == 2)
        assemblyUnit.AddInstruction(new INSTR_STORE_2(-2));
      else if (size == 4)
        assemblyUnit.AddInstruction(new INSTR_STORE_4(-4));
      else if (size == 8)
        assemblyUnit.AddInstruction(new INSTR_STORE_8(-8));
      else
        throw new CompilerException("Неверный размер загружаемого объекта");
    }
    */
  }
}
