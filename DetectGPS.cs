////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           DetectGPS
//
//  Facility:       ????????????? ????? GPS ?????????.
//
//
//  Abstract:       ????? ??? ?????? GPS ?????????.
//
//  Environment:    VC# 8.0
//
//  Author:         ?????? ?.?.
//
//  Creation Date:  05?06.2007
//
//  Copyright (C) OOO "???????", 2005-2007. ??? ????? ????????.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: DetectGPS.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 19.06.07   Time: 7:42
 * Created in $/LightCom/.NET/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 9.06.07    Time: 9:11
 * Updated in $/LightCom/.NET/GPSDetector
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 8.06.07    Time: 21:38
 * Updated in $/LightCom/.NET/GPSDetector
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 8.06.07    Time: 21:20
 * Updated in $/LightCom/.NET/GPSDetector
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 8.06.07    Time: 8:26
 * Updated in $/LightCom/.NET/GPSDetector
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 5.06.07    Time: 13:42
 * Created in $/LightCom/.NET/GPSDetector
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LightCom.Gps
{   
    /// <summary>
    /// ????????????? ????? GPS ?????????
    /// </summary>
    public class DetectGPS
    {
        public enum EventType
        {
            etPortScanStarted,
            etPortScanFinished,
            etGPSPortFound,
            etGPSPortNotFound,
            etBaudRateScanStarted,
            etBaudRateScanFinished,
            etGPSBaudRateFound,
            etGPSBaudRateNotFound,
            etAbortRequestReceived
        };
        
        /// <summary>
        /// ??????? ????????? ??????? ????????????
        /// </summary>
        /// <param name="et">??? ???????</param>
        /// <param name="port">??? ?????</param>
        /// <param name="baudRate">???????? ?????</param>
        public delegate void GPSDetectorEventHandler (EventType et, string port, int baudRate);

        /// <summary>
        /// ????? ??????
        /// </summary>
        public class ThreadClass
        {
            /// <summary>
            /// ??????????? ??????
            /// </summary>
            /// <param name="sr">????????? ??????</param>
            /// <param name="handler">?????????? ??????? ????????????</param>
            public ThreadClass (SearchResult sr,
                                GPSDetectorEventHandler handler) 
            { 
                searchResult = sr;
                eventHandler = handler;
                abortRequest = false;
            }

            /// <summary>
            /// ?????? ?? ?????????? ????????????
            /// </summary>
            public void SendAbortRequest ()
            {
                abortRequest = true;
            }

            /// <summary>
            /// ????????? ??????? ????????????
            /// </summary>
            /// <param name="eventType">??? ?????????</param>
            /// <param name="baudRate">???????? ?????</param>
            void GenerateEvent (EventType eventType, int baudRate)
            {
                if (null == this.eventHandler)
                {
                    return;
                }
                this.eventHandler.Invoke (eventType, this.searchResult.portName, baudRate);
            }
                           
            /// <summary>
            /// ??????? ??????????? ????????? ??????
            /// </summary>
            private static int [] baudRates = new int [] { 57600, /*56000, 38400, 28800,*/ 19200, /*14400,*/ 9600, /*7200,*/ 4800 };

            /// <summary>
            /// ??????? ??????
            /// </summary>
            public void DoWork () 
            {   
                System.IO.Ports.SerialPort port = null;
                try
                {
                    GenerateEvent (EventType.etPortScanStarted, 0);
                    port = new System.IO.Ports.SerialPort (searchResult.portName);
                    try
                    {
                        port.Open ();
                        Thread.Sleep (2000);
                        searchResult.baudRates = new List<int> (baudRates.Length);
                        foreach (int baudRate in baudRates)
                        {
                            if (abortRequest)
                            {
                                GenerateEvent (EventType.etAbortRequestReceived, baudRate);
                                return;
                            }

                            try
                            {
                                GenerateEvent (EventType.etBaudRateScanStarted, baudRate);
                                port.Close ();
                                port.BaudRate = baudRate;
                                port.ReadTimeout = 2000;
                                port.Open ();
                                Thread.Sleep (3000);
                                if (GPSReader.IsValidGPSPort (port))
                                {
                                    searchResult.baudRates.Add (baudRate);
                                    GenerateEvent (EventType.etGPSBaudRateFound, baudRate);
                                }
                                else
                                {
                                    GenerateEvent (EventType.etGPSBaudRateNotFound, baudRate);
                                }
                            }
                            catch (Exception)
                            {
                            }

                            GenerateEvent (EventType.etBaudRateScanFinished, baudRate);
                        }

                        if (null != searchResult.baudRates && searchResult.baudRates.Count > 0)
                        {
                            GenerateEvent (EventType.etGPSPortFound, 0);
                        }
                        else
                        {
                            GenerateEvent (EventType.etGPSPortNotFound, 0);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                finally
                {
                    if (null != port)
                    {
                        port.Close ();
                    }
                    GenerateEvent (EventType.etPortScanFinished, 0);
                }
            }

            /// <summary>
            /// ????????? ??????
            /// </summary>
            private SearchResult searchResult;

            /// <summary>
            /// ?????? ?? ?????????? ??????
            /// </summary>
            public bool abortRequest;

            /// <summary>
            /// ?????????? ??????? ????????????
            /// </summary>
            GPSDetectorEventHandler eventHandler;
        }


        /// <summary>
        /// ????????? ?????? GPS ?????????
        /// </summary>
        public struct SearchResult
        {
            /// <summary>
            /// ??? ?????
            /// </summary>
            public string portName;

            /// <summary>
            /// ????? ?????????, ?? ??????? ??????? ????
            /// </summary>
            public List<int> baudRates;            
        };

       /// <summary>
       /// ?????????, ??????????? ?? ????????????
       /// </summary>
        public bool IsScanning
        {
            get
            {
                if (null == threads)
                {
                    return false;
                }

                foreach (Thread thread in threads)
                {
                    try
                    {
                        if (!thread.Join (0))
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// ?????????? ?????? ????????? GPS ??????
        /// </summary>
        public List<SearchResult> ScanResult
        {   
            get 
            {
                if (IsScanning)
                {
                    return null;
                }

                if (null == gpsPorts)
                {
                    return null;
                }

                List<SearchResult> res = new List<SearchResult>();                

                //
                // ???????????? ??????????
                //

                for (int idx = gpsPorts.Count - 1; idx >= 0; --idx)
                {
                    if (gpsPorts [idx].baudRates != null && gpsPorts [idx].baudRates.Count > 0)
                    {
                        res.Add (gpsPorts [idx]);
                        continue;
                    }
                    gpsPorts.RemoveAt (idx);
                }

                return res;
           }
        }

        /// <summary>
        /// ???????? ?????????? ????????????
        /// </summary>
        public void WaitForScanCompleted ()
        {
            if (null == threads)
            {
                return;
            }

            //
            // ???? ?????????? ???????
            //

            foreach (Thread thread in threads)
            {
                try
                {
                    thread.Join ();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// ?????? ??????????????? ?????? GPS ?????????.
        /// </summary>
        /// <returns>true, ???? ???????????? ??????</returns>
        public bool Start (GPSDetectorEventHandler handler)
        {
            gpsPorts.Clear ();
            try
            {
                //string [] portNames = System.IO.Ports.SerialPort.GetPortNames ();
                //if (null == portNames)
                //{
                //    return false;
                //}

                string [] portNames = new string [16];
                int idx = 0;
                for (idx = 0; idx < portNames.Length; ++idx)
                {
                    portNames [idx] = string.Format ("COM{0}:", idx + 1);
                }

                    
                if (portNames.Length < 1)
                {
                    return false;
                }

                threads = new Thread [portNames.Length];
                idx = 0;
                foreach (string portName in portNames)
                {
                    SearchResult sr = new SearchResult ();
                    sr.portName = portName;
                    gpsPorts.Add (sr);
                    ThreadClass tc = new ThreadClass (sr, handler);
                    threadContextes.Add (tc);
                    threads [idx] = new Thread (tc.DoWork);
                    threads [idx].Start ();
                    ++idx;
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ?????????? ????? GPS ?????????
        /// </summary>
        /// <param name="handler">??????? ??? ?????? ????????? ? ????????? ????????????</param>
        /// <returns>????????? ????? ? ????????</returns>
        public List<SearchResult> ScanAndWait (GPSDetectorEventHandler handler)
        {
            if (!Start (handler))
            {
                return new List<SearchResult> ();
            }

            WaitForScanCompleted ();

            List<SearchResult> scanResult = this.ScanResult;
            return scanResult;
        }

        /// <summary>
        /// ?????? ?? ?????????? ????????????
        /// </summary>
        public void SendAbortRequest ()
        {

            foreach (ThreadClass tc in threadContextes)
            {
                tc.SendAbortRequest ();
            }

        }

        /// <summary>
        /// ????????????? ????????????
        /// </summary>
        /// <param name="waitTimeout">????? ???????? ?????????? ??????? (??)
        /// </param>
        public void Stop (int waitTimeout)
        {
            if (!this.IsScanning)
            {
                return;
            }
            SendAbortRequest ();

            while (this.IsScanning)
            {
                foreach (Thread thread in threads)
                {
                    try
                    {
                        if (!thread.Join (waitTimeout / threads.Length))
                        {
                            thread.Abort ();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }            
        }

        /// <summary>
        /// ?????? ????????????.
        /// </summary>
        private Thread [] threads;
        
        /// <summary>
        /// ?????? ????????????.
        /// </summary>
        public Thread [] Threads
        {
            get { return threads; }
        }

        /// <summary>
        /// ?????????? ????????????
        /// </summary>
        private List<SearchResult> gpsPorts = new List<SearchResult> ();
        
        /// <summary>
        /// ????????? ???????.
        /// </summary>
        private List<ThreadClass> threadContextes = new List<ThreadClass> ();                
    }
}
