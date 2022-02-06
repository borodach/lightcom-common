///////////////////////////////////////////////////////////////////////////////
//
//  File:           WinMobile5GPSWrapper.cs
//
//  Facility:       Windows Mobile GPS API
//                  
//
//
//  Abstract:       Обертка для работы с Windows Mobile 5 GPS API
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
* $History: WinMobile5GPSWrapper.cs $
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
using System.Runtime.InteropServices;
using System.Globalization;

namespace LightCom.WinCE
{

    /// <summary>
    /// Обертка для работы с Windows Mobile 5 GPS API
    /// </summary>
    public class WinMobile5GPSWrapper
    {
        #region "DataTypes"
        public const int GPS_MAX_SATELLITES = 12;      //Maximum number of GPS satellites used by the GPS Intermediate Driver. 
        public const int GPS_MAX_PREFIX_NAME = 16;     //Maximum size, in characters, of prefixes used for CreateFile device names. For example, this is the maximum size of the szGPSDriverPrefix and szGPSMultiplexPrefix fields in the GPS_POSITION structure.  
        public const int GPS_MAX_FRIENDLY_NAME = 64;   //Maximum size, in characters, of data like the friendly name of an input source. For example, this is the maximum size of the szGPSFriendlyName field in the GPS_POSITION structure. 
        public const int GPS_VERSION_1 = 1;            //GPS API version

        /// <summary>
        /// Флаги валидности структуры GPS_POSITION
        /// </summary>
        [Flags]
        public enum GPS_VALID : int
        {
            GPS_VALID_UTC_TIME = 0x00000001,                                    // If set, the stUTCTime field is valid. 
            GPS_VALID_LATITUDE = 0x00000002,                                    // If set, the dblLatitude field is valid. 
            GPS_VALID_LONGITUDE = 0x00000004,                                   //If set, the dblLongitude field is valid. 
            GPS_VALID_SPEED = 0x00000008,                                       //If set, the flSpeed field is valid. 
            GPS_VALID_HEADING = 0x00000010,                                     //If set, the flHeading field is valid. 
            GPS_VALID_MAGNETIC_VARIATION = 0x00000020,                          //If set, the dblMagneticVariation field is valid. 
            GPS_VALID_ALTITUDE_WRT_SEA_LEVEL = 0x00000040,                      //If set, the flAltitudeWRTSeaLevel field is valid. 
            GPS_VALID_ALTITUDE_WRT_ELLIPSOID = 0x00000080,                      //If set, the flAltitudeWRTEllipsoid field is valid. 
            GPS_VALID_POSITION_DILUTION_OF_PRECISION = 0x00000100,              //If set, the flPositionDilutionOfPrecision field is valid. 
            GPS_VALID_HORIZONTAL_DILUTION_OF_PRECISION = 0x00000200,            //If set, the flHorizontalDilutionOfPrecision field is valid. 
            GPS_VALID_VERTICAL_DILUTION_OF_PRECISION = 0x00000400,              //If set, the flVerticalDilutionOfPrecision field is valid. 
            GPS_VALID_SATELLITE_COUNT = 0x00000800,                             //If set, the dwSatelliteCount field is valid. 
            GPS_VALID_SATELLITES_USED_PRNS = 0x00001000,                        //If set, the rgdwSatellitesUsedPRNs field is valid. 
            GPS_VALID_SATELLITES_IN_VIEW = 0x00002000,                          //If set, the dwSatellitesInView field is valid. 
            GPS_VALID_SATELLITES_IN_VIEW_PRNS = 0x00004000,                     //If set, the rgdwSatellitesInViewPRNs field is valid. 
            GPS_VALID_SATELLITES_IN_VIEW_ELEVATION = 0x00008000,                //If set, the rgdwSatellitesInViewElevation field is valid. 
            GPS_VALID_SATELLITES_IN_VIEW_AZIMUTH = 0x000010000,                 //If set, the rgdwSatellitesInViewAzimuth field is valid. 
            GPS_VALID_SATELLITES_IN_VIEW_SIGNAL_TO_NOISE_RATIO = 0x00020000,    //If set, the rgdwSatellitesInViewSignalToNoiseRatio field is valid. 
        };

