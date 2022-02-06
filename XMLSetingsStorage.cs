////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           XMLSetingsStorage.cs
//
//  Facility:       ���������� ��������.
//
//
//  Abstract:       ������ �������� ����������� ������ XMLSetingsStorage, 
//                  ������������ ������� ��������� � XML �����.
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
 * $History: XMLSetingsStorage.cs $
 * 
 * *****************  Version 11  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:20
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 10  *****************
 * User: Sergey       Date: 22.11.06   Time: 20:50
 * Updated in $/GPSTracing.root/Src/Lightcom.Common
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:05
 * Updated in $/gps/Lightcom.Common
 * ��������� �������� ������� �������� ��� ������.
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:07
 * Updated in $/gps/Lightcom.Common
 * �������� ����������, ����������� Flush
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 * *****************  Version 6  *****************
 * User: Admin        Date: 18.11.06   Time: 14:32
 * Updated in $/gps/Lightcom.Common
 * 
 */            

using System;
using System.Xml;

namespace LightCom.Common
{

/// 
/// <summary>
/// XML ���������.
/// </summary>
///
 
public class XMLSetingsStorage : SettingsStorage 
{

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// Constructor.
/// </summary>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public XMLSetingsStorage ()
{
    this.m_strFileName = LightCom.Common.Utils.ExeDirectory + "\\settings.txt";    
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// Destructor
/// </summary>
/// 
//////////////////////////////////////////////////////////////////////////////// 
~XMLSetingsStorage ()
{
    Flush ();
}
    

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� ������������� ������� - ��� ������ ������������ � 
/// ��������� ���������.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public override void Reset ()
{
    m_Document = new XmlDocument ();    
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����� ���������� ���������� ������ � �������� ���������.
/// </summary>
/// <returns>
/// ���������� true, ���� ����� ���� ������ �������.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override bool Flush ()
{
    XmlTextWriter writer = new XmlTextWriter (this.FileName, null);
    writer.Formatting = Formatting.Indented;
    try 
    {
        this.m_Document.Save (writer);
    }
    catch (Exception )
    {
        writer.Close ();
        return false;
    }
    writer.Close ();

    return true;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� �������������, ����������� ��� �������� ������.
/// </summary>
/// <returns>
/// ���������� true, ���� ������������� ������ �������.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

    public override bool PreLoad ()
{
    this.Reset ();

    XmlTextReader reader = new XmlTextReader (this.FileName);

    try
    {
        this.m_Document.Load (reader);
    }
    catch (Exception)
    {
        reader.Close ();
        return false;
    }
    reader.Close ();

    return true;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="strType">IN �������� ����/��������� ���������</param>
/// <param name="strName">IN �������� ���������</param>
/// <param name="strValue">IN �������� ���������</param>
/// <returns>���������� true, ���� ������ ������ �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override bool Write (string strType, 
    string strName, 
    string strValue)
{
    
    XmlNode root = this.m_Document [m_strDocumentName];
    if (null == root)
    {
        root = this.m_Document.CreateElement (m_strDocumentName);
        this.m_Document.AppendChild (root);
    }

    XmlElement elem = root [strType];
    if (null == elem)
    {
        elem = this.m_Document.CreateElement (strType);
        root.AppendChild (elem);
    }
    elem.SetAttribute (strName, strValue);

    return true;

}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// 
/// </summary>
/// <param name="strType">IN �������� ����/��������� ���������</param>
/// <param name="strName">IN �������� ���������</param>
/// <param name="strValue">OUT �������� ���������</param>
/// <returns>���������� true, ���� ������ ������ �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override bool Read (string strType, 
    string strName, 
    out string strValue)
{
    strValue = "";

    XmlNode root = this.m_Document [m_strDocumentName];
    if (null == root)
    {
        return false;
    }

    XmlElement elem = root [strType];
    if (null == elem)
    {
        return false;
    }
    if (null == elem.GetAttributeNode (strName))
    {
        return false;
    }

    strValue = elem.GetAttribute (strName);

    return true;
}

///
/// <summary>
/// XML ��������.
/// </summary>
/// 

protected XmlDocument m_Document = new XmlDocument ();

///
/// <summary>
/// ��� ����� � �����������.
/// </summary>
/// 

public string FileName {get {return this.m_strFileName;} set {this.m_strFileName = value;}}
protected string m_strFileName;

/// 
/// <summary>
/// ��� ��������� ���� XML ���������.
/// </summary>
/// 

protected const string m_strDocumentName = "settings";

}
}