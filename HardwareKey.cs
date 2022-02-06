////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           HardwareKey.cs
//
//  Facility:       Лицензирование с аппаратной привязкой.
//
//
//  Abstract:       Класс для работы с файлом лицензии, привязанной к аппаратуре.
//
//  Environment:    VC# 8
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  03/06/2006
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: HardwareKey.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace LightCom.WinCE
{
    /// <summary>
    /// Класс для работы с файлом лицензии, привязанной к аппаратуре.
    /// </summary>
    public class HardwareKey
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public HardwareKey ()
        {
            Reset ();
        }

        /// <summary>
        /// Очистка объекта
        /// </summary>
        public void Reset ()
        {
            m_strPlatformId = string.Empty;
            m_strPresetId = string.Empty;
            m_strLicenseOwner = string.Empty;
        }

        /// <summary>
        /// Записывает лицензию в файл strFileName
        /// </summary>
        /// <param name="strFileName">Имя файла лицензии</param>
        /// <param name="strCustomKey">Произвольный текст, подписывающий лицензию</param>
        /// <returns>true, если файл был успешно подписан</returns>
        public bool SaveLicense (string strFileName, string strCustomKey)
        {
            try
            {
                StreamWriter sw = new StreamWriter (strFileName);
                sw.WriteLine (PresetId);
                sw.WriteLine (PlatformId);
                sw.WriteLine (LicenseOwner);
                string strKey = PresetId + PlatformId + m_strLicenseOwner + strCustomKey;

                MD5 md5 = new MD5CryptoServiceProvider ();
                byte [] bytes = UnicodeEncoding.Unicode.GetBytes (strKey);
                byte [] hash = md5.ComputeHash (bytes);
                sw.Write (Convert.ToBase64String (hash));

                sw.Close ();
            }
#if TRACE
            catch (Exception e)
#else
            catch (Exception)
#endif
            {
#if TRACE
            System.Diagnostics.Trace.WriteLine (e.ToString ());
#endif
                return false;
            }

            return true;
        }

        /// <summary>
        /// Записывает лицензию в файл strFileName
        /// </summary>
        /// <param name="strFileName">Имя файла лицензии</param>
        /// <param name="strCustomKey">Произвольный текст, подписывающий лицензию</param>
        /// <returns>true, если файл был успешно подписан</returns>
        public bool LoadLicense (string strFileName, string strCustomKey)
        {
            Reset ();
            try
            {
                StreamReader sr = new StreamReader (strFileName);
                PresetId = sr.ReadLine ();
                PlatformId = sr.ReadLine ();
                LicenseOwner = sr.ReadLine ();
                
                string strKey = PresetId + PlatformId + m_strLicenseOwner + strCustomKey;
                MD5 md5 = new MD5CryptoServiceProvider ();
                byte [] bytes = UnicodeEncoding.Unicode.GetBytes (strKey);
                byte [] hash = md5.ComputeHash (bytes);
                string strComputedHash = Convert.ToBase64String (hash);
                string strHash = sr.ReadLine ();
                sr.Close ();

                if (strComputedHash != strHash)
                {
                    Reset ();
                    return false;
                }
                else
                {
                    return true;
                }
            }
#if TRACE
            catch (Exception e)
#else
            catch (Exception)
#endif
            {
#if TRACE
            System.Diagnostics.Trace.WriteLine (e.ToString ());
#endif
            }

            Reset ();
                return false;
        }

        /// <summary>
        /// Preset ID
        /// </summary>
        protected string m_strPresetId;
        public string PresetId
        {
            get
            {
                return m_strPresetId;
            }
            set
            {
                m_strPresetId = value;
            }
        }

        /// <summary>
        /// Platform ID
        /// </summary>
        protected string m_strPlatformId;
        public string PlatformId
        {
            get
            {
                return m_strPlatformId;
            }
            set
            {
                m_strPlatformId = value;
            }
        }
        
        /// <summary>
        /// Владелец лицензии
        /// </summary>
        protected string m_strLicenseOwner;
        public string LicenseOwner
        {
            get
            {
                return m_strLicenseOwner;
            }
            set
            {
                m_strLicenseOwner = value;
            }
        }
    }
}