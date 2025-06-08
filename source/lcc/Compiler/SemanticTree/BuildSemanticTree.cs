using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using LC2.LCCompiler.Compiler.SemanticTree.Parsers;
using System;
using System.Collections.Generic;

namespace LC2.LCCompiler.Compiler.SemanticTree
{
  class BuildSemanticTree : LCLangBaseVisitor<Node>
  {
    public bool BuildOK { get; private set; }

    string moduleName;
    CompilerLogger logger;
    ICharStream Stream;

    /// <summary>
    /// Текущий уровень парсинга. Если парсинг находится в корне, то true,
    /// если мы парсим содержимое функции, то false
    /// </summary>
    bool currentLevelRoot = true;


    ModuleInitNode moduleInit;

    public BuildSemanticTree(string moduleName, ICharStream stream, CompilerLogger logger)
    {
      this.moduleName = moduleName;
      this.logger = logger;
      Stream = stream;

      BuildOK = true;
    }




    public override Node VisitCompilationUnit([NotNull] LCLangParser.CompilationUnitContext context)
    {
      currentLevelRoot = true;
      ModuleRootNode r = new ModuleRootNode();
      moduleInit = r.ModuleInit; //Создаем ссылку на ноду инициализации

      var useDirectives = context.useDirectives();
      var compilationUnitElement = context.compilationUnitElement();

      ParceUses(useDirectives, r.Uses);

      foreach (var e in compilationUnitElement)
        r.AddChild(Visit(e));

      return r;
    }

    private void ParceUses(LCLangParser.UseDirectivesContext useDirectives, UseDirectives uses)
    {
      if (useDirectives == null)
        return;

      var useDirective = useDirectives.useDirective();
      foreach (var e in useDirective)
      {
        var stringLiteral = e.StringLiteral();

        string module = stringLiteral.Symbol.Text;
        module = module.Substring(1, module.Length - 2);
        LocateElement locate = new LocateElement(stringLiteral);

        uses.UseModule.Add(new UseDirective(module, locate));
      }
    }

    /***********************************************************/

    public override Node VisitCompilationUnitElement([NotNull] LCLangParser.CompilationUnitElementContext context)
    {
      return Visit(context.GetChild(0));
    }

    public override Node VisitVarableDeclarationStatement([NotNull] LCLangParser.VarableDeclarationStatementContext context)
    {
      return VisitVarableDeclaration(context.varableDeclaration());
    }

    public override Node VisitStructDeclarationStatement([NotNull] LCLangParser.StructDeclarationStatementContext context)
    {
      return VisitStructDeclaration(context.structDeclaration());
    }

    public override Node VisitStructDeclaration([NotNull] LCLangParser.StructDeclarationContext context)
    {
      LCStructDeclarator type;
      LCStructTypeLocate typeLocate;
      ParserStructDeclaration.Parse(context, logger, out type, out typeLocate);

      StructDeclaratorNode r = new StructDeclaratorNode(moduleName, type, typeLocate);
      return r;
    }

    public override Node VisitFunctionDeclaration([NotNull] LCLangParser.FunctionDeclarationContext context)
    {
      var functionReturnTypeContext = context.lcPrimitiveType();
      var functionNameContext = context.Identifier();
      var functionParametersContext = context.functionParameters();
      var functionBodyContext = context.compoundStatement();

      //Парсим тело функции
      currentLevelRoot = false; //переходим внутрь парсинга функции
      BlockCodeNode body = (BlockCodeNode)VisitCompoundStatement(functionBodyContext);
      currentLevelRoot = true; //выходим из парсинга функции

      //Парсим возвращаемое значение
      LCPrimitiveType returnType;
      LocateElement returnTypeLocate;

      ParserPrimitiveType.ParsePrimitiveType(functionReturnTypeContext, out returnType, out returnTypeLocate);

      LCObjectType objectReturnType = new LCObjectType(returnType);

      //Парсим имя функции
      string functionName = functionNameContext.Symbol.Text;
      LocateElement functionNameLocate = new LocateElement(functionNameContext);

      //Парсим параметры функции
      VariableDeclaratorNode[] functionObjectParameters = ParseFunctionParameters(functionParametersContext);

      ManagedFunctionDeclaratorNode r = new ManagedFunctionDeclaratorNode(functionName, moduleName,
        functionObjectParameters, objectReturnType, body, functionNameLocate, returnTypeLocate);

      return r;
    }

