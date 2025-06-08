using LC2.LCCompiler.Compiler.Actions;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  internal partial class SemanticCheck : SemanticAutoVisitor
  {
    #region CheckUnaryOpBit - побитовые унарные операции

    public override void PostVisit(NotNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpBit(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Not(n, logger));
    }

    #endregion

    #region CheckUnaryOpIncrDecr - Операции инкремента/декремента

    public override void PostVisit(PostfixIncrementNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpIncrDecr(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.PostfixIncr(n, logger));
    }

    public override void PostVisit(PostfixDecrementNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpIncrDecr(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.PostfixDecr(n, logger));
    }

    public override void PostVisit(PrefixIncrementNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpIncrDecr(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.PrefixIncr(n, logger));
    }

    public override void PostVisit(PrefixDecrementNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpIncrDecr(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.PrefixDecr(n, logger));
    }

    #endregion

    #region CheckUnaryOpLogical - Логические унарные операции
    public override void PostVisit(LogicNotNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpLogical(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.LogicalNot(n, logger));
    }

    #endregion

    #region CheckUnaryOpArithmetic - Арифметические унарные операции
    public override void PostVisit(NegativeNode n)
    {
      //SetPassOk(CheckArgumentTypeValidation.CheckUnaryOpArithmetic(n, logger));
      SetPassOk(checkUnaryOperation.Check(n));
      SetPassOk(ActionExpressionEvaluator.Inv(n, logger));
    }

    #endregion

  }
}
