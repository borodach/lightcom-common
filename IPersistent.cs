///////////////////////////////////////////////////////////////////////////////
//
//  File:           IPersistent.cs
//
//  Facility:       Persistance.
//
//
//  Abstract:       Интерфейс сохранения состояния объекта в бинарный объект
//                  и восстановления из него.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
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
    /// Интерфейс сохранения состояния объекта в бинарный объект и 
    /// восстановления из него.
    /// </summary>
    /// 

    public interface IPersistent
    {
        /// 
        /// <summary>
        /// Сбрасывает объек в начальное состояние.
        /// </summary>
        /// 

        void Reset ();

        /// 
        /// <summary>
        /// Сохраняет состояние объекта в поток stream.
        /// </summary>
        /// <param name="stream">Поток, в который выполняется сохранение.
        /// </param>
        /// <returns>true, если операция выполнилась успешно.</returns>
        ///
 
        bool SaveGuts (Stream stream);

        /// 
        /// <summary>
        /// Восстанавливает сосояние объекта из потока stream.
        /// </summary>
        /// <param name="stream">Поток, из которго выполняется восстановление.
        /// </param>
        /// <returns>true, если операция выполнилась успешно.</returns>
        ///

        bool RestoreGuts (Stream stream);
    }
}