        /// <summary>
        /// Тип GPS сигнала
        /// </summary>
        public enum GPS_FIX_QUALITY : int
        {
            GPS_FIX_QUALITY_UNKNOWN = 0,
            GPS_FIX_QUALITY_GPS,
            GPS_FIX_QUALITY_DGPS
        } ;

        /// <summary>
        /// Двухмерное или трехмерное позиционирование
        /// </summary>
        public enum GPS_FIX_TYPE : int
        {
            GPS_FIX_UNKNOWN = 0,
            GPS_FIX_2D,
            GPS_FIX_3D
        };

        /// <summary>
        /// Способ выбора GPS fix
        /// </summary>
        public enum GPS_FIX_SELECTION : int
        {
            GPS_FIX_SELECTION_UNKNOWN = 0,
            GPS_FIX_SELECTION_AUTO,
            GPS_FIX_SELECTION_MANUAL
        };

        /// <summary>
        /// Состояние GPS
        /// </summary>
        public enum GPS_DATA_FLAGS : int
        {
            GPS_DATA_FLAGS_HARDWARE_OFF = 0x00000001,   //The GPS Intermediate Driver does not have a connection to GPS hardware. The returned data has been retrieved from the GPS Intermediate Driver cache.
        };

        /// <summary>
        /// Состояние сервиса GPS
        /// </summary>
        public enum IOCTL_SERVICE_STATUS : int
        {
            SERVICE_STATE_OFF = 0, //The service is turned off. 
            SERVICE_STATE_ON = 1, // The service is turned on. 
            SERVICE_STATE_STARTING_UP = 2, // The service is in the process of starting up. 
            SERVICE_STATE_SHUTTING_DOWN = 3, // The service is in the process of shutting down. 
            SERVICE_STATE_UNLOADING = 4, // The service is in the process of unloading. 
            SERVICE_STATE_UNINITIALIZED = 5, // The service is not uninitialized. 
            SERVICE_STATE_UNKNOWN = -1, // The state of the service is unknown. 
        };

