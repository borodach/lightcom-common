///////////////////////////////////////////////////////////////////////////////
//
//  File:           Utils.cs
//
//  Facility:       Common utils.
//
//
//  Abstract:       Разные полезные методы.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-11-2005
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: Utils.cs $
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 3.04.07    Time: 21:53
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 31.03.07   Time: 12:21
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:39
 * Updated in $/LightCom/.NET/Common
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.IO;
using System.Text;

namespace LightCom.Common
{
    /// 
    /// <summary>
    /// Разные полезные методы.
    /// </summary>
    ///

    public class Utils
    {

        /// 
        /// <summary>
        /// Возвращает плное имя образа сборки.
        /// </summary>
        /// <returns>Возвращает плное имя образа сборки.
        /// </returns>
        /// 

        public static string ExePath
        {
            get
            {
                System.Reflection.Assembly asm =
                    System.Reflection.Assembly.GetCallingAssembly ();
                if (null != asm)
                {
                    System.Reflection.Module [] modules = asm.GetModules ();
                    if (null != modules && modules.Length > 0)
                    {
                        string strPath = modules [0].FullyQualifiedName;
                        return strPath;
                    }
                }

                return "";
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Возвращает папку, которой находится образ сборки.
        /// </summary>
        /// <returns>Возвращает папку,  которой находится образ сборки.
        /// </returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static string ExeDirectory
        {
            get
            {
                string strPath = ExePath;
                int nIdx = strPath.LastIndexOf ('\\');
                if (nIdx >= 0)
                {
                    return strPath.Substring (0, nIdx);
                }

                return "";
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Записывает целое число в поток.
        /// </summary>
        /// <param name="stream">Поток для записи.</param>
        /// <param name="nData">Записываемое число.</param>
        /// 
        ////////////////////////////////////////////////////////////////////////////////
        /*
        public static void Write (Stream stream, uint nData)
        {
            byte [] data = BitConverter.GetBytes (nData);            
            if (! BitConverter.IsLittleEndian) Array.Reverse (data, 0, data.Length);

            stream.Write (data, 0, data.Length);            
        }
        */
        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Читает целое число из потока.
        /// </summary>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Прочитанное число.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////
        /*
        public static uint ReadUInt (Stream stream)
        {
            uint nResult = 0;
            byte [] data = BitConverter.GetBytes (nResult);            
            stream.Read (data, 0, data.Length);            
            if (! BitConverter.IsLittleEndian) Array.Reverse (data, 0, data.Length);
            nResult = BitConverter.ToUInt32 (data, 0);
            return nResult;
        }
        */

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Записывает целое число в поток.
        /// </summary>
        /// <param name="stream">Поток для записи.</param>
        /// <param name="nData">Записываемое число.</param>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static void Write (Stream stream, int nData)
        {
            byte [] data = BitConverter.GetBytes (nData);
            stream.Write (data, 0, data.Length);
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Читает целое число из потока.
        /// </summary>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Прочитанное число.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static int ReadInt (Stream stream)
        {
            int nResult = 0;
            byte [] data = BitConverter.GetBytes (nResult);
            stream.Read (data, 0, data.Length);
            nResult = BitConverter.ToInt32 (data, 0);

            return nResult;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Записывает строчку в поток.
        /// </summary>
        /// <param name="stream">Поток для записи строки.</param>
        /// <param name="strData">Записываемая строка.</param>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static void Write (Stream stream, string strData)
        {
            byte [] data;
            UTF8Encoding encoder = new UTF8Encoding ();
            data = encoder.GetBytes (strData);
            Write (stream, data.Length);
            stream.Write (data, 0, data.Length);
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Записывает строчку в поток.
        /// </summary>
        /// <param name="stream">Поток для чтения строки.</param>
        /// <returns>Прочитанная строка.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static string ReadString (Stream stream)
        {
            int nSize = ReadInt (stream);
            if (nSize < 0)
            {
                throw new Exception ("Ошибка чтения строки из потока: размер строки отрицательный.");
            }
            byte [] data = new byte [nSize];
            stream.Read (data, 0, data.Length);
            UTF8Encoding encoder = new UTF8Encoding ();
            return encoder.GetString (data, 0, data.Length);
        }

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// Записывает DateTime в поток.
        /// </summary>
        /// <param name="stream">Поток для записи.</param>
        /// <param name="dateTime">DateTime</param>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static void Write (Stream stream, DateTime dateTime)
        {
            string strDateTime = dateTime.ToString (m_strDateTimeFormat);
            Write (stream, strDateTime);
        }

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// Чтение DateTime из потока.
        /// </summary>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Прочитанное значение.</returns> 
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static DateTime ReadDateTime (Stream stream)
        {
            string strDateTime = ReadString (stream);
            if (strDateTime.Length != m_strDateTimeFormat.Length)
            {
                throw new Exception ("Ошибка чтения даты и времени из потока: неправильный размер (\"" +
                    strDateTime + "\").");
            }

            //  01234567890123456789012
            //("dd.MM.yyyy HH:mm:ss.fff");
            int nDay = Convert.ToInt32 (strDateTime.Substring (0, 2));
            int nMonth = Convert.ToInt32 (strDateTime.Substring (3, 2));
            int nYear = Convert.ToInt32 (strDateTime.Substring (6, 4));
            int nHour = Convert.ToInt32 (strDateTime.Substring (11, 2));
            int nMinute = Convert.ToInt32 (strDateTime.Substring (14, 2));
            int nSecond = Convert.ToInt32 (strDateTime.Substring (17, 2));
            int nMillisecond = Convert.ToInt32 (strDateTime.Substring (20, 3));
            return new DateTime (nYear, nMonth, nDay, nHour, nMinute, nSecond, nMillisecond);
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Вычисляет контрольную сумму.
        /// </summary>
        /// <param name="data">Данные, для которых нужно подсчитать контрольную 
        /// сумму.</param>
        /// <returns>Возвращает контрольную сумму.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public static int CalculateCRC (byte [] data)
        {
            int nSize = data.Length;
            int nResult = 0;
            for (int i = 0; i < nSize; ++i)
                nResult += (int) data [i];
            return nResult;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Возвращает буфер с данными, помещенными в MemoryStream.
        /// </summary>
        /// <param name="ms">MemoryStream</param>
        /// <returns>Возвращает буфер с данными, помещенными в MemoryStream.</returns>
        ///
        ////////////////////////////////////////////////////////////////////////////////

        public static byte [] GetStreamData (MemoryStream ms)
        {
            int nStreamSize = (int) ms.Length;
            byte [] secretData = new byte [nStreamSize];
            byte [] streamBuffer = ms.GetBuffer ();
            for (int i = 0; i < nStreamSize; ++i)
            {
                secretData [i] = streamBuffer [i];
            }
            return secretData;
        }

        // 
        //Формат даты и времени.
        //

        public const string m_strDateTimeFormat = "dd.MM.yyyy HH:mm:ss.fff";

        /// <summary>
        /// Коэффициент для преобразования градусов в радианы
        /// </summary>
        public const double m_Deg2Rad = (double) Math.PI / 180;

        /// <summary>
        /// Вычисляет расстояние между двумя точками, заданными географическими 
        /// координатами
        /// </summary>
        /// <param name="fLat0">Широта первой точки, в градусах</param>
        /// <param name="fLong0">Долгота первой точки, в градусах</param>
        /// <param name="fLat1">Широта второй точки, в градусах</param>
        /// <param name="fLong1">Долгота второй точки, в градусах</param>
        /// <returns>Расстояние между точками в метрах</returns>
        public static double CalculateDistantion (double fLat0, double fLong0,
            double fLat1, double fLong1)
        {
            fLat0 = fLat0 * m_Deg2Rad;
            fLat1 = fLat1 * m_Deg2Rad;
            fLong0 = fLong0 * m_Deg2Rad;
            fLong1 = fLong1 * m_Deg2Rad;


            double a = 6378137, b = 6356752.3142, f = 1 / 298.257223563;  // WGS-84 ellipsiod
            double L = fLong1 - fLong0;
            double U1 = Math.Atan ((1 - f) * Math.Tan (fLat0));
            double U2 = Math.Atan ((1 - f) * Math.Tan (fLat1));
            double sinU1 = Math.Sin (U1), cosU1 = Math.Cos (U1);
            double sinU2 = Math.Sin (U2), cosU2 = Math.Cos (U2);

            double lambda = L, lambdaP = 2 * Math.PI;
            int iterLimit = 20;
            double cosSqAlpha = 0;
            double sinAlpha = 0;
            double cosSigma = 0;
            double cos2SigmaM = 0;
            double sigma = 0;
            double sinSigma = 0;

            while (Math.Abs (lambda - lambdaP) > 1e-12 && --iterLimit > 0)
            {
                double sinLambda = Math.Sin (lambda), cosLambda = Math.Cos (lambda);
                sinSigma = Math.Sqrt ((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) *
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));

                if (sinSigma == 0)
                    return 0;  // co-incident points

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2 (sinSigma, cosSigma);
                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                if (cosSqAlpha == 0)
                    return Math.Abs (a * L);
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                    (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            }

            if (iterLimit == 0)
                return double.NaN;  // formula failed to converge

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double s = b * A * (sigma - deltaSigma);
            return s;
        }
        
        /// <summary>
        /// Вычисляет координаты точки по заданному смещению
        /// </summary>
        /// <param name="dLat0">Широта начальной точки</param>
        /// <param name="dLon0">Долгота начальной точки</param>
        /// <param name="dBearing">Азимут (в градусах)</param>
        /// <param name="dDistance">Растояние (в метрах)</param>
        /// <param name="dLat1">Широта конечной точки</param>
        /// <param name="dLon1">Долгота конечной точки</param>
        void CaculatePosition (double dLat0, double dLon0, 
                               double dBearing, double dDistance, 
                               out double dLat1, out double dLon1)
        {

            dLat0 *= m_Deg2Rad;
            dLon0 *= m_Deg2Rad;
            dBearing *= m_Deg2Rad;

            double a = 6378137, b = 6356752.3142, f = 1 / 298.257223563;  // WGS-84 ellipsiod
            double sinAlpha1 = Math.Sin (dBearing), cosAlpha1 = Math.Cos (dBearing);

            double tanU1 = (1 - f) * Math.Tan (dLat0);
            double cosU1 = 1 / Math.Sqrt ((1 + tanU1 * tanU1)), sinU1 = tanU1 * cosU1;
            double sigma1 = Math.Atan2 (tanU1, cosAlpha1);
            double sinAlpha = cosU1 * sinAlpha1;
            double cosSqAlpha = 1 - sinAlpha * sinAlpha;
            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            double sigma = dDistance / (b * A), sigmaP = 2 * Math.PI;
            double cos2SigmaM = 1;
            double sinSigma = 0;
            double cosSigma = 1;
            double deltaSigma;
            while (Math.Abs (sigma - sigmaP) > 1e-12)
            {
                cos2SigmaM = Math.Cos (2 * sigma1 + sigma);
                sinSigma = Math.Sin (sigma);
                cosSigma = Math.Cos (sigma);
                deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                  B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
                sigmaP = sigma;
                sigma = dDistance / (b * A) + deltaSigma;
            }

            double tmp = sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1;
            double lat2 = Math.Atan2 (sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1,
                (1 - f) * Math.Sqrt (sinAlpha * sinAlpha + tmp * tmp));
            double lambda = Math.Atan2 (sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);
            double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
            double L = lambda - (1 - C) * f * sinAlpha *
                (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

            double revAz = Math.Atan2 (sinAlpha, -tmp);  // final bearing

            dLat1 = lat2 * 180 / Math.PI;
            dLon1 = (dLon0 + L) * 180 / Math.PI;
        }
    }
}
