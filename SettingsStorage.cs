////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           SettingsStorage.cs
//
//  Facility:       Хранение настроек.
//
//
//  Abstract:       Модуль содержит определение класса, выполняющего
//                  сохранение и загрузку настроек. 
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  13/09/2005
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: SettingsStorage.cs $
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 1.12.06    Time: 20:38
 * Updated in $/GPSTracing.root/Src/Lightcom.Common
 * Отменены изменения, связанные с отделением UI от логики приложения
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 22.11.06   Time: 20:50
 * Updated in $/GPSTracing.root/Src/Lightcom.Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Globalization;
namespace LightCom.Common
{
                              
/// <summary>
/// Содержит методы для соранения/загрузки именованных параметров.
/// Каждый параметр имеет тип, имя и значение. Поддерживаются только
/// текстовые значения. 
/// </summary>

public abstract class SettingsStorage 
{
    /// <summary>
    /// Выполняет инициализацию объекта - все данные сбрасываются в 
    /// начальное состояние.
    /// </summary>
    public abstract void Reset ();

    /// <summary>
    /// Если реализация интерфейса поддерживает буферизацию записи, то
    /// метод сбрасывает содержимое буфера в реальное хранилище.
    /// </summary>
    /// <returns>
    /// Возвращает true, если сброс кэша прошел успешно.
    /// </returns>
    public abstract bool Flush ();

    /// <summary>
    /// Выполняет инициализацию, необходимую для загрузки данных.
    /// </summary>
    /// <returns>
    /// Возвращает true, если инициализация прошла успешно.
    /// </returns>
    public abstract bool PreLoad ();

    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public abstract bool Write (string strType, 
                                string strName, 
                                string value);
    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               int value)
    {
        return Write (strType, strName, value.ToString ()); 
    }

    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <param name="provider">Формат для преобразования в строку</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               double value,
                               NumberFormatInfo provider)
    {
        string strValue = Convert.ToString (value, provider);
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               double value)
    {
        return Write (strType, strName, value, SettingsStorage.provider);
    }

    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               DateTime value)
    {
        string strValue = value.ToString ("u");
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// Сохраняет параметр.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">IN Значение параметра</param>
    /// <returns>Возвращает true, если запись прошла успешно.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               TimeSpan value)
    {
        string strValue = value.ToString ();
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public abstract bool Read (string strType, 
                                  string strName, 
                                  out string value);

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                                 string strName,
                                 out string value,
                                 string defaulint)
    {
        if (! Read (strType, strName, out value))
        {
            value = defaulint;
        }
    }
        
    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                              string strName,
                              out int value,
                              int defaulint)
    {
        if (!Read (strType, strName, out value))
        {
            value = defaulint;
        }
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                              string strName,
                              out double value,
                              double defaulint)
    {
        if (!Read (strType, strName, SettingsStorage.provider, out value))
        {
            value = defaulint;
        }
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="provider">Формат хранения чисел</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                              string strName,
                              out double value,
                              NumberFormatInfo provider,
                              double defaulint)
    {
        if (!Read (strType, strName, provider, out value))
        {
            value = defaulint;
        }
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                              string strName,
                              out DateTime value,
                              DateTime defaulint)
    {
        if (!Read (strType, strName, out value))
        {
            value = defaulint;
        }
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="defaulint">Значение по-умолчанию</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual void Read (string strType,
                              string strName,
                              out TimeSpan value,
                              TimeSpan defaulint)
    {
        if (!Read (strType, strName, out value))
        {
            value = defaulint;
        }
    }
   

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              out int value)
    {
        value = 0;
        string strValue;
        if (!Read (strType, strName, out strValue))
        {
            return false;
        }
        try
        {
            value = Convert.ToInt32 (strValue);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <param name="provider">Формат хранения чисел</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              NumberFormatInfo provider,
                              out double value)
    {
        value = double.NaN;
        string strValue;
        if (!Read (strType, strName, out strValue))
        {
            return false;
        }
        try
        {
            value = Convert.ToDouble (strValue, provider);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              out double value)
    {
        return Read (strType, strName, provider, out value);
    }
    
    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              out DateTime value)
    {
        value = DateTime.MinValue;
        string strValue;
        if (!Read (strType, strName, out strValue))
        {
            return false;
        }
        try
        {
            value = DateTime.ParseExact (strValue, 
                                         "u", 
                                         CultureInfo.InvariantCulture );
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Читает значение.
    /// </summary>
    /// <param name="strType">IN Название типа/категории параметра</param>
    /// <param name="strName">IN Название параметра</param>
    /// <param name="value">OUT Значение параметра</param>
    /// <returns>Возвращает true, если чтение прошло успешно.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              out TimeSpan value)
    {
        value = TimeSpan.MinValue;
        string strValue;
        if (!Read (strType, strName, out strValue))
        {
            return false;
        }
        try
        {
            value = TimeSpan.Parse (strValue);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Статический конструктор
    /// </summary>
    static SettingsStorage ()
    {
        provider = new NumberFormatInfo ();
        provider.NumberDecimalSeparator = ",";
        provider.NumberGroupSeparator = " ";
    }

    /// <summary>
    /// Формат по-умолчанию хранения чисел
    /// </summary>
    protected static NumberFormatInfo provider;
}
}