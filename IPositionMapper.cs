////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           IPositionMapper.cs
//
//  Facility:       Преобразование координат.
//
//
//  Abstract:       В этом модуле объявлены основные типы данных, используемые 
//                  при преобразовании координат.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  30/10/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: IPositionMapper.cs $
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:43
 * Updated in $/LightCom/.NET/Common
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
    /// Координаты точки на каринке.
    /// </summary>
    /// 

    public class MapPoint
    {
        public int x
        {
            get { return (int) Math.Round (fx); }
            set { fx = value; }
        }
        public int y
        {
            get { return (int) Math.Round (fy); }
            set { fy = value; }
        }

        public double fx;
        public double fy;

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// Возвращает расстояние между точками.
        /// </summary>
        /// <param name="other">Точка, расстояние до которой нужно вычислить.
        /// </param>
        /// <returns>Возвращает расстояние между точками.</returns>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        public static double operator - (MapPoint one, MapPoint other)
        {
            return Math.Sqrt ((one.fx - other.fx) * (one.fx - other.fx) +
                (one.fy - other.fy) * (one.fy - other.fy));
        }
    }

    /// 
    /// <summary>
    /// Географические координаты точки.
    /// </summary>
    ///

    public class GlobalPoint
    {

        public double x;
        public double y;

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// Возвращает расстояние между точками.
        /// </summary>
        /// <param name="other">Точка, расстояние до которой нужно вычислить.
        /// </param>
        /// <returns>Возвращает расстояние между точками.</returns>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        public static double operator - (GlobalPoint one, GlobalPoint other)
        {
            return Math.Sqrt ((one.x - other.x) * (one.x - other.x) +
                (one.y - other.y) * (one.y - other.y));
        }
    }

    /// 
    /// <summary>
    /// Интерфейс для преобразования географических координат в кординаты на 
    /// карте и обратно.
    /// </summary>
    /// 

    public interface IPositionMapper
    {

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

        bool GlobalToMap (GlobalPoint global, MapPoint map);

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

        bool MapToGlobal (MapPoint map, GlobalPoint global);
    }
}