    VariableDeclaratorNode[] ParseFunctionParameters(LCLangParser.FunctionParametersContext context)
    {
      if (context == null)
        return new VariableDeclaratorNode[0];

      var functionParametersContext = context.functionParameter();

      List<VariableDeclaratorNode> funcParams = new List<VariableDeclaratorNode>();

      for (int i = 0; i < functionParametersContext.Length; i++)
      {
        var fpContext = functionParametersContext[i];
        var fpTypeContext = fpContext.lcFunctionParamType();
        var fpNameContext = fpContext.Identifier();

        LCType paramType;
        LocateElement paramTypeLocate;

        string paramName = fpNameContext.Symbol.Text;
        LocateElement paramNameLocate = new LocateElement(fpNameContext);

        ParserFunctionParamType.Parse(fpTypeContext, out paramType, out paramTypeLocate);

        LCObjectType objFuncParam = new LCObjectType(paramType);
        var funcParam = new VariableDeclaratorNode(objFuncParam, paramName, moduleName, 
          new LCTypeLocate(paramTypeLocate), paramNameLocate);

        funcParam.ClassValue = ObjectDeclaratorNode.DeclaratorClass.ClassFunctionParam;
        funcParams.Add(funcParam);
      }

      return funcParams.ToArray();
    }

    public override Node VisitVarableDeclaration([NotNull] LCLangParser.VarableDeclarationContext context)
    {
      var varDeclaratorContext = context.varableDeclarator();
      var varInitializerContext = context.initializer();
      var attributeSpecifier = context.attributeSpecifier();
      var assignContext = context.Assign();
      var varTypeContext = varDeclaratorContext.lcVariableType();
      var varNameContext = varDeclaratorContext.Identifier();

      Node r = null;

      LCType variableType; //Тип переменной
      LCTypeLocate variableTypeLocate; //Расположение типа в исходном коде

      //Если возникли ошибки при парсинге типа, выходим без дальнейшего разбора
      if (ParserVariableType.Parse(varTypeContext, logger, out variableType, out variableTypeLocate) == false)
      {
        BuildOK = false;
        return null;
      }

      string variableName = varNameContext.Symbol.Text; //Имя переменной
      LocateElement variableNameLocate = new LocateElement(varNameContext); //Расположение имени в исходном коде


      LCObjectType variableObjType = new LCObjectType(variableType); //Создаем тип объекта
      variableObjType.SetAccessAttributes(true, true); //Переменная доступна на чтение и запись

      VariableDeclaratorNode declarator = new VariableDeclaratorNode(variableObjType, variableName, moduleName,
        variableTypeLocate, variableNameLocate);

      //Устанавливаем класс переменной, глобальная или локальная
      if (currentLevelRoot == true)
        declarator.ClassValue = ObjectDeclaratorNode.DeclaratorClass.ClassGlobal;
      else
        declarator.ClassValue = ObjectDeclaratorNode.DeclaratorClass.ClassLocal;


      if (varInitializerContext != null) //Если присутствует инициализатор
      {
        //Выражение, которое реализует инициализатор переменной
        var expression = varInitializerContext.expression();

        //Инициализируемая переменная
        ObjectNode objectNode = new ObjectNode(declarator, variableNameLocate);

        //Кусок семанического дерева, который является инициализатором
        var initializerExpr = Visit(expression);

        AssignNode assignNode = new AssignNode(new LocateElement(assignContext),
          new LocateElement(variableNameLocate, new LocateElement(expression)));

        assignNode.AddChild(objectNode);
        assignNode.AddChild(initializerExpr);
        assignNode.StatementTxt = GetText(context);
        if (currentLevelRoot)
        {
          //Мы находимся в корне, значит инициализатор
          //добавляем в специальный блок кода
          moduleInit.AddChild(assignNode);
          r = declarator;
        }
        else
        {
          //Мы находимся в функции,
          //добавляем инициализатор после декларатора
          r = new CollectionNode();
          r.AddChild(declarator);
          r.AddChild(assignNode);
        }

        return r;
      }

      if (attributeSpecifier != null)
      {
        var attribute = attributeSpecifier.StringLiteral().GetText().Trim('"');
        declarator.Attribute = attribute;
      }

      return declarator;
    }

