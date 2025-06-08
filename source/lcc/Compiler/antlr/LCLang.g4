grammar LCLang;

// entry point
compilationUnit
	: useDirectives? compilationUnitElement* EOF
	;

compilationUnitElement
	: varableDeclarationStatement
	| structDeclarationStatement
	| functionDeclaration
	;

///
/// func Definition
///

functionDeclaration 
	: lcPrimitiveType Identifier LeftParen functionParameters? RightParen compoundStatement
	;

functionParameters : functionParameter (',' functionParameter)* ; 

functionParameter
	: lcFunctionParamType Identifier
	;
///

/*
 * тело функции, условия или итератора
 */
compoundStatement 
	: LeftBrace blockItemList? RightBrace
	; 

blockItemList
	: blockItem+
	;

blockItem 
	: statement 
	| varableDeclarationStatement; 


statement 
	: labeledStatement
	| compoundStatement
	| expressionStatement
	| selectionStatement
	| iterationStatement
	| jumpStatement
	;

labeledStatement
	: labelCase
	| labelDefault
	/* | labelIdentifier */
	;

/*
labelIdentifier
	: Identifier Colon statement
	;
*/
	
labelCase
	: Case expression Colon statement
	;

labelDefault
	: Default Colon statement
	;

selectionStatement 
	: selectionIFStatement
	| selectionSwitchStatement
	; 

selectionIFStatement 
	:  If LeftParen condition=expression RightParen iftrue=statement ('else' iffalse=statement)?
	;

selectionSwitchStatement
	: Switch LeftParen expression RightParen compoundStatement
	;

iterationStatement 
	: iterationWhileStatement
	| iterationDoStatement
	| iterationForStatement
	; 

iterationWhileStatement
	: While LeftParen condition=expression RightParen body=statement 
	;

iterationDoStatement
	: Do body=statement While LeftParen condition=expression RightParen ';' 
	;

iterationForStatement
	: For LeftParen condition=forSections RightParen body=statement
	;

forSections 
	: exprInit=forInitializer? ';' exprCond=forCondition? ';' exprLoop=forIteratorExpression? 
	; 

forInitializer
	: forInitializerItem (',' forInitializerItem)?
	;

forInitializerItem
	: expression
	| varableDeclaration
	;

forCondition
	: expression
	;

forIteratorExpression
	: forIteratorExpressionItem (',' forIteratorExpressionItem)?
	;

forIteratorExpressionItem
	: expression
	;

/*
jumpStatement 
	: 'return' expr=expression? ';'  //('goto' Identifier | ('continue'| 'break') | 'return' expression?) ';' 
	;  
*/

jumpStatement
	:  (jumpContinue | jumpBreak | jumpReturn) ';'
	;

jumpContinue
	: Continue
	;
	
jumpBreak
	: Break
	;
	
jumpReturn
	: Return expression?
	;
	

/*
 * оператор use
 */
 
useDirectives 
	:	useDirective+
	;
	
useDirective
	: Use StringLiteral ';'?
	;
	
objectPath
	: Identifier ('.' Identifier)*
	;

structDeclarationStatement
	: structDeclaration ';'
	;

structDeclaration
	: Struct Identifier LeftBrace structDeclarationElements RightBrace
	;

structDeclarationElements
	: structDeclarationElement+ 
	;

structDeclarationElement
	: lcStructElementType Identifier ';'
	;

/*
 * Обявление переменных
 */

varableDeclarationStatement
	: varableDeclaration ';'
	;

varableDeclaration
	: varableDeclarator (Assign initializer)? attributeSpecifier?
	;

attributeSpecifier
    : At StringLiteral
    ;
 
varableDeclarator 
	: lcVariableType Identifier
	;
	
	
initializer
	: expression
	;
	
groupInitializer
	: '{' groupInitializerItems ','? '}'
	;

groupInitializerItems
	: groupInitializerItem (',' groupInitializerItem)*
	;

groupInitializerItem
	: expression
	//| unionItemInit
	;

// Тип данных элемента структуры
lcStructElementType
	: lcPrimitiveType
	| lcArrayPrimitiveType
	;

//Тип данных параметра функции
lcFunctionParamType
	: lcPrimitiveType
	| lcRefArrayPrimitiveType
	| lcUserType
	;

//Тип данных переменной
lcVariableType
	: lcPrimitiveType
	| lcArrayPrimitiveType
	| lcUserType
	;

//Примитивные типы
lcPrimitiveType
	: TypeName = 
	 (SByte
	| Short
	| Int
	| Long
	| Byte
	| UShort
	| UInt
	| ULong
	| Float
	| Double
	| Bool
	| Void) 
	;

lcRefArrayPrimitiveType
	: lcPrimitiveType LeftBracket RightBracket
	;
	
lcArrayPrimitiveType
	: lcPrimitiveType LeftBracket intConstant RightBracket
	;

//Пользовательские типы
lcUserType
	: Identifier
	;

