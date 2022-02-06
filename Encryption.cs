///////////////////////////////////////////////////////////////////////////////
//
//  File:           Encryption
//
//  Facility:       Шифрование
//
//
//  Abstract:       Простое XOR шифрование
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-18-2006
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: Encryption.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;

namespace LightCom.Common
{

///
/// <summary>
/// Класс содержит шифрования/дешифрования.
/// </summary>
/// 

public class Encryption
{

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Шифрует массив байтов.
/// </summary>
/// <param name="data">Данные для шифрования.</param>
/// <param name="key">Ключ.</param>
/// <returns>Зашифрованные данные.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static byte [] Encrypt (byte [] data, byte key)
{
    int nSize = data.Length;
    byte [] result = new byte [nSize];
    byte mask = key;
    for (int nIdx = 0; nIdx < nSize; ++nIdx)
    {
        result [nIdx] = (byte) (data [nIdx] ^ mask);
        mask = (byte)((result [nIdx] << 4) | ((result [nIdx] >> 4) & 0xf));
    }

    return result;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Расшифровывает массив байтов.
/// </summary>
/// <param name="data">Зашифрованные данные.</param>
/// <param name="key">Ключ.</param>
/// <returns>Расшифрованные данные.</returns>
/// 
/// ////////////////////////////////////////////////////////////////////////////////

public static byte [] Decrypt (byte [] data, byte key)
{
    int nSize = data.Length;
    byte [] result = new byte [nSize];
    byte mask = key;
    for (int nIdx = 0; nIdx < nSize; ++nIdx)
    {
        result [nIdx] = (byte)(data [nIdx] ^ mask);
        mask = (byte) ((data [nIdx] << 4) | ((data [nIdx] >> 4) & 0xf));
    }

    return result;
}
}
}
