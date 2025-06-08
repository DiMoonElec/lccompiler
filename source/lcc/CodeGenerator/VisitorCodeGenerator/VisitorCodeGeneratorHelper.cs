using System;
using System.Collections.Generic;
using LC2.LCCompiler.CodeGenerator.AsmInstruction;
using LC2.LCCompiler.Compiler;

namespace LC2.LCCompiler.CodeGenerator
{
  internal partial class VisitorCodeGenerator : SemanticVisitor
  {

    string GetGlobalLabelName(DeclaratorNode declarator)
    {
      return string.Format("{0}::{1}", declarator.ModuleName, declarator.Name);
    }

    string GetLabelName()
    {
      LabelCounter++;
      return string.Format("{0}::{1}::lbl_{2}", 
        assemblyUnit.ModuleName, 
        currentFunctionName, 
        LabelCounter.ToString());
      //return string.Format("lbl_{0}", LabelCounter.ToString());
    }

    void InsertComment(Node n)
    {
      if ((n.StatementTxt != null) && (n.StatementTxt != ""))
      {
        assemblyUnit.AddInstruction(new LCVMComment(""));
        assemblyUnit.AddInstruction(new LCVMComment(n.StatementTxt));
      }
    }

    void InsertComment(string statementTxt)
    {
      if ((statementTxt != null) && (statementTxt != ""))
      {
        assemblyUnit.AddInstruction(new LCVMComment(""));
        assemblyUnit.AddInstruction(new LCVMComment(statementTxt));
      }
    }






    /// <summary>
    /// Добавляет StatementBegin в ассемблерный листинг только в случае, если GenerateDebugInfo равно PartiallyExpression
    /// </summary>
    void PartiallyNodeStatementBegin(SourceLocatedNode n)
    {
      if (n.GenerateDebugInfo == GenerateDebugValue.PartiallyExpression)
        StatementBegin(n);
    }

    /// <summary>
    /// Добавляет StatementBegin в ассемблерный листинг только в случае, если GenerateDebugInfo равно FullExpression
    /// </summary>
    void FullNodeStatementBegin(SourceLocatedNode n)
    {
      if (n.GenerateDebugInfo == GenerateDebugValue.FullExpression)
        StatementBegin(n);
    }

    void StatementBegin(FunctionDeclaratorNode n)
    {

    }

    void StatementEnd(FunctionDeclaratorNode n)
    {

    }

    /// <summary>
    /// Добавляет StatementBegin в ассемблерный листинг
    /// </summary>
    void StatementBegin(SourceLocatedNode n)
    {
      if (assemblyUnit.CurrentSectionIsCode())
        assemblyUnit.StatementBegin(n.ExpressionLocate);
    }


    /// <summary>
    /// Добавляет StatementEnd в ассемблерный листинг
    /// </summary>
    void StatementEnd(SourceLocatedNode n)
    {
      if (assemblyUnit.CurrentSectionIsCode())
        assemblyUnit.StatementEnd();
    }

    
    private void CheckContainExpressionNode(Node n, Node expression)
    {
      if(ContainStatementNode(expression))
        n.GenerateDebugInfo = GenerateDebugValue.PartiallyExpression;
      else
        n.GenerateDebugInfo = GenerateDebugValue.FullExpression;
    }
    

    /// <summary>
    /// Если среди потомков ноды n содержится нода типа IStatementMarker, то устанавливает
    /// GenerateDebugInfo в PartiallyExpression, иначе в FullExpression
    /// </summary>
    private void CheckContainStatementNode(Node n)
    {
      var childs = n.GetAllChilds();

      foreach (var c in childs)
      {
        if (ContainStatementNode(c))
        {
          n.GenerateDebugInfo = GenerateDebugValue.PartiallyExpression;
          return;
        }
      }

      n.GenerateDebugInfo = GenerateDebugValue.FullExpression;
    }


    private bool ContainStatementNode(Node n)
    {
      if (n is IStatementMarker)
        return true;

      foreach (var child in n.GetAllChilds())
        if (ContainStatementNode(child) == true)
          return true;

      return false;
    }
  }
}