    public override Node VisitCompoundStatement([NotNull] LCLangParser.CompoundStatementContext context)
    {
      var blockItemList = context.blockItemList();

      BlockCodeNode r = new BlockCodeNode();

      if (blockItemList != null)
      {
        var blockItems = blockItemList.blockItem();

        foreach (var e in blockItems)
          r.AddChild(VisitBlockItem(e));
      }

      return r;
    }

    public override Node VisitBlockItem([NotNull] LCLangParser.BlockItemContext context)
    {
      var statement = context.statement();
      var varableDeclarationStatement = context.varableDeclarationStatement();

      if (statement != null)
        return VisitStatement(statement);
      else if (varableDeclarationStatement != null)
        return VisitVarableDeclarationStatement(varableDeclarationStatement);
      else
        throw new Exception("Неизвестная нода в BlockItemContext");
    }

    public override Node VisitStatement([NotNull] LCLangParser.StatementContext context)
    {
      var labeledStatement = context.labeledStatement();
      var compoundStatement = context.compoundStatement();
      var expressionStatement = context.expressionStatement();
      var selectionStatement = context.selectionStatement();
      var iterationStatement = context.iterationStatement();
      var jumpStatement = context.jumpStatement();

      if (labeledStatement != null)
      {
        var r = VisitLabeledStatement(labeledStatement);
        return r;
      }
      else if (compoundStatement != null)
      {
        return VisitCompoundStatement(compoundStatement);
      }
      else if (expressionStatement != null)
      {
        var r = VisitExpressionStatement(expressionStatement);
        if (r != null)
          r.StatementTxt = GetText(context);
        return r;
      }
      else if (selectionStatement != null)
        return VisitSelectionStatement(selectionStatement);
      else if (iterationStatement != null)
        return VisitIterationStatement(iterationStatement);
      else if (jumpStatement != null)
      {
        var r = VisitJumpStatement(jumpStatement);
        r.StatementTxt = GetText(context);
        return r;
      }
      else
        throw new Exception("Неизвестная нода в StatementContext");
    }

    public override Node VisitSelectionStatement([NotNull] LCLangParser.SelectionStatementContext context)
    {
      var selectionIFStatement = context.selectionIFStatement();
      var selectionSwitchStatement = context.selectionSwitchStatement();

      if (selectionIFStatement != null)
        return VisitSelectionIFStatement(selectionIFStatement);
      else if (selectionSwitchStatement != null)
        return VisitSelectionSwitchStatement(selectionSwitchStatement);
      else
        throw new Exception("Неизвестная нода в SelectionStatementContext");
    }

    public override Node VisitSelectionIFStatement([NotNull] LCLangParser.SelectionIFStatementContext context)
    {
      IfNode r = new IfNode(
        new LocateElement(context.If()),
        new LocateElement(context.If(), context.RightParen()));

      r.Condition.AddChild(Visit(context.condition));
      r.TrueBody.AddChild(VisitStatement(context.iftrue));

      var iffalse = context.iffalse;
      if (iffalse != null)
        r.FalseBody.AddChild(VisitStatement(iffalse));

      r.StatementTxt = GetText(context.Start.StartIndex, context.RightParen().Symbol.StopIndex);

      return r;
    }

