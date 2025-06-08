/*
  This file is generated automatically. 
  To make changes, modify the 'SemanticTreeVisitorsGenerator.py' and re-generate this file.
*/

using System;

namespace LC2.LCCompiler.Compiler
{
  class SemanticAutoVisitor : SemanticVisitor
  {
    public virtual void PreVisit(ModuleRootNode n) { }
    public virtual void PostVisit(ModuleRootNode n) { }

    public virtual void PreVisit(ModuleInitNode n) { }
    public virtual void PostVisit(ModuleInitNode n) { }

    public virtual void PreVisit(InitializerNode n) { }
    public virtual void PostVisit(InitializerNode n) { }

    public virtual void PreVisit(ArrayValueNode n) { }
    public virtual void PostVisit(ArrayValueNode n) { }

    public virtual void PreVisit(BlockCodeNode n) { }
    public virtual void PostVisit(BlockCodeNode n) { }

    public virtual void PreVisit(ManagedFunctionDeclaratorNode n) { }
    public virtual void PostVisit(ManagedFunctionDeclaratorNode n) { }

    public virtual void PreVisit(NativeFunctionDeclaratorNode n) { }
    public virtual void PostVisit(NativeFunctionDeclaratorNode n) { }

    public virtual void PreVisit(VariableDeclaratorNode n) { }
    public virtual void PostVisit(VariableDeclaratorNode n) { }

    public virtual void PreVisit(StructDeclaratorNode n) { }
    public virtual void PostVisit(StructDeclaratorNode n) { }

    public virtual void PreVisit(ConstantValueNode n) { }
    public virtual void PostVisit(ConstantValueNode n) { }

    public virtual void PreVisit(TerminalIdentifierNode n) { }
    public virtual void PostVisit(TerminalIdentifierNode n) { }

    public virtual void PreVisit(ObjectNode n) { }
    public virtual void PostVisit(ObjectNode n) { }

    public virtual void PreVisit(CollectionNode n) { }
    public virtual void PostVisit(CollectionNode n) { }

    public virtual void PreVisit(FieldNode n) { }
    public virtual void PostVisit(FieldNode n) { }

    public virtual void PreVisit(FunctionCallNode n) { }
    public virtual void PostVisit(FunctionCallNode n) { }

    public virtual void PreVisit(IndexerNode n) { }
    public virtual void PostVisit(IndexerNode n) { }

    public virtual void PreVisit(ElementAccessNode n) { }
    public virtual void PostVisit(ElementAccessNode n) { }

    public virtual void PreVisit(PostfixIncrementNode n) { }
    public virtual void PostVisit(PostfixIncrementNode n) { }

    public virtual void PreVisit(PostfixDecrementNode n) { }
    public virtual void PostVisit(PostfixDecrementNode n) { }

    public virtual void PreVisit(PrefixIncrementNode n) { }
    public virtual void PostVisit(PrefixIncrementNode n) { }

    public virtual void PreVisit(PrefixDecrementNode n) { }
    public virtual void PostVisit(PrefixDecrementNode n) { }

    public virtual void PreVisit(NegativeNode n) { }
    public virtual void PostVisit(NegativeNode n) { }

    public virtual void PreVisit(LogicNotNode n) { }
    public virtual void PostVisit(LogicNotNode n) { }

    public virtual void PreVisit(NotNode n) { }
    public virtual void PostVisit(NotNode n) { }

    public virtual void PreVisit(TypeCastNode n) { }
    public virtual void PostVisit(TypeCastNode n) { }

    public virtual void PreVisit(SummNode n) { }
    public virtual void PostVisit(SummNode n) { }

    public virtual void PreVisit(SubNode n) { }
    public virtual void PostVisit(SubNode n) { }

    public virtual void PreVisit(MulNode n) { }
    public virtual void PostVisit(MulNode n) { }

    public virtual void PreVisit(DivNode n) { }
    public virtual void PostVisit(DivNode n) { }

    public virtual void PreVisit(RemNode n) { }
    public virtual void PostVisit(RemNode n) { }

