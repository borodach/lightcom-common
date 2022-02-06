////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           IPositionMapper.cs
//
//  Facility:       �������������� ���������.
//
//
//  Abstract:       � ���� ������ ��������� �������� ���� ������, ������������ 
//                  ��� �������������� ���������.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
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
    /// ���������� ����� �� �������.
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
        /// ���������� ���������� ����� �������.
        /// </summary>
        /// <param name="other">�����, ���������� �� ������� ����� ���������.
        /// </param>
        /// <returns>���������� ���������� ����� �������.</returns>
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
    /// �������������� ���������� �����.
    /// </summary>
    ///

    public class GlobalPoint
    {

        public double x;
        public double y;

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// ���������� ���������� ����� �������.
        /// </summary>
        /// <param name="other">�����, ���������� �� ������� ����� ���������.
        /// </param>
        /// <returns>���������� ���������� ����� �������.</returns>
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
    /// ��������� ��� �������������� �������������� ��������� � ��������� �� 
    /// ����� � �������.
    /// </summary>
    /// 

    public interface IPositionMapper
    {

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// ����������� �������������� ���������� � ���������� �� �����.
        /// </summary>
        /// <param name="global">�������������� ����������.</param>
        /// <param name="map">���������� �� �����.</param>
        /// <returns>true, ���� �������������� ��������� �������.</returns>
        /// 
        //////////////////////////////////////////////////////////////////////////////// 

        bool GlobalToMap (GlobalPoint global, MapPoint map);

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// ����������� ���������� �� ����� � �������������� ���������� .
        /// </summary>
        /// <param name="map">���������� �� �����.</param>
        /// <param name="global">�������������� ����������.</param>
        /// <returns>true, ���� �������������� ��������� �������.</returns>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        bool MapToGlobal (MapPoint map, GlobalPoint global);
    }
}
