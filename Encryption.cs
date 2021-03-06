///////////////////////////////////////////////////////////////////////////////
//
//  File:           Encryption
//
//  Facility:       ??????????
//
//
//  Abstract:       ??????? XOR ??????????
//
//  Environment:    VC# 8.0
//
//  Author:         ?????? ?.?.
//
//  Creation Date:  11-18-2006
//
//  Copyright (C) OOO "???????", 2005-2006. ??? ????? ????????.
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
/// ????? ???????? ??????????/????????????.
/// </summary>
/// 

public class Encryption
{

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ??????? ?????? ??????.
/// </summary>
/// <param name="data">?????? ??? ??????????.</param>
/// <param name="key">????.</param>
/// <returns>????????????? ??????.</returns>
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
/// ?????????????? ?????? ??????.
/// </summary>
/// <param name="data">????????????? ??????.</param>
/// <param name="key">????.</param>
/// <returns>?????????????? ??????.</returns>
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
