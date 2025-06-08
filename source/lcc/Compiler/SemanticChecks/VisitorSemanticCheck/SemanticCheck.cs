using LC2.LCCompiler.Compiler.Actions;
using LC2.LCCompiler.Compiler.SemanticChecks.Checks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LC2.LCCompiler.Compiler.LCTypesUtils;

namespace LC2.LCCompiler.Compiler.SemanticChecks
{
  /// <summary>
  /// Визитор основной семантической проверки
  /// </summary>
  internal partial class SemanticCheck : SemanticAutoVisitor
  {
    CompilerLogger logger;
    SymbolTable[] Symbols;
    UseDirectives Uses;
    string ModulePath;
    public bool PassOk { get; private set; }


    CheckBinaryOperation checkBinaryOperation;
    CheckUnaryOperation checkUnaryOperation;

    private void InitCheckRules()
    {
      CheckBinaryOperationRule[] binaryOperationRules =
      {
          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(LessNode),
              typeof(LessEqualNode),
              typeof(MoreNode),
              typeof(MoreEqualNode) },

            LeftOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt 
                                                            | GroupPrimitiveTypes.ClassUInt 
                                                            | GroupPrimitiveTypes.ClassFloat),

            RightOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt 
                                                            | GroupPrimitiveTypes.ClassUInt 
                                                            | GroupPrimitiveTypes.ClassFloat),