    public override Node VisitSelectionSwitchStatement([NotNull] LCLangParser.SelectionSwitchStatementContext context)
    {
      SwitchNode r = new SwitchNode(
        new LocateElement(context.Switch()),
        new LocateElement(context.Switch(), context.RightParen()));

      r.Expression.AddChild(Visit(context.expression()));
      r.Body.AddChild(VisitCompoundStatement(context.compoundStatement()));

      r.StatementTxt = GetText(context.Start.StartIndex, context.RightParen().Symbol.StopIndex);
      return r;
    }

    public override Node VisitIterationStatement([NotNull] LCLangParser.IterationStatementContext context)
    {
      var iterationWhileStatement = context.iterationWhileStatement();
      var iterationDoStatement = context.iterationDoStatement();
      var iterationForStatement = context.iterationForStatement();

      if (iterationWhileStatement != null)
        return VisitIterationWhileStatement(iterationWhileStatement);
      else if (iterationDoStatement != null)
        return VisitIterationDoStatement(iterationDoStatement);
      else if (iterationForStatement != null)
        return VisitIterationForStatement(iterationForStatement);
      else
        throw new Exception("Неизвестная нода в IterationStatementContext");
    }

    public override Node VisitIterationWhileStatement([NotNull] LCLangParser.IterationWhileStatementContext context)
    {
      WhileNode r = new WhileNode(
        new LocateElement(context.While()),
        new LocateElement(context.While(), context.RightParen()));

      r.Condition.AddChild(Visit(context.condition));
      r.Body.AddChild(VisitStatement(context.body));

      r.StatementTxt = GetText(context.Start.StartIndex, context.RightParen().Symbol.StopIndex);

      return r;
    }

    public override Node VisitIterationDoStatement([NotNull] LCLangParser.IterationDoStatementContext context)
    {
      DoNode r = new DoNode(
        new LocateElement(context.Do()),
        new LocateElement(context.While(), context.RightParen()));

      r.Condition.AddChild(Visit(context.condition));
      r.Body.AddChild(VisitStatement(context.body));

      r.StatementTxt = GetText(context.Do().Symbol.StartIndex, context.Do().Symbol.StopIndex);
      r.StatementDoWhileTxt = GetText(context.While().Symbol.StartIndex, context.RightParen().Symbol.StopIndex);

      return r;
    }

    public override Node VisitIterationForStatement([NotNull] LCLangParser.IterationForStatementContext context)
    {
      //Блок кода, в котором будет размещен декларатор переменной,
      //ее инициализатор и нода оператора for

      BlockCodeNode r = new BlockCodeNode();

      ForNode forNode = new ForNode(
        new LocateElement(context.For()),
        new LocateElement(context.For(), context.RightParen()));

      var forInitializer = context.forSections().forInitializer();
      if (forInitializer != null)
      {
        var forInitializerItem = forInitializer.forInitializerItem();
        foreach (var e in forInitializerItem)
          r.AddChild(VisitForInitializerItem(e));
      }

      var forCondition = context.forSections().forCondition();
      if (forCondition != null)
        forNode.Condition.AddChild(Visit(forCondition.expression()));

      var forIteratorExpression = context.forSections().forIteratorExpression();
      if (forIteratorExpression != null)
      {
        var forIteratorExpressionItem = forIteratorExpression.forIteratorExpressionItem();
        foreach (var e in forIteratorExpressionItem)
          forNode.Loop.AddChild(VisitForIteratorExpressionItem(e));
      }

      forNode.Body.AddChild(Visit(context.body));

      r.AddChild(forNode);
      return r;
    }

    public override Node VisitForIteratorExpressionItem([NotNull] LCLangParser.ForIteratorExpressionItemContext context)
    {
      return Visit(context.expression());
    }

    public override Node VisitForInitializerItem([NotNull] LCLangParser.ForInitializerItemContext context)
    {
      return Visit(context.GetChild(0));
    }


    public override Node VisitJumpStatement([NotNull] LCLangParser.JumpStatementContext context)
    {
      return Visit(context.GetChild(0));
    }

