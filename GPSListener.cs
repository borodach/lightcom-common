///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSListener.cs
//
//  Facility:       Получение сведений от GPS приемника
//                  
//
//
//  Abstract:       Базовый класс обработки GPS данных
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  18-Jun-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: GPSListener.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Updated in $/LightCom/.NET/Common
 * Реализована поддержка Windows Mobile GPS API
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 25.06.07   Time: 8:29
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 19.06.07   Time: 7:42
 * Created in $/LightCom/.NET/Common
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using LightCom.Common;

namespace LightCom.Gps
{
    /// <summary>
    /// Базовый класс обработки GPS данных
    /// </summary>
    public abstract class GPSListener: PipeLine, ISettings
    {
        ///
        /// <summary>
        /// Стостояние объекта.
        /// </summary>
        /// 

        public enum State
        {
            OK,
            Error,
            Warning
        };

        ////////////////////////////////////////////////////////////////////////////////
        ///
        /// <summary>
        /// Состояние GPS приемника.
        /// </summary>
        /// 
        /// ////////////////////////////////////////////////////////////////////////////

        protected State GPSReceiverState
        {
            get
            {
                return this.m_GPSReceiverState;
            }
            set
            {

                bool bStateChanged = this.m_GPSReceiverState != value;
                this.m_GPSReceiverState = value;
                if (bStateChanged)
                    OnGPSReceiverStateChanged (this.m_GPSReceiverState);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        ///
        /// <summary>
        /// Состояние передатчика.
        /// </summary>
        /// 
        /// ////////////////////////////////////////////////////////////////////////////

        protected State PublisherState
        {
            get
            {
                return this.m_PublisherState;
            }
            set
            {

                bool bStateChanged = this.m_PublisherState != value;
                this.m_PublisherState = value;
                if (bStateChanged)
                    OnPublisherStateChanged (this.m_PublisherState);
            }
        }

        #region "ISettings methods"

        /// <summary>
        /// Сбрасывает состояние объекта в начальное состояние.
        /// </summary>
        public abstract void Reset ();

        /// <summary>
        /// Сохраняет настройки объекта в заданном хранилище.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если сохранение прошло успешно.
        /// </returns>
        public abstract bool Save (SettingsStorage storage);
        
        /// <summary>
        /// Загружает настройки объекта из заданного хранилища.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если загрузка прошла успешно.
        /// </returns>
        public abstract bool Load (SettingsStorage storage);
        
        #endregion

        #region "PipeLine methods"
        /// <summary>
        /// Обработка элемента конвейера.
        /// Метод выполняется в отдельном потоке.
        /// </summary>
        /// <param name="obj">Обрабатываемый элемент.</param>
        protected override abstract void ProcessItem (object obj);
                
        /// <summary>
        /// Обработка элемента конвейера.
        /// Метод выполняется в отдельном потоке.
        /// </summary>
        /// <param name="obj">Обрабатываемый элемент.</param>
        protected override abstract bool GetDataItem (out object obj);
        

        #endregion

        /// <summary>
        /// The protected OnPublisherStateChanged method raises the event by invoking  
        /// the delegates. The sender is always this, the current instance of the class.
        /// </summary>
        /// <param name="state">Новое состояние передатчика.</param>
        protected virtual void OnPublisherStateChanged (State state)
        {
            if (PublisherStateChanged != null)
            {
                //
                // Invokes the delegates. 
                //

                PublisherStateChanged (this, state);
            }
        }

        /// <summary>
        /// Делегат обработки изменения состояния.
        /// </summary>
        public delegate void StateChangedEventHandler (GPSListener source,
                                                       State state);

        /// <summary>
        /// Событие "изменилось состояние GPS приемника".
        /// </summary>
        public event StateChangedEventHandler GPSReceiverStateChanged;

        /// <summary>
        /// Событие "изменилось состояние передатчика данных".
        /// </summary>
        public event StateChangedEventHandler PublisherStateChanged;

        /// <summary>
        /// Состояние GPS приемника.
        /// </summary>
        protected State m_GPSReceiverState = State.OK;

        /// <summary>
        /// Состояние передатчика.
        /// </summary>
        protected State m_PublisherState = State.OK;

        /// <summary>
        /// The protected OnGPSReceiverStateChanged method raises the event by invoking  
        /// the delegates. The sender is always this, the current instance of the class.
        /// </summary>
        /// <param name="state">Новое состояние GPS приемника.</param>
        protected virtual void OnGPSReceiverStateChanged (State state)
        {
            if (GPSReceiverStateChanged != null)
            {
                //
                // Invokes the delegates. 
                //

                GPSReceiverStateChanged (this, state);
            }
        }


        /// <summary>
        /// Делегат обработки новых GPS данных
        /// </summary>
        /// <param name="source">Источник события</param>
        /// <param name="position">Сведения GPS</param>
        public delegate void GPSDataReadEventHandler (GPSListener source,
            LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION position);

        /// 
        /// <summary>
        /// Событие "получены новые GPS данные"
        /// </summary>
        /// 

        public event GPSDataReadEventHandler GPSDataRead;

        /// <summary>
        /// Обработчик получения новых GPS данных
        /// </summary>
        /// <param name="position">Сведения GPS</param>
        protected virtual void OnGPSDataRead (LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION position)
        {
            if (GPSDataRead != null)
            {
                //
                // Invokes the delegates. 
                //

                GPSDataRead (this, position);
            }
        }

        /// <summary>
        /// Закрывает GPS соединение
        /// </summary>
        public abstract void CloseGps ();     

        /// <summary>
        /// Открывает GPS соединение
        /// </summary>
        public abstract bool OpenGps ();

        /// <summary>
        /// Открыто ли GPS соединение
        /// </summary>
        public abstract bool IsOpen
        {
            get;
        }
    }
}