///////////////////////////////////////////////////////////////////////////////
//
//  File:           BaseMapBuilder.cs
//
//  Facility:       ��������� ����
//
//
//  Abstract:       ������� ����� ��� ����������� ��������� ��������� �����, 
//                  �������� � ���� ��������� ��������.
//
//  Environment:    VC# 8.0
//
//  Author:         ������ �.�.
//
//  Creation Date:  11-18-2006
//
//  Copyright (C) OOO "�������", 2005-2006. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: BaseMapBuilder.cs $
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:39
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 19.11.06   Time: 23:39
 * Updated in $/gps/Lightcom.Common
 * �������� ������� ����, ��� �������� ���� ����� dispose
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 19.11.06   Time: 0:21
 * Updated in $/gps/Lightcom.Common
 * ��������� �������
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:41
 * Updated in $/gps/Lightcom.Common
 * ���������� ���������
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:37
 * Updated in $/gps/Lightcom.Common
 * ��������� ��������������
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:18
 * Created in $/gps/Lightcom.Common
 * ����� ������ ������
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LightCom.Common
{
            
    /// <summary>
    /// ������� ����� ��� ����������� ��������� ��������� �����, �������� � 
    /// ���� ��������� ��������
    /// </summary>
    public abstract class BaseMapBuilder
    {
        /// <summary>
        /// �����������
        /// </summary>
        public BaseMapBuilder ()
        {
        }

        /// <summary>
        /// ������ �������� �����
        /// </summary>
        /// <param name="g">Graphics ��� ���������</param>
        /// <param name="gaphicsRect">������� ��� ���������</param>
        /// <param name="mapRect">������� �����</param>
        public virtual void Draw (Graphics g,
                                  Rectangle gaphicsRect,
                                  Rectangle mapRect)
        {
            //
            // ������ ���� ��� �� �����. ����� ��������� �������� �����������.
            //

            if (varPieceSize.Width <= 0 || varPieceSize.Height <= 0 ||
                gaphicsRect.Width == 0 || gaphicsRect.Height == 0 ||
                mapRect.Width == 0 || mapRect.Height == 0)
            {
                return;
            }

            //
            // ���������� ������� �����
            //

            int dx = mapRect.Left % varPieceSize.Width;
            int dy = mapRect.Top % varPieceSize.Height;
            Point pt = new Point (mapRect.Left - dx,
                                  mapRect.Top - dy);

            Point pt0 = new Point (pt.X,
                                   pt.Y);

            double scaleX = gaphicsRect.Width / (double) mapRect.Width;
            double scaleY = gaphicsRect.Height / (double) mapRect.Height;

            bool canDispose;
            for (; pt.X <= mapRect.Right; pt.X += varPieceSize.Width)
            {
                for (pt.Y = pt0.Y; pt.Y <= mapRect.Bottom; pt.Y += varPieceSize.Height)
                {
                    Image img = GetImagePiece (pt, 
                                               gaphicsRect, 
                                               mapRect, 
                                               out canDispose);
                    if (null == img) break;
                    Rectangle srcRect = new Rectangle (0, 0, img.Width, img.Height);
                    Rectangle dstRect = new Rectangle ((int)  ((pt.X - pt0.X - dx) * scaleX), 
                                                       (int)  ((pt.Y  - pt0.Y - dy) * scaleY), 
                                                       (int)  (srcRect.Width * scaleX), 
                                                       (int)  (srcRect.Height * scaleY));
                    g.DrawImage (img, dstRect, srcRect, GraphicsUnit.Pixel);
                    if (canDispose)
                    {
                        img.Dispose ();
                    }
                }
            }
        }

        /// <summary>
        /// ���������� �������� ������� �����
        /// </summary>
        /// <param name="piecePos">���������� �������</param>
        /// <param name="gaphicsRect">������� ��� ���������, ���������� � 
        /// ����� Draw</param>
        /// <param name="mapRect">������� ��������������� � 
        /// ����� Draw</param>
        /// <param name="canDispose">������� ����, ��� �������� ����� dispose
        /// </param>
        /// <returns>�������� �������</returns>
        protected abstract Image GetImagePiece (Point piecePos, 
                                                Rectangle gaphicsRect,
                                                Rectangle mapRect,
                                                out bool canDispose);

        /// <summary>
        /// ������ ������� �����
        /// </summary>
        public Size PieceSize
        {
            get {return varPieceSize;}
            set {varPieceSize = value;}
        }
        protected Size varPieceSize;  
    }
}
