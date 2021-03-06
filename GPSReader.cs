////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           GPSReader.cs
//
//  Facility:       ?????? ?????? GPS ?????????.
//
//
//  Abstract:       ?????? ???????? ???????? ???????.
//
//  Environment:    VC# 7.1
//
//  Author:         ?????? ?. ?.
//
//  Creation Date:  24/10/2005
//
//  Copyright (C) OOO "???????", 2005-2006. ??? ????? ????????.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: GPSReader.cs $
 * 
 * *****************  Version 10  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Updated in $/LightCom/.NET/Common
 * ??????????? ????????? Windows Mobile GPS API
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 25.06.07   Time: 8:29
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 19.06.07   Time: 7:36
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 9.06.07    Time: 9:11
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 5.06.07    Time: 13:42
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 1.06.07    Time: 7:43
 * Updated in $/LightCom/.NET/Common
 * ????????? ????????? ?????????? GPS
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LightCom.Gps
{
    ///
    /// <summary>
    /// ?????? ?????? ?? GPS ?????????
    /// </summary>
    /// 

    public class GPSReader
    {

        ////////////////////////////////////////////////////////////////////////////////
        ///
        /// <summary>
        ///  ?????? ??????? ?? GPS ?????????.
        /// </summary>
        /// <param name="port">COM ????.</param>
        /// <param name="gpsData">???????? ?????? ???????-GPS ??????.</param>
        /// <param name="buffer">?????, ? ??????? ???????? RAW ??????.</param>
        /// <returns>true, ???? ?????? ?????? ???????.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static bool ReadGPSData (System.IO.Ports.SerialPort port,
            out string [] gpsData)
        {
            gpsData = null;

            //
            // ?????? ?????.
            //

            string strCommands = null;
            try
            {
                port.Encoding = ASCIIEncoding.ASCII;
                strCommands = port.ReadExisting ();
            }
            catch (Exception /*e*/)
            {
                strCommands = null;
            }
            if (string.IsNullOrEmpty (strCommands))
            {
                return false;
            }

            //
            // ??????????? ???????? ????? ? ???????.
            //

            //ASCIIEncoding encoder = new ASCIIEncoding ();
            //string strCommands = encoder.GetString (buffer, 0, (int) nBytesRead);

            //
            // ???? ?????? ?????? ???????.
            //

            int nFirst = strCommands.IndexOf ('$');
            if (-1 == nFirst)
            {
                return false;
            }

            //
            // ???? ????????? ?????? ???????.
            //

            string delimStr = "$\n";
            char [] delimiters = delimStr.ToCharArray ();
            int nLast = strCommands.LastIndexOfAny (delimiters);
            if (-1 == nLast)
            {
                return false;
            }

            strCommands = strCommands.Substring (nFirst, nLast - nFirst);

            string [] textCommands = strCommands.Split (delimiters);
            int nSize = textCommands.Length;
            uint nCount = 0;
            for (int nIdx = 0; nIdx < nSize; ++nIdx)
            {
                if (String.IsNullOrEmpty (textCommands [nIdx]))
                    continue;
                ++nCount;
            }

            gpsData = new string [nCount];
            int nParsedIdx = -1;
            for (int nIdx = 0; nIdx < nSize; ++nIdx)
            {
                if (String.IsNullOrEmpty (textCommands [nIdx]))
                    continue;
                gpsData [++nParsedIdx] = "$" + textCommands [nIdx].Trim ();
            }

            return true;
        }


        ////////////////////////////////////////////////////////////////////////////////
        ///
        /// <summary>
        /// ????????? ?????????? ?? RMC ???????.
        /// </summary>
        /// <param name="strData">RMC ???????</param>
        /// <param name="fLongitude">???????.</param>
        /// <param name="fLatitude">??????.</param>
        /// <param name="fCourse">???????? ??????????? ????? ? ???????? </param>
        /// <param name="fixTime">????? ????????? ?????????</param>
        /// <returns>true if operation completed sucessfully</returns>
        /// 
        //////////////////////////////////////////////////////////////////////////////// 

        public static bool GetPosition (string strData,
            out double fLongitude,
            out double fLatitude,
            out double fSpeed,
            out double fCourse,
            out DateTime fixTime)
        {
            fixTime = DateTime.Now;
            fSpeed = 0;
            fLongitude = 0;
            fLatitude = 0;
            fCourse = 0;
            try
            {
                //   0     1    2    3      4   5        6  7      8    9       10  11
                //$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800, 14.8,W*41

                NumberFormatInfo provider = new NumberFormatInfo ();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = " ";

                if (!IsGPSDataValid (strData))
                    return false;

                char [] separator = new char [1];
                separator [0] = ',';
                char [] zeros = new char [1];
                zeros [0] = '0';

                string [] parsedData = strData.Split (separator);
                parsedData [5] = parsedData [5].TrimStart (zeros);
                fLongitude = Convert.ToDouble (parsedData [5], provider);
                /*
                fLongitude = (int) (nTmp / 100) + ((nTmp % 100) / 60.0) + 
                    (fLongitude - nTmp) / 36;
                */

                uint nDeg = (uint) (((uint) fLongitude) / 100);
                fLongitude = nDeg + ((fLongitude - nDeg * 100) / 60.0);

                if (parsedData [6] == "S")
                    fLongitude = -fLongitude;

                parsedData [3] = parsedData [3].TrimStart (zeros);

                fLatitude = Convert.ToDouble (parsedData [3], provider);
                /*
                fLatitude = (int) (nTmp / 100) + ((nTmp % 100) / 60.0) + 
                    (fLatitude - nTmp) / 36;
                */
                nDeg = (uint) (((uint) fLatitude) / 100);
                fLatitude = nDeg + ((fLatitude - nDeg * 100) / 60.0);

                if (parsedData [4] == "W")
                    fLatitude = -fLatitude;

                parsedData [5] = parsedData [5].TrimStart (zeros);

                //try
                //{
                fSpeed = Convert.ToDouble (parsedData [7], provider);
                fSpeed = fSpeed * 1.853;
                /*}
                catch (Exception)
                {
                    fSpeed = -1;
                }*/

                try
                {
                    fCourse = Convert.ToDouble (parsedData [8], provider);
                }
                catch (Exception)
                {
                    fCourse = 0;
                }

                string value;
                value = parsedData [9].Substring (4, 2);
                int nYear = 2000 + Convert.ToInt32 (value);
                value = parsedData [9].Substring (2, 2);
                int nMonth = Convert.ToInt32 (value);
                value = parsedData [9].Substring (0, 2);
                int nDay = Convert.ToInt32 (value);
                
                value = parsedData [1].Substring (0, 2);
                int nHour = Convert.ToInt32 (value);
                value = parsedData [1].Substring (2, 2);
                int nMinute = Convert.ToInt32 (value);
                value = parsedData [1].Substring (4, 2);
                int nSecond = Convert.ToInt32 (value);

                fixTime = new DateTime (nYear, nMonth, nDay, nHour, nMinute, nSecond, DateTimeKind.Utc);
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
        /// Check if GPS data is valid.
        /// </summary>
        /// <param name="strData">GPS data.</param>
        /// <returns>Check result.</returns>
        /// 
        //////////////////////////////////////////////////////////////////////////////// 

        public static bool IsGPSDataValid (string strData)
        {
            try
            {
                char [] separator = new char [1];
                separator [0] = ',';
                string [] parsedData = strData.Split (separator);
                if (parsedData [0] != "$GPRMC")
                    return false;
                //if (parsedData [2] != "A")
                //    return false;
                //if (String.IsNullOrEmpty (parsedData [3]) || String.IsNullOrEmpty (parsedData [4]) ||
                //    String.IsNullOrEmpty (parsedData [5]) || String.IsNullOrEmpty (parsedData [6]))
                //    return false;
                           
                if (! IsChecksumValid (strData))
                {
                    return false;
                }                
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// ?????????? ??????????? ?????
        /// </summary>
        /// <param name="strData">??????, ??????????? ????? ??????? ????? ?????????</param>
        /// <returns>??????????? ?????</returns>
        public static int CalculateChecksum (string strData)
        {
            int checSum = 0;
            int nLen = strData.Length;
            for (int i = 1; i < nLen && strData [i] != '*'; ++i)
            {
                checSum ^= (int) (strData [i]);
            }
            
            return checSum;
        }
        
        /// <summary>
        /// ???????? ??????????? ?????
        /// </summary>
        /// <param name="strData">??????, ??????????? ????? ??????? ????? ?????????</param>
        /// <returns>true, ???? ??????????? ????? ???????</returns>
        public static bool IsChecksumValid (string strData)
        {
            int checSum = CalculateChecksum (strData);
            string strChecSum = '*' + System.Uri.HexEscape ((char) checSum).Substring (1);
            if (!strData.EndsWith (strChecSum))
            {
                return false;
            }           
            
            return true;
        }       
        
        
        /// <summary>
        /// ?????????, ?? ????? ????????? ???????? ?????? GPS ?????? ?? ?????.
        /// </summary>
        /// <param name="port">??????????? ????</param>
        /// <param name="baudRates">??????????? ????????</param>
        /// <returns>?????? ?????????, ?? ??????? ??????? ???????? GPS ??????</returns>
        public static List<int> CheckBaudRates (System.IO.Ports.SerialPort port, int [] baudRates)
        {
            if (null == baudRates)
            {
                baudRates = new int [] {1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 28800, 38400, 56000, 57600};
            }
            List<int> result = new List<int> (baudRates.Length);
            foreach (int baudRate in baudRates)
            {
                try
                {
                    port.BaudRate = baudRate;
                    if (IsValidGPSPort (port))
                    {
                        result.Add(baudRate);
                    }
                }
                catch (Exception)
                {                    
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// ?????????, ???????? ?? GPS ???? ?????????? GPS ??????
        /// </summary>
        /// <param name="port">??????????? ????. ?????? ???? ??????.</param>
        /// <param name="buffer">????? ??? ?????? ???????? ??????</param>
        /// <returns>true, ???? ?? ????? ??????? ????????? ???????? GPS ??????</returns>
        public static bool IsValidGPSPort (System.IO.Ports.SerialPort port)
        {
            try
            {                
                //
                // ????????? ?????, ??????????? ??? ?????? ??????. ?????????? 
                // ????????? ????? ???????
                //
                
                //int nReadTimeout = buffer.Length * 8 * 4 * 1000 / port.BaudRate;
                //port.ReadTimeout = nReadTimeout;
                
                string [] result;
                if (! GPSReader.ReadGPSData (port, out result))
                {
                    return false;
                }
                
                if (null == result)
                {
                    return false;
                }
                
                foreach (string val in result)
                {
                    if (string.IsNullOrEmpty (val))
                    {
                        continue;
                    }
                    
                    if (! val.StartsWith ("$", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    
                    if (GPSReader.IsChecksumValid (val))
                    {
                        return true;
                    }
                }
                
            }
            catch (Exception)
            {
                return false;
            }
            
            return false;
        }
    }
}