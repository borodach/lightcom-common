////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           SettingsStorage.cs
//
//  Facility:       �������� ��������.
//
//
//  Abstract:       ������ �������� ����������� ������, ������������
//                  ���������� � �������� ��������. 
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
//
//  Creation Date:  13/09/2005
//
//  Copyright (C) OOO "�������", 2005-2006. ��� ����� ��������.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: SettingsStorage.cs $
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 1.12.06    Time: 20:38
 * Updated in $/GPSTracing.root/Src/Lightcom.Common
 * �������� ���������, ��������� � ���������� UI �� ������ ����������
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
/// �������� ������ ��� ���������/�������� ����������� ����������.
/// ������ �������� ����� ���, ��� � ��������. �������������� ������
/// ��������� ��������. 
/// </summary>

public abstract class SettingsStorage 
{
    /// <summary>
    /// ��������� ������������� ������� - ��� ������ ������������ � 
    /// ��������� ���������.
    /// </summary>
    public abstract void Reset ();

    /// <summary>
    /// ���� ���������� ���������� ������������ ����������� ������, ��
    /// ����� ���������� ���������� ������ � �������� ���������.
    /// </summary>
    /// <returns>
    /// ���������� true, ���� ����� ���� ������ �������.
    /// </returns>
    public abstract bool Flush ();

    /// <summary>
    /// ��������� �������������, ����������� ��� �������� ������.
    /// </summary>
    /// <returns>
    /// ���������� true, ���� ������������� ������ �������.
    /// </returns>
    public abstract bool PreLoad ();

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public abstract bool Write (string strType, 
                                string strName, 
                                string value);
    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               int value)
    {
        return Write (strType, strName, value.ToString ()); 
    }

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <param name="provider">������ ��� �������������� � ������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               double value,
                               NumberFormatInfo provider)
    {
        string strValue = Convert.ToString (value, provider);
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               double value)
    {
        return Write (strType, strName, value, SettingsStorage.provider);
    }

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               DateTime value)
    {
        string strValue = value.ToString ("u");
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// ��������� ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">IN �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Write (string strType,
                               string strName,
                               TimeSpan value)
    {
        string strValue = value.ToString ();
        return Write (strType, strName, strValue);
    }

    /// <summary>
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public abstract bool Read (string strType, 
                                  string strName, 
                                  out string value);

    /// <summary>
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="provider">������ �������� �����</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="defaulint">�������� ��-���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <param name="provider">������ �������� �����</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
    public virtual bool Read (string strType,
                              string strName,
                              out double value)
    {
        return Read (strType, strName, provider, out value);
    }
    
    /// <summary>
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ������ ��������.
    /// </summary>
    /// <param name="strType">IN �������� ����/��������� ���������</param>
    /// <param name="strName">IN �������� ���������</param>
    /// <param name="value">OUT �������� ���������</param>
    /// <returns>���������� true, ���� ������ ������ �������.</returns>
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
    /// ����������� �����������
    /// </summary>
    static SettingsStorage ()
    {
        provider = new NumberFormatInfo ();
        provider.NumberDecimalSeparator = ",";
        provider.NumberGroupSeparator = " ";
    }

    /// <summary>
    /// ������ ��-��������� �������� �����
    /// </summary>
    protected static NumberFormatInfo provider;
}
}