    public override Node VisitJumpContinue([NotNull] LCLangParser.JumpContinueContext context)
    {
      return new ContinueNode(new LocateElement(context.Continue()));
    }

    public override Node VisitJumpBreak([NotNull] LCLangParser.JumpBreakContext context)
    {
      return new BreakNode(new LocateElement(context.Break()));
    }

    public override Node VisitJumpReturn([NotNull] LCLangParser.JumpReturnContext context)
    {
      ReturnNode r;
      var expression = context.expression();

      if (expression != null)
      {
        var left = new LocateElement(context.Return());
        var right = new LocateElement(expression);
        r = new ReturnNode(left, new LocateElement(left, right));
        r.AddChild(Visit(expression));
      }
      else
      {
        r = new ReturnNode(
          new LocateElement(context.Return()),
          new LocateElement(context.Return()));
      }

      return r;
    }

    public override Node VisitLabeledStatement([NotNull] LCLangParser.LabeledStatementContext context)
    {
      var labelCase = context.labelCase();
      var labelDefault = context.labelDefault();

      if (labelCase != null)
        return VisitLabelCase(labelCase);
      else if (labelDefault != null)
        return VisitLabelDefault(labelDefault);
      else
        throw new Exception("Неизвестная нода в LabeledStatementContext");
    }

    public override Node VisitLabelCase([NotNull] LCLangParser.LabelCaseContext context)
    {
      CollectionNode r = new CollectionNode();
      LabelCaseNode labelCaseNode = new LabelCaseNode(
        new LocateElement(context.Case()),
        new LocateElement(context.Case(), context.Colon()));

      labelCaseNode.AddChild(Visit(context.expression()));
      labelCaseNode.StatementTxt = GetText(context.Start.StartIndex, context.Colon().Symbol.StopIndex); //context.GetText();
      r.AddChild(labelCaseNode);
      r.AddChild(Visit(context.statement()));

      return r;
    }

    public override Node VisitLabelDefault([NotNull] LCLangParser.LabelDefaultContext context)
    {
      CollectionNode r = new CollectionNode();
      LabelDefaultNode labelDefaultNode = new LabelDefaultNode(
        new LocateElement(context.Default()),
        new LocateElement(context.Default(), context.Colon()));

      labelDefaultNode.StatementTxt = GetText(context.Start.StartIndex, context.Colon().Symbol.StopIndex);
      r.AddChild(labelDefaultNode);
      r.AddChild(Visit(context.statement()));

      return r;
    }

    public override Node VisitExpressionStatement([NotNull] LCLangParser.ExpressionStatementContext context)
    {
      var expression = context.expression();

      if (expression != null)
        return Visit(expression);

      return null;
    }

    public override Node VisitExprIdentifier([NotNull] LCLangParser.ExprIdentifierContext context)
    {
      var identifier = context.Identifier();

      string identifierString = identifier.Symbol.Text;
      LocateElement identifierStringLocate = new LocateElement(identifier);

      TerminalIdentifierNode r = new TerminalIdentifierNode(identifierString, identifierStringLocate);
      return r;
    }

    public override Node VisitExprConstant([NotNull] LCLangParser.ExprConstantContext context)
    {
      return VisitConstant(context.constant());
    }

    public override Node VisitExprBaseConstant([NotNull] LCLangParser.ExprBaseConstantContext context)
    {
      return VisitBaseConstant(context.baseConstant());
    }

    public override Node VisitExprIndex([NotNull] LCLangParser.ExprIndexContext context)
    {
      var indexer = context.arrayIndexer(); //Индекс
      var expression = context.expression(); //индекируемое выражение

      var leftBracket = indexer.LeftBracket();
      var rightBracket = indexer.RightBracket();
      var indexerExpression = indexer.expression();

      LocateElement indexerLocate = new LocateElement(leftBracket, rightBracket);

      IndexerNode r = new IndexerNode(indexerLocate, indexerLocate);
      r.IndexingObj.AddChild(Visit(expression)); //Парсим индексируемое значение
      r.Index.AddChild(Visit(indexerExpression)); //Парсим индекс
      return r;
    }

