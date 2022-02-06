///////////////////////////////////////////////////////////////////////////////
//
//  File:           SafeFileDialogLauncher.cs
//
//  Facility:       Безопасное открытие CommonDialog  и в CE и в Windows NT
//
//
//  Abstract:       Безопасное открытие CommonDialog  и в CE и в Windows NT
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  04-05-2008
//
//  Copyright (C) OOO "ЛайтКом", 2005-2008. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: SafeCommonDialogLauncher.cs $
 * 
 * *****************  Version 1  *****************
 * User: Serg         Date: 4.05.08    Time: 20:33
 * Created in $/LightCom/.NET/Common
 * Реализована поддержка CommonDialog в Vista
 * 
 */

using System;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace LightCom.Common
{
    public class SafeCommonDialogLauncher
    {
        /// <summary>
        /// Запускает диалог безопасным способом
        /// </summary>
        /// <param name="dlg">Диалог, который надо запустить</param>
        /// <returns>Результат работы диалога</returns>
        public DialogResult Launch (CommonDialog dlg)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                return dlg.ShowDialog(); ;
            }

            dialog = dlg;
            Thread t = new Thread(new ThreadStart(ShowDialog));            
            Type type = t.GetType();
            MethodInfo mi = type.GetMethod("SetApartmentState");
            mi.Invoke(t, new Object[] { Enum.Parse(typeof(ApartmentState), "STA", true) });            
            t.Start();
            t.Join();
            return result;
        }

        /// <summary>
        /// Результат выполнения диалога
        /// </summary>
        private DialogResult result;

        /// <summary>
        /// Диалог, который надо запустить
        /// </summary>
        private CommonDialog dialog;

        /// <summary>
        /// Запускает диалог
        /// </summary>
        private void ShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }
}