arrayIndexer
	: LeftBracket expression RightBracket
	;
	
  /*
tupleType
	: LeftParen tupleElements RightParen
	;

tupleElements
	: lcPrimitiveType (',' lcPrimitiveType)*
	;
  */
/*
 * Выражения 
 */

expressionSequence
	: expression (',' expression)*
	;

expressionStatement 
	: expr=expression? ';' 
	;
	
expression
	: Identifier														#exprIdentifier
  /*| module=Identifier Dot obj=Identifier								#exprExternalObjectAccess */
	| constant															#exprConstant
	| baseConstant														#exprBaseConstant
	
	| '(' expression ')'												#exprParens
	| operand=expression op=('++' | '--')								#exprPostfix
	| Identifier LeftParen expressionSequence? RightParen     			#exprCall
	| expression arrayIndexer											#exprIndex
    | obj=expression Arrow objField=Identifier							#exprElementAccess
	| op=('++' | '--') operand=expression								#exprPrefix
	| op=('+' | '-') operand=expression									#exprUnary
	| op=('!' | '~') operand=expression									#exprUnary
	| LeftParen lcPrimitiveType RightParen operand=expression			#exprTypeCast
	| left=expression op=('*' | '/' | '%') right=expression				#exprInfix
	| left=expression op=('+' | '-') right=expression					#exprInfix
	| left=expression op=('<<' | '>>') right=expression					#exprInfix
	| left=expression op=('<' | '<=' | '>' | '>=') right=expression 	#exprInfix
	| left=expression op=('==' | '!=') right=expression					#exprInfix
	| left=expression op='&' right=expression							#exprInfix
	| left=expression op='^' right=expression							#exprInfix
	| left=expression op='|' right=expression							#exprInfix
	| left=expression op='&&' right=expression							#exprInfix
	| left=expression op='||' right=expression							#exprInfix
	| left=expression op='=' right=expression							#exprInfix	
	; 

/*
globalObjVisibilityModifiers
	: Public
	| Private
	;

variableVisibilityModifiers
	: Public
	| Private
	| Static
	;
*/
varMemoryLocation
	: Const
	;

baseConstant
	: False
	| True
	;

constant
	: floatConstant
	| intConstant
	;
	
intConstant
	: DecimalConstant
	| OctalConstant
	| HexadecimalConstant
	| BinaryConstant
	;
	
floatConstant
	: FloatingConstant
	;

/*
constant
	:   integerConstant
	|   FloatingConstant
	|   CharacterConstant
	;

integerConstant
	:   DecimalConstant
	|   OctalConstant 
	|   HexadecimalConstant
	|	BinaryConstant
	;
*/


SByte : 'sbyte';
Short : 'short';
Int : 'int';
Long : 'long';

Byte : 'byte';
UShort : 'ushort';
UInt : 'uint';
ULong : 'ulong';

Float : 'float';
Double : 'double';
Bool : 'bool';
Void : 'void';

False : 'false';
True : 'true'; 
//Null : 'null';

Break : 'break';
Case : 'case';
Const : 'const';
Continue : 'continue';
Default : 'default';
Do : 'do';
Else : 'else';
For : 'for';
Goto : 'goto';
If : 'if';




Native : 'native';
Return : 'return';
//Public: 'public';
//Private: 'private';
//Static : 'static';
Struct : 'struct';
Switch : 'switch';
Use : 'use';
While : 'while';

Underscore : '_';

LeftParen : '(';
RightParen : ')';
LeftBracket : '[';
RightBracket : ']';
LeftBrace : '{';
RightBrace : '}';

Less : '<';
LessEqual : '<=';
Greater : '>';
GreaterEqual : '>=';
LeftShift : '<<';
RightShift : '>>';

Plus : '+';
PlusPlus : '++';
Minus : '-';
MinusMinus : '--';
Star : '*';
Div : '/';
Mod : '%';

And : '&';
Or : '|';
AndAnd : '&&';
OrOr : '||';
Caret : '^';
Not : '!';
Tilde : '~';

Semi : ';';
Comma : ',';

Colon : ':';

Ref : 'ref';

Assign : '=';
StarAssign : '*=';
DivAssign : '/=';
ModAssign : '%=';
PlusAssign : '+=';
MinusAssign : '-=';
LeftShiftAssign : '<<=';
RightShiftAssign : '>>=';
AndAssign : '&=';
XorAssign : '^=';
OrAssign : '|=';

Equal : '==';
NotEqual : '!=';

Dot : '.';

Arrow : '->';

At : '@' ;

Identifier
	:   IdentifierNondigit (IdentifierNondigit | Digit)*
	;

fragment
IdentifierNondigit
	:   Nondigit
	/*|   UniversalCharacterName*/
	;

fragment
Nondigit
	:   [a-zA-Z_]
	;

fragment
Digit
	:   [0-9]
	;

fragment
UniversalCharacterName
	:   '\\u' HexQuad
	|   '\\U' HexQuad HexQuad
	;

