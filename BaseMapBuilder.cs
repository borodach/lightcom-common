///////////////////////////////////////////////////////////////////////////////
//
//  File:           BaseMapBuilder.cs
//
//  Facility:       Отрисовка карт
//
//
//  Abstract:       Базовый класс для составления заданного фрагмента карты, 
//                  хранимой в виде множеству кусочков.
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
 * $History: BaseMapBuilder.cs $
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:39
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 19.11.06   Time: 23:39
 * Updated in $/gps/Lightcom.Common
 * Добавлен признак того, что элементы кэша нужно dispose
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 19.11.06   Time: 0:21
 * Updated in $/gps/Lightcom.Common
 * Выполнена отладка
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:41
 * Updated in $/gps/Lightcom.Common
 * Исправлена кодировка
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:37
 * Updated in $/gps/Lightcom.Common
 * Небольшое форматирование
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:18
 * Created in $/gps/Lightcom.Common
 * Альфа версия класса
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LightCom.Common
{
            
    /// <summary>
    /// Базовый класс для составления заданного фрагмента карты, хранимой в 
    /// виде множеству кусочков
    /// </summary>
    public abstract class BaseMapBuilder
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseMapBuilder ()
        {
        }

        /// <summary>
        /// Рисует фрагмент карты
        /// </summary>
        /// <param name="g">Graphics для рисования</param>
        /// <param name="gaphicsRect">Область для рисования</param>
        /// <param name="mapRect">Область карты</param>
        public virtual void Draw (Graphics g,
                                  Rectangle gaphicsRect,
                                  Rectangle mapRect)
        {
            //
            // Вечный цикл нам не нужен. Также блокируем ненужные перерисовки.
            //

            if (varPieceSize.Width <= 0 || varPieceSize.Height <= 0 ||
                gaphicsRect.Width == 0 || gaphicsRect.Height == 0 ||
                mapRect.Width == 0 || mapRect.Height == 0)
            {
                return;
            }

            //
            // Перебираем кусочки карты
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
        /// Возвращает заданный кусочек карты
        /// </summary>
        /// <param name="piecePos">Координаты кусочка</param>
        /// <param name="gaphicsRect">Область для рисования, переданная в 
        /// метод Draw</param>
        /// <param name="mapRect">Область картыпереданная в 
        /// метод Draw</param>
        /// <param name="canDispose">Признак того, что картинку можно dispose
        /// </param>
        /// <returns>Картинку кусочка</returns>
        protected abstract Image GetImagePiece (Point piecePos, 
                                                Rectangle gaphicsRect,
                                                Rectangle mapRect,
                                                out bool canDispose);

        /// <summary>
        /// Размер кусочка карты
        /// </summary>
        public Size PieceSize
        {
            get {return varPieceSize;}
            set {varPieceSize = value;}
        }
        protected Size varPieceSize;  
    }
}