    public virtual void PreVisit(RightShiftNode n) { }
    public virtual void PostVisit(RightShiftNode n) { }

    public virtual void PreVisit(LeftShiftNode n) { }
    public virtual void PostVisit(LeftShiftNode n) { }

    public virtual void PreVisit(LessNode n) { }
    public virtual void PostVisit(LessNode n) { }

    public virtual void PreVisit(LessEqualNode n) { }
    public virtual void PostVisit(LessEqualNode n) { }

    public virtual void PreVisit(MoreNode n) { }
    public virtual void PostVisit(MoreNode n) { }

    public virtual void PreVisit(MoreEqualNode n) { }
    public virtual void PostVisit(MoreEqualNode n) { }

    public virtual void PreVisit(EqualNode n) { }
    public virtual void PostVisit(EqualNode n) { }

    public virtual void PreVisit(NotEqualNode n) { }
    public virtual void PostVisit(NotEqualNode n) { }

    public virtual void PreVisit(AndNode n) { }
    public virtual void PostVisit(AndNode n) { }

    public virtual void PreVisit(XorNode n) { }
    public virtual void PostVisit(XorNode n) { }

    public virtual void PreVisit(OrNode n) { }
    public virtual void PostVisit(OrNode n) { }

    public virtual void PreVisit(LogicAndNode n) { }
    public virtual void PostVisit(LogicAndNode n) { }

    public virtual void PreVisit(LogicOrNode n) { }
    public virtual void PostVisit(LogicOrNode n) { }

    public virtual void PreVisit(ForNode n) { }
    public virtual void PostVisit(ForNode n) { }

    public virtual void PreVisit(WhileNode n) { }
    public virtual void PostVisit(WhileNode n) { }

    public virtual void PreVisit(DoNode n) { }
    public virtual void PostVisit(DoNode n) { }

    public virtual void PreVisit(IfNode n) { }
    public virtual void PostVisit(IfNode n) { }

    public virtual void PreVisit(SwitchNode n) { }
    public virtual void PostVisit(SwitchNode n) { }

    public virtual void PreVisit(BreakNode n) { }
    public virtual void PostVisit(BreakNode n) { }

    public virtual void PreVisit(ContinueNode n) { }
    public virtual void PostVisit(ContinueNode n) { }

    public virtual void PreVisit(LabelCaseNode n) { }
    public virtual void PostVisit(LabelCaseNode n) { }

    public virtual void PreVisit(LabelDefaultNode n) { }
    public virtual void PostVisit(LabelDefaultNode n) { }

    public virtual void PreVisit(ReturnNode n) { }
    public virtual void PostVisit(ReturnNode n) { }

    public virtual void PreVisit(AssignNode n) { }
    public virtual void PostVisit(AssignNode n) { }

    public override void Visit(Node n)
    {
      Console.WriteLine("Неизвестная нода {0}", n.GetType().Name);
    }
    public override void Visit(ModuleRootNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ModuleInitNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(InitializerNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ArrayValueNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(BlockCodeNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ManagedFunctionDeclaratorNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(NativeFunctionDeclaratorNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(VariableDeclaratorNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(StructDeclaratorNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ConstantValueNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(TerminalIdentifierNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ObjectNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(CollectionNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(FieldNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(FunctionCallNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(IndexerNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ElementAccessNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(PostfixIncrementNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(PostfixDecrementNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(PrefixIncrementNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(PrefixDecrementNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(NegativeNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LogicNotNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(NotNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(TypeCastNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(SummNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(SubNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(MulNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(DivNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(RemNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(RightShiftNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LeftShiftNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LessNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LessEqualNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(MoreNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(MoreEqualNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(EqualNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(NotEqualNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(AndNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(XorNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(OrNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LogicAndNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LogicOrNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ForNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(WhileNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(DoNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(IfNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(SwitchNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(BreakNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ContinueNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LabelCaseNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(LabelDefaultNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(ReturnNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
    public override void Visit(AssignNode n)
    {
      PreVisit((dynamic)n);
      base.Visit(n);
      PostVisit((dynamic)n);
    }
  }
}
