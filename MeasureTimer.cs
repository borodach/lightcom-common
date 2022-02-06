////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           MeasureTimer
//
//  Facility:       Вычисление длительности операций
//
//
//  Abstract:       Класс для вычисления длительности операций с точностью
//                  до миллисекунд. Работает и на Windows CE.
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  25і06.2007
//
//  Copyright (C) OOO "ЛайтКом", 2005-2007. Все права защищены.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: MeasureTimer.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 26.06.07   Time: 0:50
 * Created in $/LightCom/.NET/Common
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LightCom.Common
{

    /// <summary>
    /// Класс для вычисления длительности операций с точностью
    //  до миллисекунд. Работает и на Windows CE.
    /// </summary>
    public class MeasureTimer
    {

        #region "Methods"
        /// <summary>
        /// Конструктор
        /// </summary>
        public  MeasureTimer ()
        {
            Reset ();
        }

        /// <summary>
        /// Сброс таймера
        /// </summary>
        public void Reset ()
        {
            tickCount = Environment.TickCount;
        }

        /// <summary>
        /// Неявное преобразование типа в TimeSpan
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator TimeSpan (MeasureTimer val)
        {
            return val.Value;
        }
        #endregion;

        #region "Properties";

        /// <summary>
        /// Получение длительности инервала
        /// </summary>
        public TimeSpan Value
        {
            get 
            {
                return TimeSpan.FromMilliseconds (Environment.TickCount - tickCount);
            }
        }

        #endregion;

        #region "Fields";
        
        /// <summary>
        /// Количество миллисекунд с момента старта системы
        /// </summary>
        private int tickCount;
        #endregion;
    };
}
