////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           WinCE.cs
//
//  Facility:       Библиотека полезных методов.
//
//
//  Abstract:       Модуль содержит полезные функции.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  20/10/2005
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: WinCE.cs $
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Runtime.InteropServices;

namespace LightCom.WinCE
{

/// 
/// <summary>
/// Common functions.
/// </summary>
///

public class API
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Запускает процесс.
/// </summary>
/// <param name="strCommandLine">Command line.</param>
/// <returns>true, если процесс был успешно запущен.</returns>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public static bool CreateProcess (string strCommandLine)
{   
    PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION ();
    try
    {
        bool bResult = CreateProcess (strCommandLine, 
            null, 0, 0, false, 0, 0, null, 0, out processInfo);
        if (! bResult) return false;

        CloseHandle (processInfo.hThread);
        CloseHandle (processInfo.hProcess);

        return true;
    }
    catch (Exception)
    {
        return false;
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Создает mutex.
/// </summary>
/// <param name="strName">Имя mutex.</param>
/// <returns>true, если mutex был успешно создан.</returns>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public static IntPtr CreateMutex (string strName)
{   
    try
    {
        IntPtr hMutex = CreateMutex (0, 1, strName);
        return hMutex;
    }
    catch (Exception)
    {
        return IntPtr.Zero;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// This function starts running an application when a specified event occurs.
/// </summary>
/// <param name="strAppName">String that specifies the name of the application 
/// to be started.</param>
/// <param name="lWhichEvent">Long integer that specifies the event at which the 
/// application is to be started.</param>
/// <returns></returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static bool CeRunAppAtEvent (string strAppName, 
    NotificationEventType lWhichEvent)
{
    return CeRunAppAtEvent (strAppName, (int) lWhichEvent);
}

/*
    * Parameters to the CeRunAppAtEvent() API.
*/  

public enum NotificationEventType : int
{
    NOTIFICATION_EVENT_NONE                = 0,
    NOTIFICATION_EVENT_TIME_CHANGE         = 1,
    NOTIFICATION_EVENT_SYNC_END            = 2,
    NOTIFICATION_EVENT_ON_AC_POWER         = 3,
    NOTIFICATION_EVENT_OFF_AC_POWER        = 4,
    NOTIFICATION_EVENT_NET_CONNECT         = 5,
    NOTIFICATION_EVENT_NET_DISCONNECT      = 6,
    NOTIFICATION_EVENT_DEVICE_CHANGE       = 7,
    NOTIFICATION_EVENT_IR_DISCOVERED       = 8,
    NOTIFICATION_EVENT_RS232_DETECTED      = 9,
    NOTIFICATION_EVENT_RESTORE_END         = 10,
    NOTIFICATION_EVENT_WAKEUP              = 11,
    NOTIFICATION_EVENT_TZ_CHANGE           = 12,
    NOTIFICATION_EVENT_MACHINE_NAME_CHANGE = 13
};

[StructLayout(LayoutKind.Sequential)]
    protected struct PROCESS_INFORMATION 
{ 
    public IntPtr hProcess; 
    public IntPtr hThread; 
    public int dwProcessId; 
    public int dwThreadId; 
}; 

[DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
protected static extern bool CreateProcess (string lpszImageName, 
    string lpszCmdLine, 
    int lpsaProcess, 
    int lpsaThread, 
    bool fInheritHandles, 
    int fdwCreate, 
    int lpvEnvironment, 
    string lpszCurDir, 
    int lpsiStartInfo, 
    [Out] out PROCESS_INFORMATION lppiProcInfo);     

[DllImport("Coredll.dll", SetLastError=true)]
public static extern bool CloseHandle(IntPtr hObject);

[DllImport("Coredll.dll", SetLastError=true)]
public static extern bool WaitForSingleObject (IntPtr hObject, 
    int nTimeout);

public const string APP_RUN_AT_TIME = "AppRunAtTime";

/*
    * Prefix of the command line when the user requests to run the application
    * that "owns" a notification.  It is followed by a space, and the
    * stringized version of the notification handle.
    */

public const string APP_RUN_TO_HANDLE_NOTIFICATION  = "AppRunToHandleNotification";

/*
    * Strings passed on the command line when an event occurs that the
    * app has requested via CeRunAppAtEvent.  Note that some of these
    * strings will be used as the command line *prefix*, since the rest
    * of the command line will be used as a parameter.
    */

public const string APP_RUN_AFTER_TIME_CHANGE       = "AppRunAfterTimeChange";
public const string APP_RUN_AFTER_SYNC              = "AppRunAfterSync";
public const string APP_RUN_AT_AC_POWER_ON          = "AppRunAtAcPowerOn";
public const string APP_RUN_AT_AC_POWER_OFF         = "AppRunAtAcPowerOff";
public const string APP_RUN_AT_NET_CONNECT          = "AppRunAtNetConnect";
public const string APP_RUN_AT_NET_DISCONNECT       = "AppRunAtNetDisconnect";
public const string APP_RUN_AT_DEVICE_CHANGE        = "AppRunDeviceChange";
public const string APP_RUN_AT_IR_DISCOVERY         = "AppRunAtIrDiscovery";
public const string APP_RUN_AT_RS232_DETECT         = "AppRunAtRs232Detect";
public const string APP_RUN_AFTER_RESTORE           = "AppRunAfterRestore";
public const string APP_RUN_AFTER_WAKEUP            = "AppRunAfterWakeup";
public const string APP_RUN_AFTER_TZ_CHANGE         = "AppRunAfterTzChange";
public const string APP_RUN_AFTER_EXTENDED_EVENT    = "AppRunAfterExtendedEvent";

public const int ERROR_ALREADY_EXISTS = 183;


[DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
protected static extern bool CeRunAppAtEvent (string strAppName, 
    int lWhichEvent);
[DllImport("Coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
protected static extern IntPtr CreateMutex (int nSD, 
    int bOwned,
    string strName);
   

[StructLayout(LayoutKind.Sequential)]
public struct DEVICE_ID
{ 
    int dwSize;
    int dwPresetIDOffset;
    int dwPresetIDBytes;
    int dwPlatformIDOffset;
    int dwPlatformIDBytes;
};

[DllImport("Coredll.dll", SetLastError=true)]
    public static extern bool KernelIoControl  (int dwIoControlCode, 
    byte [] lpInBuffer, 
    int nInBufSize,
    byte [] lpOutBuffer, 
    int nOutBufSize, 
    out int lpBytesReturned );
}    
}