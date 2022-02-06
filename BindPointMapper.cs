////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           BindPointMapper.cs
//
//  Facility:       Преобразование координат.
//
//
//  Abstract:       Преобразование координат с помощью ближайшей точки привязки.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  30/10/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: BindPointMapper.cs $
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:43
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Globalization;

namespace LightCom.Common
{

/// 
/// <summary>
/// Преобразование координат с помощью ближайшей точки привязки.
/// </summary>
/// 

public class BindPointMapper : IPositionMapper 
{

/// 
/// <summary>
/// Точка привязки.
/// </summary>
/// 

public struct BindPoint
{
    public GlobalPoint global;
    public MapPoint map;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Преобразует географические координаты в координаты на карте.
/// </summary>
/// <param name="global">Географические координаты.</param>
/// <param name="map">Координаты на карте.</param>
/// <returns>true, если преобразование выполнить удалось.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GlobalToMap (GlobalPoint global, MapPoint map)
{
    int nCount = this.BindPointCount ();
    if (0 == this.m_dx || 0 == this.m_dy || 0 == nCount) return false;
    double distance = -1;
    int nBasePointIdx = 0;
    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        double tmp = (this [nIdx].global - global);
        if (distance < 0 || distance > tmp)
        {
            distance = tmp;
            nBasePointIdx = nIdx;
            if (0 == distance) break;
        }
    }

    GlobalPoint gBase = this [nBasePointIdx].global;
    MapPoint mBase = this [nBasePointIdx].map;

    try
    {
        map.fx = (int) Math.Round (mBase.fx + (global.x - gBase.x) / this.m_dx);
        map.fy = (int) Math.Round (mBase.fy + (global.y - gBase.y) / this.m_dy);
    }
    catch (Exception)
    {
        return false;
    }

    return true;    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Преобразует координаты на карте в географические координаты .
/// </summary>
/// <param name="map">Координаты на карте.</param>
/// <param name="global">Географические координаты.</param>
/// <returns>true, если преобразование выполнить удалось.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool MapToGlobal (MapPoint map, GlobalPoint global)
{
    int nCount = this.BindPointCount ();
    if (0 == this.m_dx || 0 == this.m_dy || 0 == nCount) return false;
    double distance = -1;
    int nBasePointIdx = 0;
    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        double tmp = (this [nIdx].map - map);
        if (distance < 0 || distance > tmp)
        {
            distance = tmp;
            nBasePointIdx = nIdx;
            if (0 == distance) break;
        }
    }

    GlobalPoint gBase = this [nBasePointIdx].global;
    MapPoint mBase = this [nBasePointIdx].map;

