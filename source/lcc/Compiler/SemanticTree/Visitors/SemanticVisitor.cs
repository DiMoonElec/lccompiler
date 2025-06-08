/*
  This file is generated automatically. 
  To make changes, modify the 'SemanticTreeVisitorsGenerator.py' and re-generate this file.
*/

namespace LC2.LCCompiler.Compiler
{
  abstract class SemanticVisitor
  {
    public virtual void Visit(Node n) { VisitChilds(n); }
    public virtual void Visit(ModuleRootNode n) { VisitChilds(n); }
    public virtual void Visit(ModuleInitNode n) { VisitChilds(n); }
    public virtual void Visit(InitializerNode n) { VisitChilds(n); }
    public virtual void Visit(ArrayValueNode n) { VisitChilds(n); }
    public virtual void Visit(BlockCodeNode n) { VisitChilds(n); }
    public virtual void Visit(ManagedFunctionDeclaratorNode n) { VisitChilds(n); }
    public virtual void Visit(NativeFunctionDeclaratorNode n) { VisitChilds(n); }
    public virtual void Visit(VariableDeclaratorNode n) { VisitChilds(n); }
    public virtual void Visit(StructDeclaratorNode n) { VisitChilds(n); }
    public virtual void Visit(ConstantValueNode n) { VisitChilds(n); }
    public virtual void Visit(TerminalIdentifierNode n) { VisitChilds(n); }
    public virtual void Visit(ObjectNode n) { VisitChilds(n); }
    public virtual void Visit(CollectionNode n) { VisitChilds(n); }
    public virtual void Visit(FieldNode n) { VisitChilds(n); }
    public virtual void Visit(FunctionCallNode n) { VisitChilds(n); }
    public virtual void Visit(IndexerNode n) { VisitChilds(n); }
    public virtual void Visit(ElementAccessNode n) { VisitChilds(n); }
    public virtual void Visit(PostfixIncrementNode n) { VisitChilds(n); }
    public virtual void Visit(PostfixDecrementNode n) { VisitChilds(n); }
    public virtual void Visit(PrefixIncrementNode n) { VisitChilds(n); }
    public virtual void Visit(PrefixDecrementNode n) { VisitChilds(n); }
    public virtual void Visit(NegativeNode n) { VisitChilds(n); }
    public virtual void Visit(LogicNotNode n) { VisitChilds(n); }
    public virtual void Visit(NotNode n) { VisitChilds(n); }
    public virtual void Visit(TypeCastNode n) { VisitChilds(n); }
    public virtual void Visit(SummNode n) { VisitChilds(n); }
    public virtual void Visit(SubNode n) { VisitChilds(n); }
    public virtual void Visit(MulNode n) { VisitChilds(n); }
    public virtual void Visit(DivNode n) { VisitChilds(n); }
    public virtual void Visit(RemNode n) { VisitChilds(n); }
    public virtual void Visit(RightShiftNode n) { VisitChilds(n); }
    public virtual void Visit(LeftShiftNode n) { VisitChilds(n); }
    public virtual void Visit(LessNode n) { VisitChilds(n); }
    public virtual void Visit(LessEqualNode n) { VisitChilds(n); }
    public virtual void Visit(MoreNode n) { VisitChilds(n); }
    public virtual void Visit(MoreEqualNode n) { VisitChilds(n); }
    public virtual void Visit(EqualNode n) { VisitChilds(n); }
    public virtual void Visit(NotEqualNode n) { VisitChilds(n); }
    public virtual void Visit(AndNode n) { VisitChilds(n); }
    public virtual void Visit(XorNode n) { VisitChilds(n); }
    public virtual void Visit(OrNode n) { VisitChilds(n); }
    public virtual void Visit(LogicAndNode n) { VisitChilds(n); }
    public virtual void Visit(LogicOrNode n) { VisitChilds(n); }
    public virtual void Visit(ForNode n) { VisitChilds(n); }
    public virtual void Visit(WhileNode n) { VisitChilds(n); }
    public virtual void Visit(DoNode n) { VisitChilds(n); }
    public virtual void Visit(IfNode n) { VisitChilds(n); }
    public virtual void Visit(SwitchNode n) { VisitChilds(n); }
    public virtual void Visit(BreakNode n) { VisitChilds(n); }
    public virtual void Visit(ContinueNode n) { VisitChilds(n); }
    public virtual void Visit(LabelCaseNode n) { VisitChilds(n); }
    public virtual void Visit(LabelDefaultNode n) { VisitChilds(n); }
    public virtual void Visit(ReturnNode n) { VisitChilds(n); }
    public virtual void Visit(AssignNode n) { VisitChilds(n); }

    void VisitChilds(Node n)
    {
      for (int i = 0; i < n.CountChildrens; i++)
        Visit((dynamic)(n.GetChild(i)));
    }
  }
}
