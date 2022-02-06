////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           IPositionMapper.cs
//
//  Facility:       �������������� ���������.
//
//
//  Abstract:       ������� �������������� ���������, ������������ �������������� 
//                  ���������� �������� ������ ���� ����� � �������������� 
//                  ������� ������ �������.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
//
//  Creation Date:  30/10/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: SimpleMapper.cs $
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
    /// ������ �������������� ���������, ������������ �������������� ���������� 
    /// �������� ������ ���� ����� � �������������� ������� ������ �������.
    /// </summary>
    ///

    public class SimpleMapper : IPositionMapper
    {
        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// �����������.
        /// </summary>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        public SimpleMapper ()
        {
            this.m_MapX = 0;
            this.m_MapY = 0;
            this.m_dx = 0;
            this.m_dy = 0;
        }

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// �����������.
        /// </summary>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        public SimpleMapper (double mapX, double mapY, double dx, double dy)
        {
            this.m_MapX = mapX;
            this.m_MapY = mapY;
            this.m_dx = dx;
            this.m_dy = dy;
        }

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

        public bool GlobalToMap (GlobalPoint global, MapPoint map)
{
    if (0 == this.m_dx || 0 == this.m_dy) return false;
    try
    {
        map.fx = (global.x - this.m_MapX) / this.m_dx;
        map.fy = (global.y - this.m_MapY) / this.m_dy;
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
        /// ����������� ���������� �� ����� � �������������� ���������� .
        /// </summary>
        /// <param name="map">���������� �� �����.</param>
        /// <param name="global">�������������� ����������.</param>
        /// <returns>true, ���� �������������� ��������� �������.</returns>
        ///
        //////////////////////////////////////////////////////////////////////////////// 

        public bool MapToGlobal (MapPoint map, GlobalPoint global)
        {
            global.x = this.m_MapX + this.m_dx * map.fx;
            global.y = this.m_MapY + this.m_dy * map.fy;

            return true;
        }

        /// 
        /// <summary>
        /// �������������� ���������� �������� ������ ���� �����.
        /// </summary>
        /// 

        public double MapX { get { return m_MapX; } set { m_MapX = value; } }
        protected double m_MapX;

        public double MapY { get { return m_MapY; } set { m_MapY = value; } }
        protected double m_MapY;

        /// 
        /// <summary>
        /// �������������� ������� �������.
        /// </summary>
        ///

        public double dx { get { return m_dx; } set { m_dx = value; } }
        protected double m_dx;

        public double dy { get { return m_dy; } set { m_dy = value; } }
        protected double m_dy;
    }
}