        /// <summary>
        /// Системное время
        /// </summary>
        [StructLayout (LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;

            /// <summary>
            /// Оператор преобразования STYTEMTIME в DateTime
            /// </summary>
            /// <param name="tm"></param>
            /// <returns></returns>
            public static explicit operator DateTime (SYSTEMTIME tm)
            {
                DateTime result = new DateTime (tm.wYear, tm.wMonth, tm.wDay, tm.wHour, tm.wMinute, tm.wSecond, DateTimeKind.Utc);
                return result;
            }

            /// <summary>
            /// Создание экземпляра структуры из объекта DateTime
            /// </summary>
            /// <param name="tm">Объект DateTime</param>
            /// <returns>Созданный объект</returns>
            public static SYSTEMTIME FromDateTime (DateTime tm)
            {
                tm = tm.ToUniversalTime ();
                SYSTEMTIME res = new SYSTEMTIME ();
                res.wYear = (short) tm.Year;
                res.wMonth = (short) tm.Month;
                res.wDay = (short) tm.Day;
                res.wHour = (short) tm.Hour;
                res.wMinute = (short) tm.Minute;
                res.wSecond = (short) tm.Second;
                res.wDayOfWeek = (short) tm.DayOfWeek;
                return res;
            }

        };

        /// <summary>
        /// Файловый формат хранения времени
        /// </summary>
        [StructLayout (LayoutKind.Sequential)]
        public struct FILETIME
        {
            public int dwLowDateTime;
            public int dwHighDateTime;

            /// <summary>
            /// Оператор преобразования FILETIME в DateTime
            /// </summary>
            /// <param name="tm"></param>
            /// <returns></returns>
            public static explicit operator DateTime (FILETIME tm)
            {
                long val = (uint) tm.dwHighDateTime;
                val = (val << 32) + (uint) tm.dwLowDateTime;
                DateTime result = new DateTime (val, DateTimeKind.Utc);
                return result;
            }
        };

        /// <summary>
        /// This structure contains location information, including latitude 
        /// and longitude, as well as other related information like heading, 
        /// speed, the satellites used to retrieve the location information, 
        /// and so on.
        /// </summary>
        [StructLayout (LayoutKind.Sequential)]
        public class GPS_POSITION
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public GPS_POSITION ()
            {
                dwSize = Marshal.SizeOf (this);
                dwVersion = GPS_VERSION_1;
                dwValidFields = 0;
            }

            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="other">Копируемый объект</param>
            public GPS_POSITION (GPS_POSITION other)
            {
                this.dwVersion = other.dwVersion;
                this.dwSize = other.dwSize;
                this.dblLatitude = other.dblLatitude;
                this.dblLongitude = other.dblLongitude;
                this.dblMagneticVariation = other.dblMagneticVariation;
                this.dwFlags = other.dwFlags;
                this.dwSatelliteCount = other.dwSatelliteCount;
                this.dwSatellitesInView = other.dwSatellitesInView;
                this.dwValidFields = other.dwValidFields;
                this.FixQuality = other.FixQuality;
                this.FixType = other.FixType;
                this.flAltitudeWRTEllipsoid = other.flAltitudeWRTEllipsoid;
                this.flAltitudeWRTSeaLevel = other.flAltitudeWRTSeaLevel;
                this.flHeading = other.flHeading;
                this.flHorizontalDilutionOfPrecision = other.flHorizontalDilutionOfPrecision;
                this.flPositionDilutionOfPrecision = other.flPositionDilutionOfPrecision;
                this.flSpeed = other.flSpeed;
                this.flVerticalDilutionOfPrecision = other.flVerticalDilutionOfPrecision;
                other.rgdwSatellitesInViewAzimuth.CopyTo (this.rgdwSatellitesInViewAzimuth, 0);
                other.rgdwSatellitesInViewElevation.CopyTo (this.rgdwSatellitesInViewElevation, 0);
                other.rgdwSatellitesInViewPRNs.CopyTo (this.rgdwSatellitesInViewPRNs, 0);
                other.rgdwSatellitesInViewSignalToNoiseRatio.CopyTo (this.rgdwSatellitesInViewSignalToNoiseRatio, 0);
                other.rgdwSatellitesUsedPRNs.CopyTo (this.rgdwSatellitesUsedPRNs, 0);
                this.SelectionType = other.SelectionType;
                this.stUTCTime = other.stUTCTime;
            }

            /// <summary>
            /// КонструкторЮ создающий объект из RMС строки
            /// </summary>
            /// <param name="rmcString">RMС строка</param>
            public GPS_POSITION (string rmcString)
                : this ()
            {
                //$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800, 14.8,W*41
                this.RMC = rmcString;
            }


            /// <summary>
            /// Преобразование в/из RMC строки
            /// </summary>
            public string RMC
            {
                get
                {  
                    NumberFormatInfo provider = new NumberFormatInfo ();
                    provider.NumberDecimalSeparator = ".";
                    provider.NumberGroupSeparator = "";

                    string strTime = string.Empty;
                    string strDate = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_UTC_TIME))
                    {
                        strTime = string.Format ("{0:00}{1:00}{2:00}", this.stUTCTime.wHour, this.stUTCTime.wMinute, this.stUTCTime.wSecond);
                        strDate = string.Format ("{0:00}{1:00}{2:00}", this.stUTCTime.wDay, this.stUTCTime.wMonth, this.stUTCTime.wYear % 100);
                    }

                    string strSpeed = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_SPEED))
                    {
                        strSpeed = this.flSpeed.ToString ("0.0##", provider);
                    }

