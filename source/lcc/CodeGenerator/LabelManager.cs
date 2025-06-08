using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LC2.LCCompiler.CodeGenerator
{
  internal class LabelManager
  {
    public List<CodeLabel> Labels = new List<CodeLabel>();

    /// <summary>
    /// Объявление метки в исходном коде программы
    /// </summary>
    /// <param name="name">Имя объявляемой метки</param>
    public CodeLabel Declaration(string name)
    {
      var label = Find(name);
      if (label == null) //Если метка не была объявлена ранее
      {
        //Создаем объект до его объявления, параметры объекта будут заполнены позже
        label = new CodeLabel(name);

        //Добавляем объект в список глобальных объектов
        Labels.Add(label);
      }

      label.Declared = true;

      return label;
    }

    public CodeLabelReference AddReference(string labelName)
    {
      var label = Find(labelName);
      if (label == null) //Если объект не был объявлен ранее
      {
        //Создаем объект до его объявления, параметры объекта будут заполнены позже
        label = new CodeLabel(labelName);

        //Добавляем объект в список глобальных объектов
        Labels.Add(label);
      }

      CodeLabelReference reference = new CodeLabelReference(labelName);
      //Присваиваем reference ссылку на объект, на который она ссылается
      reference.SetBackLink(label);

      //Добавляем ссылку в список ссылок данной метки
      label.AddReference(reference);

      return reference;
    }

    CodeLabel Find(string name)
    {
      foreach (var e in Labels)
      {
        if (e.LabelName == name)
          return e;
      }

      return null;
    }

    /*
    public void Merge(LabelManager labelManager)
    {
      foreach(var l in labelManager.Labels)

      //Labels.AddRange(labelManager.Labels);
    }
    */
  }


  internal class CodeLabel
  {
    /// <summary>
    /// Имя объекта
    /// </summary>
    public string LabelName { get; private set; }

    /// <summary>
    /// Объявлена ли эта метка
    /// </summary>
    public bool Declared { get; set; }

    /// <summary>
    /// Адрес объекта. Присваивается в процессе распределения памяти для глобальных объектов
    /// </summary>
    public int Address { get; set; }

    /// <summary>
    /// Ссылки в исходном коде на данный объект
    /// </summary>
    List<CodeLabelReference> References = new List<CodeLabelReference>();

    /// <summary>
    /// Этот конструктор используется в случае, если объект был использован до его объявления,
    /// и в этом случае мы знаем только имя объекта
    /// </summary>
    /// <param name="name"></param>
    public CodeLabel(string name)
    {
      LabelName = name;
      Declared = false;
    }

    /// <summary>
    /// Добавить ссылку на данный объект
    /// </summary>
    /// <param name="reference">Ссылка</param>
    public void AddReference(CodeLabelReference reference)
    {
      References.Add(reference);
    }
  }

  internal class CodeLabelReference
  {
    /// <summary>
    /// Адрес в программе, где упоминается ссылка на метку с именем LabelName
    /// </summary>
    public int Address
    {
      get
      {
        if (Label == null)
          return 0;
        return Label.Address;
      }
    }

    /// <summary>
    /// Имя объекта, на который указывает ссылка
    /// </summary>
    public string LabelName { get; private set; }

    /// <summary>
    /// Метка, на которую указывает данная ссылка
    /// </summary>
    CodeLabel Label = null;

    public CodeLabelReference(string labelName)
    {
      LabelName = labelName;
    }

    public void SetBackLink(CodeLabel label)
    {
      Label = label;
    }
  }


}