    global.x = global.x + this.m_dx * (map.fx - mBase.fx);
    global.y = global.y + this.m_dy * (map.fy - mBase.fy);

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сохранение объекта в поток.
/// </summary>
/// <param name="stream">Поток для сохранения.</param>
/// <returns>true, если операция выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool SaveGuts (Stream stream)
{
    try
    {
        NumberFormatInfo provider = new NumberFormatInfo ();
        provider.NumberDecimalSeparator = ",";
        provider.NumberGroupSeparator = "";

        StreamWriter fs = new StreamWriter (stream, 
            new System.Text.ASCIIEncoding ());
        fs.AutoFlush = true;

        fs.WriteLine (Convert.ToString (this.m_dx, provider) + " " + 
            Convert.ToString (this.m_dy, provider));
        fs.WriteLine (this.BindPointCount ());                
        
        int nCount = this.BindPointCount ();
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            GlobalPoint global = this [nIdx].global;
            MapPoint map = this [nIdx].map;
            fs.WriteLine (Convert.ToString (global.x, provider) + " " + 
                Convert.ToString (global.y, provider) + " " + 
                Convert.ToString (map.fx) + " " + 
                Convert.ToString (map.fy));
        }
    }
    catch (Exception )
    {
        return false;
    }
    
    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Загрузка объекта из потока.
/// </summary>
/// <param name="stream">Поток для восстановления.</param>
/// <returns>true, если операция выполнена успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool RestoreGuts (Stream stream)
{
    Reset ();

    try
    {
        System.Text.RegularExpressions.Regex re = 
            new System.Text.RegularExpressions.Regex ("\\s+");

        NumberFormatInfo provider = new NumberFormatInfo ();
        provider.NumberDecimalSeparator = ",";
        provider.NumberGroupSeparator = "";

        StreamReader fs = new StreamReader (stream, 
            new System.Text.ASCIIEncoding ());

        string strLine = fs.ReadLine ();
        strLine = strLine.Trim ();
        string [] strTokens = re.Split (strLine);
        this.m_dx = Convert.ToDouble (strTokens [0], provider);
        this.m_dy = Convert.ToDouble (strTokens [1], provider);

        strLine = fs.ReadLine ();                
        uint nCount = Convert.ToUInt32 (strLine);                
        for (uint nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            BindPoint bindPoint = new BindPoint ();
            strLine = fs.ReadLine ();
            strLine = strLine.Trim ();
            strTokens = re.Split (strLine);                    
            bindPoint.global = new GlobalPoint  ();
            bindPoint.global.x  = Convert.ToDouble (strTokens [0], provider);
            bindPoint.global.y  = Convert.ToDouble (strTokens [1], provider);
            bindPoint.map = new MapPoint ();
            bindPoint.map.fx  = Convert.ToInt32 (strTokens [2]);
            bindPoint.map.fy  = Convert.ToInt32 (strTokens [3]);
            this.Add (bindPoint);
        }
    }
    catch (Exception/* e*/)
    {
        Reset ();
        return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Выяисляет географические размеры пикселя.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public bool CalculatePixelSize (out double Gx, out double Gy)
{
    double mX = 0, mY = 0, mGX = 0, mGY = 0;
    double dmX = 0, dmY = 0, dgX = 0, dgY = 0;
    Gx = Gy = 0;

    int nCount = this.BindPointCount ();
    if (0 == nCount) return false;

    //
    // Вычисляем средние значения.
    //

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        GlobalPoint global = this [nIdx].global;
        MapPoint map = this [nIdx].map;    
        mX += map.fx;
        mY += map.fy;
        mGX += global.x;
        mGY += global.y;
    }

    mX /= nCount;
    mY /= nCount;
    mGX /= nCount;
    mGY /= nCount;

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        GlobalPoint global = this [nIdx].global;
        MapPoint map = this [nIdx].map;    
        
        dgX += map.fx * (mGX - global.x);
        dmX += map.fx * (mX - map.fx);

        dgY += map.fy * (mGY - global.y);
        dmY += map.fy * (mY - map.fy);
    }

    double dx, dy;
    try
    {
        dx = dgX / dmX;
        dy = dgY / dmY;
    }
    catch (Exception)
    {
        return false;
    }

    this.m_dx = dx;
    this.m_dy = dy;

    Gx = mGX - dx * mX;
    Gy = mGY - dy * mY;
    
    return true;

}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Добавление точки привязки.
/// </summary>
/// <param name="point">Точка привязки.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (BindPoint point)
{
    this.m_BindPoints.Add (point);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Удаление точки привязки.
/// </summary>
/// <param name="point">Индекс точки привязки.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void RemoveBindPoint (int index)
{
    this.m_BindPoints.Remove (index);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает количество точек привязки.
/// </summary>
/// <returns>Возвращает количество точек привязки.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public int BindPointCount ()
{
    return this.m_BindPoints.Count;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Точки привязки.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public BindPoint this [int index]
{
    get {return (BindPoint) this.m_BindPoints [(int) index];} 
    set {this.m_BindPoints [(int) index] = value;} 
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Очищает все данные объекта.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Reset ()
{
    this.m_BindPoints.Clear ();
    m_dx = 0;
    m_dy = 0;
}

/// 
/// <summary>
/// Контейнер точек привязки.
/// </summary>
/// 

protected ArrayList m_BindPoints = new ArrayList ();

/// 
/// <summary>
/// Географический размер пикселя.
/// </summary>
/// 

public double dx { get {return m_dx;} set {m_dx = value;}}
protected double m_dx;

public double dy { get {return m_dy;} set {m_dy = value;}}
protected double m_dy;

}
}
