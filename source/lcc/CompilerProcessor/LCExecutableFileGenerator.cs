using System;
using System.Collections.Generic;

namespace LC2.LCCompiler
{
  internal class PLCVariableDeclaration
  {
    public ushort ID { get; }
    public int Address { get; }
    public string Name { get; }


    public PLCVariableDeclaration(ushort id, int address, string name)
    {
      ID = id;
      Address = address;
      Name = name;
    }
  }

  internal class PLCFunctionDeclaration
  {
    public ushort ID { get; }
    public int Address { get; }
    public string Name { get; }

    public PLCFunctionDeclaration(ushort id, int address, string name)
    {
      ID = id;
      Address = address;
      Name = name;
    }
  }

  internal static class LCExecutableFileGenerator
  {
    /// <summary>
    /// Генерация исполняемого файла
    /// </summary>
    /// <param name="code">Дамп исполняемого кода</param>
    /// <param name="functionRecords">Функции</param>
    /// <param name="variableRecords">I/O-переменные</param>
    /// <returns>Дамп исполняемого файла</returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static byte[] Generate(byte[] code,
      PLCFunctionDeclaration[] functionRecords,
      PLCVariableDeclaration[] variableRecords)
    {
      int buildUID = new Random().Next(int.MinValue, int.MaxValue);

      //Размер заголовка секции состоит из типа секции и размера секции
      int sectionHeaderSize = sizeof(byte) + sizeof(uint);

      //Размер заголовка дампа + uid типа контроллера + buildUID
      int dumpHeaderSize = sizeof(uint) + sizeof(ushort) + sizeof(uint);

      int sectionVariableRecordsSize = sectionHeaderSize + (variableRecords.Length * (sizeof(ushort) + sizeof(uint))); //Размер секции variableRecords
      int sectionFunctionRecordsSize = sectionHeaderSize + (functionRecords.Length * (sizeof(ushort) + sizeof(uint))); //Размер секции functionRecords
      int sectionCodeSize = sectionHeaderSize + code.Length;  //Размер секции code
      int dumpCRCSize = sizeof(uint);  //Размер crc32 дампа

      int totalSize = dumpHeaderSize
        + sectionVariableRecordsSize
        + sectionFunctionRecordsSize
        + sectionCodeSize
        + dumpCRCSize;


      byte[] binaryDump = new byte[totalSize]; //создаем буфер дампа

      int offset = 0;

      /*** Заголовок дампа ***/
      BitConverter.GetBytes(totalSize).CopyTo(binaryDump, offset); //Размер дампа
      offset += sizeof(uint);

      BitConverter.GetBytes((ushort)0x01).CopyTo(binaryDump, offset); //Тип контроллера
      offset += sizeof(ushort);

      BitConverter.GetBytes(buildUID).CopyTo(binaryDump, offset); //UID сборки
      offset += sizeof(uint);

      /*** Секция variableRecords ***/

      //id секции variableRecords = 0x01
      binaryDump[offset] = 0x01;
      offset += 1;

      //Размер полезных данных секции
      BitConverter.GetBytes((uint)(sectionVariableRecordsSize - sectionHeaderSize)).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      foreach (var record in variableRecords)
      {
        BitConverter.GetBytes(record.ID).CopyTo(binaryDump, offset);
        offset += sizeof(ushort);

        BitConverter.GetBytes((uint)record.Address).CopyTo(binaryDump, offset);
        offset += sizeof(uint);
      }

      /*** Секция functionRecords ***/

      //id секции functionRecords = 0x02
      binaryDump[offset] = 0x02;
      offset += 1;

      //Размер полезных данных секции
      BitConverter.GetBytes((uint)(sectionFunctionRecordsSize - sectionHeaderSize)).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      foreach (var record in functionRecords)
      {
        BitConverter.GetBytes(record.ID).CopyTo(binaryDump, offset);
        offset += sizeof(ushort);

        BitConverter.GetBytes((uint)record.Address).CopyTo(binaryDump, offset);
        offset += sizeof(uint);
      }

      /*** Секция code ***/

      //id секции code = 0x00
      binaryDump[offset] = 0x00;
      offset += 1;

      //Размер полезных данных секции
      BitConverter.GetBytes((uint)code.Length).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      code.CopyTo(binaryDump, offset);
      offset += code.Length;

      /*** crc дампа ***/

      //Пока crc не рассчитываем
      BitConverter.GetBytes((uint)0).CopyTo(binaryDump, offset);

      return binaryDump;
    }


#if false
    internal static byte[] Generate(byte[] code, PLCFunctionDeclaration[] functionRecords, PLCVariableDeclaration[] variableRecords)
    {
      if (variableRecords == null || functionRecords == null || code == null)
      {
        throw new ArgumentNullException("Input data cannot be null");
      }

      // Рассчитываем размеры записей
      const int recordSize = sizeof(ushort) + sizeof(uint); // ID (ushort) + Address (uint)

      // Рассчитываем смещения
      const int headerSize = sizeof(uint) * 3; // execSectionOffset + variableSectionOffset + functionSectionOffset
      int variableSectionOffset = headerSize;
      int functionSectionOffset = variableSectionOffset + sizeof(uint) + (recordSize * variableRecords.Length);
      int execSectionOffset = functionSectionOffset + sizeof(uint) + (recordSize * functionRecords.Length);

      // Вычисляем полный размер дампа
      int dumpSize = execSectionOffset + sizeof(uint) + code.Length;

      // Создаём массив для дампа
      byte[] binaryDump = new byte[dumpSize];
      int offset = 0;

      // Записываем execSectionOffset
      BitConverter.GetBytes(execSectionOffset).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      // Записываем variableSectionOffset
      BitConverter.GetBytes(variableSectionOffset).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      // Записываем functionSectionOffset
      BitConverter.GetBytes(functionSectionOffset).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      // Записываем variable_section
      BitConverter.GetBytes((uint)variableRecords.Length).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      foreach (var record in variableRecords)
      {
        BitConverter.GetBytes(record.ID).CopyTo(binaryDump, offset);
        offset += sizeof(ushort);

        BitConverter.GetBytes((uint)record.Address).CopyTo(binaryDump, offset);
        offset += sizeof(uint);
      }

      // Записываем function_section
      BitConverter.GetBytes((uint)functionRecords.Length).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      foreach (var record in functionRecords)
      {
        BitConverter.GetBytes(record.ID).CopyTo(binaryDump, offset);
        offset += sizeof(ushort);

        BitConverter.GetBytes((uint)record.Address).CopyTo(binaryDump, offset);
        offset += sizeof(uint);
      }

      // Записываем exec_section
      BitConverter.GetBytes((uint)code.Length).CopyTo(binaryDump, offset);
      offset += sizeof(uint);

      code.CopyTo(binaryDump, offset);

      return binaryDump;
    }
#endif
  }
}
