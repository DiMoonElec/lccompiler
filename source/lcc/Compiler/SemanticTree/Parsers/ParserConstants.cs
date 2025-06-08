using System;
using System.Globalization;
using static LCLangParser;
using static System.Net.Mime.MediaTypeNames;

namespace LC2.LCCompiler.Compiler.SemanticTree.Parsers
{
  static class ParserConstants
  {
    public static bool ParseIntConstant(IntConstantContext context,
      CompilerLogger logger,
      out ulong value, out LocateElement locate)
    {
      var decimalConstant = context.DecimalConstant();
      var octalConstant = context.OctalConstant();
      var hexadecimalConstant = context.HexadecimalConstant();
      var binaryConstant = context.BinaryConstant();

      if (decimalConstant != null)
        return ParseDecimalConstant(decimalConstant, logger, out value, out locate);
      else if (octalConstant != null)
        return ParseOctalConstant(octalConstant, logger, out value, out locate);
      else if (hexadecimalConstant != null)
        return ParseHexConstant(hexadecimalConstant, logger, out value, out locate);
      else if (binaryConstant != null)
        return ParseBinConstant(binaryConstant, logger, out value, out locate);
      else
      {
        throw new Exception("Неизвестный тип целночисленной константы");
      }
    }

    public static void ParseFloatConstant(FloatConstantContext context,
      CompilerLogger logger,
      out double value, out LocateElement locate)
    {
      NumberFormatInfo formatProvider =
        new NumberFormatInfo() { NumberDecimalSeparator = "." };

      var floatingConstant = context.FloatingConstant();

      string strValue = floatingConstant.Symbol.Text;

      try
      {
        value = Convert.ToDouble(strValue, formatProvider);
      }
      catch
      {
        throw new Exception("Неверный формат константы с плавающей точкой: " + strValue);
      }

      locate = new LocateElement(floatingConstant);
    }

    private static bool ParseBinConstant(Antlr4.Runtime.Tree.ITerminalNode binaryConstant, CompilerLogger logger, out ulong value, out LocateElement locate)
    {
      string strValue = binaryConstant.Symbol.Text;
      locate = new LocateElement(binaryConstant);

      try
      {
        value = Convert.ToUInt64(strValue.Substring(2, strValue.Length - 2), 2);
        return true;

      }
      catch
      {
        logger.Error(locate,
          string.Format("Неверный формат двоичной константы: \"{0}\"", strValue));

        value = 0;
        return false;
      }
    }

    private static bool ParseHexConstant(Antlr4.Runtime.Tree.ITerminalNode hexadecimalConstant, CompilerLogger logger, out ulong value, out LocateElement locate)
    {
      string strValue = hexadecimalConstant.Symbol.Text;
      locate = new LocateElement(hexadecimalConstant);

      try
      {
        value = Convert.ToUInt64(strValue.Substring(2, strValue.Length - 2), 16);
        return true;
      }
      catch
      {
        logger.Error(locate,
          string.Format("Неверный формат шестнадцатиричной константы: \"{0}\"", strValue));

        value = 0;
        return false;
      }
    }

    private static bool ParseOctalConstant(Antlr4.Runtime.Tree.ITerminalNode octalConstant, CompilerLogger logger, out ulong value, out LocateElement locate)
    {
      string strValue = octalConstant.Symbol.Text;
      locate = new LocateElement(octalConstant);

      try
      {
        value = Convert.ToUInt64(strValue, 8);
        return true;
      }
      catch
      {
        logger.Error(locate,
          string.Format("Неверный формат восьмиричной константы: \"{0}\"", strValue));

        value = 0;
        return false;
      }
    }

    private static bool ParseDecimalConstant(Antlr4.Runtime.Tree.ITerminalNode decimalConstant, CompilerLogger logger, out ulong value, out LocateElement locate)
    {
      string strValue = decimalConstant.Symbol.Text;
      locate = new LocateElement(decimalConstant);

      try
      {
        value = Convert.ToUInt64(strValue);
        return true;
      }
      catch
      {
        logger.Error(locate,
          string.Format("Неверный формат децимальной константы: \"{0}\"", strValue));

        value = 0;
        return false;
      }
    }
  }
}