            ResultTypeResolver = (left, right) => new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool),

            Description = "Правило проверки операторов '<', '<=', '>', '>='"
          },

          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(AndNode),
              typeof(XorNode),
              typeof(OrNode),
              typeof(RemNode)},

            LeftOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                            | GroupPrimitiveTypes.ClassUInt),

            RightOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                            | GroupPrimitiveTypes.ClassUInt),

            ResultTypeResolver = (left, right) => left.Clone(),

            Description = "Правило проверки операторов '&', '^', '|'"
          },

          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(RightShiftNode),
              typeof(LeftShiftNode)},

            DifferendOperandTypes = true,

            LeftOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                            | GroupPrimitiveTypes.ClassUInt),

            RightOperandValidTypes = new[] {LCPrimitiveType.PrimitiveTypes.LCTypeByte},

            ResultTypeResolver = (left, right) => left.Clone(),

            Description = "Правило проверки операторов '>>', '<<'"
          },

          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(LogicAndNode),
              typeof(LogicOrNode)
            },

            LeftOperandValidTypes = new[] {LCPrimitiveType.PrimitiveTypes.LCTypeBool},

            RightOperandValidTypes = new[] {LCPrimitiveType.PrimitiveTypes.LCTypeBool},

            ResultTypeResolver = (left, right) => new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool),

            Description = "Правило проверки операторов '&&', '||'"
          },

          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(SummNode),
              typeof(SubNode),
              typeof(MulNode),
              typeof(DivNode)},

            LeftOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                            | GroupPrimitiveTypes.ClassUInt
                                                            | GroupPrimitiveTypes.ClassFloat),

            RightOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                            | GroupPrimitiveTypes.ClassUInt
                                                            | GroupPrimitiveTypes.ClassFloat),

            ResultTypeResolver = (left, right) => left.Clone(),

            Description = "Правило проверки операторов '+', '-', '*', '/'"
          },

          new CheckBinaryOperationRule
          {
            OperatorType = new Type[]{
              typeof(EqualNode),
              typeof(NotEqualNode)
            },

            LeftOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassAll),

            RightOperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassAll),

            ResultTypeResolver = (left, right) => new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool),

            Description = "Правило проверки операторов '==', '!='"
          },
      };

      checkBinaryOperation = new CheckBinaryOperation(binaryOperationRules, logger);


      CheckUnaryOperationRule[] unaryOperationRules =
      {
        new CheckUnaryOperationRule
        {
          OperatorType = new Type[]{typeof(NotNode) },

          OperandMustBeWritable = false,

          OperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                      | GroupPrimitiveTypes.ClassUInt),

          ResultTypeResolver = (type) => type.Clone(),

          Description = "Правило проверки операторов '~'"
        },

        new CheckUnaryOperationRule
        {
          OperatorType = new Type[]{
            typeof(PostfixIncrementNode),
            typeof(PostfixDecrementNode),
            typeof(PrefixIncrementNode),
            typeof(PrefixDecrementNode)
          },

          OperandMustBeWritable = true,

          OperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                     | GroupPrimitiveTypes.ClassUInt
                                                     | GroupPrimitiveTypes.ClassFloat),

          ResultTypeResolver = (type) => type.Clone(),

          Description = "Правило проверки операторов 'i++', 'i--', '++i', '--i'"
        },

        new CheckUnaryOperationRule
        {
          OperatorType = new Type[]{typeof(LogicNotNode)},

          OperandMustBeWritable = false,

          OperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassBool),

          ResultTypeResolver = (type) => new LCPrimitiveType(LCPrimitiveType.PrimitiveTypes.LCTypeBool),

          Description = "Правило проверки операторов '!'"
        },

        new CheckUnaryOperationRule
        {
          OperatorType = new Type[]{typeof(NegativeNode)},

          OperandMustBeWritable = false,

          OperandValidTypes = LCTypesUtils.GetGroupList(GroupPrimitiveTypes.ClassInt
                                                     | GroupPrimitiveTypes.ClassFloat),

          ResultTypeResolver = (type) => type.Clone(),

          Description = "Правило проверки операторов '-i'"
        },
      };

      checkUnaryOperation = new CheckUnaryOperation(unaryOperationRules, logger);
    }

    void SetPassOk(bool value)
    {
      if (value == false)
        PassOk = false;
    }
    /// <summary>
    /// Конструктор класса семантических проверок
    /// </summary>
    /// <param name="logger">Логгер</param>
    /// <param name="modulePath">Путь данного модуля</param>
    /// <param name="symbols">Таблица символов проекта</param>
    public SemanticCheck(CompilerLogger logger, string modulePath, SymbolTable[] symbols)
    {
      this.logger = logger;
      Symbols = symbols;
      ModulePath = modulePath;
      PassOk = true;

      InitCheckRules();
    }

    public override void PreVisit(ModuleRootNode n)
    {
      Uses = n.Uses;
      SetPassOk(CheckUses.Check(Uses, ModulePath, Symbols, logger));
    }

    public override void PostVisit(ModuleInitNode n)
    {
      SetPassOk(CheckModuleInit.Check(n, logger));
    }

    public override void PreVisit(ManagedFunctionDeclaratorNode n)
    {
      //Проверка имени функции
      SetPassOk(CheckDeclaratorName.CheckFunction(n, logger));

      //Проверка корректности объявления функции
      SetPassOk(CheckDeclarator.CheckFunction(n, logger));
    }

    public override void PreVisit(VariableDeclaratorNode n)
    {
      //SetPassOk(CheckDeclaratorName.(n, logger));
      SetPassOk(CheckDeclarator.CheckVariable(n, logger));
    }

    public override void PreVisit(StructDeclaratorNode n)
    {
      //SetPassOk(CheckDeclaratorName.CheckUserType(n, Logger));
      SetPassOk(CheckDeclarator.CheckStruct(n, logger));
    }

    public override void PostVisit(TerminalIdentifierNode n)
    {
      //Выполняет поиск переменной по имени и заменяет
      //данную ноду на ObjectNode
      SetPassOk(CheckTerminalID.Check(n, Uses, logger));
    }

    public override void PostVisit(ReturnNode n)
    {
      //Оператор return должен быть вложен в функцию
      SetPassOk(ComplementaryOperatorSearch.OperatorReturn(n, logger));

      //Тип возвращаемого объекта должен соответствовать типу функции
      SetPassOk(CheckArgumentTypeValidationReturn.CheckOperatorReturn(n, logger));
    }

    public override void PostVisit(LabelCaseNode n)
    {
      //Оператор case должен быть включен в ноду switch
      SetPassOk(ComplementaryOperatorSearch.OperatorCase(n, logger));
    }

    public override void PostVisit(LabelDefaultNode n)
    {
      //Оператор default должен быть включен в ноду switch
      SetPassOk(ComplementaryOperatorSearch.OperatorDefault(n, logger));
    }

    public override void PostVisit(BreakNode n)
    {
      SetPassOk(ComplementaryOperatorSearch.OperatorBreak(n, logger));
    }

    public override void PostVisit(ContinueNode n)
    {
      SetPassOk(ComplementaryOperatorSearch.OperatorContinue(n, logger));
    }


    public override void PostVisit(FunctionCallNode n)
    {
      SetPassOk(CheckFunctionCall.Check(n, logger));
    }

    public override void PostVisit(IfNode n)
    {
      SetPassOk(CheckOperator.CheckIf(n, logger));
    }

    public override void PostVisit(DoNode n)
    {
      SetPassOk(CheckOperator.CheckDo(n, logger));
    }

    public override void PostVisit(WhileNode n)
    {
      SetPassOk(CheckOperator.CheckWhile(n, logger));
    }

    public override void PostVisit(ForNode n)
    {
      SetPassOk(CheckOperator.CheckFor(n, logger));
    }

    public override void PostVisit(SwitchNode n)
    {
      SetPassOk(CheckOperatorSwitch.CheckSwitch(n, logger));
    }

    public override void PostVisit(IndexerNode n)
    {
      SetPassOk(CheckIndexer.Check(n, logger));
    }

    public override void PostVisit(ElementAccessNode n)
    {
      SetPassOk(CheckElementAccess.Check(n, logger));
    }


    public override void PostVisit(TypeCastNode n)
    {
      SetPassOk(CheckArgumentTypeValidationTypeCast.Check(n, logger));
      SetPassOk(ActionExpressionEvaluator.TypeCast(n, logger));
    }


    #region CheckBinaryOpAssign - Операция присвоения

    public override void PostVisit(AssignNode n)
    {
      SetPassOk(CheckArgumentTypeValidationAssign.CheckBinaryOpAssign(n, logger));
    }

    #endregion

  }
}