    public override Node VisitExprParens([NotNull] LCLangParser.ExprParensContext context)
    {
      return Visit(context.expression());
    }

    public override Node VisitExprCall([NotNull] LCLangParser.ExprCallContext context)
    {
      var identifier = context.Identifier(); //Имя функции
      var expressionSequence = context.expressionSequence(); //Параметры вызываемой функции

      LocateElement identifierLocate = new LocateElement(identifier);

      FunctionCallNode r = new FunctionCallNode(identifier.Symbol.Text, identifierLocate, new LocateElement(context));

      if (expressionSequence != null)
        r.Params.AddChild(VisitExpressionSequence(expressionSequence));

      return r;
    }

    public override Node VisitExpressionSequence([NotNull] LCLangParser.ExpressionSequenceContext context)
    {
      CollectionNode r = new CollectionNode();

      var expression = context.expression();

      foreach (var e in expression)
        r.AddChild(Visit(e));

      return r;
    }

    public override Node VisitExprElementAccess([NotNull] LCLangParser.ExprElementAccessContext context)
    {
      var structObjContext = context.obj;
      var fieldContext = context.objField;

      string field = fieldContext.Text;
      LocateElement fieldLocate = new LocateElement(fieldContext);
      LocateElement operatorLocate = new LocateElement(context.Arrow());

      var structNode = Visit(structObjContext);

      ElementAccessNode r = new ElementAccessNode(field, structNode,
        fieldLocate, operatorLocate);

      return r;
    }


    public override Node VisitExprPostfix([NotNull] LCLangParser.ExprPostfixContext context)
    {
      var op = context.op;
      var operand = context.operand;

      UnaryOperationNode r;

      switch (context.op.Type)
      {
        case LCLangLexer.PlusPlus:
          r = new PostfixIncrementNode(new LocateElement(op), new LocateElement(context));
          break;

        case LCLangLexer.MinusMinus:
          r = new PostfixDecrementNode(new LocateElement(op), new LocateElement(context));
          break;

        default:
          throw new Exception("Неизвестная нода в ExprPostfixContext");
      }

      r.AddChild(Visit(operand));

      return r;
    }

    public override Node VisitExprPrefix([NotNull] LCLangParser.ExprPrefixContext context)
    {
      var op = context.op;
      var operand = context.operand;

      UnaryOperationNode r;

      switch (context.op.Type)
      {
        case LCLangLexer.PlusPlus:
          r = new PrefixIncrementNode(new LocateElement(op), new LocateElement(context));
          break;

        case LCLangLexer.MinusMinus:
          r = new PrefixDecrementNode(new LocateElement(op), new LocateElement(context));
          break;

        default:
          throw new Exception("Неизвестная нода в ExprPostfixContext");
      }

      r.AddChild(Visit(operand));

      return r;
    }

    public override Node VisitExprUnary([NotNull] LCLangParser.ExprUnaryContext context)
    {
      var op = context.op;
      var operand = context.operand;

      UnaryOperationNode r;
      var opLocate = new LocateElement(op);
      var contextLocate = new LocateElement(context);

      switch (context.op.Type)
      {
        case LCLangLexer.Plus:
          return Visit(operand);

        case LCLangLexer.Minus:
          r = new NegativeNode(opLocate, contextLocate);
          break;

        case LCLangLexer.Not:
          r = new LogicNotNode(opLocate, contextLocate);
          break;

        case LCLangLexer.Tilde:
          r = new NotNode(opLocate, contextLocate);
          break;

        default:
          throw new Exception("Неизвестная нода в ExprPostfixContext");
      }

      r.AddChild(Visit(operand));

      return r;
    }

