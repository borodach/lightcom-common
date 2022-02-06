///////////////////////////////////////////////////////////////////////////////
//
//  File:           IPersistent.cs
//
//  Facility:       Persistance.
//
//
//  Abstract:       ��������� ���������� ��������� ������� � �������� ������
//                  � �������������� �� ����.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  11-11-2005
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: IPersistent.cs $
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.IO;
namespace LightCom.Common
{
    /// 
    /// <summary>
    /// ��������� ���������� ��������� ������� � �������� ������ � 
    /// �������������� �� ����.
    /// </summary>
    /// 

    public interface IPersistent
    {
        /// 
        /// <summary>
        /// ���������� ����� � ��������� ���������.
        /// </summary>
        /// 

        void Reset ();

        /// 
        /// <summary>
        /// ��������� ��������� ������� � ����� stream.
        /// </summary>
        /// <param name="stream">�����, � ������� ����������� ����������.
        /// </param>
        /// <returns>true, ���� �������� ����������� �������.</returns>
        ///
 
        bool SaveGuts (Stream stream);

        /// 
        /// <summary>
        /// ��������������� �������� ������� �� ������ stream.
        /// </summary>
        /// <param name="stream">�����, �� ������� ����������� ��������������.
        /// </param>
        /// <returns>true, ���� �������� ����������� �������.</returns>
        ///

        bool RestoreGuts (Stream stream);
    }
}
