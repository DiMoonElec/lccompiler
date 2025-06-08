using LC2.LCCompiler.Compiler.Actions;
using LC2.LCCompiler.Compiler.SemanticChecks.Checks;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  internal partial class SemanticCheck : SemanticAutoVisitor
  {

    #region CheckBinaryOpCompare - бинарные операции сравнения

    public override void PostVisit(LessNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Less(n, logger));
    }

    public override void PostVisit(LessEqualNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.LessEqual(n, logger));
    }

    public override void PostVisit(MoreNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.More(n, logger));
    }

    public override void PostVisit(MoreEqualNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.MoreEqual(n, logger));
    }

    #endregion

    #region CheckBinaryOpBit - побитовые бинарные операции

    public override void PostVisit(AndNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.And(n, logger));
    }

    public override void PostVisit(XorNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Xor(n, logger));
    }

    public override void PostVisit(OrNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Or(n, logger));
    }

    public override void PostVisit(RightShiftNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.RightShift(n, logger));
    }

    public override void PostVisit(LeftShiftNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.LeftShift(n, logger));
    }

    #endregion

    #region CheckBinaryOpLogical - Логические бинарные операции

    public override void PostVisit(LogicAndNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.LogicalAnd(n, logger));
    }

    public override void PostVisit(LogicOrNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.LogicalOr(n, logger));
    }

    #endregion

    #region CheckBinaryOpArithmetic Бинарные арифметические операции

    public override void PostVisit(SummNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Summ(n, logger));
    }

    public override void PostVisit(SubNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Sub(n, logger));
    }

    public override void PostVisit(MulNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Mul(n, logger));
    }

    public override void PostVisit(DivNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Div(n, logger));
    }

    public override void PostVisit(RemNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Rem(n, logger));
    }


    #endregion

    #region CheckBinaryOpEqual - Операции равенства/неравенства

    public override void PostVisit(EqualNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Eq(n, logger));
    }

    public override void PostVisit(NotEqualNode n)
    {
      SetPassOk(checkBinaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Neq(n, logger));
    }

    #endregion

  }
}