    public override Node VisitExprTypeCast([NotNull] LCLangParser.ExprTypeCastContext context)
    {
      var lcPrimitiveType = context.lcPrimitiveType(); //Тип, к которому приводится выражение
      var operand = context.operand;

      LCPrimitiveType primitiveType;
      LocateElement primitiveTypeLocate;

      ParserPrimitiveType.ParsePrimitiveType(lcPrimitiveType, out primitiveType, out primitiveTypeLocate);

      TypeCastNode r = new TypeCastNode(primitiveType, primitiveTypeLocate, new LocateElement(context));
      r.AddChild(Visit(operand));
      return r;
    }

    public override Node VisitExprInfix([NotNull] LCLangParser.ExprInfixContext context)
    {
      var left = context.left;
      var right = context.right;
      var op = context.op;


      SourceLocatedNode r;
      LocateElement opLocate = new LocateElement(op);
      LocateElement expressionLocate = new LocateElement(context);

      switch (op.Type)
      {
        case LCLangLexer.Star:
          r = new MulNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Div:
          r = new DivNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Mod:
          r = new RemNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Plus:
          r = new SummNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Minus:
          r = new SubNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.LeftShift:
          r = new LeftShiftNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.RightShift:
          r = new RightShiftNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Less:
          r = new LessNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.LessEqual:
          r = new LessEqualNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Greater:
          r = new MoreNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.GreaterEqual:
          r = new MoreEqualNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Equal:
          r = new EqualNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.NotEqual:
          r = new NotEqualNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.And:
          r = new AndNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Caret:
          r = new XorNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Or:
          r = new OrNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.AndAnd:
          r = new LogicAndNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.OrOr:
          r = new LogicOrNode(opLocate, expressionLocate);
          break;

        case LCLangLexer.Assign:
          r = new AssignNode(opLocate, expressionLocate);
          break;

        default:
          throw new Exception("Неизвестная нода в ExprInfixContext");
      }

      r.AddChild(Visit(left));
      r.AddChild(Visit(right));
      return r;
    }

    public override Node VisitConstant([NotNull] LCLangParser.ConstantContext context)
    {
      var floatConstant = context.floatConstant();
      var intConstant = context.intConstant();

      if (floatConstant != null)
        return VisitFloatConstant(floatConstant);
      else if (intConstant != null)
        return VisitIntConstant(intConstant);
      else
        throw new Exception("Неизвестная нода в ConstantContext");
    }

    public override Node VisitFloatConstant([NotNull] LCLangParser.FloatConstantContext context)
    {
      double value;
      LocateElement locate;
      ParserConstants.ParseFloatConstant(context, logger, out value, out locate);

      ConstantValueNode node;
      BuilderConstantValueNode.BuildFloatConstantNode(value, locate, logger, out node);
      return node;
    }

    public override Node VisitIntConstant([NotNull] LCLangParser.IntConstantContext context)
    {
      ulong value;
      LocateElement locate;
      if (ParserConstants.ParseIntConstant(context, logger, out value, out locate) == false)
        BuildOK = false;

      ConstantValueNode node;
      if (BuilderConstantValueNode.BuildIntConstantNode(value, locate, logger, out node) == false)
        BuildOK = false;

      return node;
    }

    public override Node VisitBaseConstant([NotNull] LCLangParser.BaseConstantContext context)
    {
      var valueFalse = context.False();
      var valueTrue = context.True();

      ConstantValue c;
      LocateElement locate;

      if (valueFalse != null)
      {
        c = new BooleanConstantValue(false);
        locate = new LocateElement(valueFalse);
      }
      else if (valueTrue != null)
      {
        c = new BooleanConstantValue(true);
        locate = new LocateElement(valueTrue);
      }
      else
        throw new InternalCompilerException("Неизвестный тип константы");

      return new ConstantValueNode(c, locate);
    }

    string GetText(ParserRuleContext context)
    {
      int a = context.Start.StartIndex;
      int b = context.Stop.StopIndex;

      return Stream.GetText(new Interval(a, b));
    }

    string GetText(int a, int b)
    {
      return Stream.GetText(new Interval(a, b));
    }
  }
}
