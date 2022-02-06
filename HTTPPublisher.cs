////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           HTTPPublisher.cs
//
//  Facility:       Передача данных на сервер.
//
//
//  Abstract:       Передача данных на сервер через HTTP.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  14/09/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: HTTPPublisher.cs $
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:20
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Net;
using System.IO;

namespace LightCom.Common
{

/// 
/// <summary>
/// Паредача данных по HTTP. Поддерживается работа с двумя серверами:
/// основным и резервным. Резервный используется, только если основной
/// недоступен.
/// </summary>
/// 

public class HTTPPublisher : IPublisher, ISettings 
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Кодирует бинарные данные.
/// </summary>
/// <param name="data">Кодируемые данные.</param>
/// <returns>Возвращает строку с закодированными данными.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static byte [] bin2hex (byte [] data)
{
    byte [] result = null;
    if (null == data) return result;
    int nSize = data.Length;
    result = new byte [nSize * 2];
    for (int nIdx = 0; nIdx < nSize; ++ nIdx)
    {
        string strResult = string.Format ("{0:x}", data [nIdx]);    
        if (strResult.Length == 1) strResult = "0" + strResult;
        result [2 * nIdx] = (byte) (strResult [0]);
        result [2 * nIdx + 1] = (byte) (strResult [1]);

    }
    return result;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Передает данные на HTTP сервер  получает от него ответ.
/// </summary>
/// <param name="data">Передаваемые на сервер данные.</param>
/// <param name="receiveStream">Ответ сервера.</param>
/// <param name="strURL">URL, на который отправляется запрос.</param>
/// <param name="strUser">Имя пользователя.</param>
/// <param name="strPassword">Пароль.</param>
/// <returns>Возвращает true, если передача прошла успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Publish (byte [] data, out Stream receiveStream,
    string strURL, string strUser, 
    string strPassword)
{
    /*
    try
    {
        // Create a web request for an invalid site. Substitute the "invalid site" strong in the Create call with a invalid name.
        HttpWebRequest myHttpWebRequest = (HttpWebRequest) WebRequest.Create(strURL);

        // Get the associated response for the above request.
        HttpWebResponse myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();
        myHttpWebResponse.Close();
    }
    catch (Exception e)
    {
        m_strStatus = e.ToString ();
        System.Windows.Forms.MessageBox.Show (m_strStatus, strURL);
    }
    System.Windows.Forms.MessageBox.Show ("Hello");
    */

    this.m_strStatus = "Success";

    Stream requestStream = null;
    //Stream receiveStream = null;
    HttpWebRequest webRequest = null;
    WebResponse webResponse = null;

    receiveStream = null;

    //
    //##Подготавливаем запрос.
    //

    try
    {
        webRequest = (HttpWebRequest) WebRequest.Create (strURL);
        webRequest.AllowWriteStreamBuffering = true;
        webRequest.AllowAutoRedirect = true;
        //webRequest.ProtocolVersion = HttpVersion.Version10;
        
        webRequest.PreAuthenticate = true;
        NetworkCredential networkCredential = 
            new NetworkCredential (strUser, strPassword);
        webRequest.Credentials = networkCredential;
        
        //webRequest.ContentType = "text/plain";
        webRequest.UserAgent = this.UserAgent;
        //webRequest.ContentType = "application/octet-stream";
        webRequest.ContentType = "multipart/form-data";
        //webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.Method = "POST";
        webRequest.ContentLength = data.Length;
        //webRequest.SendChunked = true;
        //webRequest.TransferEncoding="gzip";        
        requestStream = webRequest.GetRequestStream ();
        requestStream.Write (data, 0, data.Length);
        requestStream.Close ();        
    }
    catch (Exception e)
    {
        if (null != requestStream) requestStream.Close ();
        m_strStatus = e.Message;
        //System.Windows.Forms.MessageBox.Show (m_strStatus, strURL);
        return false;
    }
    requestStream.Close ();

    //
    //##Выполняем запрос и читаем ответ.
    //

    try
    {
        webResponse = webRequest.GetResponse ();
        receiveStream = webResponse.GetResponseStream ();
        /*
        if (0 >= webResponse.ContentLength)
        {
            response = new byte [0];
            webResponse.Close ();
            return true;
        }

        response = new byte [webResponse.ContentLength];
        receiveStream.Read (response, 0, (int) webResponse.ContentLength);
        receiveStream.Close (); */
    }
    catch (Exception e)
    {
        if (null != webResponse) webResponse.Close ();
        if (null != receiveStream) receiveStream.Close ();
        m_strStatus = e.ToString ();
        //System.Windows.Forms.MessageBox.Show (m_strStatus, strURL);
        return false;
    }
    //System.Windows.Forms.MessageBox.Show ("OK");

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Передает данные на сервер и получает ответ.
/// </summary>
/// <param name="data">Передаваемые на сервер данные.</param>
/// <param name="receiveStream">Ответ сервера.</param>
/// <returns>Возвращает true, если передача прошла успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Publish (byte [] data, out Stream receiveStream)
{
    if (! Publish (data, 
        out receiveStream, 
        this.m_strURL, 
        this.m_strUser, 
        this.m_strPassword))
    {
        if (string.Empty == this.m_strBackupURL) return false;
        return Publish (data, 
            out receiveStream, 
            this.m_strBackupURL, 
            this.m_strBackupUser, 
            this.m_strBackupPassword);
    }
    else
    {
        return true;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сохраняет настройки объекта в заданном хранилище.
/// </summary>
/// <param name="storage">Интерфейс сохранения параметров.</param>
/// <returns>Возвращает true, если сохранение прошло успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Save (SettingsStorage storage)
{
    bool bResult;
    bResult = storage.Write ("Publisher", "URL", this.URL);
    bResult &= storage.Write ("Publisher", "User", this.User);
    bResult &= storage.Write ("Publisher", "Password", this.Password);

    bResult &= storage.Write ("Publisher", "BackupURL", this.BackupURL);
    bResult &= storage.Write ("Publisher", "BackupUser", this.BackupUser);
    bResult &= storage.Write ("Publisher", "BackupPassword", this.BackupPassword);

    return bResult;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Загружает настройки объекта из заданного хранилища.
/// </summary>
/// <param name="storage">Интерфейс сохранения параметров.</param>
/// <returns>Возвращает true, если загрузка прошла успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Load (SettingsStorage storage)
{
    storage.Read ("Publisher", 
                  "URL",
                  out this.m_strURL,
                  string.Empty);
    storage.Read ("Publisher", 
                  "User",
                  out this.m_strUser,
                  string.Empty);
    storage.Read ("Publisher", 
                  "Password", 
                  out this.m_strPassword,
                  string.Empty);

    storage.Read ("Publisher", 
                  "BackupURL", 
                  out this.m_strBackupURL,
                  string.Empty);
    storage.Read ("Publisher", 
                  "BackupUser", 
                  out this.m_strBackupUser,
                  string.Empty);
    storage.Read ("Publisher", 
                  "BackupPassword", 
                   out this.m_strBackupPassword,
                   string.Empty);

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сбрасывает объект в начальное состояние.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Reset ()
{
    this.m_strURL = "";
    this.m_strUser = "";
    this.m_strPassword = "";

    this.m_strBackupURL = "";
    this.m_strBackupUser = "";
    this.m_strBackupPassword = "";

    this.m_strStatus = m_strDefaultStatus;
}


/// 
/// <summary>
/// Статический экземпляр класса.
/// </summary>
/// 

public static HTTPPublisher instance = null;

/// 
/// <summary>
/// URL основного сервера.
/// </summary>
/// 

public string URL {get {return this.m_strURL;} set {this.m_strURL = value;}}
protected string m_strURL = "";

/// 
/// <summary>
/// Имя пользователя для доступа к основному серверу.
/// </summary>
///

public string User {get {return this.m_strUser;} set {this.m_strUser = value;}}
protected string m_strUser = "";

/// 
/// <summary>
/// Пароль для доступа к основному серверу.
/// </summary>
/// 

public string Password {get {return this.m_strPassword;} set {this.m_strPassword = value;}}
protected string m_strPassword = "";

/// 
/// <summary>
/// URL резервного сервера.
/// </summary>
/// 

public string BackupURL {get {return this.m_strBackupURL;} set {this.m_strBackupURL = value;}}
protected string m_strBackupURL = "";

/// 
/// <summary>
/// Имя пользователя для доступа к резервному серверу.
/// </summary>
/// 

public string BackupUser {get {return this.m_strBackupUser;} set {this.m_strBackupUser = value;}}
protected string m_strBackupUser = "";

/// 
/// <summary>
/// Пароль для доступа к резервному серверу.
/// </summary>
/// 

public string BackupPassword {get {return this.m_strBackupPassword;} set {this.m_strBackupPassword = value;}}
protected string m_strBackupPassword = "";

/// 
/// <summary>
/// Строка статуса последнего обращения к серверу.
/// </summary>
/// 

public string Status {get {return m_strStatus;}}
protected string m_strStatus = m_strDefaultStatus;

///
/// <summary>
/// Строка статуса по-умолчанию.
/// </summary>
/// 

protected const string m_strDefaultStatus = "None";

///
/// <summary>
/// HTTP user agent.
/// </summary>
/// 

public virtual string UserAgent {get {return this.m_strUserAgent;} set {this.m_strUserAgent = value;}}
protected string m_strUserAgent = "";
}
}