                    string strCourse = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_HEADING))
                    {
                        strCourse = this.flHeading.ToString ("0.0##", provider);
                    }

                    string strMagnetic = string.Empty;
                    string strMagneticEW = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_MAGNETIC_VARIATION))
                    {
                        strMagnetic = Math.Abs (this.dblMagneticVariation).ToString ("0.0##", provider);
                        strMagneticEW = this.dblMagneticVariation <= 0 ? "W": "E";    
                    }

                    string strLat = string.Empty;
                    string strLatNS = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_LATITUDE))
                    {
                        double lat = Math.Abs (dblLatitude);
                        double deg = Math.Floor (lat);
                        double min = (lat - deg) * 60;
                        double val = 100 * deg + min; 
                        strLat = val.ToString ("0000.0###", provider);
                        strLatNS = this.dblLatitude <= 0 ? "S": "N";    
                    }

                    string strLon = string.Empty;
                    string strLonEW = string.Empty;
                    if (0 != (this.dwValidFields & GPS_VALID.GPS_VALID_LONGITUDE))
                    {
                        double lon = Math.Abs (dblLongitude);
                        double deg = Math.Floor (lon);
                        double min = (lon - deg) * 60;
                        double val = 100 * deg + min; 
                        strLon = val.ToString ("00000.0###", provider);
                        strLonEW = this.dblLongitude <= 0 ? "W" : "E";    
                    }

                    string result = string.Format ("$GPRMC,{0},A,{1},{2},{3},{4},{5},{6},{7},{8},{9}*", strTime, strLat, strLatNS, strLon, strLonEW, strSpeed, strCourse, strDate, strMagnetic, strMagneticEW);
                    int checSum = LightCom.Gps.GPSReader.CalculateChecksum (result);
                    return result + System.Uri.HexEscape ((char) checSum).Substring (1);
                }
                set
                {
                    try
                    {
                        //   0     1    2    3      4   5        6  7      8    9       10  11
                        //$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800, 14.8,W*41

                        NumberFormatInfo provider = new NumberFormatInfo ();
                        provider.NumberDecimalSeparator = ".";
                        provider.NumberGroupSeparator = "";
                             
                        if (!LightCom.Gps.GPSReader.IsGPSDataValid (value))
                            return;

                        char [] separator = {',', '*'};
                        char [] zeros = { '0' };
                        uint nDeg;
                        string [] parsedData = value.Split (separator);
                        this.dwValidFields = 0;

                        //
                        // Longitude
                        //

                        try
                        {
                            parsedData [5] = parsedData [5].TrimStart (zeros);
                            dblLongitude = Convert.ToDouble (parsedData [5], provider);

                            nDeg = (uint) (((uint) dblLongitude) / 100);
                            dblLongitude = nDeg + ((dblLongitude - nDeg * 100) / 60.0);
                            if (parsedData [6] == "W")
                                dblLongitude = -dblLongitude;

                            this.dwValidFields |= GPS_VALID.GPS_VALID_LONGITUDE;
                        }
                        catch (Exception)
                        {
                        }

                        //
                        // Latitude
                        //

                        try
                        {
                            parsedData [3] = parsedData [3].TrimStart (zeros);
                            dblLatitude = Convert.ToDouble (parsedData [3], provider);
                            nDeg = (uint) (((uint) dblLatitude) / 100);
                            dblLatitude = nDeg + ((dblLatitude - nDeg * 100) / 60.0);

                            if (parsedData [4] == "S")
                                dblLatitude = -dblLatitude;
                            this.dwValidFields |= GPS_VALID.GPS_VALID_LATITUDE;
                        }
                        catch (Exception)
                        {
                        }

                        //
                        // Speed
                        //
                        try
                        {
                            flSpeed = (float) Convert.ToDouble (parsedData [7], provider);
                            this.dwValidFields |= GPS_VALID.GPS_VALID_SPEED;
                        }
                        catch (Exception)
                        {
                        }

                        //
                        // Heading
                        //

                        try
                        {
                            flHeading = (float) Convert.ToDouble (parsedData [8], provider);
                            this.dwValidFields |= GPS_VALID.GPS_VALID_HEADING;
                        }
                        catch (Exception)
                        {
                        }


                        //
                        // Magnetic variation
                        //

                        try
                        {
                            this.dblMagneticVariation = (float) Convert.ToDouble (parsedData [10], provider);
                            if (parsedData [11] == "W")
                                this.dblMagneticVariation = -this.dblMagneticVariation;
                            this.dwValidFields |= GPS_VALID.GPS_VALID_MAGNETIC_VARIATION;
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            string val;
                            val = parsedData [9].Substring (4, 2);
                            int nYear = 2000 + Convert.ToInt32 (val);
                            val = parsedData [9].Substring (2, 2);
                            int nMonth = Convert.ToInt32 (val);
                            val = parsedData [9].Substring (0, 2);
                            int nDay = Convert.ToInt32 (val);

                            val = parsedData [1].Substring (0, 2);
                            int nHour = Convert.ToInt32 (val);
                            val = parsedData [1].Substring (2, 2);
                            int nMinute = Convert.ToInt32 (val);
                            val = parsedData [1].Substring (4, 2);
                            int nSecond = Convert.ToInt32 (val);

                            this.stUTCTime = SYSTEMTIME.FromDateTime (new DateTime (nYear, nMonth, nDay, nHour, nMinute, nSecond, DateTimeKind.Utc));
                            this.dwValidFields |= GPS_VALID.GPS_VALID_UTC_TIME;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            /// <summary>
            /// Version of the GPS Intermediate Driver expected by the 
            /// application. Must be set before the structure is passed to 
            /// GPSGetPosition. Must be GPS_VERSION_1
            /// </summary>
            public int dwVersion;

            /// <summary>
            /// Size of the structure, in bytes. Must be set before the 
            /// structure is passed to GPSGetPosition.
            /// </summary>
            public int dwSize;

            /// <summary>
            /// Valid fields in this instance of the structure. 
            /// This field is a combination of some number of GPS_VALID_ flags, 
            /// or is 0 if no fields are valid. Valid fields depend on the GPS 
            /// hardware, how old the location data can be (controlled by the 
            /// dwMaximumAge parameter of the GPSGetPosition call), and the 
            /// current satellite signals, among other things.
            /// </summary>
            public GPS_VALID dwValidFields;

            /// <summary>
            /// Information about the state of the data retrieved in a call 
            /// to GPSGetPosition. This field is a combination of 
            /// GPS_DATA_FLAGS flags. 
            /// </summary>
            public GPS_DATA_FLAGS dwFlags;

            /// <summary>
            /// Universal time (UTC) according to information provided by 
            /// GPS satellites. 
            /// </summary>
            public SYSTEMTIME stUTCTime;

            /// <summary>
            /// Latitude, in degrees. Positive numbers indicate north latitude.
            /// </summary>
            public double dblLatitude;

            /// <summary>
            /// Longitude, in degrees. Positive numbers indicate east longitude. 
            /// </summary>
            public double dblLongitude;

            /// <summary>
            /// Speed, in knots (nautical miles). 
            /// km/h = flSpeed * 1.853;
            /// </summary>
            public float flSpeed;

            /// <summary>
            /// Speed, in km/h
            /// </summary>
            public float speedKMH
            {
                get
                {
                    return (float) (flSpeed * 1.853);
                }
            }

            /// <summary>
            /// Heading, in degrees. A heading of zero is true north. 
            /// </summary>
            public float flHeading;

            /// <summary>
            /// Magnetic variation, which is the difference between the 
            /// bearing to true north and the bearing shown on a magnetic 
            /// compass. Positive numbers indicate east.
            /// </summary>
            public double dblMagneticVariation;

            /// <summary>
            /// Altitude, in meters, with respect to sea level.
            /// </summary>
            public float flAltitudeWRTSeaLevel;

            /// <summary>
            /// Altitude, in meters, with respect to the WGS84 ellipsoid. 
            /// For more information about the use of the WGS84 ellipsoid 
            /// with GPS, see this NMEA Web site. 
            /// </summary>
            public float flAltitudeWRTEllipsoid;

            /// <summary>
            /// Quality of the GPS fix, which is one of invalid, normal GPS, or 
            /// differential GPS (DGPS). This field contains one of the values 
            /// in the GPS_FIX_QUALITY enumeration. 
            /// </summary>
            public GPS_FIX_QUALITY FixQuality;

            /// <summary>
            /// Type of GPS fix, either 2-D (only latitude and longitude, from 
            /// three satellites), or 3-D (latitude, longitude, and altitude, 
            /// from four or more satellites). This field contains one of the 
            /// values in the GPS_FIX_TYPE enumeration. 
            /// </summary>
            public GPS_FIX_TYPE FixType;


            /// <summary>
            /// Whether 2-D or 3-D mode is selected automatically or manually. 
            /// This field contains one of the values in the GPS_FIX_SELECTION enumeration. 
            /// </summary>
            public GPS_FIX_SELECTION SelectionType;

            /// <summary>
            /// Degree to which the overall position is affected by positional 
            /// dilution of position (PDOP). PDOP is caused by the location of 
            /// the satellites providing the GPS fix. Lower numbers indicate a 
            /// more accurate position. A value of 1.0 indicates the least 
            /// dilution (highest accuracy); a value of 50.0 indicates the most 
            /// dilution (lowest accuracy). 
            /// </summary>
            public float flPositionDilutionOfPrecision;

            /// <summary>
            /// Degree to which the horizontal position (latitude and longitude) 
            /// is affected by horizontal dilution of position (HDOP). HDOP is 
            /// caused by the location of the satellites providing the GPS fix. 
            /// Lower numbers indicate a more accurate position. A value of 1.0 
            /// indicates the least dilution (highest accuracy); a value of 50.0 
            /// indicates the most dilution (lowest accuracy). 
            /// </summary>
            public float flHorizontalDilutionOfPrecision;

            /// <summary>
            /// Degree to which the vertical position (altitude) is affected by 
            /// vertical dilution of position (VDOP). VDOP is caused by the 
            /// location of the satellites providing the GPS fix. Lower numbers 
            /// indicate a more accurate position. A value of 1.0 indicates the 
            /// least dilution (highest accuracy); a value of 50.0 indicates the 
            /// most dilution (lowest accuracy). 
            /// </summary>
            public float flVerticalDilutionOfPrecision;

            /// <summary>
            /// Number of satellites used to obtain the position. 
            /// </summary>
            public int dwSatelliteCount;

            /// <summary>
            /// PRN (pseudo-random noise) numbers of the satellites used to obtain the position. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            public int [] rgdwSatellitesUsedPRNs;

            /// <summary>
            /// Number of satellites in view of the GPS hardware. This value ranges from 0 to GPS_MAX_SATELLITES. 
            /// </summary>
            public int dwSatellitesInView;

            /// <summary>
            /// PRN (pseudo-random noise) numbers of the satellites in view of 
            /// the GPS hardware
            /// </summary>
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            public int [] rgdwSatellitesInViewPRNs;


            /// <summary>
            /// Elevation, in degrees, of the satellites in view of the GPS hardware. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            public int [] rgdwSatellitesInViewElevation;

            /// <summary>
            /// Azimuth, in degrees, of the satellites in view of the GPS hardware. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            public int [] rgdwSatellitesInViewAzimuth;

            /// <summary>
            /// Signal to noise ratio of the satellites in view of the GPS hardware. 
            /// Higher numbers indicate greater signal strength. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            public int [] rgdwSatellitesInViewSignalToNoiseRatio;
        };


        /// <summary>
        /// This structure contains information about the GPS Intermediate 
        /// Driver and GPS hardware used by the GPS Intermediate Driver.
        /// </summary>
        [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class GPS_DEVICE
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GPS_DEVICE ()
            {
                dwSize = Marshal.SizeOf (this);
                dwVersion = GPS_VERSION_1;
            }

            /// <summary>
            /// Клонирование
            /// </summary>
            public GPS_DEVICE Clone ()
            {
                return (GPS_DEVICE) this.MemberwiseClone ();
            }

            /// <summary>
            /// Version of the GPS Intermediate Driver expected by the application. 
            /// Must be set before the structure is passed to GPSGetDeviceState. 
            /// Must be GPS_VERSION_1. 
            /// </summary>
            public int dwVersion;

            /// <summary>
            /// Size of the structure, in bytes. Must be set before the 
            /// structure is passed to GPSGetDeviceState. 
            /// </summary>
            public int dwSize;

            /// <summary>
            /// State of the GPS Intermediate Driver. Contains one of the 
            /// SERVICE_STATE values defined IOCTL_SERVICE_STATUS. 
            /// </summary>
            public int dwServiceState;

            /// <summary>
            /// State of the driver controlling the GPS hardware. Contains one 
            /// of the SERVICE_STATE values defined in IOCTL_SERVICE_STATUS.
            /// </summary>
            public int dwDeviceState;

            /// <summary>
            /// Most recent time the GPS Intermediate Driver received information 
            /// from the GPS hardware. This time is based on UTC according to the 
            /// local system clock, not the clock information used by GPS 
            /// satellites. 
            /// </summary>
            public FILETIME ftLastDataReceived;

            /// <summary>
            /// Prefix string used to open the GPS hardware. This entry is 
            /// defined by the CommPort registry entry for hardware that 
            /// connects through a serial (or virtual serial) interface. 
            /// This entry is empty when the GPS Intermediate Driver reads 
            /// information from static files. For more information about GPS 
            /// hardware registry settings, see GPS Intermediate Driver GPS 
            /// Hardware Registry Settings. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValTStr, SizeConst = GPS_MAX_PREFIX_NAME)]
            public string szGPSDriverPrefix;

            /// <summary>
            /// Prefix string used to open the GPS Intermediate Driver 
            /// multiplexer. This entry is defined by the DriverInterface 
            /// registry entry in the Multiplexer key. For more information 
            /// about multiplexer registry settings, see GPS Intermediate 
            /// Driver Multiplexer Registry Settings. 
            /// 
            ///For more information about accessing data using the GPS 
            ///Intermediate Driver multiplexer, see Accessing Raw GPS Data. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValTStr, SizeConst = GPS_MAX_PREFIX_NAME)]
            public string szGPSMultiplexPrefix;

            /// <summary>
            /// Friendly name of the GPS hardware currently used by the 
            /// GPS Intermediate Driver. This entry is defined by the 
            /// FriendlyName registry entry. For more information about this 
            /// registry entry, see GPS Intermediate Driver Input Source 
            /// Registry Settings. 
            /// </summary>
            [MarshalAs (UnmanagedType.ByValTStr, SizeConst = GPS_MAX_FRIENDLY_NAME)]
            public string szGPSFriendlyName;
        };
        #endregion

        #region "Functions"

        /// <summary>
        /// This function creates a connection to the GPS Intermediate Driver. 
        /// 
        /// Note   Because this function results in turning on the GPS 
        /// hardware (if it's not already turned on), this function should only 
        /// be called immediately before using GPS information. For more 
        /// information, see GPS Intermediate Driver Power Management.
        /// </summary>
        /// <param name="hNewLocationData">Handle to a Windows CE event created 
        /// using CreateEvent, or NULL. The GPS Intermediate Driver signals the 
        /// passed event whenever it has new GPS location information.</param>
        /// <param name="hDeviceStateChange">Handle to a Windows CE event 
        /// created using CreateEvent, or NULL. The GPS Intermediate Driver 
        /// signals the passed event whenever the state of the device 
        /// changes.</param>
        /// <param name="szDeviceName">Reserved. Must be NULL. </param>
        /// <param name="dwFlags">Reserved. Must be 0.</param>
        /// <returns>If successful, returns a handle to the GPS Intermediate Driver. 
        /// If unsuccessful, returns NULL.</returns>
        /// <remarks>Because GPS hardware generally uses a large amount of power, 
        /// the GPS Intermediate Driver keeps the GPS hardware turned off until 
        /// this function is called (or a CreateFile call using the raw 
        /// interface is made). Subsequent calls to this function (or to 
        /// CreateFile) when the GPS hardware is already turned are 
        /// reference-counted by the GPS Intermediate Driver so that the 
        /// GPS Intermediate Driver can automatically turn off the GPS hardware 
        /// when it is no longer in use.</remarks>

        [DllImport ("Gpsapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GPSOpenDevice (IntPtr hNewLocationData,
                              IntPtr hDeviceStateChange,
                              string szDeviceName,
                              int dwFlags);

        /// <summary>
        /// This function closes the connection to the GPS Intermediate Driver. 
        /// If this is the only connection to the GPS Intermediate Driver, the 
        /// GPS Intermediate Driver turns off the GPS hardware to save power.
        /// </summary>
        /// <param name="hGPSDevice">Handle returned by a call to GPSOpenDevice.</param>
        /// <returns>If successful, returns ERROR_SUCCESS. 
        /// If unsuccessful, returns an error code.</returns>
        [DllImport ("Gpsapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GPSCloseDevice (IntPtr hGPSDevice);

        /// <summary>
        /// This function retrieves location information, including latitude and 
        /// longitude, by setting the fields of the passed GPS_POSITION structure 
        /// with location information that meets the specified maximum age 
        /// criteria.
        /// </summary>
        /// <param name="hGPSDevice">Handle returned by a call to GPSOpenDevice. 
        /// This parameter can also be NULL. If 
        /// NULL, the GPS Intermediate Driver does not turn on the GPS hardware, 
        /// but does return data if the data has already been received (through 
        /// some other use of the GPS Intermediate Driver), and the data 
        /// satisfies the criteria set by the dwMaximumAge parameter. 
        /// Passing NULL enables applications that can use location information, 
        /// but do not require it, to retrieve existing data without requiring 
        /// that the GPS hardware consume power.
        /// </param>
        /// <param name="pGPSPosition">Pointer to a GPS_POSITION structure. 
        /// On return, this structure is filled with location data obtained by 
        /// the GPS Intermediate Driver. The dwValidFields member of the 
        /// GPS_POSITION instance specifies which fields of the instance are 
        /// valid.</param>
        /// <param name="dwMaximumAge">Maximum age, in milliseconds, of location 
        /// information. The GPS Intermediate Driver only returns information 
        /// that has been received within the time specified by this parameter. 
        /// Any information that is older than this age is not returned. The 
        /// elements of the GPS_POSITION instance that are valid for the given 
        /// dwMaximumAge value are specified in the dwValidFields element of 
        /// the instance.</param>
        /// <param name="dwFlags">Reserved. Must be 0.</param>
        /// <returns>If successful, returns ERROR_SUCCESS. 
        /// If unsuccessful, returns an error code.</returns>
        /// <remarks>Applications most commonly call this function when the 
        /// event passed in the hNewLocationData parameter of the 
        /// GPSOpenDevice call is signaled. Alternatively, applications can 
        /// poll using this method, or just call it a single time and then 
        /// close the device using GPSCloseDevice.</remarks>
        [DllImport ("Gpsapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GPSGetPosition (IntPtr hGPSDevice,
                                          [In, Out] GPS_POSITION pGPSPosition,
                                          int dwMaximumAge,
                                          int dwFlags);

        /// <summary>
        /// This function retrieves information about the current state of the 
        /// GPS hardware by setting the fields of the passed GPS_DEVICE structure.
        /// </summary>
        /// <param name="pGPSDevice">Pointer to a GPS_DEVICE structure. On 
        /// return, this structure is filled with device data for the GPS 
        /// hardware managed by the GPS Intermediate Driver. </param>
        /// <returns>If successful, returns ERROR_SUCCESS. 
        /// If unsuccessful, returns an error code.</returns>
        /// <remarks>For more information about setting configurable GPS 
        /// Intermediate Driver settings, like the driver and multiplex 
        /// prefixes, see Configuring the GPS Intermediate Driver.</remarks>
        [DllImport ("Gpsapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GPSGetDeviceState ([In, Out] GPS_DEVICE pGPSDevice);
        #endregion
    }
}
