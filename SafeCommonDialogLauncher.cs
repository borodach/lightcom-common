///////////////////////////////////////////////////////////////////////////////
//
//  File:           SafeFileDialogLauncher.cs
//
//  Facility:       ���������� �������� CommonDialog  � � CE � � Windows NT
//
//
//  Abstract:       ���������� �������� CommonDialog  � � CE � � Windows NT
//
//  Environment:    VC# 8.0
//
//  Author:         ������ �.�.
//
//  Creation Date:  04-05-2008
//
//  Copyright (C) OOO "�������", 2005-2008. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: SafeCommonDialogLauncher.cs $
 * 
 * *****************  Version 1  *****************
 * User: Serg         Date: 4.05.08    Time: 20:33
 * Created in $/LightCom/.NET/Common
 * ����������� ��������� CommonDialog � Vista
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
        /// ��������� ������ ���������� ��������
        /// </summary>
        /// <param name="dlg">������, ������� ���� ���������</param>
        /// <returns>��������� ������ �������</returns>
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
        /// ��������� ���������� �������
        /// </summary>
        private DialogResult result;

        /// <summary>
        /// ������, ������� ���� ���������
        /// </summary>
        private CommonDialog dialog;

        /// <summary>
        /// ��������� ������
        /// </summary>
        private void ShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }
}