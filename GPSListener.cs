///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSListener.cs
//
//  Facility:       ????????? ???????? ?? GPS ?????????
//                  
//
//
//  Abstract:       ??????? ????? ????????? GPS ??????
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ?????? ?.?.
//
//  Creation Date:  18-Jun-2007
//
//  Copyright (C) OOO "???????", 2007. ??? ????? ????????.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: GPSListener.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Updated in $/LightCom/.NET/Common
 * ??????????? ????????? Windows Mobile GPS API
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
    /// ??????? ????? ????????? GPS ??????
    /// </summary>
    public abstract class GPSListener: PipeLine, ISettings
    {
        ///
        /// <summary>
        /// ?????????? ???????.
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
        /// ????????? GPS ?????????.
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
        /// ????????? ???????????.
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
        /// ?????????? ????????? ??????? ? ????????? ?????????.
        /// </summary>
        public abstract void Reset ();

        /// <summary>
        /// ????????? ????????? ??????? ? ???????? ?????????.
        /// </summary>
        /// <param name="storage">????????? ?????????? ??????????.</param>
        /// <returns>?????????? true, ???? ?????????? ?????? ???????.
        /// </returns>
        public abstract bool Save (SettingsStorage storage);
        
        /// <summary>
        /// ????????? ????????? ??????? ?? ????????? ?????????.
        /// </summary>
        /// <param name="storage">????????? ?????????? ??????????.</param>
        /// <returns>?????????? true, ???? ???????? ?????? ???????.
        /// </returns>
        public abstract bool Load (SettingsStorage storage);
        
        #endregion

        #region "PipeLine methods"
        /// <summary>
        /// ????????? ???????? ?????????.
        /// ????? ??????????? ? ????????? ??????.
        /// </summary>
        /// <param name="obj">?????????????? ???????.</param>
        protected override abstract void ProcessItem (object obj);
                
        /// <summary>
        /// ????????? ???????? ?????????.
        /// ????? ??????????? ? ????????? ??????.
        /// </summary>
        /// <param name="obj">?????????????? ???????.</param>
        protected override abstract bool GetDataItem (out object obj);
        

        #endregion

        /// <summary>
        /// The protected OnPublisherStateChanged method raises the event by invoking  
        /// the delegates. The sender is always this, the current instance of the class.
        /// </summary>
        /// <param name="state">????? ????????? ???????????.</param>
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
        /// ??????? ????????? ????????? ?????????.
        /// </summary>
        public delegate void StateChangedEventHandler (GPSListener source,
                                                       State state);

        /// <summary>
        /// ??????? "?????????? ????????? GPS ?????????".
        /// </summary>
        public event StateChangedEventHandler GPSReceiverStateChanged;

        /// <summary>
        /// ??????? "?????????? ????????? ??????????? ??????".
        /// </summary>
        public event StateChangedEventHandler PublisherStateChanged;

        /// <summary>
        /// ????????? GPS ?????????.
        /// </summary>
        protected State m_GPSReceiverState = State.OK;

        /// <summary>
        /// ????????? ???????????.
        /// </summary>
        protected State m_PublisherState = State.OK;

        /// <summary>
        /// The protected OnGPSReceiverStateChanged method raises the event by invoking  
        /// the delegates. The sender is always this, the current instance of the class.
        /// </summary>
        /// <param name="state">????? ????????? GPS ?????????.</param>
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
        /// ??????? ????????? ????? GPS ??????
        /// </summary>
        /// <param name="source">???????? ???????</param>
        /// <param name="position">???????? GPS</param>
        public delegate void GPSDataReadEventHandler (GPSListener source,
            LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION position);

        /// 
        /// <summary>
        /// ??????? "???????? ????? GPS ??????"
        /// </summary>
        /// 

        public event GPSDataReadEventHandler GPSDataRead;

        /// <summary>
        /// ?????????? ????????? ????? GPS ??????
        /// </summary>
        /// <param name="position">???????? GPS</param>
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
        /// ????????? GPS ??????????
        /// </summary>
        public abstract void CloseGps ();     

        /// <summary>
        /// ????????? GPS ??????????
        /// </summary>
        public abstract bool OpenGps ();

        /// <summary>
        /// ??????? ?? GPS ??????????
        /// </summary>
        public abstract bool IsOpen
        {
            get;
        }
    }
}