fragment
HexQuad
	:   HexadecimalDigit HexadecimalDigit HexadecimalDigit HexadecimalDigit
	;

/*
Constant
	:   IntegerConstant
	|   FloatingConstant
	//|   EnumerationConstant
	|   CharacterConstant
	;
*/

/*
fragment
IntegerConstant
	:   DecimalConstant IntegerSuffix?
	|   OctalConstant IntegerSuffix?
	|   HexadecimalConstant IntegerSuffix?
	|	BinaryConstant
	;
*/



//fragment
BinaryConstant
	:	'0' [bB] [0-1]+
	;

//fragment
DecimalConstant
	:   NonzeroDigit Digit*
	;

//fragment
OctalConstant
	:   '0' OctalDigit*
	;

//fragment
HexadecimalConstant
	:   HexadecimalPrefix HexadecimalDigit+
	;

fragment
HexadecimalPrefix
	:   '0' [xX]
	;

fragment
NonzeroDigit
	:   [1-9]
	;

fragment
OctalDigit
	:   [0-7]
	;

fragment
HexadecimalDigit
	:   [0-9a-fA-F]
	;
	
/*
fragment
IntegerSuffix
	:   UnsignedSuffix LongSuffix?
	|   UnsignedSuffix LongLongSuffix
	|   LongSuffix UnsignedSuffix?
	|   LongLongSuffix UnsignedSuffix?
	;

fragment
UnsignedSuffix
	:   [uU]
	;

fragment
LongSuffix
	:   [lL]
	;

fragment
LongLongSuffix
	:   'll' | 'LL'
	;
*/

/*
fragment
FloatingConstant
	:   DecimalFloatingConstant
	|   HexadecimalFloatingConstant
	;
*/


FloatingConstant
	:   DecimalFloatingConstant
	;

/*
fragment
DecimalFloatingConstant
	:   FractionalConstant ExponentPart? FloatingSuffix?
	|   DigitSequence ExponentPart FloatingSuffix?
	;
*/
fragment
DecimalFloatingConstant
	:   FractionalConstant ExponentPart? 
	|   DigitSequence ExponentPart 
	;

/*
fragment
HexadecimalFloatingConstant
	:   HexadecimalPrefix (HexadecimalFractionalConstant | HexadecimalDigitSequence) BinaryExponentPart FloatingSuffix?
	;
*/
fragment
FractionalConstant
	:   DigitSequence? '.' DigitSequence
	|   DigitSequence '.'
	;

fragment
ExponentPart
	:   [eE] Sign? DigitSequence
	;

fragment
Sign
	:   [+-]
	;

DigitSequence
	:   Digit+
	;
/*
fragment
HexadecimalFractionalConstant
	:   HexadecimalDigitSequence? '.' HexadecimalDigitSequence
	|   HexadecimalDigitSequence '.'
	;
*/
/*
fragment
BinaryExponentPart
	:   [pP] Sign? DigitSequence
	;
*/
/*
fragment
HexadecimalDigitSequence
	:   HexadecimalDigit+
	;
*/
/*
fragment
FloatingSuffix
	:   [flFL]
	;
*/

/*
fragment
CharacterConstant
	:   '\'' CCharSequence '\''
	|   'L\'' CCharSequence '\''
	|   'u\'' CCharSequence '\''
	|   'U\'' CCharSequence '\''
	;
 */
 
CharacterConstant
	:   '\'' CCharSequence '\''
	;
 
fragment
CCharSequence
	:   CChar+
	;

fragment
CChar
	:   ~['\\\r\n]
	|   EscapeSequence
	;

fragment
EscapeSequence
	:   SimpleEscapeSequence
	|   OctalEscapeSequence
	|   HexadecimalEscapeSequence
	|   UniversalCharacterName
	;

fragment
SimpleEscapeSequence
	:   '\\' ['"?abfnrtv\\]
	;

fragment
OctalEscapeSequence
	:   '\\' OctalDigit OctalDigit? OctalDigit?
	;

fragment
HexadecimalEscapeSequence
	:   '\\x' HexadecimalDigit+
	;

/*
StringLiteral
	:   EncodingPrefix? '"' SCharSequence? '"'
	;
*/

StringLiteral
	:   '"' SCharSequence? '"'
	;

/*
fragment
EncodingPrefix
	:   'u8'
	|   'u'
	|   'U'
	|   'L'
	;
*/
fragment
SCharSequence
	:   SChar+
	;

fragment
SChar
	:   ~["\\\r\n]
	|   EscapeSequence
	|   '\\\n'   // Added line
	|   '\\\r\n' // Added line
	;

Whitespace
	:   [ \t]+
		-> skip
	;

Newline
	:   (   '\r' '\n'?
		|   '\n'
		)
		-> skip
	;

BlockComment
	:   '/*' .*? '*/'
		-> skip
	;

LineComment
	:   '//' ~[\r\n]*
		-> skip
	;
	